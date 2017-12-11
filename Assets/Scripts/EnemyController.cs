using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	private OscSender oscSender;
	public int speed = 1;
	private int randPosIdx;
	private Transform playerPos;
	private bulletController bulletController;
	private float[] xArray = new float[]{0,Mathf.Sqrt(2)/2,1,Mathf.Sqrt(2)/2,0,-Mathf.Sqrt(2)/2,-1,-Mathf.Sqrt(2)/2};
	private float[] yArray = new float[]{1,Mathf.Sqrt(2)/2,0,-Mathf.Sqrt(2)/2,-1,-Mathf.Sqrt(2)/2,0,Mathf.Sqrt(2)/2};
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
			bulletController = other.gameObject.GetComponent<bulletController>();
			bulletController.getIdx();
			float posX = this.transform.position.x;
			float posZ = this.transform.position.z;
			int[] res = findNearest(posX,posZ,15);
			if (res[1] == 0) {
				oscSender.setRhythmTemplate(bulletController.getIdx(),res[0]);
			} else {
				oscSender.setMelodyTemplate(bulletController.getIdx(),res[0]);
			}
			Destroy(other.gameObject);
		}
	}

	int[] findNearest(float posX, float posZ, float scale){
		float minDis = 10000;
		int res = -1;
		int type = 0;
		for(int i = 0; i < xArray.Length; i++){
			float distance = Mathf.Sqrt(Mathf.Pow((posX-xArray[i]*scale),2)+Mathf.Pow((posZ - yArray[i]*scale),2));
			if (distance < minDis){
				minDis = distance;
				res = i;
			}
		}
		for(int i = 0; i < xArray.Length; i++){
			float distance = Mathf.Sqrt(Mathf.Pow((posX-xArray[i]*scale*2),2)+Mathf.Pow((posZ - yArray[i]*scale*2),2));
			if (distance < minDis){
				minDis = distance;
				res = i;
				type = 1;
			}
		}
		return new int[]{res,type};
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
