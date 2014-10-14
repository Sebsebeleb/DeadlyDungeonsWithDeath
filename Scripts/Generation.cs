using UnityEngine;
using System.Collections;
using Pathfinding;

static public class Generation{
	static public int size_x = 20;
	static public int size_y = 20;

	static private TileType[,] lvl;
	static private LevelData data;
	static private GridGraph pathGrid;

	static public LevelData GenerateLevel() {

		data = new LevelData(size_x, size_y);

		for (int xx=0; xx < size_x; xx++){
			for (int yy=0; yy < size_y; yy++){
				data.tiles[xx, yy] = TileType.Wall;
			}
		}

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
			data.tiles[xx, 0] = TileType.Wall;
			data.tiles[xx, size_y-1] = TileType.Wall;
		}
		for (int yy =0; yy < size_y; yy++){
			data.tiles[0, yy] = TileType.Wall;
			data.tiles[size_x-1, yy] = TileType.Wall;
		}

		MakeExit();

		return data;
	}

	//Automata caves
	static public LevelData GenerateCaves(){

		data = new LevelData(size_x, size_y);

		int[,] noise = new int[size_x, size_y];
		//MAKE SOME NOISE!
		for (int x = 0; x < size_x; x++){
			for (int y = 0; y < size_y; y++){
				float c = Random.Range(0.0f, 1.0f);
				float chance = 0.40f;
				if (c < chance){
					noise[x, y] = 1;
				}
				else{
					noise[x, y] = 0;
				}
			}
		}
		for (int i=0; i<5; i++){
			 noise = CellularAutomation.CaveSmoothen(noise);
		}

		//Convert from noise to tiles
		for (int x = 0; x < size_x; x++){
			for (int y = 0; y < size_y; y++){
				if (noise[x, y] == 1){
					data.tiles[x, y] = TileType.Wall;
				}
				else{
					data.tiles[x, y] = TileType.Floor;
				}
			}
		}

		//Walls on edges
		for (int x = 0; x < size_x; x++) {
			data.tiles[x, 0] = TileType.Wall;
			data.tiles[x, size_y-1] = TileType.Wall;
		}
		for (int y = 0; y < size_y; y++) {
			data.tiles[0, y] = TileType.Wall;
			data.tiles[size_x-1, y] = TileType.Wall;
		}

		//Find entrance and exit
		int tries = 10000;
		bool found = false;
		while (tries > 0 && !found){
			int x = Random.Range(0, size_x);
			int y = Random.Range(0, size_y);

			if (data.tiles[x, y] != TileType.Wall) {
				found = true;
				MakeEntrance(x, y);
			}
			tries--;
		}


		
		// Exit
		tries = 10000;
		found = false;
		while (tries > 0 && !found){
			int x = Random.Range(0, size_x);
			int y = Random.Range(0, size_y);

			if (data.tiles[x, y] != TileType.Wall && (x != data.entrance_x) && (y != data.entrance_y)) {
				found = true;
				data.exit_x = x;
				data.exit_y = y;
			}

			// Check if exit is far enough away from entrance
			tries--;
		}

		// If not completable, retry
		Debug.Log("Entrance is at: " + data.entrance_x + ", " + data.entrance_y);
		Debug.Log("Exit is at: " + data.exit_x + ", " + data.exit_y);
		IsCompletable();

		MakeItems(5);

		return data;
	}

	static private void makeSquare(int x, int y, int x2, int y2){
		x = Mathf.Max(x, 0);
		y = Mathf.Max(y, 0);

		x2 = Mathf.Min(x2, size_x);
		y2 = Mathf.Min(y2, size_y);

		for (int xx = x; xx < x2; xx++){
			for (int yy = y; yy < y2; yy++){
				data.tiles[xx, yy] = TileType.Floor;
			}
		}
	}

	static private void MakeItems(int num_items) {
		int num_generated = 0;
		int tries = 10000;

		while ((tries > 0) && (num_generated < num_items)) {
			int x, y;
			x = Random.Range(0, size_x);
			y = Random.Range(0, size_y);

			if (data.tiles[x, y] == TileType.Floor) {
				data.items[x, y] = true;
				num_generated++;
			}
			tries--;
		}
	}

	// Must be called before attempting to use pathfinding
	static private void UpdatePathfinding() {
		Debug.Log("Starting update");
		GridGraph pathGrid = AstarPath.active.astarData.gridGraph;
		for (int x = 0; x < size_x;x++){
			for (int y = 0; y < size_y;y++ ) {
				pathGrid.nodes[y * pathGrid.width + x].Walkable = (data.tiles[x, y] == TileType.Floor);
			}
		}
		//Vector3 center = new Vector3(size_x/2, size_y/2);
		Vector3 center = new Vector3(0, 0, 0);

		Vector3 size = new Vector3(size_x/2, size_y/2);
		Debug.Log("Actual update");
		AstarPath.active.UpdateGraphs(new Bounds(center, size));
		Debug.Log("Finished");
	}

	// Can you reach the exit from the entrance?
	static private bool IsCompletable(){
		UpdatePathfinding();
		//Update the pathfinding grid if it isnt already

		ABPath p = Pathfinding.PathPool<ABPath>.GetPath();
		p.startPoint = new Vector3(data.entrance_x, 0.0f, data.entrance_y);
		p.endPoint = new Vector3(data.exit_x, 0.0f, data.exit_y);
		AstarPath.StartPath(p);
		AstarPath.WaitForPath(p);


		Debug.Log(p);
		Debug.Log(p.error);
		Debug.Log(p.IsDone());
		Debug.Log(p.path);

		Debug.Log("Start is: " + p.startPoint);
		Debug.Log("End is: " + p.endPoint);
		Debug.Log(p.startNode.position);
		foreach (GraphNode node in p.path) {
			Debug.Log(node.position);
		}

		return true;
		

	}

	static private void MakeEntrance(int x, int y){

		data.entrance_x = x;
		data.entrance_y = y;

		//Make a square around start
		//makeSquare(x - 2, y - 2, x + 2, y + 2);
	}


	static private void MakeExit(){;
		int x, y;

		x = Random.Range(0, size_x);
		y = Random.Range(0, size_y);

		data.exit_x = x;
		data.exit_y = y;

		//Make a square around start
		//makeSquare(x, y, x+2, y+2);

		data.tiles[x, y] = TileType.Downstairs;
	}
}
