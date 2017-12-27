using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkGenerator : MonoBehaviour {
    public GameObject DroneMark, NpcMark, DropBoxMark, EnemyRedMark;
    GameObject[] Npcs, Boxes;
    GameObject[] Enemys;
    Quaternion MarkRot;
    //List<GameObject> EnemyRedMarks;
    // Use this for initialization
    private void Awake()
    {
        MarkRot.eulerAngles = new Vector3(90.0f,0,0); 
    }

    public void BoxGened(GameObject Box)//아이템이 새로 추가되어 미니맵에 표시할 때 호출한다.
    {
        GameObject temp = Instantiate(DropBoxMark, new Vector3(0, 297f, 0), Quaternion.identity);
        temp.GetComponent<MinimapMark>().target = Box;
    }
    public void DroneGened(GameObject Drone)//아이템이 새로 추가되어 미니맵에 표시할 때 호출한다.
    {
        GameObject temp = Instantiate(DroneMark, new Vector3(0, 297f, 0), MarkRot);
        temp.GetComponent<DroneMark>().target = Drone;
    }
    public void EnmeyGened(GameObject Enemy)//아이템이 새로 추가되어 미니맵에 표시할 때 호출한다.
    {
        GameObject temp = Instantiate(EnemyRedMark, new Vector3(0, 297f, 0), MarkRot);
        temp.GetComponent<ZombieMark>().SetTarget(Enemy);
    }
}
