using UnityEngine;
using System.Collections;

static public class Generation{
	static public int size_x = 20;
	static public int size_y = 15;

	static private TileData[,] lvl;
	static private LevelData data;

	static public LevelData GenerateLevel() {

		data = new LevelData(size_x, size_y);



		for (int xx=0; xx < size_x; xx++){
			for (int yy=0; yy < size_y; yy++){
				data.tiles[xx, yy] = new TileData(Level.TileType.Wall);
			}
		}

		MakeEntrance();
		MakeExit();

		//Randomly place squares of variable size to make a cave-like level
		int num_squares = 40;
		for (int i = 0; i < num_squares; i++){
			int square_x = Random.Range(0, size_x);
			int square_y = Random.Range(0, size_y);
			int size = Random.Range(3, 4);
			int square_w = Random.Range(-1, 1);
			int square_h = Random.Range(-1, 1);
			makeSquare(square_x, square_y, square_x + size + square_w, square_y + size + square_h);
		}

		//At the end we make all the outer tiles walls

		for (int xx=0; xx < size_x; xx++){
			data.tiles[xx, 0] = new TileData(Level.TileType.Wall);
			data.tiles[xx, size_y-1] = new TileData(Level.TileType.Wall);
		}
		for (int yy =0; yy < size_y; yy++){
			data.tiles[0, yy] = new TileData(Level.TileType.Wall);
			data.tiles[size_x-1, yy] = new TileData(Level.TileType.Wall);
		}

		return data;
	}


	static private void makeSquare(int x, int y, int x2, int y2){
		x = Mathf.Max(x, 0);
		y = Mathf.Max(y, 0);

		x2 = Mathf.Min(x2, size_x);
		y2 = Mathf.Min(y2, size_y);

		for (int xx = x; xx < x2; xx++){
			for (int yy = y; yy < y2; yy++){
				data.tiles[xx, yy] = new TileData(Level.TileType.Floor);
			}
		}
	}


	static private void MakeEntrance(){
		int x, y;

		x = Random.Range(0, size_x);
		y = Random.Range(0, size_y);


		data.entrance_x = x;
		data.entrance_y = y;

		//Make a square around start
		makeSquare(x - 2, y - 2, x + 2, x + 2);
	}


	static private void MakeExit(){;
		int x, y;

		x = Random.Range(0, size_x);
		y = Random.Range(0, size_y);

		data.exit_x = x;
		data.exit_y = y;

		//Make a square around start
		makeSquare(x, y, 2, 2);
	}
}
