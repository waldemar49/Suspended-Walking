using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionsController : MonoBehaviour {

    public Texture[] instructions;
    private int num;
    private bool isVisible;

    void Start() {
        num = -1;
    }

	void Update () {
        // Switch instructions
        if (Input.GetKeyDown(KeyCode.Keypad7)) {
            if (num > 0) { num--;
                GetComponent<RawImage>().texture = instructions[num];
                isVisible = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad8)) {
            num = -1;
            GetComponent<RawImage>().texture = null;
            isVisible = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9)) {
            if (num < instructions.Length-1) num++;
            GetComponent<RawImage>().texture = instructions[num];
            isVisible = true;
        }

        if (!isVisible) {
            GetComponent<RawImage>().color = Color.clear;
        } else {
            GetComponent<RawImage>().color = Color.white;
        }
    }
}
