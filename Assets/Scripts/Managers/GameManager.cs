using System;
using UnityEngine;

public class GameManager : MonoBehaviour{
	public static GameManager Instance;
	public static Grid GameGrid;

	public int height = 10;
	public int width = 10;
	public float gameSpeed = 0.5f;
	public GameObject tilePrefab;

	public Color tileColor;
	public Color pointColor;

	[NonSerialized] 
	public bool pause = false;
	
	private Transform _transform;

	private void Awake(){
		if (Instance == null) Instance = this;
		else Destroy(gameObject);

		_transform = transform;

		GameGrid = new Grid(height, width, tilePrefab, _transform, InstantiateTile);
		GameGrid.GenerateGrid();

		gameObject.AddComponent<Controller>();
		gameObject.AddComponent<AISnake>();
	}

	private void Start(){
		GameGrid.GeneratePoint();
	}

	private void InstantiateTile(GameObject gameObject, int x, int y){
		GameObject tile = Instantiate(gameObject);
		tile.transform.position = new Vector3(x, y);
		tile.transform.parent = _transform;

		GridTile gridTile = new GridTile(new Vector2Int(x, y), tile.GetComponent<MeshRenderer>(), tileColor);
		GameGrid.grid[x, y] = gridTile;

		gridTile.gridTile = tile;
	}

	public void GameOver(){
		Debug.Log("Game Over");
		pause = true;
	}

	public void Win(){
		Debug.Log("Win");
		pause = true;
	}
}