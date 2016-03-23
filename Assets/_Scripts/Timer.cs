using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public float startMinutes;
	public float startSeconds;

	public Text timerText;
	public bool countDown;

	private float initTime;
	private float minutes;
	private float seconds;

	void Start () {
		Reset ();
	}

	void Update () {
		if (countDown) {
			Countdown ();
		}
	}

	void UpdateTime () {
		timerText.text = "";

		if (minutes < 10) {
			timerText.text = "0";
		} 
		timerText.text += "" + minutes + ":";

		if (seconds < 10) {
			timerText.text += "0";
		}
		timerText.text += "" + seconds;
	}

	void Countdown () {
		if (!TimeOut () && initTime + 1f < Time.time) {
			if (seconds > 0) {
				seconds--;
			} else {
				minutes--;
				seconds = 59;
			}
			UpdateTime ();
			initTime = Time.time;
		}
	}

	public void IsVisible (bool visible) {
		timerText.enabled = visible;
	}

	public void Reset () {
		initTime = Time.time;
		minutes = startMinutes;
		seconds = startSeconds;
		UpdateTime ();
	}

	public bool TimeOut () {
		if (minutes == 0 && seconds == 0) {
			return true;
		}
		return false;
	}

	public void SetTime (float minutes, float seconds) {
		this.minutes = startMinutes = minutes;
		this.seconds = startSeconds = seconds;
	}
}
