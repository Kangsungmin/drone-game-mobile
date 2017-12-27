using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMark : MonoBehaviour {

    public GameObject target;
    Quaternion markRot = Quaternion.identity;
    void Update()
    {
        if (target != null)
        {
            if (target.activeSelf == false) transform.gameObject.SetActive(false); //타겟이 제거되면 미니맵에서도 제거
            transform.position = new Vector3(target.transform.position.x, 297f, target.transform.position.z);
            markRot = Quaternion.Euler(0.0f, target.transform.eulerAngles.y, 0.0f);
            transform.rotation = markRot;
        }
    }
}
