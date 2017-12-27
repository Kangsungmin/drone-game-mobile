using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    protected Vector3 Forward, Right;
    protected float F_Force, R_Force;


    public GameObject playEnvironment, InventoryManager;
    public UIManager ui_Manager;
    protected Rigidbody thisRB;

    protected VirtualJS_Left moveJoystickLeft;//조이스틱 객체
    protected Button upButton, downButton;
    protected SwipeController swipeController;
    public Transform Claw;
    

    //=================================================

    abstract public void getFuel();
    //abstract public void Hit(int damage);
    //abstract public void GetItem(GameObject item);
    abstract public void GrabSomthing(GameObject target);
    abstract public void DropSomthing();
    abstract public void AddControll(float val);
}
