using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelControl : MonoBehaviour {

    public float maxTurnAngle = 2 * 360;

    public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
    public OVRInput.Controller rightController = OVRInput.Controller.RTouch;

    public GameObject leftHand;
    public GameObject rightHand;

    private float angle;

    private bool leftHeld = false;
    private bool rightHeld = false;
    private float prevLeftTriggerValue = 1;
    private float prevRightTriggerValue = 1;
    private string whichHand = "NULL";

    private Vector3 originalAngles;
    private Vector3 prevLocalAngle;
    private Quaternion leftSetRot;
    private Quaternion rightSetRot;
    private Vector3 leftVector;
    private Vector3 rightVector;

	// Use this for initialization
	void Start () {
        originalAngles = transform.localEulerAngles;
        angle = 0;
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(new Vector3(0, 1, 0));

        // Check leave updates
        if (leftHeld && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, leftController) < 0.9f) {
            leftHeld = false;
            prevLeftTriggerValue = 1;
            print("Left Hand OFF");
        }
        if (rightHeld && OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, rightController) < 0.9f) {
            rightHeld = false;
            prevRightTriggerValue = 1;
            print("Right Hand OFF");
        }

        // Update rotation
        Quaternion newRotation = Quaternion.identity;
        if (leftHeld && rightHeld) {
            if (whichHand.Equals("LEFT")) {
                Vector3 leftCurrVector = projectedVector(leftHand.transform.position - transform.position);
                Quaternion rot = Quaternion.FromToRotation(leftVector, leftCurrVector);
                newRotation = rot * leftSetRot;
            }
            else {
                Vector3 rightCurrVector = projectedVector(rightHand.transform.position - transform.position);
                Quaternion rot = Quaternion.FromToRotation(rightVector, rightCurrVector);
                newRotation = rot * rightSetRot;
            }
        }
        else if (leftHeld) {
            Vector3 currVector = projectedVector(leftHand.transform.position - transform.position);
            Quaternion rot = Quaternion.FromToRotation(leftVector, currVector);
            newRotation = rot * leftSetRot;
        }
        else if (rightHeld) {
            Vector3 currVector = projectedVector(rightHand.transform.position - transform.position);
            Quaternion rot = Quaternion.FromToRotation(rightVector, currVector);
            newRotation = rot * rightSetRot;
        }
        else return;

        // Make sure it's fixated on one local axis
        transform.rotation = newRotation;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        float diff = transform.localEulerAngles.y - prevLocalAngle.y;
        if (diff > 180) {
            diff -= 360;
        } else if (diff < -180) {
            diff += 360;
        }
        angle += diff;
        angle = Mathf.Clamp(angle, -maxTurnAngle, maxTurnAngle);
        transform.localEulerAngles = new Vector3(0, angle, 0);
        prevLocalAngle = transform.localEulerAngles;
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("LeftController")) {
            float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, leftController);
            if (!leftHeld && prevLeftTriggerValue < .9f && triggerValue >= 0.9f) {
                leftHeld = true;
                leftVector = projectedVector(leftHand.transform.position - transform.position);
                print("Left Hand ON");
                leftSetRot = transform.rotation;
                if (!rightHeld) whichHand = "LEFT";
            }
            else prevLeftTriggerValue = triggerValue;
        } else if (other.CompareTag("RightController")) {
            float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, rightController);
            if (!rightHeld && prevRightTriggerValue < .9f && triggerValue >= 0.9f) {
                rightHeld = true;
                rightVector = projectedVector(rightHand.transform.position - transform.position);
                print("Right Hand ON");
                rightSetRot = transform.rotation;
                if (!leftHeld) whichHand = "RIGHT";
            }
            else prevRightTriggerValue = triggerValue;
        }
    }

    Vector3 projectedVector(Vector3 vec)
    {
        return Vector3.ProjectOnPlane(vec, transform.up);
    }

    public float clampAngle()
    {
        float ans = angle / maxTurnAngle;
        return Mathf.Clamp(ans, -1, 1);
    }
}
