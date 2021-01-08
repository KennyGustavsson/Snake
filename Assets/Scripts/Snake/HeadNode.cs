using System;
using UnityEngine;

public class HeadNode{
	public TailNode child = null;
	public Vector2Int position = Vector2Int.zero;
	public Color snakeColor = Color.white;
	public Color tileColor = Color.white;

	public int score = 0;
	
	public HeadNode(Vector2Int position, Color snakeColor, Color tileColor){
		if (CheckIfCollide(position)){
			throw new InvalidOperationException("Snake spawned on a snake");
		}
		
		this.snakeColor = snakeColor;
		this.tileColor = tileColor;
		this.position = position;
	}

	public void AddTail(){
		if (child == null){
			child = new TailNode(position);
			return;
		}

		TailNode current = child;
		while (current.child != null){
			current = current.child;
		}

		current.child = new TailNode(current.position);
	}

	public void MoveSnake(Vector2Int direction){
		Vector2Int move = position + direction;
		move = LimitMoveToGrid(move);
		Vector2Int oldPos = position;

		if (CheckIfCollide(move)){
			GameManager.Instance.GameOver();
		}
		
		GameManager.GameGrid.MoveToTile(position, move, snakeColor, tileColor);
		position = move;
		
		CheckIfPoint();
		MoveTail(oldPos, child);
	}

	public void MoveSnakePos(Vector2Int position){
		Vector2Int move = position;
		move = LimitMoveToGrid(move);
		Vector2Int oldPos = this.position;

		if (CheckIfCollide(move)){
			GameManager.Instance.GameOver();
		}
		
		GameManager.GameGrid.MoveToTile(this.position, move, snakeColor, tileColor);
		this.position = move;

		CheckIfPoint();
		MoveTail(oldPos, child);
	}
	
	private void MoveTail(Vector2Int pos, TailNode tailNode){
		if(tailNode == null) return;
		Vector2Int oldPos = tailNode.position;
		
		pos = LimitMoveToGrid(pos);
		if(CheckIfCollide(pos)) return;
		
		GameManager.GameGrid.MoveToTile(tailNode.position, pos, snakeColor, tileColor);
		tailNode.position = pos;
		
		MoveTail(oldPos, tailNode.child);
	}

	public Vector2Int LimitMoveToGrid(Vector2Int pos){
		if (pos.x > GameManager.Instance.width - 1){
			pos.x = 0;
		}

		if (pos.x < 0){
			pos.x = GameManager.Instance.width - 1;
		}

		if (pos.y > GameManager.Instance.height - 1){
			pos.y = 0;
		}

		if (pos.y < 0){
			pos.y = GameManager.Instance.height - 1;
		}

		return pos;
	}
	
	public bool CheckIfCollide(Vector2Int pos){
		return !GameManager.GameGrid.grid[pos.x, pos.y].walkable;
	}

	private void CheckIfPoint(){
		if (!GameManager.GameGrid.GetTile(position).point) return;
		
		GameManager.GameGrid.GetTile(position).point = false;
		AddTail();
		score++;
		
		GameManager.GameGrid.GeneratePoint();
	}
}