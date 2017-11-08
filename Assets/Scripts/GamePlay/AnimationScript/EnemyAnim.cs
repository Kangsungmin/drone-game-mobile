using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : MonoBehaviour {
    private Animator Anicont;
    public bool targeted = false;
    public bool isDead = false;

    // Use this for initialization

    void Start()
    {
        Anicont = GetComponent<Animator>();
    }

    public void Targeted()
    {
        targeted = true;
    }

    public void UnTargeted()
    {
        targeted = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (isDead == false && targeted)
        {
            Anicont.SetInteger("State", 1);
        }
        else if(isDead)
        {
            Anicont.SetInteger("State", -1);
        }
        else
        {
            Anicont.SetInteger("State", 0);
        }
	}
}
