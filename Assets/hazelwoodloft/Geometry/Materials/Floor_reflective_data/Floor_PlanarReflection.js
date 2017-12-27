// The script is quite long, most of the code deals with oblique-clipped
// projection matrices and whatnot. The code is very similar to
// what ReflectionRenderTexture script in Pro Standard Assets does,
// but I included everything into a single file to avoid confusion and
// dependencies.
 
var renderTextureSize = 256;
var clipPlaneOffset = 0.01;
var disablePixelLights = true;
 
private var renderTexture : RenderTexture;
private var restorePixelLightCount : int;
private var sourceCamera : Camera; // The camera we are going to reflect
 
 
function Start()
{
    if( !RenderTexture.enabled ) {
        print("Render textures are not available. Disabling mirror script");
        enabled = false;
        return;
    }
 
    renderTexture = new RenderTexture( renderTextureSize, renderTextureSize, 16 );
    renderTexture.isPowerOfTwo = true;
 
    gameObject.AddComponent.<Camera>();
    var cam : Camera = GetComponent.<Camera>();
    var mainCam = Camera.main;
    cam.targetTexture = renderTexture;
    cam.clearFlags = mainCam.clearFlags;
    cam.backgroundColor = mainCam.backgroundColor;
    cam.nearClipPlane = mainCam.nearClipPlane;
    cam.farClipPlane = mainCam.farClipPlane;
    cam.fieldOfView = mainCam.fieldOfView;
 
    GetComponent.<Renderer>().material.SetTexture("_ReflectionTex", renderTexture);
}
 
function Update()
{
    var scaleOffset = Matrix4x4.TRS( Vector3(0.5,0.5,0.5), Quaternion.identity, Vector3(0.5,0.5,0.5) );
    GetComponent.<Renderer>().material.SetMatrix( "_ProjMatrix",
        scaleOffset *
        GetComponent.<Camera>().main.projectionMatrix *
        GetComponent.<Camera>().main.worldToCameraMatrix *
        transform.localToWorldMatrix );
}
 
function OnDisable() {
    Destroy(renderTexture);
}
 
 
function LateUpdate ()
{
    // Use main camera for reflection
    sourceCamera = Camera.main;
 
    // Figure out if we can do reflection/refraction
    if (!RenderTexture.enabled)
    {
        GetComponent.<Camera>().enabled = false; // no RTs - can't do
    }
    else if (GetComponent.<Camera>().targetTexture == null)
    {
        Debug.Log ("No Render Texture assigned! Disabling reflection.");
        GetComponent.<Camera>().enabled = false;
    }
    else if( !sourceCamera )
    {
        Debug.Log ("Reflection rendering requires that a Camera that is tagged \"MainCamera\"! Disabling reflection.");
        GetComponent.<Camera>().enabled = false;
    }
    else
    {
        GetComponent.<Camera>().enabled = true;
    }
}
 
function OnPreCull ()
{
    sourceCamera = Camera.main;
    if( sourceCamera )
    {            
        // find out the reflection plane: position and normal in world space
        var pos = transform.position;
        var normal = transform.up;
 
        // need to reflect the source camera around reflection plane
        var d = -Vector3.Dot (normal, pos) - clipPlaneOffset;
        var reflectionPlane = Vector4 (normal.x, normal.y, normal.z, d);
 
        var reflection = CalculateReflectionMatrix(reflectionPlane);
        GetComponent.<Camera>().worldToCameraMatrix = sourceCamera.worldToCameraMatrix * reflection;
 
        // Setup oblique projection matrix so that near plane is our reflection
        // plane. This way we clip everything below/above it for free.
        var clipPlane = CameraSpacePlane( pos, normal );
        GetComponent.<Camera>().projectionMatrix = CalculateObliqueMatrix( sourceCamera.projectionMatrix, clipPlane );
    }
    else
    {
        GetComponent.<Camera>().ResetWorldToCameraMatrix ();
    }
}
 
function OnPreRender ()
{
    // we need to revert backface culling
    GL.SetRevertBackfacing (true);
 
    if( disablePixelLights )
    {
        restorePixelLightCount = Light.pixelLightCount;
        Light.pixelLightCount = 0;
    }
}
 
function OnPostRender ()
{
    // restore the backface culling
    GL.SetRevertBackfacing (false);
 
    if( disablePixelLights )
        Light.pixelLightCount = restorePixelLightCount;
}
 
// Given position/normal of the plane, calculates plane in camera space.
function CameraSpacePlane( pos : Vector3, normal : Vector3 ) : Vector4
{
    var offsetPos = pos + normal * clipPlaneOffset;
    var m = GetComponent.<Camera>().worldToCameraMatrix;
    var cpos = m.MultiplyPoint( offsetPos );
    var cnormal = m.MultiplyVector( normal ).normalized;
    return Vector4( cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos,cnormal) );
}
 
// Extended sign: returns -1, 0 or 1 based on sign of a
static function sgn(a : float) : float
{
    if (a > 0.0) return 1.0;
    if (a < 0.0) return -1.0;
    return 0.0;
}
 
// Adjusts the given projection matrix so that near plane is the given clipPlane
// clipPlane is given in camera space. See article in GPG5.
static function CalculateObliqueMatrix( projection : Matrix4x4, clipPlane : Vector4 ) : Matrix4x4
{
    var q : Vector4;
    q.x = (sgn(clipPlane.x) + projection[8]) / projection[0];
    q.y = (sgn(clipPlane.y) + projection[9]) / projection[5];
    q.z = -1.0;
    q.w = (1.0 + projection[10]) / projection[14];
 
    var c = clipPlane * (2.0 / (Vector4.Dot (clipPlane, q)));
 
    projection[2] = c.x;
    projection[6] = c.y;
    projection[10] = c.z + 1.0;
    projection[14] = c.w;
 
    return projection;
}
 
// Calculates reflection matrix around the given plane
static function CalculateReflectionMatrix( plane : Vector4 ) : Matrix4x4
{
    var reflectionMat : Matrix4x4;
    reflectionMat.m00 = (1 - 2*plane[0]*plane[0]);
    reflectionMat.m01 = (  - 2*plane[0]*plane[1]);
    reflectionMat.m02 = (  - 2*plane[0]*plane[2]);
    reflectionMat.m03 = (  - 2*plane[3]*plane[0]);
 
    reflectionMat.m10 = (  - 2*plane[1]*plane[0]);
    reflectionMat.m11 = (1 - 2*plane[1]*plane[1]);
    reflectionMat.m12 = (  - 2*plane[1]*plane[2]);
    reflectionMat.m13 = (  - 2*plane[3]*plane[1]);
 
    reflectionMat.m20 = (  - 2*plane[2]*plane[0]);
    reflectionMat.m21 = (  - 2*plane[2]*plane[1]);
    reflectionMat.m22 = (1 - 2*plane[2]*plane[2]);
    reflectionMat.m23 = (  - 2*plane[3]*plane[2]);
 
    reflectionMat.m30 = 0;
    reflectionMat.m31 = 0;
    reflectionMat.m32 = 0;
    reflectionMat.m33 = 1;
    return reflectionMat;
}