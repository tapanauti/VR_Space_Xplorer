using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRevolution : MonoBehaviour {

    public int revolveSpeed;
    private Vector3 sun;
	// Use this for initialization
	void Start () {
        sun = new Vector3(0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(sun, Vector3.up, revolveSpeed * Time.deltaTime);
	}
}
