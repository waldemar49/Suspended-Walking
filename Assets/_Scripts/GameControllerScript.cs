using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

	public GameObject token;
	public GameObject obstacle;
	public GameObject player;
	//public GameObject groundPlane;
	public Timer timer;
	public Text scoreText;
	public Text startText;
	public Text finalScoreText;
	public int maxCollectibles;
	public int amountCollectibles;
	public int amountTokens;

	private int score;
	private PlayerMain playerScript;
	private float maxPos;
	// Game mode 1 = walking, 2 = swinging, 3 = flying
	private int gameMode = 1;
	// Game started?
	private bool started = false;
	// Use collectibles?
	private bool collectibles = true;
	private bool following = false;
	
	void Start () {
		score = 0;
		UpdateScore();

		playerScript = player.GetComponent<PlayerMain> ();
		maxPos = playerScript.maxPos;

		// Set third person view to true
		playerScript.SetView (true);
	}

	void Update() {
		if (timer.TimeOut ()) {
			finalScoreText.text = "Your final score: " + score;
			Reset ();
		}

		if (!started) {
			// End game
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
			// Walking
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				gameMode = 1;
				/*if (!following) {
					//groundPlane.SetActive(true);
					playerScript.wScript.gameObject.GetComponent<Rigidbody>().useGravity = true;
				}*/
				Debug.Log("Game mode: Walking");
			}
			// Swinging
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				gameMode = 2;
				//groundPlane.SetActive (false);
				Debug.Log("Game mode: Swinging");
			}
			// Flying
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				gameMode = 3;
				//groundPlane.SetActive (false);
				Debug.Log("Game mode: Flying");
			}

			playerScript.SetGameMode(gameMode);

			// Follow way points
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				following = true;
				timer.IsVisible(false);
				playerScript.SetFollowing(true);
				collectibles = false;
				scoreText.enabled = false;

				playerScript.wScript.gameObject.GetComponent<Rigidbody>().useGravity = !following;
				Debug.Log ("Following waypoins: " + !collectibles);
			}
            // Free mode
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                following = false;
                collectibles = false;
                timer.IsVisible(false);
                scoreText.enabled = false;
                playerScript.SetFollowing(false);
                if (gameMode != 1) {
                    playerScript.compass.SetActive(true);
                }
                Debug.Log("Free movement");
            }
            // Collect mode
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                following = false;
                collectibles = true;
                timer.IsVisible(true);
                scoreText.enabled = true;
                playerScript.SetFollowing(false);
                if (gameMode != 1) {
                    playerScript.compass.SetActive(true);
                }
                Debug.Log("Collect");
            }
            // Show glass floor, when walking and not following waypoints
            /*if (gameMode == 1 && !following) {
                groundPlane.SetActive(true);
            } else {
                groundPlane.SetActive(false);
            }*/

        } else {
			if(collectibles) {
				SpawnObjects();
				timer.countDown = true;
			}
		}

		// Change view
		if (Input.GetKeyDown (KeyCode.Alpha0)) playerScript.SetView(!playerScript.thirdPerson);

		// Start and reset game
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (started){
				Reset ();
			} else {
				startText.enabled = false;
				playerScript.move = true;
				started = true;
				finalScoreText.text = "";
			}
		}
	}

	// Spawn tokens and obstacles
	void SpawnObjects(){
		while(amountCollectibles < maxCollectibles){

			Vector3 spawnPosition;

			// Change positions if player is walking
			if (gameMode != 1){
				spawnPosition = new Vector3 ((Random.value * (2*maxPos)) - maxPos, 
			                                     	(Random.value * (2*maxPos)) - maxPos, (Random.value * (2*maxPos)) - maxPos);
			} else {
				int yPos = Random.Range (0, 3);

				if(yPos == 1){
					yPos = 0;
				}

				spawnPosition = new Vector3 ((Random.value * (2*maxPos)) - maxPos, yPos, (Random.value * (2*maxPos)) - maxPos);
			}

			GameObject child;

			// Use tokens and obstacles
			if(amountTokens < maxCollectibles * 0.7){
				child = Instantiate (token, spawnPosition, Quaternion.identity) as GameObject;
				amountTokens++;
			} else {
				child = Instantiate (obstacle, spawnPosition, Quaternion.identity) as GameObject;
			}

			child.transform.parent = this.transform; 
			
			amountCollectibles++;
		}
	}

	// Delete tokens and obstacles
	void DeleteObjects(){
		GameObject[] tokens = GameObject.FindGameObjectsWithTag ("Token");
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");

		for (int i = 0; i < tokens.Length; i++) {
			Destroy (tokens[i]);
		}

		for (int j = 0; j < obstacles.Length; j++) {
			Destroy(obstacles[j]);
		}
		amountCollectibles = 0;
		amountTokens = 0;
	}

	void ResetScore(){
		score = 0;
		UpdateScore ();
	}

	void UpdateScore(){
		scoreText.text = "Score: " + score;
	}

	void Reset () {
		startText.enabled = true;
		playerScript.Reset ();
		ResetScore ();
		started = false;
		DeleteObjects();
		timer.Reset ();
		timer.countDown = false;
	}

	public void AddScore(int points){
		score += points;
		UpdateScore ();
	}
}
