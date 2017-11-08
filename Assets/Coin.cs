using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    GameObject Target;
    GameObject playEnvironment;
    public GameObject child_Item;
    // Update is called once per frame
    private void Awake()
    {
        playEnvironment = GameObject.Find("PlayEnvironment");
    }
    void Update () {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, 0.5f);
        }
	}

    void OnTriggerEnter(Collider target)
    {
        if (target.tag.Contains("Player"))
        {
            //플레이어가 돈을 획득
            playEnvironment.SendMessage("MoneyPlus", 2);
            transform.parent.gameObject.SetActive(false);
        }
    }
    
    void MoveToTarget(GameObject T)
    {
        Target = T;
    }
}
