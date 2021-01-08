using UnityEngine;

public class Heuristic{
	public static float Distance(Vector3 a, Vector3 b){
		return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
	}
}
