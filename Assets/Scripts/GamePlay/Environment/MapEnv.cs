using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnv : MonoBehaviour {
    int ran;
    float rot;
	// Use this for initialization
	void Start () {
        
        ran = Random.Range(1,4);
        switch (ran)
        {
            case 1: rot = 0; break;
            case 2: rot = 90; break;
            case 3: rot = 180; break;
            case 4: rot = 270; break;

        }
        //transform.Rotate(Vector3.up*rot);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
