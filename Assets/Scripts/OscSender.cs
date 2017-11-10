using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscSender : MonoBehaviour {
	public string IPAddress = "127.0.0.1";
	public int SendPort = 5555;
	public int ListenPort = 2222;
	private Osc osc;
	private ArrayList OscMList;
	private UDPPacketIO udp;
	private int i = 1;
	private double time;
	void Awake () {
		osc = GetComponent<Osc>();
		udp = GetComponent<UDPPacketIO>();
		udp.init(IPAddress, SendPort, ListenPort);
		osc.init(udp);
	}
	void Start () {
		time = Time.time;
		OscMList = new ArrayList();
		OscMList.Add(Osc.StringToOscMessage("/input1/ "+3));
		OscMList.Add(Osc.StringToOscMessage("/input2/ "+6));
	}
	
	// Update is called once per frame
	void Update () {
		osc.Send(OscMList);
		i++;
		//Debug.Log("sending");
		
	}
	void AllMessageHandler (OscMessage oscM) {

	}
}
