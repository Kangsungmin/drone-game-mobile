using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHuman : MonoBehaviour {
    public Animator Ani;
    GameObject Target;

    private void Awake()
    {
    }
    public void SetReference(GameObject[] Refs)
    {
        Target = Refs[0];
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, Target.transform.position) > 4.5f)
        {
            Vector3 tempT = new Vector3(Target.transform.position.x, 1.0f, Target.transform.position.z);
            transform.LookAt(tempT);
        }
            
	}

    public void ChangeBattery()
    {
        Ani.SetInteger("State", 1);
    }
    public void ChangeBatteryEnd()
    {
        Ani.SetInteger("State", 0);
    }

    public void Die()
    {
        Ani.SetInteger("State", -1);
    }
}
