using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	private OscSender oscSender;
	public int speed = 1;
	private int randPosIdx;
	private Transform playerPos;
	// Use this for initialization
	void Start () {
		GameObject manager = GameObject.FindGameObjectWithTag("Manager");
		oscSender = manager.GetComponent<OscSender>();
		GameObject player = GameObject.FindGameObjectWithTag("MyPlayer");
		playerPos = player.transform;
		findPosition();
	}
	
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		transform. position = Vector3. MoveTowards(transform. position, playerPos.position, step);
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("enter");
		if (other.CompareTag("FancyBullet")){
			oscSender.decreaseEnemy();
			DestorySelf();
			Destroy(other.gameObject);
		}
	}

	void findPosition(){
		ArrayList unusedposition = new ArrayList();
		int[] rhythmTemplate = oscSender.getRhythmTemplate();
		for (int i = 0; i < rhythmTemplate.Length; i++){
			if (rhythmTemplate[i] == 0) unusedposition.Add(i);
		}
		int randPos = Random.Range(0,unusedposition.Count);
		randPosIdx = (int)unusedposition[randPos];
		int randValue = Random.Range(1,5);
		oscSender.setRhythmTemplate(randPosIdx, randValue);
	}
	void DestorySelf(){
		oscSender.setRhythmTemplate(randPosIdx, 0);
		Destroy(this.gameObject);
	}
}
