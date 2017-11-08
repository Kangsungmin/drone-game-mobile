using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCluster : MonoBehaviour {
    Rigidbody thisRB;
    GameObject HidItem;
    // Update is called once per frame

    private void Awake()
    {
        thisRB = GetComponent<Rigidbody>();
        HidItem = transform.GetChild(0).gameObject;
        HidItem.SetActive(false);
    }
    void OnCollisionEnter(Collision collision)
    {
        thisRB.isKinematic = true;
        HidItem.SetActive(true);
        GetComponent<Collider>().isTrigger = true;
    }

}
