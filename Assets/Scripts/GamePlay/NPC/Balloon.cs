using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {
    Kid_Balloon parent;
    private void Awake()
    {
        parent = transform.parent.parent.GetComponent<Kid_Balloon>();
    }
    void OnTriggerEnter(Collider target)
    {
        parent.OnTriggerEnter(target);
    }

}
