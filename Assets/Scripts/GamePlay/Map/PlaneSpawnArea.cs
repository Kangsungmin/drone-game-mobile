using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawnArea : MonoBehaviour {
    public GameObject[] PlaneObject;
    Animator SpawnPointMove;
    private void Awake()
    {
        SpawnPointMove = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start()
    {
        StartCoroutine(SpwanPlane(0.0f));
    }

    IEnumerator SpwanPlane(float time)
    {
        SpawnPointMove.enabled = true;
        yield return new WaitForSeconds(time);
        SpawnPointMove.enabled = false;
        GameObject instance = Instantiate(PlaneObject[0], transform.position, transform.rotation);
        instance.transform.SetParent(transform);
    }
}
