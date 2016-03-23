using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public int scoreValue;

	private GameControllerScript gameController;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameControllerScript>();
		} else {
			throw new UnityException("Game Controller not found.");
		}
	}
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "Token") {
			gameController.AddScore (scoreValue);
			gameController.amountTokens--;
		}
		if (other.tag == "Obstacle") {
			gameController.AddScore (-scoreValue);
		}

		gameController.amountCollectibles--;
		Destroy (other.gameObject);
	}
}
