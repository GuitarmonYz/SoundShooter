// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class sequenceEnemyOrbit : MonoBehaviour {
	public GameObject center;
	public float speed;
	void Update () {
		transform.RotateAround(center.transform.position, Vector3.forward, speed*Time.deltaTime);
	}
}
