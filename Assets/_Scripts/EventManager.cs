using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {

    public GameObject playerController;
    public GameObject playerEyeAnchor;
    public GameObject player;
    public GameObject CoreUI;
    public GameObject UIPanel;
    public GameObject mainCanvas;
    public GameObject teleportPanel;
    private GameObject selectedPlanet;
    private GameObject textPanel;

    public Camera ovrCam;

    [HideInInspector]
    public Transform destination;
    public Transform planets;

    public LineRenderer lineRenderer;

    public Text planetName;

    RaycastHit hitInfo;

    private Animator anim;

    private bool uiPanelOpened,canContinue,planetCoreOpened;

    private string selectedPlanetName;

    private VoiceBasedTeleporter vbt;
    private ExampleStreaming es;

    private SphereCollider playerCollider;

    private IEnumerator teleport,planetCoreStatus;

    private float timePassed;

    public Vector3 uIOffset;
    private Vector3 playerRotation;
    private Vector3 UIPanelRotation;

    private Vector2 touchPosition;
    public Vector2 teleportPanelOffset;

    private void Start()
    {
        uiPanelOpened = false;
        vbt = GetComponent<VoiceBasedTeleporter>();
        playerCollider = player.GetComponent<SphereCollider>();
        es = GetComponent<ExampleStreaming>();
    }

    // Update is called once per frame
    void Update () {

        //Ending teleportation.
        if (destination != null)
        {
            if (player.transform.position == destination.position)
            {
                destination = null;
                StopCoroutine(teleport);
                playerCollider.enabled = true;
                vbt.WarpingParticles.SetActive(false);
                ovrCam.clearFlags = CameraClearFlags.Skybox;
                ovrCam.cullingMask = LayerMask.NameToLayer("Everything");
                teleportPanel.SetActive(false);
                vbt.micUI.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))        //TEMP remove later
        {
            es.enabled = true;
        }

        if (Input.GetKey(KeyCode.Q) || OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            if (touchPosition.x < 0)
            {
                lineRenderer.enabled = true;
                timePassed = 0f;
            }                

            else if (touchPosition.x > 0)
            {
                timePassed += Time.deltaTime;
                lineRenderer.enabled = false;
            }
                
        }

        //Animate selected planet to expose core.
        if (Input.GetKeyUp(KeyCode.Q) || OVRInput.GetUp(OVRInput.Touch.PrimaryTouchpad))
        {
            lineRenderer.enabled = false;
            if (timePassed >= vbt.buttonPressLimit)
            {
                es.enabled = !es.enabled;
                if (es.enabled)
                    vbt.micUI.SetActive(true);
                else
                {
                    vbt.micUI.SetActive(false);
                    es.enabled = false;
                }        
            }

            else
            {
                if (Physics.Raycast(playerController.transform.position, playerController.transform.forward, out hitInfo, Mathf.Infinity, 9) && touchPosition.x < 0)
                {
                    if (hitInfo.transform.parent.CompareTag("Planet") && hitInfo.transform.parent.GetComponent<Planet>().uIAccess) //->ADD LATER!!!!!!
                    {
                        selectedPlanet = hitInfo.transform.parent.gameObject;
                        if (hitInfo.transform.parent.gameObject.GetComponent<Animator>() != null)
                        {
                            anim = hitInfo.transform.parent.gameObject.GetComponent<Animator>();
                            selectedPlanetName = hitInfo.transform.parent.name;
                            StartCoroutine(PlanetCoreStatus());
                        }
                    }

                    if (hitInfo.transform.CompareTag("ExploreButton"))
                    {
                        SceneManager.LoadScene(selectedPlanetName);
                    }

                    if (hitInfo.transform.CompareTag("TeleportButton"))
                    {
                        string[] temp = hitInfo.transform.parent.name.Split();
                        destination =planets.Find(temp[0]).Find("Waypoint");
                        playerCollider.enabled = false;
                        teleport = vbt.Teleport(player.transform.position, destination.position);
                        StartCoroutine(teleport);
                    }
                }
            }
            timePassed = 0f;
        }

        //Spawn UI and adjust rotation according to player.
        if (Input.GetKeyDown(KeyCode.Z) || OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            if (Physics.Raycast(playerEyeAnchor.transform.position, playerEyeAnchor.transform.forward, out hitInfo, Mathf.Infinity, 9)  && touchPosition.x > 0)
            {
                selectedPlanetName = hitInfo.transform.parent.name;
                if (!uiPanelOpened && hitInfo.transform.parent.GetComponent<Planet>().uIAccess)
                {
                    playerRotation = playerEyeAnchor.transform.eulerAngles;
                    UIPanelRotation = UIPanel.transform.eulerAngles;
                    UIPanel.transform.position = playerEyeAnchor.transform.position + playerEyeAnchor.transform.forward * uIOffset.z+ playerEyeAnchor.transform.right * uIOffset.x+
                                                 playerEyeAnchor.transform.up*uIOffset.y;
                    UIPanelRotation += new Vector3(0,playerRotation.y,0);
                    UIPanel.transform.eulerAngles = UIPanelRotation;
                    UIPanel.SetActive(true);
                    planetName.text ="PLANET: "+ selectedPlanetName;
                    uiPanelOpened = true;
                    textPanel = UIPanel.transform.Find(selectedPlanetName + "Text").gameObject;
                    textPanel.SetActive(true);
                }

                else
                {
                    UIPanel.SetActive(false);
                    if(textPanel!=null)
                    textPanel.SetActive(false);
                    textPanel = null;
                    uiPanelOpened = false;
                    UIPanel.transform.eulerAngles = new Vector3(UIPanel.transform.eulerAngles.x,0,0);
                }             
            }
        }
       
    }

    private void ActivateUI()
    {
        mainCanvas.transform.Find(selectedPlanetName + "Cores").gameObject.SetActive(true);
        hitInfo.transform.parent.gameObject.GetComponent<Planet>().opened = true;
    }

    private void DeactivateUI()
    {
        hitInfo.transform.parent.gameObject.GetComponent<Planet>().opened = false;
    }

    public void ActivateTeleport()
    {
        teleportPanel.transform.Find("PlanetsPanel").transform.position = playerEyeAnchor.transform.position + playerEyeAnchor.transform.forward * teleportPanelOffset.y
                                                                                + playerEyeAnchor.transform.right * teleportPanelOffset.x;
        teleportPanel.SetActive(true);
        es.enabled = false;
    }

    IEnumerator PlanetCoreStatus()
    {
        if (!hitInfo.transform.parent.gameObject.GetComponent<Planet>().opened)
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime<=1)
            {
                yield return null;
            }
            anim.SetBool("open", true);
            Invoke("ActivateUI", 2.0f);
        }

        else
        {
            while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
            {
                yield return null;
            }
            anim.SetBool("open", false);
            mainCanvas.transform.Find(selectedPlanetName + "Cores").gameObject.SetActive(false);
            Invoke("DeactivateUI", 2.0f);
        }
    }

}