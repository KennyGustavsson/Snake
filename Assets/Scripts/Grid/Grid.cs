using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid{
	public int height = 10;
	public int width = 10;
	public GridTile[,] grid;
	public GameObject tilePrefab;
	private Camera _camera;
	private Action<GameObject, int, int> _action;

	public Grid(int height, int width, GameObject tilePrefab, Transform transform, Action<GameObject, int, int> Instatiator){
		this.height = height;
		this.width = width;
		this.tilePrefab = tilePrefab;
		
		_action = Instatiator;
		_camera = Camera.main;

		grid = new GridTile[width, height];
	}

	public void GenerateGrid(){
		for (int y = 0; y < height; y++){
			for (int x = 0; x < width; x++){
				_action.Invoke(tilePrefab, x, y);
			}
		}

		_camera.transform.position = new Vector3(width * 0.5f, height * 0.5f - 0.5f, -height);
		_camera.orthographicSize = height * 0.5f + 1;
	}
	
	public void GeneratePoint(){
		bool pointGenerated = false;

		while (!pointGenerated){
			int x = Random.Range(0, width);
			int y = Random.Range(0, height);

			GridTile tile = GetTile(new Vector2Int(x, y));
			if (tile.walkable && !tile.point){
				tile.point = true;
				tile.SetColor(GameManager.Instance.pointColor);
				pointGenerated = true;
				EventManager.ONGeneratedPoint(tile);
			}
		}
	}
	
	public void MoveToTile(Vector2Int current, Vector2Int move, Color snakeColor, Color tileColor){
		GridTile moveTile = grid[move.x, move.y];
		moveTile.SetColor(snakeColor);
		moveTile.walkable = false;
		
		GridTile curTile = grid[current.x, current.y];
		curTile.SetColor(tileColor);
		curTile.walkable = true;
	}

	public GridTile GetTile(Vector2Int position){
		return grid[position.x, position.y];
	}
}