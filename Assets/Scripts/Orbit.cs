// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour {
	public GameObject center;
	public float speed;
	// Update is called once per frame
	void Update () {
		transform.RotateAround(center.transform.position, Vector3.right, speed*Time.deltaTime);
	}
}
