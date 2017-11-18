using UnityEngine;

public class GunController : MonoBehaviour {
	public Rigidbody bullet;
	public int bulletSpeed;
	public Transform barrel;

	// Use this for initialization
	void Start () {
	}

	void SpawnBullet() {
		Rigidbody b;
		b = Instantiate(bullet, new Vector3(barrel.position.x, barrel.position.y, barrel.position.z), barrel.rotation) as Rigidbody;
		b.AddForce(b.transform.forward * bulletSpeed);
	}

	// Update is called once per frame
	void Update () {
		// TODO: move make this more generic
		if (Input.GetButtonDown("Fire1")) {
			SpawnBullet();
		}
	}
}
