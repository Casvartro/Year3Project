using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder {

	//Class responsible for finding the path between the start and the end nodes.

	private static PriorityQueue NodeQueue; 
	private static List<NodeController> resetList;

	//Returns the most optimal path based on the nodes calculated and resets all the node's info at the end.
	private static ArrayList calculatePath(NodeController node){
		ArrayList pathList = new ArrayList ();
		while (node != null) {
			pathList.Add (node);
			node = node.getNodeParent ();
		}
		pathList.Reverse ();

		foreach(NodeController modNode in resetList){
			modNode.resetNodeInfo();
		}

		NodeQueue.RemoveAll ();
		resetList.Clear ();


		return pathList;
	}

	//This function is responsible for finding the most optimal path using the A* search method.
	//Compares node distances between nodes as well as their children and neighbors.
	public static ArrayList getPath(GameObject currentNode, GameObject targetNode){
		
		NodeQueue = new PriorityQueue ();
		resetList = new List<NodeController> ();

		NodeController currentNodeInfo = currentNode.GetComponent<NodeController> ();
		currentNodeInfo.setTotalNodeDistance (0.0f);
		resetList.Add (currentNodeInfo);
		NodeController targetNodeInfo = targetNode.GetComponent<NodeController> ();

		for (int i = 0; i < currentNodeInfo.neighborNodes.Length; i++) {
			NodeController neighborInfo = currentNodeInfo.neighborNodes [i].GetComponent<NodeController> ();
			neighborInfo.setTotalNodeDistance (currentNodeInfo.neighborDistances [i]);
			neighborInfo.setNodeParent (currentNodeInfo);
			NodeQueue.Push (neighborInfo);
			resetList.Add (neighborInfo);
		}

		NodeController nodeInfo = null;

		while (NodeQueue.Length != 0) {

			nodeInfo = NodeQueue.GetFirstNode ();
	
			if (nodeInfo == targetNodeInfo) {
				return calculatePath (nodeInfo);
			}

			float currentTotalDistance = nodeInfo.getTotalNodeDistance ();

			for (int i = 0; i < nodeInfo.neighborNodes.Length; i++) {
				
				NodeController neighborInfo = nodeInfo.neighborNodes [i].GetComponent<NodeController> ();
				float modifiedTotalDistance = nodeInfo.neighborDistances [i] + currentTotalDistance;
				float originalTotalDistance = neighborInfo.getTotalNodeDistance ();

				if (modifiedTotalDistance < originalTotalDistance) {
					neighborInfo.setTotalNodeDistance (modifiedTotalDistance);
					neighborInfo.setNodeParent (nodeInfo);
					if (originalTotalDistance == float.PositiveInfinity) {
						NodeQueue.Push (neighborInfo);
						resetList.Add (neighborInfo);
					} else {
						NodeQueue.sortQueue ();
					}
				}

			}

			NodeQueue.Remove (nodeInfo);
		}

		//if (nodeInfo != targetNodeInfo) {
		Debug.LogError ("Goal Not Found");
		return null;
		//}

		//return calculatePath (nodeInfo);

	}
		
	//Returns the closest node to the enemy, the start of their path.
	public static GameObject getInitialNode(GameObject[] pathNodes, Vector3 position){

		GameObject closestNode = null;
		float closestDistance = float.PositiveInfinity;

		foreach (GameObject node in pathNodes) {
			float currentDistance = Vector3.Distance (node.transform.position, position);
			if (currentDistance < closestDistance) {
				closestNode = node;
				closestDistance = currentDistance;
			}
		}
		return closestNode;
	}

	//Retrieves the end node of their path the destination.
	public static GameObject getEndNode(GameObject[] pathNodes, GameObject startNode){

		GameObject endNode = null;
		bool nodeFound = false;

		while (!nodeFound) {
			endNode = pathNodes [Random.Range (0, pathNodes.Length)];
			if (startNode != endNode) {
				nodeFound = true;
			}
		}

		return endNode;
	}
		
	public static GameObject getPlaneNode(IDictionary<string, List<GameObject>> planeNodes, Vector3 position, RaycastHit objectHit){

		List<GameObject> planeList = planeNodes [objectHit.collider.name];
		GameObject closestNode = null;
		float closestDistance = float.PositiveInfinity;

		foreach (GameObject node in planeList) {
			float currentDistance = Vector3.Distance (node.transform.position, position);
			if (currentDistance < closestDistance) {
				closestNode = node;
				closestDistance = currentDistance;
			}
		}

		return closestNode;

	}

	public static IDictionary<string, List<GameObject>> getPathNodePlanes(GameObject[] pathNodes){

		IDictionary<string, List<GameObject>> planeNodes = new Dictionary<string, List<GameObject>>();
		RaycastHit planeHit;

		foreach (GameObject node in pathNodes) {
			if (Physics.Raycast (node.transform.position, Vector3.down, out planeHit, 10.0f)) {
				if (!planeNodes.ContainsKey (planeHit.collider.name)) {
					planeNodes.Add (new KeyValuePair<string, List<GameObject>> (planeHit.collider.name, new List<GameObject>()));
				}
				planeNodes [planeHit.collider.name].Add (node);
			}
		}
			
		return planeNodes;
	
	}

	//Returns the closest node to the enemy, to retreat from that is also the furthest away from the player.
	public static GameObject getRetreatNode(GameObject[] pathNodes, Transform enemyPos, Vector3 playerPos){

		GameObject closestNode = null;
		float closestDistance = float.PositiveInfinity;
		RaycastHit enemyHit;
		RaycastHit nodeHit;
		float currentDistance;
		float playerDistance;
		Vector3 position = enemyPos.position;

		foreach (GameObject node in pathNodes) {
			
			if (Physics.Raycast (position, Vector3.down, out enemyHit, 10.0f) && 
				Physics.Raycast (node.transform.position, Vector3.down, out nodeHit, 10.0f)) {

				if ((enemyHit.collider.name == nodeHit.collider.name)) {
					
					currentDistance = Vector3.Distance (node.transform.position, position);
					playerDistance = Vector3.Distance (node.transform.position, playerPos);

					if ((currentDistance < closestDistance && currentDistance < playerDistance) && 
						isNodeBehind(node.transform.position, enemyPos)) {
						closestNode = node; 
						closestDistance = currentDistance;
					}
				}
			}
		}

		if (closestNode == null) {
			foreach (GameObject node in pathNodes) {
				currentDistance = Vector3.Distance (node.transform.position, position);
				playerDistance = Vector3.Distance (node.transform.position, playerPos);

				if ((currentDistance < closestDistance && currentDistance < playerDistance) && 
					isNodeBehind(node.transform.position, enemyPos)) {
					closestNode = node; 
					closestDistance = currentDistance;
				}
			}
		}

		return closestNode;
	}

	//Returns closest neighbor of the current node to retreat too.
	public static GameObject getRetreatNeighbor(GameObject currentNode, Transform enemyPos, Vector3 playerPos){

		NodeController currentNodeInfo = currentNode.GetComponent<NodeController> ();
		GameObject[] currentNeighbors = currentNodeInfo.neighborNodes;
		GameObject closestNode = null;
		float closestDistance = float.PositiveInfinity;
		Vector3 position = enemyPos.position;


		foreach (GameObject node in currentNeighbors) {
			float currentDistance = Vector3.Distance (node.transform.position, position);
			float playerDistance = Vector3.Distance (node.transform.position, playerPos);
			if (currentDistance < closestDistance && currentDistance < playerDistance) {
				closestNode = node; 
				closestDistance = currentDistance;
			}
		}

		return closestNode;
	}


	//Checks if the node is behind the enemy so it doesnt double back.
	public static bool isNodeBehind(Vector3 targetPos, Transform sourcePos){

		Vector3 toTarget = (targetPos - sourcePos.position).normalized;
		if (Vector3.Dot(toTarget, sourcePos.forward) > 0) {
			return false;
		} else {
			return true;
		}
	}

	//Checks if the enemy has reached its node destination.
	public static bool atDestination(Transform currentPosition, Vector3 destPosition){

		Vector3 direction = destPosition - currentPosition.position;
		direction.y = 0;
		if (direction.magnitude < .2f){
			return true;
		}

		return false;

	}

}
