using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour, IComparable {

	//Class responsible for information around a specific path node and its neighbors, and its distances. 
	public GameObject[] neighborNodes;
	public float[] neighborDistances;
	private NodeController parentNode;
	private float totalNodeDistance;

	void Start(){
		parentNode = null;
		totalNodeDistance = float.PositiveInfinity;
		neighborDistances = new float[neighborNodes.Length];
		for(int i = 0; i < neighborNodes.Length; i++){
			Vector3 positionDistance = this.transform.position - neighborNodes[i].transform.position;
			neighborDistances[i] = positionDistance.magnitude;
		}
	}

	//Compares the node distance between two nodes. Used for sort().
	public int CompareTo(object otherNode){
		NodeController otherNodeInfo = (NodeController)otherNode;
		if (this.totalNodeDistance < otherNodeInfo.getTotalNodeDistance ()) {
			return -1;
		}
		if (this.totalNodeDistance > otherNodeInfo.getTotalNodeDistance ()) {
			return 1;
		}
		return 0;
	}

	//Resets the nodes information at the end.
	public void resetNodeInfo(){
		totalNodeDistance = float.PositiveInfinity;
		parentNode = null;
	}

	//Returns the parent node that led to this path node.
	public NodeController getNodeParent(){
		return parentNode;
	}

	//Sets the parent node of the previous node that led to this.
	public void setNodeParent(NodeController node){
		parentNode = node;
	}

	//Returns the total distance taken to get to this node.
	public float getTotalNodeDistance(){
		return totalNodeDistance;
	}

	//Sets the total node distance taken to get to this node.
	public void setTotalNodeDistance(float distance){
		totalNodeDistance = distance;
	}

}
