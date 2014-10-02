using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
	public GameObject[] PrefabItems;
	private List<GameObject> ActingEntities = new List<GameObject>(); // All entities that should recieve onTurn events

	// Reference to the gameobject holdign tiles
	private GameObject tileMap;
	private BehaviourMovement pmove;
	private LevelData data;
	private GameObject world;
	private PlayerFOV FOV;

	private BehaviourMap Pathing;


	void Awake() {
		player = GameObject.FindWithTag("Player");
		tileMap = GameObject.FindWithTag("Tilemap");
		pmove = player.GetComponent<BehaviourMovement>();
		world = GameObject.Find("World");
		FOV = player.GetComponent<PlayerFOV>();
		Pathing = GetComponent<BehaviourMap>();
	}

	// Use this for initialization
	void Start () {
		MakeLevel();
	}

	public List<GameObject> GetActingEntities() {
		return ActingEntities;
	}

	void SetupPlayer(){

		int x, y;
		x = data.entrance_x;
		y = data.entrance_y;

		pmove.ForceMove(x, y);
		if (!ActingEntities.Contains(player)) {
			ActingEntities.Add(player);
		}

		//setAt(EntityType.ACTOR, pmove.lx, pmove.ly, player);

		TileData tile = getAt(x, y);

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
		Pathing.InitPathfinding();

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

		SetupPlayer();
		MakeEnemies();
		MakeItems();
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
				if (tile.item != null) {
					Destroy(tile.item);
				}
				foreach (GameObject wep in tile.weapons){
					if (wep != null && wep.transform.IsChildOf(player.transform)) {
						continue;
					}
					Destroy(wep);
				}
			}
		}
	}

	//Temporary? should be taken care of by the generation stuff
	void MakeEnemies(){
		int tries = 10000;
		int enemiesLeft = 10;
		while (tries > 0 && enemiesLeft > 0) {

			int x = Random.Range(0, size_x-1);
			int y = Random.Range(0, size_y-1);

			if (_canSpawn(EntityType.ACTOR, x, y)){
				int c = Random.Range(0, PrefabEnemies.GetLength(0));
				GameObject prefab = PrefabEnemies[c];
				GameObject s = Spawn(prefab, x, y);
				enemiesLeft--;
			}

			tries--;
		}
	}

	private bool _canSpawn(EntityType entityType, int x, int y) {
		switch (entityType) {
			case (EntityType.ACTOR):
				TileData t = getAt(x, y);
				if (t.wall != null || t.actor != null)
					return false;
				else
					return true;
			default:
				return false;

		}
	}

	void MakeItems() {
		for (int xx = 0; xx < data.tiles.GetLength(0); xx++) {
			for (int yy = 0; yy < data.tiles.GetLength(1); yy++) {
				bool is_item = data.items[xx, yy];
				if (is_item) {
					int c = Random.Range(0, PrefabItems.GetLength(0));
					GameObject item_prefab = PrefabItems[c];
					GameObject i = SpawnItem(item_prefab, xx, yy);
				}
			}
		}
	}

	void SetTile(TileType flag, GameObject tile, int x, int y){
		TileData td = levelData[x, y];

		switch (flag){
			case TileType.Wall:
				if (td.wall != null)
					Destroy(td.wall);

				levelData[x, y].wall = tile;
				break;
			case TileType.Floor:
				if (td.wall != null)
					Destroy(td.floor);

				levelData[x, y].floor = tile;
				break;
			case TileType.Downstairs:
				if (td.wall != null)
					Destroy(td.floor);

				levelData[x, y].floor = tile;
				break;
		}
	}	

	// Add a ground effect, returns true if succesfully placed
	bool addOnGround(GameObject effect, int x, int y) {
		TileData tile = getAt(x, y);
		if (tile == null) {
			return false;
		}
		tile.on_floor = effect;
		return true;
	}

	public GameObject SpawnItem(GameObject item, int x, int y) {
		GameObject it = Instantiate(item, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
		it.renderer.enabled = false;
		levelData[x, y].item = it;

		return it;
	}

	//Spawn an actor/weapon, return true if spawned or false if not
	public GameObject Spawn(GameObject go, int x, int y){
		//TODO: Check if it can be spawned here
		GameObject actor = Instantiate(go, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
		BehaviourMovement actor_move = actor.gameObject.GetComponent<BehaviourMovement>();
		actor_move.ForceMove(x, y);
		switch (actor_move.fAct) {
			case ActorType.ACTOR:
				levelData[x, y].actor = actor;
				break;
			case ActorType.WEAPON:
			case ActorType.PROJECTILE:
				setAt(EntityType.WEAPON, x, y, actor);
				break;
		}


		if (!actor_move.isAnchored) {
			actor.transform.parent = world.transform;
		}
		actor.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

		if (!FOV.CanSee(x, y)) {
			actor.renderer.enabled = false;
		}
		ActingEntities.Add(actor);

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
		if (mover_type == ActorType.ACTOR) {
			foreach (GameObject wep in tile.weapons) {
				// FIXME: This being needed appears to not be correct. Implement destruction of objects on levelData on destroyed stuff?
				if (wep == null || wep.transform.IsChildOf(mover.transform)) {
					continue;
				}

				BehaviourMovement wepMove = wep.GetComponent<BehaviourMovement>();
				if (wepMove != null && wepMove.fAct == ActorType.PROJECTILE) {
					continue;
				}

				if (!wep.transform.IsChildOf(mover.transform)) {
					return false;
				}
			}
		}
		if (tile.wall || tile.actor) {
			return false;
		}

		return true;
	}

	// Try to move an actor, return True if successful.
	public bool MoveActor(GameObject mover, ActorType mover_type, int orig_x, int orig_y, int x, int y){
		if (!_canMove(mover, mover_type, orig_x, orig_y, x, y)){
			return false;
		}

		BehaviourMovement actorMovement = mover.GetComponent<BehaviourMovement>();
		if (actorMovement != null) {
			actorMovement.SetPos(x, y);
		}

		//Update our reference of the thing moving.
		//TODO: Flags for what type that wants to move

		TileData tile = levelData[x, y];
		if (tile == null) {
			if (mover_type == ActorType.ACTOR || mover_type == ActorType.PROJECTILE) {
				return false;
			}
			else if (mover_type == ActorType.WEAPON) {
				return true;
			}
		}
		TileData old_tile = levelData[orig_x, orig_y];
		switch(mover_type){
			case ActorType.ACTOR:
				old_tile.actor = null;
				tile.actor = mover;

				//Floor and item events
				if (tile.floor != null){
					tile.floor.BroadcastMessage("OnSteppedUpon", mover, SendMessageOptions.DontRequireReceiver);
				}
				if (tile.item != null) {
					tile.item.BroadcastMessage("OnSteppedUpon", mover, SendMessageOptions.DontRequireReceiver);
				}
				if (tile.on_floor != null) {
					tile.on_floor.BroadcastMessage("OnSteppedUpon", mover, SendMessageOptions.DontRequireReceiver);
				}

				break;
			case ActorType.PROJECTILE:
			case ActorType.WEAPON:
				old_tile.weapons.Remove(mover);
				tile.weapons.Add(mover);

				if (tile.on_floor != null) {
					tile.on_floor.BroadcastMessage("OnSteppedUpon", mover, SendMessageOptions.DontRequireReceiver);
				}

				break;
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

	public bool isWalkable(int x, int y) {
		TileData tile = getAt(x, y);
		if (tile.wall) {
			return false;
		}
		return true;
	}

	public void NextLevel(BehaviourDownstairs stairs){
		DeleteLevel();
		MakeLevel();
	}
}