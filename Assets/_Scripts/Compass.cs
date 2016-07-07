using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

	//public UnityEngine.UI.RawImage artHorizon;

    /*// Max flying height of the player object
	private float maxHeight;
	// Max room for the artHorizon to navigate
	private static float MAX_HEIGHT_COMP;

    void Start() {
        MAX_HEIGHT_COMP = (artHorizon.rectTransform.sizeDelta.y / 100) * 30;
    }

	// Angle to be assigned in euler angle
	public void UpdateCompass(float height, float angle){
		// Relative height for compass
		height = height / maxHeight * MAX_HEIGHT_COMP;

		artHorizon.rectTransform.anchoredPosition = new Vector3(0, height, 0);
        artHorizon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

	public void setMaxHeight(float height){
		maxHeight = height;
	}*/

    public void UpdateCompass(float playerRot) {
        //transform.localEulerAngles = new Vector3(0, 0, playerRot);
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, playerRot)); 
    }
}