using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnimation : MonoBehaviour {
    private Animator StarAnimator;
    // Use this for initialization
	/*
    public void Update()
    {
        if (gameObject.activeSelf)
        {
            StarAnimator = GetComponent<Animator>();
            StarAnimator.SetInteger("Show", 1);
        }
    }*/

    public void showStar()
    {
        StarAnimator.SetInteger("Show", 1);
    }
}
