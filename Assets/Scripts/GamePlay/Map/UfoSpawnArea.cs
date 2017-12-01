using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoSpawnArea : MonoBehaviour {
    public GameObject ufo_Object;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(SpwanUfo(0.0f));
    }
    
    IEnumerator SpwanUfo(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject instance = Instantiate(ufo_Object, transform.position, transform.rotation);
        instance.transform.SetParent(transform);
    }
}
