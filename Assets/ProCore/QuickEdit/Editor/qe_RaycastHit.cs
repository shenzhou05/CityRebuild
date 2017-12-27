/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;

namespace QuickEdit
{
	public struct qe_RaycastHit
	{
		public float Distance;
		public Vector3 Point;
		public Vector3 Normal;
		public int FaceIndex;

		public qe_RaycastHit(float InDistance, Vector3 InPoint, Vector3 InNormal, int InFaceIndex)
		{
			this.Distance = InDistance;
			this.Point = InPoint;
			this.Normal = InNormal;
			this.FaceIndex = InFaceIndex;
		}

		public override string ToString()
		{
			return string.Format("Point: {0}  Triangle: {1}", Point, FaceIndex);
		}
	}
}
