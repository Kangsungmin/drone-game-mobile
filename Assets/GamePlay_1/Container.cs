using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour {
    float Durable = 50.0f;
    // Use this for initialization
    void Awake()
    {
        StartCoroutine(DiscountDurable(Durable));
    }

    IEnumerator DiscountDurable(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
