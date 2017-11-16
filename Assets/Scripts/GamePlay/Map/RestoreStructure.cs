using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreStructure : MonoBehaviour
{
    public GameObject RestoreTarget;
    // Use this for initialization\

    private void Awake()
    {
        RestoreTarget = transform.GetChild(0).gameObject;
    }
    
    public IEnumerator Restore()
    {
        yield return new WaitForSeconds(10.0f);
        RestoreTarget.SetActive(true);
        RestoreTarget.GetComponent<Animator>().SetBool("IsBreak", false);
        RestoreTarget.GetComponent<BreakableObject>().isBreakable = true;
    }
}
