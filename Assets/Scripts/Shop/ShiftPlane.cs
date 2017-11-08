using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftPlane : MonoBehaviour {
    Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void ShiftRight()
    {
        //우로 이동 animation 시작
        //parent position이동
        //animator 초기화
        animator.SetInteger("shift", 1);
        transform.parent.transform.position = animator.transform.position;
        animator.Play("R_Shift",0, 0.0f);
    }
    public void ShiftLeft()
    {
        animator.SetInteger("shift", -1);
        transform.parent.transform.position = animator.transform.position;
        animator.Play("L_Shift", 0, 0.0f);
    }
}
