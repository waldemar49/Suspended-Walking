using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PathController : MonoBehaviour {

    public Text wPInstructions;

    private List<PathScript> paths = new List<PathScript> ();
    private PathScript curCourse;
    private int curCourseNum = 0;

	void Awake() {
        // Get paths
        foreach (Transform child in transform) {
            if (child.tag == "Course") {
                paths.Add(child.gameObject.GetComponent<PathScript>());
            }
        }

        // Let courses initialise themselves
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
            child.gameObject.SetActive(false);
        }

        // Set course one as active
        curCourse = paths[curCourseNum];
        curCourse.gameObject.SetActive(true);

        wPInstructions.enabled = false;

        // Wait until object is needed
        gameObject.SetActive (false);
    }

    public void SetCourse(int num) {
        curCourse.gameObject.SetActive (false);

        if (num >= 0 && num < paths.Count) {
            curCourseNum = num;
            curCourse = paths[curCourseNum];
            curCourse.gameObject.SetActive (true);
        }
    }

    public void NextCourse() {
        curCourse = paths[++curCourseNum];
    }

    public void LastCourse() {
        curCourse = paths[--curCourseNum];
    }

    public PathScript GetCurCourse() {
        return paths[curCourseNum];
    }

    public GameObject GetFirstWayPoint() {
        return curCourse.GetFirstWayPoint ();
    }

    public GameObject GetNextWayPoint() {
        SetInstructions();
        return curCourse.GetNextWayPoint();
    }

    public GameObject PeekNextWayPoint() {
        return curCourse.PeekNextWayPoint();
    }

    public void ResetWayPointCounter () {
        curCourse.ResetWayPointCounter();
    }

    public bool ReachedEndOfPath() {
        return curCourse.ReachedEndOfPath();
    }

    public GameObject GetLastWayPoint() {
        return curCourse.GetLastWayPoint ();
    }

    private void SetInstructions() {
        WayPoint wP = curCourse.GetInstructions().GetComponent<WayPoint>();

        if (wP.tag != "Bezier Point") {
            wPInstructions.text = wP.label;
        }
    }
}
