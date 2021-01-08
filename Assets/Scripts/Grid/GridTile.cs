using UnityEngine;

public class GridTile{
	public bool walkable = true;
	public GameObject gridTile = null;
	public Vector2Int position;
	public int cost = 0;
	public bool point = false;
	
	private MeshRenderer _renderer = null;
	
	public GridTile(Vector2Int position, MeshRenderer renderer, Color tileColor){
		this.position = position;
		_renderer = renderer;
		SetColor(tileColor);
	}

	public void SetColor(Color color){
		_renderer.material.color = color;
	}
}