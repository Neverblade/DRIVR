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
	private float prevLeftTriggerValue = 1;
	private float prevRightTriggerValue = 1;

	// Use this for initialization
	void Start () {
		angle = transform.localEulerAngles.y;
		stateAngles = new float[] {angle - angleGapBetweenStates, angle, angle + angleGapBetweenStates};
	}
	
	// Update is called once per frame
	void Update () {
		// Check for release
		if ((currHand == hand.LEFT && OVRInput.Get (OVRInput.Axis1D.PrimaryHandTrigger, leftController) < 0.9f)
			|| (currHand == hand.RIGHT && OVRInput.Get (OVRInput.Axis1D.PrimaryHandTrigger, rightController)) < 0.9f) {

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
			Vector3 vec = Vector3.ProjectOnPlane (leftHand.transform.position - transform.position, transform.up);
			angle = Mathf.Clamp(stateAngles[state] + Vector3.Angle (setVec, vec), stateAngles[0], stateAngles[stateAngles.Length - 1]);
		} else if (currHand = hand.RIGHT) {
			Vector3 vec = Vector3.ProjectOnPlane (rightHand.transform.position - transform.position, transform.up);
			angle = Mathf.Clamp(stateAngles[state] + Vector3.Angle (setVec, vec), stateAngles[0], stateAngles[stateAngles.Length - 1]);
		}

		// Update rotation of lever
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
	}

	void OnTriggerStay(Collider other) {
		if (currHand != hand.NONE)
			return;
		
		if (other.CompareTag ("LeftController")) {
			float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, leftController);
			if (prevLeftTriggerValue < .9f && triggerValue >= 0.9f) {
				setVec = Vector3.ProjectOnPlane (leftHand.transform.position - transform.position, transform.up);
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
		}
	}
}
