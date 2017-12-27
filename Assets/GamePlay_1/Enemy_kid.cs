using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_kid : Enemy {
    private void Awake()
    {
        _audio = gameObject.AddComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.loop = false;
        _audio.clip = getHitSound;
        
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

        // Navigation 적용
        myTransform = this.gameObject.GetComponent<Transform>();
        playerTransform = Target.GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        nvAgent.enabled = false;
        nvAgent.enabled = true;
        nvAgent.destination = playerTransform.position;
        // --------------
    }
	// Update is called once per frame
	void Update () {
        switch (State)
        {
            case "Idle":
                break;
            case "Move":
                nvAgent.speed = Speed;
                EnemyAnimator.SetInteger("State", 2);
                //transform.LookAt(Target.transform);
                //transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);//보는방향으로 움직인다.
                if (Vector3.Distance(transform.position, Target.transform.position) < 5.0f) State = "Attack";
                break;
            case "Attack":
                nvAgent.speed = 0;
                //environment.SendMessage("AttackMain", Power);//메인주인공 공격 알림
                if (AttackReady)
                {
                    _audio.clip = attackSound;
                    _audio.Play();
                    StartCoroutine(Attack());
                }
                break;
            case "Blocked":
                nvAgent.speed = 0;
                //애니메이션은 그대로, 포지션 이동은 하지 않는다.
                break;
            case "Die":
                nvAgent.enabled = false;
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
            }
            else if (other.CompareTag("Barrier"))
            {
                if (!State.Equals("Blocked"))
                {
                    StartCoroutine(Blocked((float)other.GetComponent<Rigidbody>().velocity.magnitude));
                }
                
            }
        }
    }

    public void Damaged(float amount)
    {
        //_audio.Play();
        float getScore = 0.0f;
        getScore = (amount > HP) ? HP : amount;
        environment.IncreaseScore((int)getScore, 0);

        HP -= amount;
        if (HP <= 0.0f)
        {
            isDead = true;
            environment.IncreaseMoney(money);
            State = "Die";
        }
    }

    IEnumerator Blocked(float speed)
    {
        State = "Blocked";

        if (speed > 2.0f)
            Damaged(1.0f);

        nvAgent.updatePosition = false;
        yield return new WaitForSeconds(0.5f);
        if(!isDead)
            State = "Move";
        nvAgent.updatePosition = true;

    }

    private void Dead()
    {
        _audio.clip = dieSound;
        _audio.volume = 0.4f;
        _audio.Play();
        //StartCoroutine(DieSoundDelay());
        State = "Exit";
        isDead = true;
        EnemyAnimator.enabled = false;
        boxcoll.enabled = false;
        StartCoroutine(ReserveUnable());//오브젝트 꺼짐 예약
        
    }

    IEnumerator Attack()
    {
        AttackReady = false;
        //공격 모션
        yield return new WaitForSeconds(1.0f);
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
