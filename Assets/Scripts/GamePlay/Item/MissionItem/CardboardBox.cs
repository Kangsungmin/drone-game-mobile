using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardBox : Box {
    private void Awake()
    {
        Title = this.name;
        //CardboardBox는 3개의 아이템을 가진다.
        for (int i=0; i < 3; i++)
        {
            int val = Random.Range(1,100);
            int id = 1;

            if (val > 90) id = 3;// 10%
            else if (val > 70) id = 2;// 20%
            else id = 1;// 70%

            ContainParts.Add(new Part(id));
        }
    }

    public override int[] PartIdList()
    {
        int[] IdList = new int[ContainParts.Count];
        for (int i=0; i< IdList.Length; i++)
        {
            IdList[i] = ContainParts[i].getID();
        }
        return IdList;
    }
}
