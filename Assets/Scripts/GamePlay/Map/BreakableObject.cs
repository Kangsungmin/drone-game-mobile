using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour {
    Animator ObjectAni;
    public DropItem dropItem;
    // Use this for initialization

    private void Awake()
    {
        ObjectAni = GetComponent<Animator>();
        
    }
    /*
    void OnTriggerEnter(Collider col)
    {
        if (col.tag.Contains("Player"))
        {
            ObjectAni.SetBool("IsBreak", true);
        }
    }*/

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            ObjectAni.SetBool("IsBreak", true);
            dropItem.GenerateItem();
        }
    }

}
