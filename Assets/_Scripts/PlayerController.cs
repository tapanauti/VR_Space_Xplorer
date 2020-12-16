using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public GameObject playerController;
    public GameObject playerEyeAnchor;
    public GameObject Text;

    public AudioSource movementAudioSource,jumpAudioSource;
    private Rigidbody rb;

    public int movementSpeed;
    public int jumpForce;

    public float gravityModifier;
    private float yVelocity;
    private float rayCastDistance;
    public float audioGap;
    private float currTime;

    private Vector3 finalGravity;

    private RaycastHit hit;

    private bool canJump;

    private Vector2 touchCoordinates;

    public LineRenderer lineRenderer;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rayCastDistance = 7;
        currTime = 0f;
	}

    // Update is called once per frame
    void Update()
    {
        canJump = Physics.Raycast(transform.position,Vector3.down,rayCastDistance);
        if (Physics.Raycast(playerEyeAnchor.transform.position, playerEyeAnchor.transform.forward, out hit))
        {
            if (hit.transform.name == "Sky")
            {
                Text.SetActive(true);
            }
            else
            {
                Text.SetActive(false);
            }
        }
        else
            Text.SetActive(false);

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)  || Input.GetKey(KeyCode.F))
        {
            transform.Translate(playerController.transform.forward * movementSpeed * Time.deltaTime);
            if (Time.time > currTime)
            {
                movementAudioSource.Play();
                currTime = Time.time + audioGap;
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyUp(KeyCode.F))
        {
            movementAudioSource.Stop();
        }

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
            if (Physics.Raycast(playerController.transform.position, playerController.transform.forward, out hit) && touchCoordinates.x < 0)
            {
                if (hit.transform.name == "Sky")
                {
                    SceneManager.LoadScene(1);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump || OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) && canJump)
        {
            touchCoordinates = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            if (touchCoordinates.x > 0)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpAudioSource.Play();
            }  
        }
    }

    private void FixedUpdate()
    {
        yVelocity = rb.velocity.y;
        if (yVelocity < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * gravityModifier * Time.deltaTime ;
        }
    }
}
