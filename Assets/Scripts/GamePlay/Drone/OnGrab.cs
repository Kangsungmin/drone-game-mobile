using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//리프트 버튼에 삽입된 스크립트
public class OnGrab : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static string grabState;//Idle, Using
    GameObject Drone;
    GameObject DroneClaw;
    Animator GrabModeCtrl;//UI애니메이션
    public GameObject GaugeUI;
	// Use this for initialization

    public void SetReference(GameObject[] Refs)
    {
        Drone = Refs[0];
        DroneClaw = Refs[1];
        GrabModeCtrl = Refs[2].GetComponent<Animator>();
    }

	void Start () {
        grabState = "Idle";
        GaugeUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)//버튼 눌린상태
    {
        //게이지 활성화
        switch (grabState)
        {
            case "Idle"://게이지 활성화
                GaugeUI.SetActive(true);
                break;
            case "Using"://현재 들고있는 상자를 떨어뜨린다.
                Drone.SendMessage("DropSomthing");
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //현재의 gauge 포지션 받아옴
        //조건 만족할시, 상자를 든다.
        //게이지 비활성화
        if (grabState.Equals("Idle"))
        {
            GameObject gauge = GaugeUI.transform.GetChild(0).gameObject;
            Debug.Log(gauge.transform.localPosition);
            if (gauge.transform.localPosition.x >= -10.0f && gauge.transform.localPosition.x <= 10)
            {
                Grab();
                Debug.Log("잡는다");
            }
            //gauge.GetComponent<GaugeHand>().ResetGauge();
            GaugeUI.SetActive(false);
        }
        else
        {
            grabState = "Idle";
            GrabModeCtrl.SetBool("ATK", false);
        }
    }

    public void Grab()//Claw의 Grab호출
    {
        grabState = "Using";
        GrabModeCtrl.SetBool("ATK", true);
        DroneClaw.GetComponent<Grab>().GrabMode();
    }
}
