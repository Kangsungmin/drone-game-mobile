using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item {
    // Update is called once per frame
    private void Awake()
    {
        playEnvironment = GameObject.Find("PlayEnvironment");
    }

    void FixedUpdate() {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, 0.3f);
        }
	}

    
    void OnTriggerEnter(Collider target)
    {
        if (target.tag.Contains("Player"))
        {
            //플레이어가 돈을 획득
            playEnvironment.SendMessage("MoneyPlus", 2);
            if(isRegenerable) transform.parent.SendMessage("ReserveReGen",20.0f);
            Target = null;
            gameObject.SetActive(false);
        }
    }

    public override void MoveToTarget(GameObject T)
    {
        Target = T;
    }
}
