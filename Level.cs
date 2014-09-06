using UnityEngine;
using System.Collections;


public class Level : MonoBehaviour {


	public int size_x = 20;
	public int size_y = 20;

	// Player reference
	public GameObject player;


	private TileData[,] levelData;
	private GameObject[,] items;

	public GameObject PrefabWall;
	public GameObject PrefabFloor;
	public GameObject PrefabDownstairs;

	public GameObject[] PrefabEnemies;

	// Reference to the gameobject holdign tiles
	private GameObject tileMap;
	private BehaviourMovement pmove;
	private LevelData data;


	void Awake() {
		player = GameObject.FindWithTag("Player");
		tileMap = GameObject.FindWithTag("Tilemap");
		pmove = player.GetComponent<BehaviourMovement>();
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

		setAt(EntityType.ACTOR, pmove.lx, pmove.ly, player);

	}

	public void MakeLevel(){
		// Initialize data containers
		levelData = new TileData[size_x, size_y];

		for (int x=0; x<size_x; x++){
			for (int y=0; y<size_y; y++){
				levelData[x, y] = new TileData();
			}
		}

		//data = Generation.GenerateLevel();

		data = Generation.GenerateCaves();

		for (int xx=0; xx<data.tiles.GetLength(0); xx++){
			for (int yy=0; yy<data.tiles.GetLength(1); yy++){
				GameObject new_tile;

				TileType typ = data.tiles[xx, yy];
				switch (typ){
					case TileType.Wall:
						new_tile = Instantiate(PrefabWall, new Vector3(xx, yy, 0.0f), Quaternion.identity) as GameObject;
						break;
					case TileType.Floor:
						new_tile = Instantiate(PrefabFloor, new Vector3(xx, yy, 0.0f), Quaternion.identity) as GameObject;
						break;
					case TileType.Downstairs:
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
        //Generate downstairs
        GameObject down = Instantiate(PrefabDownstairs, new Vector3(data.exit_x, data.exit_y, 0.0f), Quaternion.identity) as GameObject;
        SetTile(TileType.Downstairs, down, data.exit_x, data.exit_y);
        down.renderer.enabled = false;
        down.renderer.sortingOrder = -data.exit_y;
        down.transform.parent = tileMap.transform;
	}

	public void DeleteLevel(){
		//Dirty check if we even can delete?
		if (levelData == null){
			return;
		}

		//First we remove the player from the map references
		for (int x = 0; x<size_x; x++){
			for (int y = 0; y<size_y; y++){
				//TODO: dont remove player etc.
				TileData tile = levelData[x, y];
                if (tile.actor != null && tile.actor.tag != "Player")
                {
                    Destroy(tile.actor);
                }
				Destroy(tile.floor);
				Destroy(tile.wall);
				foreach (GameObject wep in tile.weapons){
					Destroy(wep);
				}
			}
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

	void SetTile(TileType flag, GameObject tile, int x, int y){
		switch (flag){
			case TileType.Wall:
				levelData[x, y].wall = tile;
				break;
			case TileType.Floor:
				levelData[x, y].floor = tile;
				break;
			case TileType.Downstairs:
				levelData[x, y].floor = tile;
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
		levelData[x, y].actor = actor;

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
		TileData tile = levelData[x, y];

		//Check if weapons block
		foreach (GameObject wep in tile.weapons){
			// FIXME: This being needed appears to not be correct. Implement destruction of objects on levelData on destroyed stuff?
			if (wep == null || wep.transform.IsChildOf(mover.transform)){
				continue;
			}

			if (!wep.transform.IsChildOf(mover.transform)){
				return false;
			}

		}
		if (tile.wall || tile.actor) {
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

		TileData tile = levelData[x, y];
		TileData old_tile = levelData[orig_x, orig_y];
		switch(mover_type){
			case ActorType.ACTOR:
				old_tile.actor = null;
				tile.actor = mover;

				//Floor events
				if (tile.floor != null){
					tile.floor.BroadcastMessage("OnSteppedUpon", mover, SendMessageOptions.DontRequireReceiver);
				}
				break;
			case ActorType.WEAPON:
				old_tile.weapons.Remove(mover);
				tile.weapons.Add(mover);
				break;
			//TODO: Handle Default somehow..?

		}

		return true;
	}

	public TileData getAt(int x, int y){
		if (isOutOfBounds(x, y)){
			return null;
		}

		return levelData[x, y];
	}

	public void setAt(EntityType typ, int x, int y, GameObject to){
		TileData tile = getAt(x, y);
		switch (typ){
			case EntityType.ACTOR:
				tile.actor = to;
				break;
			case EntityType.WEAPON:
				tile.weapons.Add(to);
				break;
			case EntityType.FLOOR:
				tile.floor = to;
				break;
			case EntityType.WALL:
				tile.wall = to;
				break;
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