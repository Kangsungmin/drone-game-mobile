using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;

public abstract class Drone : MonoBehaviour {
    //Drone 객체 변수
    //=================================================
    public Animator DroneAnimator;//gameobjectanimator
    public bool AnimatorState;//idle
    public bool flyDelay = true;//canControl
    public bool fuelDelay = true;
    public bool droneStarting = false;
    public bool DronePowerOn = false;
    public bool GameOver = false;

    public int Speed;
    //public float Hp = 100.0f, Max_Hp = 100;
    public float Fuel = 50.0f, Max_Fuel = 50;
    public float currentY;//currentY : Left조이스틱회전값을 저장하는 변수 
    public float Thrust = 0.000f, hovering_Thrust = 48.031f, MaxThrust = 100.0f;
    public Vector3 wingDir;
    public Vector3 bodyDir;
    public Vector3 rotVec;

    public GameObject playEnvironment, InventoryManager, UIManager;
    protected Rigidbody thisRB;

    protected VirtualJS_Left moveJoystickLeft;//조이스틱 객체
    protected VirtualJS_Right moveJoystickRight;
    protected Transform Claw;
    

    //=================================================

    abstract public void getFuel();
    //abstract public void Hit(int damage);
    //abstract public void GetItem(GameObject item);
    abstract public void GrabSomthing(GameObject target);
    abstract public void DropSomthing();

}
