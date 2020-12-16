using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject controller;
    public int speed;
	
	// Update is called once per frame
	void Update () {
        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKey(KeyCode.F))
            transform.Translate(controller.transform.forward*speed*Time.deltaTime);
	}
}
