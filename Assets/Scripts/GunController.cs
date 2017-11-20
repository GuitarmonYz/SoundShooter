using UnityEngine;

public class GunController : MonoBehaviour {
	public Rigidbody bullet;
	public int bulletSpeed;
	public Transform barrel;

	private Rigidbody preBullet;
	private Transform prePosition;

	// Use this for initialization
	void Start () {
		preBullet = Instantiate(bullet, new Vector3(barrel.position.x, barrel.position.y, barrel.position.z), barrel.rotation) as Rigidbody;
		prePosition = preBullet.transform;
	}

	void SpawnBullet() {
		preBullet.AddForce(-barrel.transform.forward * bulletSpeed);
		preBullet = Instantiate(bullet, new Vector3(barrel.position.x, barrel.position.y, barrel.position.z), barrel.rotation) as Rigidbody;
		//b.AddForce(b.transform.forward * bulletSpeed);
	}

	void Update () {
		preBullet.transform.position = Vector3.Lerp(prePosition.position, barrel.position, Time.time);
		prePosition = preBullet.transform;
	}

	// Update is called once per frame
	void FixedUpdate () {
		// TODO: move make this more generic
		if (Input.GetButtonDown("Fire1")) {
			SpawnBullet();
		}
	}
}
