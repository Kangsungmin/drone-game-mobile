using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MissionReward : MonoBehaviour {
    public Text StageGetMoney;
    public Text NowLevel;
    public Text NowExp;
    public Text NowStage;

    public void ViewUpdate(string stage, int getMoney, int nowLevel, int nowExp, Dictionary<int, int> getPartsList)
    {
        StageGetMoney.GetComponent<Text>().text = getMoney.ToString();
        NowLevel.GetComponent<Text>().text = nowLevel.ToString();
        NowExp.GetComponent<Text>().text = nowExp.ToString();
        NowStage.GetComponent<Text>().text = "스테이지."+stage;
        //획득한 파츠 리스트를 출력한다.

    }
	

}
