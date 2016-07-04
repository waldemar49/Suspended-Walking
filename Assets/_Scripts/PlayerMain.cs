using UnityEngine;
using System.Collections;

public class PlayerMain : MonoBehaviour {

	public float curSpeed;
	public float maxSpeed;
	public float minSpeed;
	// Max movement range of player
	public float maxPos;
	public float accelConst;

	public bool thirdPerson;
	// Player is allowed to move
	public bool move = false;
	public Vector3 fPersView;

	public GameObject compass;

	public WalkingScript wScript;
	public float walkingSpeedFlw;
	public SwingingScript sScript;
	public float swingingSpeedFlw;
	public FlyingScript fScript;
	public float flyingSpeedFlw;

	public PathController pathController;

	private Compass compScript;
	// Follow way points
	private bool following = false;
    private FollowWayPoints flwWayPnts;

	void Start() {
		compScript = compass.GetComponent<Compass> ();
		//compScript.setMaxHeight (maxPos);
		fPersView = sScript.fPerView;
        flwWayPnts = GetComponent<FollowWayPoints> ();
	}

	void Update() {
		// Toggle compass
		if (Input.GetKeyDown (KeyCode.C)){
			compass.SetActive(!compass.activeInHierarchy);
		}

		// Player is allowed to move
		if (move) {
			if  (!following) {
				// Get correct movement script
				if (wScript.gameObject.activeInHierarchy) {
					wScript.Move ();
				}
				if (sScript.gameObject.activeInHierarchy) {
					sScript.Move ();
				}
				if (fScript.gameObject.activeInHierarchy) {
					fScript.Move ();
				}

				// Limit translation
				float x = Mathf.Clamp (transform.position.x, -maxPos, maxPos);
				float y = Mathf.Clamp (transform.position.y, -maxPos, maxPos);
				float z = Mathf.Clamp (transform.position.z, -maxPos, maxPos);
				
				transform.position = new Vector3 (x, y, z);
            } else {
				flwWayPnts.FollowWayPoint ();
			}

            float accel = Input.GetAxis("Speed") * accelConst * Time.deltaTime;

            curSpeed += accel;

            // Limit speed
            curSpeed = Mathf.Clamp(curSpeed, minSpeed, maxSpeed);
        } else {
			// Only if object is waiting to start, change course
			if (Input.GetKeyDown (KeyCode.Keypad1)) {
				pathController.SetCourse (0);
				Reset ();
				Debug.Log ("Course: 1");
			}
			if (Input.GetKeyDown (KeyCode.Keypad2)) {
				pathController.SetCourse (1);
				Reset ();
				Debug.Log ("Course: 2"); 
			}
			if (Input.GetKeyDown (KeyCode.Keypad3)) {
				pathController.SetCourse (2);
				Reset ();
				Debug.Log ("Course: 3");
			}
            if (Input.GetKeyDown(KeyCode.Keypad4)) {
                pathController.SetCourse(3);
                Reset();
                Debug.Log("Course: 4");
            }
        }

		compScript.UpdateCompass (transform.rotation.eulerAngles.y);
	}

	void ResetLookDir () {
		if (following) {
			transform.LookAt (flwWayPnts.curWayPoint.transform.position);
		} else {
			transform.LookAt (new Vector3 (0, 0, 1));
		}
	}

	// Sets view to third person (true) or first person (false)
	public void SetView (bool view){
		thirdPerson = view;
	}

	public void Reset(){
		move = false;
		transform.position = new Vector3(0, 0, 0);
		pathController.ResetWayPointCounter();
        flwWayPnts.Reset();
		ResetLookDir();

		wScript.Reset();
		sScript.Reset();
		fScript.Reset();
	}

	public void SetFollowing(bool active){
		if(active) {
			following = true;
            pathController.wPInstructions.enabled = true;
			pathController.gameObject.SetActive(true);
			compass.SetActive(false);
		} else {
			following = false;
            pathController.wPInstructions.enabled = false;
            pathController.gameObject.SetActive(false);
		}
		Reset();
	}

	// Switch between locomotion
	public void SetGameMode(int mode){
        // Walking
        if (mode == 1)
        {
            wScript.gameObject.SetActive(true);
            sScript.gameObject.SetActive(false);
            fScript.gameObject.SetActive(false);
            compass.SetActive(false);

            curSpeed = walkingSpeedFlw;
            fPersView = wScript.fPerView;
            flwWayPnts.curSpeed = curSpeed;
            // Swinging
        } else if (mode == 2) {
            wScript.gameObject.SetActive (false);
            sScript.gameObject.SetActive (true);
            fScript.gameObject.SetActive (false);

			if (!following) {
				compass.SetActive (true);
			}

			curSpeed = swingingSpeedFlw;
            fPersView = sScript.fPerView;
            Debug.Log(sScript.fPerView);
            Debug.Log(fPersView);
            flwWayPnts.curSpeed = curSpeed;
        // Flying
        } else if (mode == 3) {
            wScript.gameObject.SetActive(false);
            sScript.gameObject.SetActive(false);
            fScript.gameObject.SetActive(true);

			if (!following) {
				compass.SetActive (true);
			}

			curSpeed = flyingSpeedFlw;
			fPersView = fScript.fPerView;
            flwWayPnts.curSpeed = curSpeed;
        }
	}
}
