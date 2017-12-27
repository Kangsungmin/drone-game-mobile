using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ThrustDownButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public UIManager ui_manager;
    // Use this for initialization
    public virtual void OnPointerDown(PointerEventData ped)
    {
        //추력 증가
        ui_manager.ThrustAddValue = -1.0f;
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        //추력 증가 멈춤
        ui_manager.ThrustAddValue = 0.0f;
    }

}
