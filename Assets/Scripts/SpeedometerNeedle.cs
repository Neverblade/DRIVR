using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedometerNeedle : MonoBehaviour {

    public Rigidbody car;
    public float maxMPH = 160;
    public float minAngle = -110;
    public float maxAngle = 110;

    private const float MPS_TO_MPH = 2.23694f;
	
	// Update is called once per frame
	void Update () {
        float v = car.velocity.magnitude * MPS_TO_MPH;
        float angle = Mathf.Lerp(minAngle, maxAngle, v / maxMPH);
        Vector3 tmp = transform.localEulerAngles;
        tmp.z = angle;
        transform.localEulerAngles = tmp;
	}
}
