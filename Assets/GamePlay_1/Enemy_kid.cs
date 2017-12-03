using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_kid : Enemy {
    

    private void Awake()
    {
        isDead = false;
        State = "Move";
        HP = 5.0f;
        Max_HP = 5.0f;
        Speed = 2.0f;
        Power = 1.0f;
        money = 5;
        EnemyAnimator = GetComponent<Animator>();
    }
    public void SetReference(GameObject[] Refs)
    {
        environment = Refs[0].GetComponent<Environment>();
        Target = Refs[1];
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (State)
        {
            case "Idle":
                break;
            case "Move":
                EnemyAnimator.SetInteger("State", 2);
                transform.LookAt(Target.transform);
                transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);//보는방향으로 움직인다.
                if (Vector3.Distance(transform.position, Target.transform.position) < 5.0f) State = "Attack";
                break;
            case "Attack":
                //environment.SendMessage("AttackMain", Power);//메인주인공 공격 알림
                if(AttackReady) StartCoroutine(Attack());
                break;
            case "Blocked":
                //애니메이션은 그대로, 포지션 이동은 하지 않는다.
                break;
            case "Die":
                Dead();
                
                break;
            case "Exit":
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isDead)
        {
            if (other.CompareTag("Player"))
            {
                Damaged(1.0f);
                environment.IncreaseScore(1, 0);
            }
            else if (other.CompareTag("Barrier"))
            {
                if (!State.Equals("Blocked"))
                {
                    StartCoroutine(Blocked());
                }
                
            }
        }
    }

    private void Damaged(float amount)
    {
        HP -= amount;
        if (HP <= 0.0f)
        {
            isDead = true;
            environment.IncreaseMoney(money);
            State = "Die";
        }
    }

    IEnumerator Blocked()
    {
        State = "Blocked";
        Damaged(1.0f);
        environment.IncreaseScore(1, 0);
        yield return new WaitForSeconds(0.5f);
        State = "Move";
    }

    private void Dead()
    {
        isDead = true;
        EnemyAnimator.enabled = false;
        boxcoll.enabled = false;
        StartCoroutine(ReserveUnable());//오브젝트 꺼짐 예약
        State = "Exit";
    }

    IEnumerator Attack()
    {
        AttackReady = false;
        //공격 모션
        yield return new WaitForSeconds(2.0f);
        environment.SendMessage("AttackMain", Power);//메인주인공 공격 알림
        AttackReady = true;
    }

    IEnumerator ReserveUnable()
    {
        yield return new WaitForSeconds(8.0f);
        Environment.EnemyCount -= 1;
        gameObject.SetActive(false);
    }
}
