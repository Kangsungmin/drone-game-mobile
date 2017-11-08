using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAnim : MonoBehaviour {
    private Animator Anicont;
    public VirtualJS_Left moveJoystickLeft;//조이스틱 객체

    void Start () {
        Anicont = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (moveJoystickLeft.Vertical() > 0.0f)
        {
            Anicont.SetInteger("State", 1);
        }
        if (moveJoystickLeft.Vertical() < 0.0f)
        {
            Anicont.SetInteger("State", -1);
        }
        if (moveJoystickLeft.Vertical() == 0.0f)
        {
            Anicont.SetInteger("State", 0);
        }
    }
}
