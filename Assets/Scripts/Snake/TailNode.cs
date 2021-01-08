using UnityEngine;

public class TailNode{
	public TailNode child = null;
	public Vector2Int position = Vector2Int.zero;

	public TailNode(Vector2Int position){
		this.position = position;
	}
}