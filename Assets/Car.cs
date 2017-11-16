using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {
    bool crashed;
    public float speed;
    public Rigidbody thisRB;
    Animator TruckAni;
    // Use this for initialization
    private void Awake()
    {
        crashed = false;
        thisRB = GetComponent<Rigidbody>();
        TruckAni = GetComponent<Animator>();
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
