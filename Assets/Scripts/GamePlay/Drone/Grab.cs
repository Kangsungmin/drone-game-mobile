using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grab : MonoBehaviour
{
    float Range = 4.5f;
    public GameObject GrabButtonAni, GrabButton, GaugueUI;
    GameObject target;
    GameObject[] Boxestemp;
    List<GameObject> Boxes = new List<GameObject>();
    
    public void SetReference(GameObject[] Refs)
    {
        GrabButton = Refs[0];
        GrabButtonAni = Refs[1];
        GaugueUI = Refs[2];
    }

    void Start()
    {
        /*
        GrabButtonAni = GameObject.Find("UI").transform.Find("GrabButtonParent").gameObject;
        GrabButton = GameObject.Find("UI").transform.Find("GrabButton").gameObject;
        GaugueUI = GameObject.Find("UI").transform.Find("GaugePanel").gameObject;
        */
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isBox = false;
        if (Boxes != null)
        {
            foreach (GameObject B in Boxes)
            {
                float distanceToBox = Vector3.Distance(transform.position, B.transform.position);
                if (distanceToBox < Range)
                {
                    isBox = true;
                    target = B;
                    GrabButtonAni.SetActive(true);
                    GrabButton.SetActive(true);//상자 들기 버튼 활성화
                }
            }
            if (!isBox)
            {
                target = null;
                GrabButtonAni.SetActive(false);
                GrabButton.SetActive(false);
                GaugueUI.SetActive(false);
            } //버튼 비활성화
        }
        else
        {
            //게이지 UI가 있다면 해제
            GaugueUI.SetActive(false);
        }
    }
    public void AddBoxList(GameObject box)
    {
        Boxes.Add(box);
    }
    public void RemoveBoxList(GameObject box)
    {
        foreach (GameObject B in Boxes)
        {
            if (B == box)
            {
                Boxes.Remove(B);
                Debug.Log(B + "제거");
                break;
            }
        }
    }

    public void GrabMode()
    {
        transform.root.SendMessage("GrabSomthing", target);
    }

}