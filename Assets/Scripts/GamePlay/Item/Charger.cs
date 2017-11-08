using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour {
    public GameObject chargeArea;
    public bool chargeOn = true;
    // Use this for initialization
	
	// Update is called once per frame
	void Update () {
        if (!chargeOn)//배터리 끌때
        {
            chargeArea.SetActive(false);
            StartCoroutine("chargerOn");//20초 후에 킨다.
            chargeOn = true;//키는것 예약 했으므로 true
        }
    }
    IEnumerator chargerOn()
    {
        yield return new WaitForSeconds(20.0f);//20초 후에 연료 생성
        chargeArea.SetActive(true);
    }
}
