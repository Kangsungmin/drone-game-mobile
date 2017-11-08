using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftCamera : MonoBehaviour {
    Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShiftRight()
    {
        //우로 이동 animation 시작
        //parent position이동
        //animator 초기화
        animator.SetInteger("shift", 1);
        transform.parent.position = new Vector3(animator.transform.position.x, 0, 0);
        animator.Play("CamShift_R", 0, 0.0f);
    }
    public void ShiftLeft()
    {
        animator.SetInteger("shift", -1);
        transform.parent.position = new Vector3(animator.transform.position.x, 0, 0);
        animator.Play("CamShift_L", 0, 0.0f);
    }
}