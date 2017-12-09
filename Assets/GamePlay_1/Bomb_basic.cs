using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_basic : MonoBehaviour {
    public Animator BombAni;
    public float power = 10.0f;
    public float radius = 5.0f;
    public float upForce = 1.0f;

	// Use this for initialization
	void Start () {
        //Invoke("Detonate", 3);

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Enemy"))
        {
            print("데미지 전달");
            other.SendMessage("Damaged", 10.0f);
            //BombAni.SetBool("Bombed", true);
            //other.SendMessage("Damaged", 5.0f);
            //other.GetComponent<Animator>().enabled = false;
            
        }
    }

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
}
