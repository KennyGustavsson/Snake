using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnake : MonoBehaviour{
	[Header("Snake")]
	public int startingTailLength = 2;
	public Color snakeColor = Color.red;
	public Vector2Int startingLocation = Vector2Int.zero;

	private HeadNode _snake = null;
	private Vector2Int _dir = Vector2Int.zero;
	private Coroutine _movingCoroutine;
	
	private void Awake(){
		_snake = new HeadNode(startingLocation, snakeColor, GameManager.Instance.tileColor);

		for (int i = 0; i < startingTailLength; i++){
			_snake.AddTail();
		}
	}

	private void OnEnable(){
		EventManager.OnGeneratedPoint += SnakePath;
	}

	private void OnDisable(){
		EventManager.OnGeneratedPoint -= SnakePath;
	}

	private void SnakePath(GridTile target){
		if(_movingCoroutine != null){
			StopCoroutine(_movingCoroutine);
			_movingCoroutine = null;
		}
		
		_movingCoroutine = StartCoroutine(MoveSnakeAStar(_snake,
		 	AStarSearch.AStar(GameManager.GameGrid.GetTile(_snake.position), target, _dir), target));
	}
	
	private IEnumerator MoveSnakeAStar(HeadNode snake, Queue<GridTile> path, GridTile target){
		while (GameManager.Instance.pause){
			yield return new WaitForEndOfFrame();
		}
		
		GridTile pos = GameManager.GameGrid.grid[snake.position.x, snake.position.y];
		
		// No path available
		if(path == null || path.Count == 0){
			// Do some random movement then check if path is available
			int i = 0;
			while (i < 4){
				switch (i){
					case 0:
						if (!snake.CheckIfCollide(snake.LimitMoveToGrid(snake.position + Vector2Int.up))){
							snake.MoveSnake(Vector2Int.up);
							_dir = Vector2Int.up;
							yield return new WaitForSeconds(GameManager.Instance.gameSpeed);
							SnakePath(target);
							yield break;
						}
						
						i++;
						break;
				
					case 1:
						if (!snake.CheckIfCollide(snake.LimitMoveToGrid(snake.position + Vector2Int.down))){
							snake.MoveSnake(Vector2Int.down);
							_dir = Vector2Int.down;
							yield return new WaitForSeconds(GameManager.Instance.gameSpeed);
							SnakePath(target);
							yield break;
						}
						i++;
						break;
			
					case 2:
						if (!snake.CheckIfCollide(snake.LimitMoveToGrid(snake.position + Vector2Int.left))){
							snake.MoveSnake(Vector2Int.left);
							_dir = Vector2Int.left;
							yield return new WaitForSeconds(GameManager.Instance.gameSpeed);
							SnakePath(target);
							yield break;
						}
						i++;
						break;
			
					case 3:
						if (!snake.CheckIfCollide(snake.LimitMoveToGrid(snake.position + Vector2Int.right))){
							snake.MoveSnake(Vector2Int.right);
							_dir = Vector2Int.right;
							yield return new WaitForSeconds(GameManager.Instance.gameSpeed);
							SnakePath(target);
							yield break;
						}
						i++;
						break;
				}
			}

			// Can't move trigger win
			GameManager.Instance.Win();
			yield break;
		}
		
		// First path is the tile that the snake stands on
		path.Dequeue();
		
		foreach (var grid in path){
			if (grid == null) yield break;

			while (GameManager.Instance.pause){
				yield return new WaitForEndOfFrame();
			}
			
			if(grid == GameManager.GameGrid.GetTile(snake.position)){
				SnakePath(target);
				yield break;
			}

			if (snake.child != null){
				if(grid == GameManager.GameGrid.GetTile(snake.child.position)){
					SnakePath(target);
					yield break;
				}
			}
			
			if (!grid.walkable){
				SnakePath(target);
				yield break;
			}

			_dir = grid.position - snake.position;
			snake.MoveSnakePos(grid.position);
			yield return new WaitForSeconds(GameManager.Instance.gameSpeed);
		}
	}
}
