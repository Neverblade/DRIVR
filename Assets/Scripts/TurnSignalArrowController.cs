using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSignalArrowController : MonoBehaviour {

    public GameObject leftArrow, rightArrow;
    public TurnSignalControl control;

    private int prvState = 1;
    private float t;
    private bool curFlash;

	// Use this for initialization
	void Start () {
        leftArrow.SetActive(false);
        rightArrow.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        int state = control.getState();

        if (state != prvState) {
            t = 0;
            if (prvState == 1) {
                prvState = state;
                flash(true);
            } else {
                flash(false);
                prvState = state;
            }
        } else if (state != 1) {
            t += Time.deltaTime;
            if (t > 0.5f) {
                flash(!curFlash);
                t = 0;
            }
        }
	}

    void flash(bool on)
    {
        curFlash = on;
        if (prvState == 0) leftArrow.SetActive(on);
        if (prvState == 2) rightArrow.SetActive(on);
    }
}
