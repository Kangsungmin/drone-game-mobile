using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuboid : Item {
    int thisScore;
    //1의스코어를 얻는다
    private void Awake()
    {
        thisScore = 2;
        playEnvironment = GameObject.Find("PlayEnvironment");
    }
    void FixedUpdate()
    {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, 0.3f);
        }
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player" )
        {
            //플레이어가 점수
            playEnvironment.SendMessage("IncreaseScore", thisScore);
            if (isRegenerable) transform.parent.SendMessage("ReserveReGen", 50.0f);
            Target = null;
            gameObject.SetActive(false);
        }
    }

    public override void MoveToTarget(GameObject T)
    {
        Target = T;
    }
}
