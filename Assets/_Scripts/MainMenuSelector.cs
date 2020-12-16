using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSelector : MonoBehaviour {

    public LineRenderer lineRenderer;
    public GameObject AppManager;
    private ApplicationManager appManager;
    private Vector2 touchCoordinates;
    //RaycastHit hitInfo;
    private void Start()
    {
        appManager = AppManager.GetComponent<ApplicationManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) || OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            touchCoordinates = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            if (touchCoordinates.x < 0)
                lineRenderer.enabled = true;
            else
                lineRenderer.enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.Q) || OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))
        {
            lineRenderer.enabled = false;
            if (Physics.Raycast(transform.position, transform.forward) && touchCoordinates.x < 0)
            {
                appManager.Begin();
            }
        }
    }
}
