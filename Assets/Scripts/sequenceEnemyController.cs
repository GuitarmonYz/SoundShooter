using UnityEngine;

public class sequenceEnemyController : MonoBehaviour {
	public int speed = 1;
	private GameObject center;
	private OscSender oscSender;
	private int[] sequencePos;
	void Start () {
		GameObject manager = GameObject.FindGameObjectWithTag("Manager");
		oscSender = manager.GetComponent<OscSender>();
		center = GameObject.FindGameObjectWithTag("CircleCenter");
	}
	void Update () {
		transform.RotateAround(center.transform.position, Vector3.forward, speed*Time.deltaTime);
	}
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("enter");
		if (other.CompareTag("FancyBullet")){
			if (sequencePos[1] == 0) {
				oscSender.setRhythmTemplate(sequencePos[0],0);
			} else {
				oscSender.setMelodyTemplate(sequencePos[0],0);
			}
			DestorySelf();
			Destroy(other.gameObject);
		}
	}
	void DestorySelf(){
		Destroy(this.gameObject);
	}
	public void setSequencePos(int[] pos){
		sequencePos = pos;
	}
}
