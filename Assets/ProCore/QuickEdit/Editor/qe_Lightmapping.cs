/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEditor;
using UnityEngine;

namespace QuickEdit
{
	/**
	 * Methods used in manipulating or creating Lightmaps.
	 */
	public static class qe_Lightmapping
	{
		/**
		 * Editor-only extension to qe_Mesh generates lightmap UVs.
		 */
	    public static void GenerateUV2(this Mesh mesh) { mesh.GenerateUV2(false); }

		public static void GenerateUV2(this Mesh mesh, bool forceUpdate)
		{
			// SetUVParams(8f, 15f, 15f, 20f);
			UnwrapParam param;
			UnwrapParam.SetDefaults(out param);
			
			Unwrapping.GenerateSecondaryUVSet(mesh, param);

			EditorUtility.SetDirty(mesh as Object);
		}

		/**
		 * Store the previous GIWorkflowMode and set the current value to OnDemand (or leave it Legacy).
		 */
		[System.Diagnostics.Conditional("UNITY_5")]
		internal static void PushGIWorkflowMode()
		{
	#if UNITY_5
			EditorPrefs.SetInt("qe_GIWorkflowMode", (int)Lightmapping.giWorkflowMode);

			if(Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.Legacy)
				Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
	#endif
		}

		/**
		 * Return GIWorkflowMode to it's prior state.
		 */
		[System.Diagnostics.Conditional("UNITY_5")]
		internal static void PopGIWorkflowMode()
		{
	#if UNITY_5
			// if no key found (?), don't do anything.
			if(!EditorPrefs.HasKey("qe_GIWorkflowMode"))
				return;

			 Lightmapping.giWorkflowMode = (Lightmapping.GIWorkflowMode)EditorPrefs.GetInt("qe_GIWorkflowMode");
	#endif
		}
	}
}
