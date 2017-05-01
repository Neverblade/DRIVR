using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCar : MonoBehaviour {

    public float minMPH = 40, maxMPH = 50;

    private const float MPH_TO_MPS = 0.44704f;
    private float speed;

	// Use this for initialization
	void Start () {
        speed = minMPH * MPH_TO_MPS;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(speed * Time.fixedDeltaTime * Vector3.forward, Space.World);
	}
}
