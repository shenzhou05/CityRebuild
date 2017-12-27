/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

#if UNITY_EDITOR

using UnityEngine;
using System.Collections;

namespace QuickEdit
{
	[ExecuteInEditMode]
	public class qe_HandleRenderer : MonoBehaviour
	{
		// HideFlags.DontSaveInEditor isn't exposed for whatever reason, so do the bit math on ints 
		// and just cast to HideFlags.
		// HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.NotEditable
		HideFlags SceneCameraHideFlags = (HideFlags) (1 | 4 | 8);

		public Mesh mesh;
		public Material material;

		void OnDestroy()
		{
			if(mesh) DestroyImmediate(mesh);
			if(material) DestroyImmediate(material);
		}

		void OnRenderObject()
		{
			// instead of relying on 'SceneCamera' string comparison, check if the hideflags match.
			// this could probably even just check for one bit match, since chances are that any 
			// game view camera isn't going to have hideflags set.
			if( (Camera.current.gameObject.hideFlags & SceneCameraHideFlags) != SceneCameraHideFlags || Camera.current.name != "SceneCamera" )
				return;

			Mesh msh = mesh;
			Material mat = material;

			if(mat == null || msh == null || !material.SetPass(0) )
				return;

			Graphics.DrawMeshNow(msh, transform.localToWorldMatrix);
		}
	}
}
#endif
