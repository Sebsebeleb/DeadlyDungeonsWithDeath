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
	
	static private int _getNbAliveNeighbors(int[,] grid, int x, int y, int radius=1){
		int alive = 0;
		for (int dx = -radius; dx <= radius; dx++){
			for (int dy = -radius; dy <=radius; dy++){
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

	// http://www.roguebasin.com/index.php?title=Cellular_Automata_Method_for_Generating_Random_Cave-Like_Levels
	static public int[,] CaveSmoothen(int[,] grid) {
		for (int i = 0; i < 4; i++) {
			grid = _smooth1(grid);
		}
		for (int i = 0; i < 3; i++) {
			grid = _smooth2(grid);
		}
		return grid;
	}

	// Repeat 4: W'(p) = R1(p) >= 5 || R2(p) <= 2
	static private int[,] _smooth1(int[,] grid) {
		int w = grid.GetLength(0);
		int h = grid.GetLength(1);
		int[,] new_grid = new int[w, h];

		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				int alive = _getNbAliveNeighbors(grid, x, y);
				int alive2 = _getNbAliveNeighbors(grid, x, y, 2);
				if (alive >= 5 || alive2 <= 2) {
					new_grid[x, y] = 1;
				}
				else {
					new_grid[x, y] = 0;
				}
			}
		}

		return new_grid;

	}
	// Repeat 3: W'(p) = R1(p) >= 5
	static private int[,] _smooth2(int[,] grid) {
		int w = grid.GetLength(0);
		int h = grid.GetLength(1);
		int[,] new_grid = new int[w, h];

		for (int x = 0; x < w; x++) {
			for (int y = 0; y < h; y++) {
				int alive = _getNbAliveNeighbors(grid, x, y);
				int alive2 = _getNbAliveNeighbors(grid, x, y, 2);
				if (alive >= 5) {
					new_grid[x, y] = 1;
				}
				else {
					new_grid[x, y] = 0;
				}
			}
		}

		return new_grid;

	}
}
