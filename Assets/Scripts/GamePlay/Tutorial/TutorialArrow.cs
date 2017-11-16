using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialArrow : MonoBehaviour
{
    Transform Player, Target;
    public GameObject UIManager;
    bool BoxLiftGuided;
    float range;
    // Use this for initialization
    void Awake()
    {
        BoxLiftGuided = false;
        range = 4.3f;
    }
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        UIManager = GameObject.Find("UI");
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어의 위에 따라다닌다.
        transform.position = new Vector3(Player.position.x, Player.position.y + 1.0f, Player.position.z);
        if(Target != null) transform.LookAt(Target.transform);
    }

    public void ReTargeting(Transform T)
    {
        Target = T;
    }
}