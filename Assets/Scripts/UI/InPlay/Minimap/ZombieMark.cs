using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMark : MonoBehaviour {

    public GameObject target;
    Enemy E;

    void Update()
    {
        if (target != null)
        {
            if (E.isDead) transform.gameObject.SetActive(false); //타겟이 제거되면 미니맵에서도 제거
            transform.position = new Vector3(target.transform.position.x, 297f, target.transform.position.z);
        }
    }
    public void SetTarget(GameObject T)
    {
        E = T.GetComponent<Enemy>();
        target = T;
    }
}
