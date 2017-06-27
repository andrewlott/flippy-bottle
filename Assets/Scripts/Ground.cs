using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {
	public Vector3 initialPosition;
	public Vector2 velocity = new Vector2(-2, 0);
	public float range = 3.0f;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		initialPosition = transform.position;
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
		rb.velocity = velocity;
		Generate.onFailDelegate += Stop;
	}
	void Update () {
		Vector2 screenPosition = Camera.main.WorldToScreenPoint (transform.position);
		if (screenPosition.x + 1530 < 0) {
			Reset ();
		}
	}

	void OnDestroy() {
		Generate.onFailDelegate -= Stop;
	}

	void Reset() {
		transform.position = initialPosition;
	}

	void Stop() {
		rb.velocity = Vector2.zero;
	}
}
