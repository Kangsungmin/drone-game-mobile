using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 필드나 드롭되는 아이템은 해당 스크립트를 가진다.
 * 획득을 통해 얻는 아이템 식별 id나 스코어는 Unity Inspector의 int수정으로 가능하다.
 */
public class Item : MonoBehaviour {
    public int thisScore = 2;//Unity 상에서 수정가능
    public int ID=0;//Unity 상에서 수정가능

    public GameObject Target;
    public GameObject playEnvironment;
    public bool isRegenerable = true;

    private void Awake()
    {
        playEnvironment = GameObject.FindGameObjectWithTag("ENV");
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
        if (target.tag == "Player")
        {
            
            if (ID != 0)//플레이어가 점수, 해당 아이템 획득
            {
                playEnvironment.SendMessage("IncreaseScore", thisScore);
                playEnvironment.SendMessage("AddPart", ID);
                playEnvironment.SendMessage("PlayerDataUpdate");
            }
            else playEnvironment.SendMessage("MoneyPlus", 1); //아이템 돈일 경우.

            if (isRegenerable) transform.parent.SendMessage("ReserveReGen", 50.0f);
            Target = null;
            gameObject.SetActive(false);
        }
    }

    public void MoveToTarget(GameObject T)
    {
        Target = T;
    }
}
