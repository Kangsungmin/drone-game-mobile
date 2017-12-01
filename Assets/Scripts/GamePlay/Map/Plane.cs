using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour {
    bool crashed;
    public Rigidbody thisRB;
    //Animator UfoAni;
    Playenv playEnvironment;
    Animator PlaneAni;
    // Use this for initialization
    private void Awake()
    {
        crashed = false;
        thisRB = GetComponent<Rigidbody>();
        PlaneAni = GetComponent<Animator>();
        playEnvironment = GameObject.FindGameObjectWithTag("ENV").GetComponent<Playenv>();
    }

    void OnCollisionEnter(Collision col)//오브젝트와 충돌시 호출.
    {
        if (col.gameObject.tag.Contains("Player") && !crashed)
        {
            
            crashed = true;
            PlaneAni.enabled = false;
            //thisRB.useGravity = true;
            int point = (int)Mathf.Abs(thisRB.velocity.y);
            playEnvironment.IncreaseScore(point * 15, 5);//두번째 파라미터 : 2 (비행기 출돌 스코어 획득)
            StartCoroutine(Disable());
            //다음 비행기 리스폰 예약
            transform.parent.SendMessage("SpwanPlane", 30.0f);
            transform.SetParent(null);//부모해제
        }
        else
        {
            gameObject.SetActive(false);
            if(transform.parent != null) transform.parent.SendMessage("SpwanPlane", 30.0f);
        }
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(20.0f);
        gameObject.SetActive(false);
    }
}
