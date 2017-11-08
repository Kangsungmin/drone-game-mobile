using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletAni : MonoBehaviour {
    public GameObject main;
    public ParticleSystem mainParticleSystem;


    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            main.SetActive(true);
        }
        if (mainParticleSystem.IsAlive() == false) main.SetActive(false);
	}  
}
