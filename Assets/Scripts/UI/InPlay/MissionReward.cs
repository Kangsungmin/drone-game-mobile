using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MissionReward : MonoBehaviour {
    public Text EndScore;
    public Text StageGetMoney;
    public Text NowExp;
    public Text NowStage;

    public void ViewUpdate(string stage, int endScore, int getMoney, int nowLevel, int nowExp, Dictionary<int, int> getPartsList)
    {
        EndScore.text = endScore.ToString();
        StageGetMoney.text = getMoney.ToString();
        NowExp.text = nowExp.ToString();
        NowStage.text = "스테이지."+stage;
    }
	

}
