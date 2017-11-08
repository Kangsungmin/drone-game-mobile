using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class InventoryButton : MonoBehaviour, IPointerDownHandler
{
    public GameObject InventoryMenu;
    public bool OnBag;//발사버튼 누름 여부
    // Use this for initialization
    void Start()
    {
        OnBag = false;
        //InventoryMenu.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)//누를 시,
    {
        if (OnBag)
        {
            OnBag = false;//가방 열려있다면 닫는다.
            InventoryMenu.SetActive(false);
        }
        else
        {
            OnBag = true;
            InventoryMenu.SetActive(true);
        }
    }
    
	
}
