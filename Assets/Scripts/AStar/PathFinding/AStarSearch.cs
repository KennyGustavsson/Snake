using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarSearch{
	public static Queue<GridTile> AStar(GridTile start, GridTile target, Vector2Int currentDirection){
        PriorityQueue<GridTile> frontier = new PriorityQueue<GridTile>();
        Dictionary<GridTile, GridTile> cameFrom = new Dictionary<GridTile, GridTile>();
        Dictionary<GridTile, float> costSoFar = new Dictionary<GridTile, float>();
        
        frontier.Enqueue(start, 0);
        cameFrom.Add(start, null);
        costSoFar.Add(start, 0);

        while (frontier.Count > 0){
            var current = frontier.Dequeue();

            if(current == target)
                break;

            int count = 0;
            
            if (currentDirection == Vector2Int.down ||
                currentDirection == Vector2Int.up ||
                currentDirection == Vector2Int.right ||
                currentDirection == Vector2Int.left){
                count = 3;
            }
            else{
                count = 4;
            }
            
            GridTile[] neighbors = new GridTile[count];
            
            Vector2Int up = LimitMoveToGrid(current.position + Vector2Int.up);
            Vector2Int right = LimitMoveToGrid(current.position + Vector2Int.right);
            Vector2Int down = LimitMoveToGrid(current.position + Vector2Int.down);
            Vector2Int left = LimitMoveToGrid(current.position + Vector2Int.left);

            int i = 0;
            if(currentDirection != Vector2Int.down){
                neighbors[i] = GameManager.GameGrid.grid[up.x, up.y];
                i++;
            }
            if(currentDirection != Vector2Int.left){
                neighbors[i] = GameManager.GameGrid.grid[right.x, right.y];
                i++;
            }
            if(currentDirection != Vector2Int.up){
                neighbors[i] = GameManager.GameGrid.grid[down.x, down.y];
                i++;
            }
            if(currentDirection != Vector2Int.right){
                neighbors[i] = GameManager.GameGrid.grid[left.x, left.y];
            }
            
            foreach (var next in neighbors){
                float newCost = costSoFar[current] + next.cost;
                
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]){
                    if(!next.walkable) continue;

                    // Cost so far
                    if (costSoFar.ContainsKey(next))
                        costSoFar[next] = newCost;
                    else
                        costSoFar.Add(next, newCost);
                    
                    // Priority
                    float priority = newCost + Heuristic.Distance(new Vector3(next.position.x, next.position.y), 
                        new Vector3(target.position.x, target.position.y));
                    frontier.Enqueue(next, priority);

                    // Came from
                    if (cameFrom.ContainsKey(next))
                        cameFrom[next] = current;
                    else
                        cameFrom.Add(next, current);
                }
            }
        }
        
        Queue<GridTile> path = new Queue<GridTile>();
        GridTile pathCurrent = target;
        path.Enqueue(pathCurrent);
        if (!cameFrom.ContainsKey(pathCurrent)){
            return null;
        }
        else{
            while (pathCurrent != start){
                pathCurrent = cameFrom[pathCurrent];
                path.Enqueue(pathCurrent);
            }
        }
        
        path = new Queue<GridTile>(path.Reverse());
        return path;
    }

    private static Vector2Int LimitMoveToGrid(Vector2Int pos){
        if (pos.x > GameManager.Instance.width - 1) pos.x = 0;
        if (pos.x < 0) pos.x = GameManager.Instance.width - 1;
        if (pos.y > GameManager.Instance.height - 1) pos.y = 0;
        if (pos.y < 0) pos.y = GameManager.Instance.height - 1;

        return pos;
    }
}
