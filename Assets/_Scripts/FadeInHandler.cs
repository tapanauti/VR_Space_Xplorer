using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInHandler : MonoBehaviour {

    public Animator anim;

    public Canvas canvas;

    public GameObject FadeInObject;
	// Update is called once per frame
	void Update () {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("FadeIn"))
        {
            //canvas.renderMode = RenderMode.WorldSpace;
            FadeInObject.SetActive(false);
            this.enabled = false;
        }
	}
}
