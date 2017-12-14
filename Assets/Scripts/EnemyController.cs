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
	void Start () {
		GameObject manager = GameObject.FindGameObjectWithTag("Manager");
		oscSender = manager.GetComponent<OscSender>();
		firstSequencePos = GameObject.FindGameObjectWithTag("FirstPos");
		int seed = Random.Range(0,7);
		int rhythmMelodySeed = Random.Range(1,10);
		targetPosition = (rhythmMelodySeed <= 5) ?  getTargetLocation(new Vector3(xArray[seed],Random.Range(5,95),yArray[seed]), 15f*1.5f) : getTargetLocation(new Vector3(xArray[seed],Random.Range(5,95),yArray[seed]), 15f*2.5f);
	}
	
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
				GameObject oldSequenceEnemy = oscSender.getRhythmObject(bulletController.getIdx());
				if (oldSequenceEnemy == null) {
					GameObject sequenceEnemy = Instantiate(SequenceEnemy, getSequenceLocation(bulletController.getIdx(), 25f, 0), Quaternion.identity) as GameObject;
					sequenceEnemy.transform.eulerAngles = new Vector3(sequenceEnemy.transform.eulerAngles.x, sequenceEnemy.transform.eulerAngles.y - 90f, sequenceEnemy.transform.eulerAngles.z);
					oscSender.setRhythmObjects(sequenceEnemy,bulletController.getIdx());
				} else {
					sequenceEnemyController  sec = oldSequenceEnemy.GetComponent<sequenceEnemyController>();
					sec.setSequencePos(new int[]{bulletController.getIdx(),0});
				}
			} else {
				oscSender.setMelodyTemplate(bulletController.getIdx(),res[0]);
				GameObject oldSequenceEnemy = oscSender.getMelodyObjects(bulletController.getIdx());
				if (oldSequenceEnemy == null) {
					GameObject sequenceEnemy = Instantiate(SequenceEnemy, getSequenceLocation(bulletController.getIdx(), 25f, 1), Quaternion.identity);
					sequenceEnemy.transform.eulerAngles = new Vector3(sequenceEnemy.transform.eulerAngles.x, sequenceEnemy.transform.eulerAngles.y - 90f, sequenceEnemy.transform.eulerAngles.z);
					oscSender.setMelodyObjects(sequenceEnemy, bulletController.getIdx());
				} else {
					sequenceEnemyController  sec = oldSequenceEnemy.GetComponent<sequenceEnemyController>();
					sec.setSequencePos(new int[]{bulletController.getIdx(),1});
				}
			}
			Destroy(other.gameObject);
			DestorySelf();
		}
	}

	Vector3 getSequenceLocation(int index, float radius, int mode){
		Vector3 firstSequence = firstSequencePos.transform.position;
		float rad = Mathf.Acos(firstSequence.x/radius);
		float degree = rad * Mathf.Rad2Deg;
		degree = degree + 45.0f * index;
		Debug.Log(degree);
		Vector3 res = (mode == 0) ? new Vector3(radius / 2f *Mathf.Cos(degree*Mathf.Deg2Rad) * 1.5f, radius / 2f *Mathf.Sin(degree*Mathf.Deg2Rad) * 1.5f + 50f, 60f) : new Vector3(radius *Mathf.Cos(degree*Mathf.Deg2Rad) * 1.5f, radius *Mathf.Sin(degree*Mathf.Deg2Rad) * 1.5f + 50f, 60f);
		return res;
	}

	Vector3 getTargetLocation(Vector3 position, float radius){
		float rad = Mathf.Acos(position.x/radius);
		float degree = rad * Mathf.Rad2Deg;
		degree += 45.0f/2f;
		int seed = Random.Range(0,2);
		Vector3 res = (seed == 0) ? new Vector3(Mathf.Cos(degree*Mathf.Deg2Rad)*radius, position.y, Mathf.Sin(degree*Mathf.Deg2Rad)*radius) : new Vector3(Mathf.Cos(degree*Mathf.Deg2Rad)*radius, position.y, -Mathf.Sin(degree*Mathf.Deg2Rad)*radius);
		return res;
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
		Destroy(this.gameObject);
	}
}
