using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHuman : MonoBehaviour {
    public Animator Ani;

    private void Awake()
    {
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Die()
    {
        Ani.SetInteger("State", -1);
    }
}
