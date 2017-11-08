using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {
    public GameObject MinimapDrone; 
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(MinimapDrone.transform.position.x, transform.position.y, MinimapDrone.transform.position.z);
        
    }
}
