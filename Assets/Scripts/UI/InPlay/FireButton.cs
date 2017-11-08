using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class FireButton : MonoBehaviour , IPointerUpHandler, IPointerDownHandler {
    public bool isFire;//발사버튼 누름 여부

    void Start()
    {
        isFire = false;
    }
    public void OnPointerDown(PointerEventData eventData)//누를 시,
    {
        isFire = true;
    }

    public void OnPointerUp(PointerEventData eventData)//뗄 시,
    {
        isFire = false;
    }

    // Use this for initialization

	
}
