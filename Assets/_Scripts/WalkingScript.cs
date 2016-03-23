using UnityEngine;
using System.Collections;

public class WalkingScript : MonoBehaviour {
	
	public float minSpeed;
	public float maxSpeed;
	public float curSpeed;
	public float rotationSpeed;
	// Position of camera for first person mode
	public Vector3 fPerView;
	public float jumpForce;
	public bool onGround;
	
	private float jumpSpeedForw;

	void Start () {
		fPerView = new Vector3 (0, 0.3f, 0);
	}

	void Jump() {
		GetComponent<Rigidbody> ().AddForce (Vector3.up * jumpForce);
		jumpSpeedForw = curSpeed;
		onGround = false;
	}

	public void Move () {
		if (onGround) {
			// rotationSpeed degrees per second
			float rotate = Input.GetAxis ("Horizontal") * rotationSpeed * Time.deltaTime;
			curSpeed = Input.GetAxis ("Vertical") * maxSpeed;

			transform.parent.Rotate (0, rotate, 0);
			// Moves only while button is pressed
			transform.parent.Translate (Vector3.forward * curSpeed * Time.deltaTime);
		} else {
			transform.parent.Translate (Vector3.forward * jumpSpeedForw * Time.deltaTime);
		}

		if (onGround && Input.GetKeyDown (KeyCode.Space)) {
			Jump ();
		}
	}

	// Reached ground
	void OnCollisionEnter (Collision hit) {
		if (hit.gameObject.tag == "Ground") {
			onGround = true;
			jumpSpeedForw = 0f;
		}
	}

	public void SetGravity (bool gravity) {
		GetComponent<Rigidbody> ().useGravity = gravity;
	}

	public void Reset () {
	}
}
