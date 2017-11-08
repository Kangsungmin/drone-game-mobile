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
        Target = GameObject.FindGameObjectWithTag("Box").transform;
        UIManager = GameObject.Find("UI");

    }

    // Update is called once per frame
    void Update()
    {
        //플레이어의 위에 따라다닌다.
        transform.position = new Vector3(Player.position.x, Player.position.y + 1.0f, Player.position.z);
        transform.LookAt(Target.transform);
        //박스와이 거리가 가까우면 화살표를 끈다
        float distanceToBox = Vector3.Distance(transform.position, Target.transform.position);
        if (distanceToBox < range)
        {
            //그랩 버튼 튜토리얼을 활성화 시킨다.
            if (!BoxLiftGuided)
            {
                BoxLiftGuided = true;
                UIManager.SendMessage("TutorialNext");
            }
            gameObject.SetActive(false);
        }

    }

    public void ReTargeting(GameObject T)
    {
        Target = T.transform;
    }
}