using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid_Balloon : MonoBehaviour {
    Animator KidAni;
    Playenv playEnvironment;
    public ParticleSystem PopEffect;
    private void Awake()
    {
        KidAni = GetComponent<Animator>();
        playEnvironment = GameObject.FindGameObjectWithTag("ENV").GetComponent<Playenv>();
    }
	

    public void OnTriggerEnter(Collider target)
    {
        if (target.tag.Contains("Player"))
        {
            KidAni.SetInteger("State",1);
            PopEffect.Play();
            StartCoroutine(SetIdle());
            playEnvironment.IncreaseScore(30, 6);
        }
    }

    IEnumerator SetIdle()
    {
        yield return new WaitForSeconds(10.0f);
        KidAni.SetInteger("State", 0);
    }
}
