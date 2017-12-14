using UnityEngine;

public class bulletController : MonoBehaviour {
	private int idx;
	public void setIdx(int idx){
		this.idx = idx;
	}
	public int getIdx(){
		return this.idx;
	}
	void Update(){
		if (Mathf.Abs(transform.position.x) > 80f || Mathf.Abs(transform.position.z) > 80f) {
			Destroy(this.gameObject);
		}
	}
}
