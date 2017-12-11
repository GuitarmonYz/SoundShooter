using System.Collections;
using System.Collections.Generic;
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
	private ArrayList OscMList;
	private UDPPacketIO udp;
	private int[] rhythmTemplate;
	private int[] melodyTemplate;
	private double preTime;
	private Rigidbody playerBody;
	void Awake () {
		osc = GetComponent<Osc>();
		udp = GetComponent<UDPPacketIO>();
		udp.init(IPAddress, SendPort, ListenPort);
		osc.init(udp);
		rhythmTemplate = new int[8];
		melodyTemplate = new int[8];
	}
	void Start () {
		preTime = Time.time;
		OscMList = new ArrayList();
		osc.Send(Osc.StringToOscMessage("/begin/ "+1));
		playerBody = player.GetComponent<Rigidbody>();
		// OscMList.Add(Osc.StringToOscMessage("/bottom/ "+3));
		// OscMList.Add(Osc.StringToOscMessage("/front/ "+6));
	}
	
	// Update is called once per frame
	void Update () {
		//osc.Send(Osc.StringToOscMessage("/bottom/ "+i++));
		sendPosition();
		osc.Send(Osc.StringToOscMessage("/velocity/ "+playerBody.velocity.magnitude));
		if (Time.time > preTime + generateFrequency){
			if (enemyCounter < 8) {
				int prob = Random.Range(0,10);	
				if (prob < 5) {
					Instantiate(enemy, new Vector3(Random.Range(-50,50),Random.Range(0,100),Random.Range(-50,50)),Quaternion.identity);
					enemyCounter++;
				} 
				Debug.Log("generate new");
			}
			preTime = Time.time;
			string tmp = "";
			foreach (int val in rhythmTemplate){
				tmp += (val.ToString()+" ");
			}
			osc.Send(Osc.StringToOscMessage("/rhythm/ "+tmp));
			Debug.Log(tmp);
		}
		
	}

	void sendPosition(){
		Vector3 playerPos = player.transform.position;
		osc.Send(Osc.StringToOscMessage("/front/ "+(playerPos.x+50)));
		osc.Send(Osc.StringToOscMessage("/bottom/ "+playerPos.y));
		osc.Send(Osc.StringToOscMessage("/left/ "+(playerPos.z+50)));
		//osc.Send(OscMList);
		//OscMList.Clear();
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
	void AllMessageHandler (OscMessage oscM) {

	}
}
