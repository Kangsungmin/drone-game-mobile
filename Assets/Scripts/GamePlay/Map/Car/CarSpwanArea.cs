using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpwanArea : MonoBehaviour {
    public GameObject[] CarList = new GameObject[5]; 
    // Use this for initialization
    void Start () {
        StartCoroutine(SpwanCar(0.0f));
    }
	
    IEnumerator SpwanCar(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject instance = Instantiate(CarList[0], transform.position, transform.rotation);
        instance.transform.SetParent(transform);
    }
}
