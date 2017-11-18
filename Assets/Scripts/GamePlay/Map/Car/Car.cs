using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    bool crashed;
    public float speed;
    public Rigidbody thisRB;
    Animator TruckAni;
    Playenv playEnvironment;
    // Use this for initialization
    private void Awake()
    {
        crashed = false;
        thisRB = GetComponent<Rigidbody>();
        TruckAni = GetComponent<Animator>();
        playEnvironment = GameObject.FindGameObjectWithTag("ENV").GetComponent<Playenv>();
    }
	
	// Update is called once per frame
	void Update () {

    }
    void OnCollisionEnter(Collision col)//오브젝트와 충돌시 호출.
    {
        if (col.gameObject.tag.Contains("Player") && !crashed)
        {
            crashed = true;
            TruckAni.enabled = false;
            int point = (int) Mathf.Abs(thisRB.velocity.y);
            playEnvironment.IncreaseScore(point * 10, 2);//두번째 파라미터 : 2 (자동차 출돌 스코어 획득)
            StartCoroutine(Disable());
            //다음 자동차 리스폰 예약
            transform.parent.SendMessage("SpwanCar", 6.0f);
        }
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }
}
