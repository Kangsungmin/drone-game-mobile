using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//SpawnPoint에서 호출
//랜덤한 방향(y축 랜덤 회전)으로 DropItem 프리팹을 생성한다.

public class DropItem : MonoBehaviour {
    GameObject drop;
    // Use this for initialization
    private void Awake()
    {
        drop = Resources.Load("Prefabs/Items/DropableItem") as GameObject;
    }
    public void GenerateItem()
    {
        for (int i=0; i<3; i++)
        {
            transform.Rotate(Vector3.up * Random.Range(0, 360));
            Instantiate(drop, transform.position, transform.rotation);
        }

    }
}
