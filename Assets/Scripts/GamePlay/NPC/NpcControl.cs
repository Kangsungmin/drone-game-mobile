using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * NPC애니메이션 컨트롤 스트립트
 * 17.09.09 성민 최종수정
 */
public class NpcControl : MonoBehaviour
{
    int speed;
    float range;
    public int State; //0~3까지의 NPC애니메이션 상태
    public GameObject Ray;
    Animator NPCAnimation;
    bool StateReserved;
    GameObject[] Player;
    // Use this for initialization
    void Start()
    {
        StateReserved = false;
        speed = 2;
        range = 3.0f;
        NPCAnimation = GetComponent<Animator>();
        Player = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        int random = Random.Range(0, 1000);
        RaycastHit hit;
        switch (State)
        {
            case 0://idle State
                NPCAnimation.SetInteger("State", State);
                if (random > 990)
                {
                    transform.Rotate(transform.up * 45, Space.World);
                    State = 1;
                }
                break;
            case 1://run1 State
                if (Physics.Raycast(transform.position, Ray.transform.forward, out hit))
                {
                    //Debug.Log(hit.distance);
                    if (hit.distance > 5.0f)
                    {
                        if (random > 3)
                        {
                            transform.Translate(Vector3.forward * speed * Time.deltaTime);
                            NPCAnimation.SetInteger("State", State);
                        }
                        else State = 0; //2%의 확률로 걷다가 idle상태로 넘어간다
                    }
                    else
                        State = 0;
                }
                else
                    State = 0;

                break;
            case 2://run2 State
                
                if (Physics.Raycast(transform.position, Ray.transform.forward, out hit))
                {
                    if (hit.distance > 5.0f)
                    {
                        transform.Translate(Vector3.forward * speed * 2 * Time.deltaTime);
                        NPCAnimation.SetInteger("State", State);
                        if (StateReserved == false)
                        {
                            StateReserved = true;
                            StartCoroutine(reserveState(1));
                        }
                    }
                    else//방향 전환 후 도망
                    {
                        transform.Rotate(transform.up * 180, Space.World);
                    }
                }
                
                break;
            case 3://scared State
                NPCAnimation.SetInteger("State", State);
                break;
        }

        
        
        foreach (GameObject p in Player)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, p.transform.position);
            if (distanceToPlayer < range) State = 2; //주변에 드론이 접근하면 도망간다.
        }

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    IEnumerator reserveState(int s)
    {
        yield return new WaitForSeconds(7.0f);
        State = s;
        StateReserved = false;
    }
}
