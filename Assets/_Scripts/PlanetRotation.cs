using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour {

    public int rotationSpeed;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
	}
}
