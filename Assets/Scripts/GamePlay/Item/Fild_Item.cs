using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fild_Item : MonoBehaviour {
    public int ID = 0;//디폴트 '0'= 동전 / '-1' = 필드 아이템
    public GameObject child_Item;
    AnimationScript ItemAni;
    Item item;
    float speed = 90.0f;
    bool Targeted;
    // Update is called once per frame
    private void Awake()
    {
        if (ID == -1) ItemReGen(); //필드아이템이므로 동전생성
    }

    void OnTriggerEnter(Collider target)
    {
        if (target.tag.Contains("Player") && !Targeted)
        {
            ItemAni.stopFloating();
            item.MoveToTarget(target.gameObject);
            Targeted = true;
        }
    }

    public void ItemReGen()
    {
        Targeted = false;
        //하위 스코어 획득 아이템 생성(플레이어 레벨로 정해진다.)
        //아이템 획득량의 경우 플레이어 레벨에 따라 달라짐(개발예정)

        if (ID < 1)//필드일 때
        {
            if (Random.Range(1, 100) < 41) child_Item = transform.GetChild(0).gameObject;
            else child_Item = transform.GetChild(1).gameObject;
        }
        else if (ID == 1)//신호등일때
        {
            if (Random.Range(1, 100) < 81) child_Item = transform.GetChild(0).gameObject;//동전
            else child_Item = transform.GetChild(2).gameObject;//플라스틱
        }
        else if (ID == 2)//신호등일때
        {
            if (Random.Range(1, 100) < 91) child_Item = transform.GetChild(0).gameObject;//동전
            else child_Item = transform.GetChild(3).gameObject;//아이언
        }
        else//기타 오브젝트일 때
        {
            child_Item = transform.GetChild(0).gameObject;
        }

        child_Item.transform.localPosition = Vector3.zero;
        child_Item.SetActive(true);
        ItemAni = child_Item.GetComponent<AnimationScript>();
        item = child_Item.GetComponent<Item>();
    }

    IEnumerator ReserveReGen(float time)
    {
        yield return new WaitForSeconds(time);
        ItemReGen();
    }

    public void AwakeDrop(int id)
    {
        ID = id;
        ItemReGen();
    }
    
}
