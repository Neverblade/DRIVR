using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSignalControl : MonoBehaviour {

	public enum hand {LEFT, RIGHT, NONE};

	public float angleGapBetweenStates = 25;

	public OVRInput.Controller leftController = OVRInput.Controller.LTouch;
	public OVRInput.Controller rightController = OVRInput.Controller.RTouch;
	public GameObject leftHand;
	public GameObject rightHand;

	private float[] stateAngles;
	private int state = 1;
	private hand currHand = hand.NONE;
	private float angle;
	private Vector3 setVec;
    private Vector3 baseVec;
	private float prevLeftTriggerValue = 1;
	private float prevRightTriggerValue = 1;
    private float prevTriggerValue = 1;

	// Use this for initialization
	void Start () {
		angle = transform.localEulerAngles.z;
		stateAngles = new float[] {angle + angleGapBetweenStates, angle, angle - angleGapBetweenStates};
        baseVec = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
        /*// Check for release
		if ((currHand == hand.LEFT && OVRInput.Get (OVRInput.Axis1D.PrimaryHandTrigger, leftController) < 0.9f)
			|| (currHand == hand.RIGHT && OVRInput.Get (OVRInput.Axis1D.PrimaryHandTrigger, rightController) < 0.9f)) {

			currHand = hand.NONE;
			// Find the nearest state
			int index = -1; float dist = -1;
			for (int i = 0; i < 3; i++) {
				float d = Mathf.Abs (angle - stateAngles [i]);
				if (index == -1 || d < dist) {
					index = i; dist = d;
				}
			}
			// Snap to position
			state = index;
			angle = stateAngles[state];
		}

		// Update angle
		if (currHand == hand.LEFT) {
			Vector3 vec = Vector3.ProjectOnPlane (leftHand.transform.position - transform.position, transform.forward);
            //angle = Mathf.Clamp(stateAngles[state] + Vector3.Angle (setVec, vec), stateAngles[0], stateAngles[stateAngles.Length - 1]);
            angle = baseVec.z + Vector3.Angle(setVec, vec);
        } else if (currHand == hand.RIGHT) {
			Vector3 vec = Vector3.ProjectOnPlane (rightHand.transform.position - transform.position, transform.up);
			angle = Mathf.Clamp(stateAngles[state] + Vector3.Angle (setVec, vec), stateAngles[0], stateAngles[stateAngles.Length - 1]);
		}

		// Update rotation of lever
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        print(angle);*/

        float val = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, leftController).y;
        if (val > .7f && prevTriggerValue < .7f && state != 2) {
            state += 1;
        } else if (val < -.7f && prevTriggerValue > -.7f && state != 0) {
            state -= 1;
        }
        print(val);
        prevTriggerValue = val;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, stateAngles[state]);
    }

	void OnTriggerStay(Collider other) {
		/*if (currHand != hand.NONE)
			return;
		
		if (other.CompareTag ("LeftController")) {
			float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, leftController);
			if (prevLeftTriggerValue < .9f && triggerValue >= 0.9f) {
				setVec = Vector3.ProjectOnPlane (leftHand.transform.position - transform.position, transform.forward);
				currHand = hand.LEFT;
			}
			else prevLeftTriggerValue = triggerValue;
		} else if (other.CompareTag ("RightController")) {
			float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, rightController);
			if (prevRightTriggerValue < .9f && triggerValue >= 0.9f) {
				setVec = Vector3.ProjectOnPlane (rightHand.transform.position - transform.position, transform.up);
				currHand = hand.RIGHT;
			}
			else prevRightTriggerValue = triggerValue;
		*/
	}

    public int getState()
    {
        return state;
    }
}
