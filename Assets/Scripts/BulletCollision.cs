using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		Destroy (gameObject);
	}

}
