using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceBasedTeleporter : MonoBehaviour {

    private ExampleStreaming es;
    private EventManager em;

    public GameObject WarpingParticles;
    public GameObject micUI;

    [HideInInspector]
    public string[] finalOutputString;
    [HideInInspector]
    public string keyWord;
    private string[] acceptableWords = {"teleport","daily board","they live for","daily for"};

    private bool matchFound;

    RaycastHit hitInfo;

    private float currTime;
    public float travelTime = 5f;
    public float buttonPressLimit;
    // Use this for initialization
    void Start () {
        es = GetComponent<ExampleStreaming>();
        em = GetComponent<EventManager>();
	}

	// Update is called once per frame
	void Update () {
        if (es.enabled)
        {
            finalOutputString = es.WatsonOutputString.Split('(');
            keyWord = finalOutputString[0].Trim().ToLower();
            for (int i = 0; i < acceptableWords.Length; i++)
            {
                if (keyWord.Equals(acceptableWords[i]))
                {
                    matchFound = true;
                    break;
                }
                else
                {
                    matchFound = false;
                }
                    
            }
            if (matchFound)
            {
                em.ActivateTeleport();
            }
        }
        else
        {
            keyWord = "";
            es.WatsonOutputString = "";
        }
    }

    public IEnumerator Teleport(Vector3 src,Vector3 dst)
    {
        em.ovrCam.cullingMask = 1 << 10;
        em.ovrCam.clearFlags = CameraClearFlags.SolidColor;
        WarpingParticles.SetActive(true);
        currTime = 0;
        while (src != dst)
        {
            currTime += Time.deltaTime;
            if (currTime >= travelTime)
                currTime = travelTime;

            float perc = currTime / travelTime;
            em.player.transform.position = Vector3.Lerp(src, dst, perc);
            yield return null;
        } 
    }
}
