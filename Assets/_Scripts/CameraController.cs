using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public int offset;

	void LateUpdate () {
		Transform target = player.transform;
		Vector3 newPos;

		// Sets the cameras position
		if (player.GetComponent<PlayerMain>().thirdPerson == true) {
			// Camera always behind player
			newPos = target.position - target.forward * offset;

			transform.position = newPos;
			transform.rotation = target.rotation;
			// Camera slightly above player
			transform.Translate (0, 1, 0);
		} else {
            //Camera in front of players face
            newPos = target.position + transform.TransformDirection(player.GetComponent<PlayerMain>().fPersView);

            transform.position = newPos;
			transform.rotation = target.rotation;
		}
	}
}