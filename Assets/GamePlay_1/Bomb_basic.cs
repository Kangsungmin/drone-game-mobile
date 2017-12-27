using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_basic : MonoBehaviour {
    private AudioSource _audio;
    public AudioClip ExpSound;
    public ParticleSystem ExpBasic;
    public Transform Exp;
    public Animator BombAni;
    public float power = 25.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;
    private MeshRenderer mesh;
    public BoxCollider col;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        mesh = GetComponent<MeshRenderer>();
        _audio.clip = ExpSound;
        _audio.loop = false;
        ExpBasic.Stop();
        Invoke("Destroy",1.5f);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ;
        }
        else if (other.CompareTag("Bomb"))
        {
            ;
        }
        else if (other.CompareTag("Enemy"))
        {
            
            _audio.Play();
            Exp.SetParent(null);
            ExpBasic.Play();
            other.SendMessage("Damaged", power);
            //gameObject.SetActive(false);
            Destroy();
        }
        else
        {
            
            ExpBasic.Play();
            _audio.Play();
            Destroy();
        }
    }

    /*
    void Detonate()
    {
        //print("폭발");
        Collider[] collider = Physics.OverlapSphere(transform.position, radius);//주변 콜라이더 탐색
        foreach (Collider hit in collider)
        {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(power, transform.position, radius, upForce, ForceMode.Impulse);
                }
        }
    }
    */

    public void Destroy()
    {
        mesh.enabled = false;
        col.enabled = false;
    }
}
