using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {
    public GameObject Target;
    public GameObject playEnvironment;
    public bool isRegenerable = true;
    public abstract void MoveToTarget(GameObject T);
}
