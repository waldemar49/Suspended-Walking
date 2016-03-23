using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathScript : MonoBehaviour {

    public bool pathEnd;
    public GameObject bezierPoint;

    private List<GameObject> wayPoints = new List<GameObject>();
    private LineRenderer path;
    // 0 = start position of player
    private int curWayPointCounter = 1;

    void Awake() {
        path = GetComponent<LineRenderer>();
        SetPath();

        SetLineRenderer();
    }

    void SetPath() {
        List<GameObject> newPath = new List<GameObject>();

        // Get path
        foreach (Transform child in transform) {
            if (child.tag == "Way Point" || child.tag == "Bezier Point") {
                wayPoints.Add(child.gameObject);
            }
        }

        for (int i = 0; i < wayPoints.Count; i++) {
            WayPoint wayPointScript = wayPoints[i].GetComponent<WayPoint>();

            newPath.Add(wayPoints[i]);

            if (wayPointScript.useBezier) {
                Vector3 start = wayPoints[i].transform.position;
                Vector3 end = wayPoints[i + 1].transform.position;

                for (float t = wayPointScript.stepSizeBezier; t <= 1; t += wayPointScript.stepSizeBezier) {
                    Vector3 Pos = GetBezierPoint(start, end, wayPointScript.contrPoint, t);

                    // Put bezier points into list
                    GameObject bezPoint = Instantiate(bezierPoint, Pos, Quaternion.identity) as GameObject;
                    bezPoint.transform.parent = wayPointScript.transform;
                    WayPoint bezPointScript = bezPoint.GetComponent<WayPoint>();
                    bezPointScript.rotationFollowCurve = true;
                    bezPointScript.isJump = wayPointScript.isJump;
                    newPath.Add(bezPoint);
                }
            }
        }

        wayPoints = newPath;
    }

    void SetLineRenderer() {
        path.SetVertexCount(wayPoints.Count);

        for (int i = 0; i < wayPoints.Count; i++) {
            path.SetPosition(i, wayPoints[i].transform.position);
        }
    }

    // Cubic bezier curve, position calculation
    Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 contPoint, float t) {
        Vector3 v1 = contPoint - p0;
        Vector3 v2 = p1 - contPoint;

        v1 = p0 + t * v1;
        v2 = contPoint + t * v2;

        Vector3 solution = v2 - v1;
        solution = v1 + t * solution;

        return solution;
    }

    // Debug
    void PrintWayPoints() {
        foreach (GameObject wpoint in wayPoints) {
            Debug.Log(wpoint.transform.position);
        }
    }

    public GameObject GetNextWayPoint() {
        // End of path?
        if (curWayPointCounter == wayPoints.Count) {
            pathEnd = true;
            return null;
        } else {
            pathEnd = false;
            return wayPoints[curWayPointCounter++];
        }
    }

    public GameObject PeekNextWayPoint() {
        return wayPoints[curWayPointCounter];
    }

    public bool ReachedEndOfPath() {
        return pathEnd;
    }

    public GameObject GetLastWayPoint() {
        return wayPoints[wayPoints.Count - 1];
    }

    public GameObject GetFirstWayPoint() {
        return wayPoints[0];
    }

    public GameObject GetWayPointByIndex(int index) {
        return null;
    }

    public void ResetWayPointCounter() {
        curWayPointCounter = 1;
    }

    public GameObject GetInstructions() {
        return wayPoints[curWayPointCounter - 1];
    }
}