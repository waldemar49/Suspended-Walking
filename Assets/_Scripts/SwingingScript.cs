using UnityEngine;
using System.Collections;
using System;

public class SwingingScript : MonoBehaviour, Rotation.Listener {

	public float minSpeed;
	public float maxSpeed;
	public float curSpeed;
	public float accelConst;
	public float rotationSpeed;
	public float startSpeed;
    public float rotDamping;
    // Position of camera for first person mode
    public Vector3 fPerView;

    void Awake() {
        fPerView = new Vector3(0, 1f, 0.5f);
    }

	void Start () {
        RotationController.CalibratedRotation().Add(this);
    }

    public void On(Quaternion q) {
        if (isActiveAndEnabled) {
            Quaternion target = new Quaternion(-0, q.x, -0, q.w);
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, transform.parent.rotation * target, Time.deltaTime * rotDamping);

            // x rotation is absolute, y rotation is relative, no rotation around z 
            Vector3 s = Quaternion.ToEulerAngles(new Quaternion(-q.y, 0, 0, q.w));
            transform.parent.rotation = Quaternion.Euler(s.x * Mathf.Rad2Deg, transform.parent.eulerAngles.y, 0);
        }
    }

    public void Move() {

        if (Input.GetKeyDown(KeyCode.KeypadPlus)) rotationSpeed += 5;
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) rotationSpeed -= 5;

        // rotationSpeed degrees per second
        float rotateX = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
		float rotateY = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        // Edit -> Project Settings -> Input
        float rotateZ = Input.GetAxis("Roll") * rotationSpeed * Time.deltaTime;

        transform.parent.Rotate(rotateX, rotateY, rotateZ);

        float accel = Input.GetAxis("Speed") * accelConst * Time.deltaTime;
		
		curSpeed += accel;
		
		// Limit speed
		curSpeed = Mathf.Clamp(curSpeed, minSpeed, maxSpeed);

        // Moves on it's own
        transform.parent.Translate(Vector3.forward * curSpeed * Time.deltaTime);
    }

	public void Reset() {
		curSpeed = startSpeed;
	}
}
