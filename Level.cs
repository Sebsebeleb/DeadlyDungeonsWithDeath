using UnityEngine;
using System.Collections;


public class Level : MonoBehaviour {

	public enum TileType{Floor, Wall, Downstairs};

	public int size_x = 20;
	public int size_y = 20;

	// Player reference
	public GameObject player;

	private GameObject[,] actors;
	private GameObject[,] ground;
	private GameObject[,] walls;
	private GameObject[,] weapons;
	private GameObject[,] items;

	public GameObject PrefabWall;
	public GameObject PrefabFloor;
	public GameObject PrefabDownstairs;

	public GameObject[] PrefabEnemies;

	// Reference to the gameobject holdign tiles
	private GameObject tileMap;
	private PlayerMovement pmove;
	private LevelData data;


	void Awake() {
		player = GameObject.FindWithTag("Player");
		tileMap = GameObject.FindWithTag("Tilemap");
		pmove = player.GetComponent<PlayerMovement>();
	}

	// Use this for initialization
	void Start () {

		MakeLevel();
		SetupPlayer();
		MakeEnemies();

	}

	void SetupPlayer(){

		int x, y;
		x = data.entrance_x;
		y = data.entrance_y;

		pmove.ForceMove(x, y);

		actors[pmove.lx, pmove.ly] = player;

	}

	public void MakeLevel(){
		// Initialize data containers
		ground = new GameObject[size_x, size_y];
		items = new GameObject[size_x, size_y];
		actors = new GameObject[size_x, size_y];
		walls = new GameObject[size_x, size_y];
		weapons = new GameObject[size_x, size_y];

		//data = Generation.GenerateLevel();

		data = Generation.GenerateCaves();

		for (int xx=0; xx<data.tiles.GetLength(0); xx++){
			for (int yy=0; yy<data.tiles.GetLength(1); yy++){
				GameObject new_tile;

				Level.TileType typ = data.tiles[xx, yy].tile_type;
				switch (typ){
					case Level.TileType.Wall:
						new_tile = Instantiate(PrefabWall, new Vector3(xx, yy, 0.0f), Quaternion.identity) as GameObject;
						break;
					case Level.TileType.Floor:
						new_tile = Instantiate(PrefabFloor, new Vector3(xx, yy, 0.0f), Quaternion.identity) as GameObject;
						break;
					case Level.TileType.Downstairs:
						new_tile = Instantiate(PrefabDownstairs, new Vector3(xx, yy, 0.0f), Quaternion.identity) as GameObject;
						break;
					default:
						new_tile = Instantiate(PrefabFloor, new Vector3(xx, yy, 0.0f), Quaternion.identity) as GameObject;
						break;
				}

				//Fog of war
				new_tile.renderer.enabled = false;
				new_tile.renderer.sortingOrder = -yy;
				new_tile.transform.parent = tileMap.transform;
				SetTile(typ, new_tile, xx, yy);
			}
		}
	}

	public void DeleteLevel(){
		//First we remove the player from the map references
		actors[pmove.lx, pmove.ly] = null;
		if (actors == null){ return; }

		foreach (GameObject actor in actors){
			if (actor != player){
				Destroy(actor);
			}
		}
		foreach (GameObject floor in ground){
			Destroy(floor);
		}
		foreach (GameObject weapon in weapons){
			Destroy(weapon);
		}
		foreach (GameObject wall in walls){
			Destroy(wall);
		}
	}

	//Temporary? should be taken care of by the generation stuff
	void MakeEnemies(){
		for (int i=0; i < 10; i++){
			int x = Random.Range(0, size_x-1);
			int y = Random.Range(0, size_y-1);

			int c = Random.Range(0, PrefabEnemies.GetLength(0));
			GameObject prefab = PrefabEnemies[c];
			GameObject s = Spawn(prefab, x, y);
		}
	}

	void SetTile(Level.TileType flag, GameObject tile, int x, int y){
		switch (flag){
			case Level.TileType.Wall:
				walls[x, y] = tile;
				break;
			case Level.TileType.Floor:
				ground[x, y] = tile;
				break;
			case Level.TileType.Downstairs:
				ground[x, y] = tile;
				break;
		}
	}

	//Spawn an actor, return true if spawned or false if not
	public GameObject Spawn(GameObject go, int x, int y){
		//TODO: Check if it can be spawned here
		GameObject actor = Instantiate(go, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
		BehaviourMovement actor_move = actor.gameObject.GetComponent<BehaviourMovement>();
		actor_move.lx = x;
		actor_move.ly = y;
		actors[x, y] = actor;

		actor.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

		actor.renderer.enabled = false;

		return actor;
	}

	public bool _canMove(GameObject mover, ActorType mover_type, int orig_x, int orig_y, int x, int y){
		// Check out of bounds
		if (x < 0 || x > size_x || y < 0 || y > size_y){
			return false;
		}

		if (mover_type == ActorType.WEAPON){
			return true;
		}

		//Check if the space is occupied or impassable
		//TODO: Check flags etc.
		GameObject floor = ground[x, y];
		GameObject wall = walls[x, y];
		GameObject other_actor = actors[x, y];
		GameObject weapon = weapons[x, y];

		if (wall || other_actor || (weapon && !weapon.transform.IsChildOf(mover.transform))) {
			return false;
		}

		return true;
	}

	// Try to move an actor, return True if successful or False.
	public bool MoveActor(GameObject mover, ActorType mover_type, int orig_x, int orig_y, int x, int y){

		if (!_canMove(mover, mover_type, orig_x, orig_y, x, y)){
			return false;
		}


		//Update our reference of the thing moving.
		//TODO: Flags for what type that wants to move
		switch(mover_type){
			case ActorType.ACTOR:
				actors[orig_x, orig_y] = null;
				actors[x, y] = mover;

				//Floor events
				ground[x, y].BroadcastMessage("OnSteppedUpon", mover, SendMessageOptions.DontRequireReceiver);
				break;
			case ActorType.WEAPON:
				weapons[orig_x, orig_y] = null;
				weapons[x, y] = mover;
				break;
			//TODO: Handle Default somehow..?

		}



		return true;
	}

	public GameObject getAt(EntityType typ, int x, int y){
		if (isOutOfBounds(x, y)){
			return null;
		}
		switch(typ){
			case EntityType.ACTOR:
				return actors[x, y];
			case EntityType.WEAPON:
				return weapons[x, y];
			case EntityType.FLOOR:
				return ground[x, y];
			case EntityType.WALL:
				return walls[x, y];
			default:
				return null;
		}
	}

	public bool isOutOfBounds(int x, int y){
		if (x < 0 || x > size_x-1 || y < 0 || y > size_y-1){
			return true;
		}
		return false;
	}

	public void NextLevel(BehaviourDownstairs stairs){
		DeleteLevel();
		//TODO: Pass in the stairs type for generation data
		MakeLevel();
	}
}

