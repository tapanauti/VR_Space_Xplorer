using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStatus : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        transform.parent.GetComponent<Planet>().uIAccess = true;
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.GetComponent<Planet>().uIAccess = false;
    }
}
