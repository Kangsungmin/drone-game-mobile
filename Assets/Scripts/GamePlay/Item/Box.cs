using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Box : MonoBehaviour {
    protected string Title;
    public List<Part> ContainParts = new List<Part>();

    abstract public int[] PartIdList();
}
