// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class OscSender : MonoBehaviour {
	public string IPAddress = "127.0.0.1";
	public int SendPort = 5555;
	public int ListenPort = 2222;
	public GameObject player;
	public GameObject enemy;
	public int generateFrequency = 2;
	private int enemyCounter = 0;
	private Osc osc;
	private UDPPacketIO udp;
	private int[] rhythmTemplate;
	private GameObject[] rhythmObjects;
	private int[] melodyTemplate;
	private GameObject[] melodyObjects;
	private double preTime;
	private Rigidbody playerBody;


	void Awake () {
		osc = GetComponent<Osc>();
		udp = GetComponent<UDPPacketIO>();
		udp.init(IPAddress, SendPort, ListenPort);
		osc.init(udp);
		rhythmTemplate = new int[8];
		rhythmObjects = new GameObject[8];
		melodyTemplate = new int[8];
		melodyObjects = new GameObject[8];
	}
	void Start () {
		preTime = Time.time;
		osc.Send(Osc.StringToOscMessage("/begin/ "+1));
		playerBody = player.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		sendPosition();
		osc.Send(Osc.StringToOscMessage("/velocity/ "+playerBody.velocity.magnitude));
		if (Time.time > preTime + generateFrequency){
			if (enemyCounter < 8) {
				int prob = Random.Range(0,10);	
				if (prob < 5) {
					Instantiate(enemy, new Vector3(Random.Range(-50,50),Random.Range(0,100),Random.Range(-50,50)),Quaternion.identity);
					enemyCounter++;
				} 
				//Debug.Log("generate new");
			}
			preTime = Time.time;
			sendSequence(0);
			sendSequence(1);
		}
	}

	void sendSequence(int rhythmOrMelody){
		string tmp = "";
		if (rhythmOrMelody == 0){
			foreach(int val in rhythmTemplate) {
				tmp += (val.ToString()+" ");
			}
			osc.Send(Osc.StringToOscMessage("/rhythm/ "+tmp));
			Debug.Log("rhythm: "+tmp);
		} else {
			foreach(int val in melodyTemplate) {
				tmp += (val.ToString()+" ");
			}
			osc.Send(Osc.StringToOscMessage("/melody/ "+tmp));
			Debug.Log("melody: "+tmp);
		}
	}

	void sendPosition(){
		Vector3 playerPos = player.transform.position;
		osc.Send(Osc.StringToOscMessage("/front/ "+(playerPos.x+50)));
		osc.Send(Osc.StringToOscMessage("/bottom/ "+playerPos.y));
		osc.Send(Osc.StringToOscMessage("/left/ "+(playerPos.z+50)));
	}
	public void sendMessage(string msg){
		osc.Send(Osc.StringToOscMessage(msg));
	}
	public int[] getRhythmTemplate(){
		return this.rhythmTemplate;
	}
	public void setRhythmTemplate(int idx, int value){
		this.rhythmTemplate[idx] = value;
	}
	public int[] getMelodyTemplate(){
		return this.melodyTemplate;
	}
	public void setMelodyTemplate(int idx, int value){
		this.melodyTemplate[idx] = value;
	}
	public void decreaseEnemy(){
		this.enemyCounter--;
	}
	public GameObject getRhythmObject(int idx){
		return this.rhythmObjects[idx];
	}
	public void setRhythmObjects(GameObject rhythmOject, int idx){
		this.rhythmObjects[idx] = rhythmOject;
	}
	public GameObject getMelodyObjects(int idx){
		return this.melodyObjects[idx];
	}
	public void setMelodyObjects(GameObject melodyObject, int idx){
		this.melodyObjects[idx] = melodyObject;
	}
}
