using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BatteryChangeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image Process, ProcessBG;
    bool pointed = false;
    float NeededTime = 20.0f, NeededTime_Max = 20.0f;
    Drone PlayerDrone;
    public Text NeededTimeText;

    void Awake()
    {
        NeededTime = 20.0f;
        NeededTimeText.text = "20.0";
        Process.gameObject.SetActive(false);
        ProcessBG.gameObject.SetActive(false);
    }
    public void SetReference(GameObject[] Refs)
    {
        PlayerDrone = Refs[0].GetComponent<Drone>();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (pointed)
        {
            //드론 시동을 끈다.
            NeededTime -= 0.1f;
            NeededTimeText.text = NeededTime.ToString("N1");
            if (NeededTime <= 0.0f)
            {
                PlayerDrone.Fuel = PlayerDrone.Max_Fuel; //배터리 교체
                Initialize();
                
            }
        }
        if(Process.gameObject.activeSelf) Process.fillAmount = (float)NeededTime / NeededTime_Max;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointed = true;
        Process.gameObject.SetActive(true);
        ProcessBG.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //드론 시동을 켠다.
        Initialize();
    }

    public void Initialize()
    {
        pointed = false;
        NeededTime = NeededTime_Max;
        NeededTimeText.text = "20.0";
        Process.gameObject.SetActive(false);
        ProcessBG.gameObject.SetActive(false);
    }
}
