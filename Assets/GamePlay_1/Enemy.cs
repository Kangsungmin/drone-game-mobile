using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour {
    protected bool isDead = false, AttackReady = true;
    public string State = "Idle";
    public Animator EnemyAnimator;
    public BoxCollider boxcoll;
    protected int score, money;
    protected float Speed, Power;
    protected float HP, Max_HP;
    protected Environment environment;
    protected GameObject Target;

    protected Transform myTransform;
    protected Transform playerTransform;
    protected NavMeshAgent nvAgent;
}
