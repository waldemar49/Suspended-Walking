using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public int scoreValue;
    public AudioClip tokenSound;
    public AudioClip obstacleSound;

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
            AudioSource.PlayClipAtPoint(tokenSound, transform.position);
			gameController.AddScore (scoreValue);
			gameController.amountTokens--;
		}
		if (other.tag == "Obstacle") {
            AudioSource.PlayClipAtPoint(obstacleSound, transform.position);
            gameController.AddScore (-scoreValue);
		}

		gameController.amountCollectibles--;
        other.gameObject.SetActive(false);
        gameController.ReplaceObject(other.gameObject);
	}
}
