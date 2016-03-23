using UnityEngine;
using System.Collections;

public class FollowWayPoints : MonoBehaviour {

    public PathController pathController;
    public float curSpeed;
    public float rotationSpeed;

    public GameObject curWayPoint;
    private GameObject lastWayPoint;
    private bool rotate;
    private Quaternion curRot;
    private float lastSpeed;

    private Quaternion lastLookDir;

    void Start() {
        Reset();
    }

    public void Reset() {
        lastWayPoint = pathController.GetFirstWayPoint();
        curWayPoint = pathController.GetNextWayPoint();
        rotate = false;
    }

    public void FollowWayPoint() {
        if (!pathController.ReachedEndOfPath()) {
            Vector3 curWayPointPos = curWayPoint.transform.position;
            WayPoint curWayPointScript = curWayPoint.GetComponent<WayPoint>();

            // Move towards way point
            if (Vector3.MoveTowards(transform.position, curWayPointPos, curSpeed * Time.deltaTime) != curWayPointPos) {
                // Move
                if (!rotate) {
                    transform.position = Vector3.MoveTowards(transform.position, curWayPointPos, curSpeed * Time.deltaTime);

                    // Rotate to next bezier point
                    if (curWayPointScript.rotationFollowCurve && !curWayPointScript.isJump) {
                        RotationFollowCurve(curWayPointPos, lastWayPoint.transform.position);
                    } else if (lastWayPoint.GetComponent<WayPoint>().rotationFollowCurve && !curWayPointScript.rotationFollowCurve && !curWayPointScript.isJump) {
                        RotationFollowCurve(curWayPointPos, lastWayPoint.transform.position);
                    }

                } else {
                    // Constant rotation speed
                    float angle = Quaternion.Angle(transform.rotation, curRot);
                    float timeToComplete = angle / rotationSpeed;
                    float percDone = Mathf.Min(1f, Time.deltaTime / timeToComplete);

                    transform.rotation = Quaternion.Slerp(transform.rotation, curRot, percDone);

                    // End rotation
                    if (transform.rotation == curRot) {
                        rotate = false;
                    }
                }

                if (curWayPointScript.speedChange != 0) {
                    // Ac-/decelerate
                    Vector3 lastWayPointPos = lastWayPoint.transform.position;

                    float dist = (lastWayPointPos - curWayPointPos).magnitude;
                    float travelled = (lastWayPointPos - transform.position).magnitude;

                    // Adjust speed by how much of distance to next way point was travelled
                    curSpeed = lastSpeed + (curWayPointScript.speedChange * travelled / dist);

                    // Round to next full number
                    if (curSpeed <= lastSpeed + curWayPointScript.speedChange + 0.1f && curSpeed >= lastSpeed + curWayPointScript.speedChange - 0.1f) {
                        curSpeed = Mathf.Round(curSpeed);
                    }
                }

            } else {
                // Only rotate on the spot after actual way point was reached
                if (curWayPointScript.rotationOnSpot) {
                    rotate = true;
                    curRot = Quaternion.LookRotation((curWayPointScript.rotateTowards - transform.position).normalized);
                }

                lastSpeed = curSpeed;

                lastWayPoint = curWayPoint;

                curWayPoint = pathController.GetNextWayPoint();

                lastLookDir = transform.rotation;
            }
        }
    }

    void RotationFollowCurve(Vector3 targetWP, Vector3 lastWP) {
        Quaternion target = Quaternion.LookRotation((targetWP - lastWP).normalized);

        float distance = (targetWP - lastWP).magnitude;
        float distTravelled = (transform.position - lastWP).magnitude;
        float percDone = (distTravelled / distance);

        if (percDone > 0.99) percDone = 1f;

        transform.rotation = Quaternion.Slerp(lastLookDir, target, percDone);
    }
}
