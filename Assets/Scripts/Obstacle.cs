using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {
	public Vector2 velocity = new Vector2(-4, 0);
	public float range = 4.0f;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
		rb.velocity = velocity;
		transform.position = new Vector3 (transform.position.x, transform.position.y - range * Random.value, transform.position.z);
		Generate.onFailDelegate += Stop;
	}

	void OnDestroy() {
		Generate.onFailDelegate -= Stop;
	}

	void Stop() {
		rb.velocity = Vector2.zero;
	}
}
