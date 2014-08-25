using UnityEngine;
using System.Collections;

public class CellularAutomation{

	static private bool _getAliveAt(int[,] grid, int x, int y){
		if (x < 0 || x > grid.GetLength(0)-1){
			return true;
		}
		if (y < 0 || y > grid.GetLength(1)-1){
			return true;
		}
		if (grid[x, y] == 1){
			return true;
		}

		return false;
	}
	static private int _getNbAliveNeighbors(int[,] grid, int x, int y){
		int alive = 0;
		for (int dx = -1; dx <= 1; dx++){
			for (int dy = -1; dy <= 1; dy++){
				if (_getAliveAt(grid, x+dx, y+dy)){
					alive++;
				}
			}
		}

		return alive;
	}


	//B5678S345678, out of bounds is considered alive
	static public int[,] PerformB5S3(int[,] grid){
		int w = grid.GetLength(0);
		int h = grid.GetLength(1);
		int[,] new_grid = new int[w, h];

		for (int x = 0; x < w; x++){
			for (int y = 0; y < h; y++){
				int alive = _getNbAliveNeighbors(grid, x, y);
				//Born + survive
				if (alive > 5){
					new_grid[x, y] = 1;
				}
				//Survive
				else if (alive > 3){
					new_grid[x, y] = grid[x, y];
				}
				//Decay
				else{
					new_grid[x, y] = 0;
				}
			}
		}

		return new_grid;
	}
}
