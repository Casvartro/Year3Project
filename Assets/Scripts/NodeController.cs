using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour, IComparable {

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
		
	public void resetNodeInfo(){
		totalNodeDistance = float.PositiveInfinity;
		parentNode = null;
	}

	public NodeController getNodeParent(){
		return parentNode;
	}

	public void setNodeParent(NodeController node){
		parentNode = node;
	}

	public float getTotalNodeDistance(){
		return totalNodeDistance;
	}

	public void setTotalNodeDistance(float distance){
		totalNodeDistance = distance;
	}

}
