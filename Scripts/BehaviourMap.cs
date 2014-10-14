using UnityEngine;
using System.Collections;
using Pathfinding;

public class BehaviourMap : MonoBehaviour {

	public Level lvl;


	public void InitPathfinding() {
		GridGraph graph = AstarPath.active.astarData.gridGraph; //First Grid Graph in the scene
		graph.width = lvl.size_x;
		graph.depth = lvl.size_y;
		graph.center = new Vector3(graph.width/2-0.5f,  graph.depth/2-0.5f, 0);
		//Update the size of the graph based on the width/depth values
		graph.UpdateSizeFromWidthDepth();
		//We want a callback when graphs are scanned
		AstarPath.OnPostScan += PostProcessGraph;
		AstarPath.active.Scan();
	}


	// TODO: Dont think this is actually needed
	void PostProcessGraph(AstarPath active) {
		AstarPath.OnPostScan -= PostProcessGraph;
		GridGraph graph = AstarPath.active.astarData.gridGraph; //First Grid Graph in the scene
		for (int x = 0; x < lvl.size_x; x++) {
			for (int y = 0; y < lvl.size_y; y++) {
				graph.nodes[y*graph.width + x].Walkable = lvl.isWalkable(x, y); //set walkability (true/false)
			}
		}
	}
}
