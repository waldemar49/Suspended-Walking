using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint : MonoBehaviour {

    public bool useBezier;
    public Vector3 contrPoint;
    public float stepSizeBezier;
    public bool rotationOnSpot;
    public Vector3 rotateTowards;
    public bool rotationFollowCurve;
    public bool isJump;
    public float speedChange;
    public string label;
    public GameObject text;

    void Start() {
        // Setup 3D-Text
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);

        GameObject navigation = Instantiate(text, pos, Quaternion.identity) as GameObject;
        navigation.transform.parent = transform;
        navigation.GetComponent<TextMesh>().text = label;
    }
}