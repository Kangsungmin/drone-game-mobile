using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fild_Item : MonoBehaviour {
    public GameObject child_Item;
    AnimationScript ItemAni;
    Item item;
    float speed = 90.0f;
    bool Targeted;
    // Update is called once per frame
    private void Awake()
    {
        ItemReGen();
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
        //40퍼센트의 확률로 돈, 60퍼센트의 확률로 스코어 아이템
        //스코어 아이템을 경우 플레이어 레벨에 따라 달라짐
        if (Random.Range(1, 100) < 41)
        {
            child_Item = transform.GetChild(0).gameObject;
        }
        else
        {
            int lev = PlayerDataManager.level;
            if (lev >= 20) child_Item = transform.GetChild(2).gameObject;
            else child_Item = transform.GetChild(1).gameObject;
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
    
}
