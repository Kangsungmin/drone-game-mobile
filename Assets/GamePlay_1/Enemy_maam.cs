using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_maam : Enemy {


    private void Awake()
    {
        isDead = false;
        State = "Move";
        HP = 12.0f;
        Max_HP = 12.0f;
        Speed = 6.0f;
        Power = 2.0f;
        EnemyAnimator = GetComponent<Animator>();
    }
    public void SetReference(GameObject[] Refs)
    {
        environment = Refs[0].GetComponent<Environment>();
        Target = Refs[1];
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case "Idle":
                break;
            case "Move":
                EnemyAnimator.SetInteger("State", 1);
                transform.LookAt(Target.transform);
                transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);//보는방향으로 움직인다.
                if (Vector3.Distance(transform.position, Target.transform.position) < 5.0f) State = "Attack";
                break;
            case "Attack":
                //environment.SendMessage("AttackMain", Power);//메인주인공 공격 알림
                if (AttackReady) StartCoroutine(Attack());
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
                HP -= 1.0f;
                if (HP <= 0.0f)
                {
                    isDead = true;
                    environment.IncreaseMoney(12);
                    State = "Die";
                }
                environment.IncreaseScore(1, 0);
            }
            else if (other.CompareTag("Barrier"))
            {
                HP -= 1.0f;
                if (HP <= 0.0f)
                {
                    isDead = true;
                    environment.IncreaseMoney(12);
                    State = "Die";
                }
            }
        }
    }


    private void Dead()
    {
        isDead = true;
        //EnemyAnimator.SetInteger("State", -1);//죽음
        EnemyAnimator.enabled = false;
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
