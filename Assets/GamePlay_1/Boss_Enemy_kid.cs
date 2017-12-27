using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Enemy_kid : Enemy
{
    bool AniOn;
    private void Awake()
    {
        _audio = gameObject.AddComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.loop = false;
        _audio.clip = getHitSound;

        isDead = false;
        State = "Move";
        HP = 200.0f;
        Max_HP = 200.0f;
        Speed = 2.5f;
        Power = 10.0f;
        money = 100;
        EnemyAnimator = GetComponent<Animator>();
        AniOn = false;
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
    void Update()
    {
        switch (State)
        {
            case "Idle":
                break;
            case "Move":
                nvAgent.speed = Speed;
                EnemyAnimator.SetInteger("State", 1);
                //transform.LookAt(Target.transform);
                //transform.Translate(transform.forward * Speed * Time.deltaTime, Space.World);//보는방향으로 움직인다.
                transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
                if (Vector3.Distance(transform.position, Target.transform.position) < 9.0f) State = "Attack";
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

    private void Damaged(float amount)
    {
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
        else if(!AniOn) StartCoroutine(GetHitMotion());
    }

    IEnumerator Blocked(float speed)
    {
        State = "Blocked";

        if (speed > 2.0f)
            Damaged(1.0f);

        nvAgent.updatePosition = false;
        environment.IncreaseScore(1, 0);
        yield return new WaitForSeconds(0.5f);
        if (!isDead)
            State = "Move";
        nvAgent.updatePosition = true;
    }

    private void Dead()
    {
        _audio.clip = dieSound;
        _audio.volume = 0.4f;
        _audio.Play();

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

    IEnumerator GetHitMotion()
    {
        AniOn = true;
        EnemyAnimator.Play("Zombie Die_01", 0, 0.0f);
        nvAgent.height = 0.0f;
        nvAgent.baseOffset = 0.0f;
        yield return new WaitForSeconds(0.18f);
        
        EnemyAnimator.SetInteger("State", 1);
        AniOn = false;
    }
}
