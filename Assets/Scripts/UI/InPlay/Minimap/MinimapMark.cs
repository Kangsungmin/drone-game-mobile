using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapMark : MonoBehaviour {
    public GameObject target;

    void Update()
    {
        if (target != null)
        {
            if (target.activeSelf == false) transform.gameObject.SetActive(false); //타겟이 제거되면 미니맵에서도 제거
            transform.position = new Vector3(target.transform.position.x, 1248, target.transform.position.z);
            //transform.forward = target.transform.forward;
        }
    }
}
