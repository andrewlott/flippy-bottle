using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Vector2 jumpForce = new Vector2(0, 300);
	public float torqueForceRatio = 0.5f;
	public Vector2 com = new Vector2 (0.0f, 0.8f);
	public Generate generate;

	bool onGround;
	public bool dead;
	private Rigidbody2D rb;

	void Start() {
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
		rb.centerOfMass = rb.centerOfMass - com;
		onGround = true;
		dead = false;
	}

	public void Jump(Vector3 jumpPower) {
		if (jumpPower.magnitude < 1.0f) {
			return;
		}
		if (!onGround) {
			return;
		}
		onGround = false;
		rb.velocity = Vector2.zero;
		rb.AddForce (jumpForce * Mathf.Sqrt(jumpPower.magnitude));
		int sign = jumpPower.x > 0.0f ? -1 : 1;
		rb.AddTorque (sign * jumpForce.y * Mathf.Sqrt(jumpPower.magnitude) * torqueForceRatio);
	}
		
	void OnCollisionEnter2D(Collision2D col) {
		HandleCollision (col);
	}

	void OnCollisionStay2D(Collision2D col) {
		HandleCollision (col);
	}

	void HandleCollision(Collision2D col) {
		if (dead) {
			return;
		}
		if (col.gameObject.tag == "Obstacle") {
			Die ();
		} else if (col.gameObject.tag == "Ground" && !onGround) {
			if (transform.rotation.z <= -0.7f + Mathf.Epsilon || transform.rotation.z >= 0.7f - Mathf.Epsilon) {
				Die ();
			} else if (Mathf.Abs (transform.localRotation.z) < 0.01f) {
				onGround = true;
				generate.AddToScore ();
			}
		}
	}

	void Die() {
		if (dead) {
			return;
		}
		generate.Lose();
		dead = true;
	}
}
