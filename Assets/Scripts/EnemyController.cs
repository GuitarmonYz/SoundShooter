// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public GameObject SequenceEnemy;
	private GameObject firstSequencePos;
	private OscSender oscSender;
	public int speed = 1;
	private Vector3 targetPosition;
	private bulletController bulletController;
	private float[] xArray = new float[]{0,Mathf.Sqrt(2)/2,1,Mathf.Sqrt(2)/2,0,-Mathf.Sqrt(2)/2,-1,-Mathf.Sqrt(2)/2};
	private float[] yArray = new float[]{1,Mathf.Sqrt(2)/2,0,-Mathf.Sqrt(2)/2,-1,-Mathf.Sqrt(2)/2,0,Mathf.Sqrt(2)/2};
	// Use this for initialization
	void Start () {
		GameObject manager = GameObject.FindGameObjectWithTag("Manager");
		oscSender = manager.GetComponent<OscSender>();
		firstSequencePos = GameObject.FindGameObjectWithTag("FirstPos");
		int seed = Random.Range(0,7);
		targetPosition = new Vector3(xArray[seed],Random.Range(5,95),yArray[seed]);
	}
	
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		transform. position = Vector3. MoveTowards(transform. position, targetPosition, step);
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("enter");
		if (other.CompareTag("FancyBullet")){
			oscSender.decreaseEnemy();
			bulletController = other.gameObject.GetComponent<bulletController>();
			bulletController.getIdx();
			float posX = this.transform.position.x;
			float posZ = this.transform.position.z;
			int[] res = findNearest(posX,posZ,15);
			if (res[1] == 0) {
				oscSender.setRhythmTemplate(bulletController.getIdx(),res[0]/2+1);
				Instantiate(SequenceEnemy, getLocation(bulletController.getIdx(), 25f), Quaternion.identity);
			} else {
				oscSender.setMelodyTemplate(bulletController.getIdx(),res[0]);
				Instantiate(SequenceEnemy, getLocation(bulletController.getIdx(),12.5f), Quaternion.identity);
			}
			DestorySelf();
			Destroy(other.gameObject);
		}
	}

	Vector3 getLocation(int index, float radius){
		Vector3 firstSequence = firstSequencePos.transform.position;
		float rad = Mathf.Acos(firstSequence.x/radius);
		float degree = rad * Mathf.Rad2Deg;
		degree = degree + 45.0f * index;
		return new Vector3(radius*Mathf.Cos(degree*Mathf.Deg2Rad), radius*Mathf.Sin(degree*Mathf.Deg2Rad), 60f);
	}

	int[] findNearest(float posX, float posZ, float scale){
		float minDis = 10000;
		int res = -1;
		int type = 0;
		for(int i = 0; i < xArray.Length; i++){
			float distance = Mathf.Sqrt(Mathf.Pow((posX-xArray[i]*scale*1.5f),2)+Mathf.Pow((posZ - yArray[i]*scale),2));
			if (distance < minDis){
				minDis = distance;
				res = i;
			}
		}
		for(int i = 0; i < xArray.Length; i++){
			float distance = Mathf.Sqrt(Mathf.Pow((posX-xArray[i]*scale*2.5f),2)+Mathf.Pow((posZ - yArray[i]*scale*2),2));
			if (distance < minDis){
				minDis = distance;
				res = i;
				type = 1;
			}
		}
		return new int[]{res,type};
	}
	void DestorySelf(){
		//oscSender.setRhythmTemplate(randPosIdx, 0);
		Destroy(this.gameObject);
	}
}
