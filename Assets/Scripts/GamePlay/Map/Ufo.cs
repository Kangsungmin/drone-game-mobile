using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour {
    bool crashed;
    public Rigidbody thisRB;
    //Animator UfoAni;
    Playenv playEnvironment;
    // Use this for initialization
    private void Awake()
    {
        crashed = false;
        thisRB = GetComponent<Rigidbody>();
        //UfoAni = GetComponent<Animator>();
        playEnvironment = GameObject.FindGameObjectWithTag("ENV").GetComponent<Playenv>();
    }
    
    void OnCollisionEnter(Collision col)//오브젝트와 충돌시 호출.
    {
        if (col.gameObject.tag.Contains("Player") && !crashed)
        {
            crashed = true;
            //UfoAni.enabled = false;
            int point = (int)Mathf.Abs(thisRB.velocity.y);
            playEnvironment.IncreaseScore(point * 30, 4);//두번째 파라미터 : 2 (ufo 출돌 스코어 획득)
            StartCoroutine(Disable());
            //다음 자동차 리스폰 예약
            transform.parent.SendMessage("SpwanUfo", 30.0f);
        }
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(20.0f);
        gameObject.SetActive(false);
    }
}
