using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour {
    
    Animator ObjectAni;
    GameObject dropItem;
    public bool isBreakable;
    public Playenv playEnvironment;

    private void Awake()
    {
        ObjectAni = GetComponent<Animator>();
        isBreakable = true;
        if (transform.parent != null) setComponent();
    }
    public void setComponent()
    {
        dropItem = transform.parent.GetChild(1).gameObject;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player") && isBreakable)
        {
            playEnvironment.IncreaseScore(9, 1);
            isBreakable = false;
            //gameObject.GetComponent<BoxCollider>().isTrigger = true;
            ObjectAni.SetBool("IsBreak", true);
            dropItem.SendMessage("GenerateItem");
            transform.parent.SendMessage("Restore");
            StartCoroutine(ActiveCTRL());
            
        }
    }
    IEnumerator ActiveCTRL()
    {
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }


}
