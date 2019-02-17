using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue {

	private ArrayList nodes = new ArrayList();

	public int Length {
		get { return nodes.Count; }
	}

	public bool Contains(object node) {
		return nodes.Contains(node);
	}

	public NodeController GetFirstNode() {
		if (nodes.Count > 0) {
			return (NodeController)nodes[0];
		}
		return null;
	}

	public void Push(NodeController node) {
		nodes.Add(node);
		nodes.Sort();
	}

	public void Remove(NodeController node) {
		nodes.Remove(node);
		nodes.Sort();
	}

	public void RemoveAll(){
		nodes.Clear ();
	}

	public void sortQueue(){
		nodes.Sort ();
	}
}