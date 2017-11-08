using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rating : MonoBehaviour {
    public GameObject[] Star = new GameObject[3];
    bool showInterval;
    public void SetRate(int Rate)
    {
        showInterval = false;
        switch (Rate)
        {
            case 0: break;
            case 1:
                StartCoroutine(Interval(0, 0f));
                break;
            case 2:
                StartCoroutine(Interval(0, 0f));
                StartCoroutine(Interval(1, 0.7f));
                break;
            case 3:
                StartCoroutine(Interval(0, 0f));
                StartCoroutine(Interval(1, 0.7f));
                StartCoroutine(Interval(2, 1.4f));
                break;
        }
    }
    
    IEnumerator Interval(int i,float time)
    {
        yield return new WaitForSeconds(time);
        Star[i].GetComponent<Animator>().SetInteger("Show", 1);
    }

}
