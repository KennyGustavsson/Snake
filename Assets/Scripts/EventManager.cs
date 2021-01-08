using System;

public static class EventManager{
	public static event Action<GridTile> OnGeneratedPoint;
	public static void ONGeneratedPoint(GridTile tile){
		OnGeneratedPoint?.Invoke(tile);
	}
}
