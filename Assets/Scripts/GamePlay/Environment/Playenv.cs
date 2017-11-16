using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 * 게임전체적인 환경을 관리한다. 
 * 스테이지 관리
 * UI버튼 관리
 * 스크린 사이즈 관리
 */
public class Playenv : MonoBehaviour
{
    UIscripts UIManager;
    public string MapName;
    public int StageLevel;
    public GameObject MissionPanel, RatingView, MissionClearView;
    GameObject PlayerDrone;
    GameObject DroneBoxSearcher;
    public Text MissionExplainText;
    public static int SpawnBoxCount;
    //public int MissionCount;//상자를 넣은 수.
    
    public int AmountMoney;//획득한 돈
    //public List<int> NowGetParts = new List<int>();
    public Dictionary<int, int> NowGetParts = new Dictionary<int, int>(); //<Key, value>
    public static bool GameOver;
    public SceneFader fader;
    //박스아이템
    public GameObject ItemBox, ItemCluster;
    public GameObject MiniMapMarkManager;
    //미션클리어UI
    public GameObject MissonBackground, Title_Mission, Title_Clear, SubTitle, MissionClearPanel;
    // Use this for initialization
    GameObject AllNpc, AllItems, AllSpwanArea;
    GameObject DroneSpawn;
    //=====컨텐츠 변수===============//
    //-----박스 퀘스트-------//
    public Vector3 BoxSpPoint;
    public Transform[] ReceiverNPCs = new Transform[8];
    Transform BoxReceiver;
    //-----박스 퀘스트-------//


    //=====컨텐츠 변수===============//

    //=====스코어, 업적 변수=========//
    public int MissionScore;
    public int[] Achivement = new int[10]; //[0]: ,[1]: 기물파손, [2]: 자동차 추돌

    //=====스코어, 업적 변수=========//

    //=====튜토리얼 변수=======//
    public GameObject TutorialArrow;
    //=====튜토리얼 변수=======//

    void Awake(){
        Time.timeScale = 1;
        GameOver = false;
        SpawnBoxCount = 0;
        //MissionCount = 0;
        MissionScore = 0;
        Screen.SetResolution(1280, 800, true);
        UIManager = GameObject.Find("UI").GetComponent<UIscripts>();
        AllNpc = GameObject.Find("KeyNpcs");
        AllItems = GameObject.Find("KeyItems");
        AllSpwanArea = GameObject.Find("SpawnAreas");

        MapName = SceneData.MapName;//현재 맵 이름
        StageLevel = int.Parse(SceneData.SceneLevelName);//현재 스테이지 레벨
        Debug.Log("맵 이름 : " + MapName + "\n스테이지 레벨: " + StageLevel);

        NowGetParts.Add(1, 0);
        NowGetParts.Add(2, 0);
        NowGetParts.Add(3, 0);
        NowGetParts.Add(4, 0);
        NowGetParts.Add(5, 0);

        //자신이 현재 선택한 드론을 생성한다.
        /*
         * ==============현재 스테이지에 필요한 오브젝트만 활성화 시킨다.[시작]================ 
         * NPC 활성화/비활성화
         * Enemy 활성화/비활성화
         * Item 활성화/비활성화
         */
        for (int i = 0; i < AllNpc.transform.childCount; i++)
        {
            Transform StageNpcs = AllNpc.transform.GetChild(i);
            if (!StageNpcs.name.Equals("Stage" + StageLevel)) StageNpcs.gameObject.SetActive(false);
        }
        for (int i = 0; i < AllItems.transform.childCount; i++)
        {
            Transform StageItems = AllItems.transform.GetChild(i);
            if (!StageItems.name.Equals("Stage" + StageLevel)) StageItems.gameObject.SetActive(false);
        }
        for (int i = 0; i < AllSpwanArea.transform.childCount; i++)
        {
            Transform StageSpawnArea = AllSpwanArea.transform.GetChild(i);
            if (!StageSpawnArea.name.Equals("Stage" + StageLevel)) StageSpawnArea.gameObject.SetActive(false);
        }
        DroneSpawn = GameObject.Find("DroneSpawnArea");
        PlayerDrone = Resources.Load("Prefabs/Drones/Drone_" + PlayerDataManager.nowUsingModel.getTitle()) as GameObject;
        PlayerDrone  = Instantiate(PlayerDrone, DroneSpawn.transform.position, DroneSpawn.transform.localRotation);
        //==============현재 스테이지에 필요한 오브젝트만 활성화 시킨다.[끝]==================

    }
    void Start()
    {
        DroneBoxSearcher = PlayerDrone.transform.Find("Claw").gameObject;
        switch (StageLevel)
        {
            case 1:
                //미션 설명
                //UIscripts.CountDown = 300.0f;
                GameObject obj = GameObject.FindGameObjectWithTag("Box");
                DroneBoxSearcher.SendMessage("AddBoxList", obj);//Grab스크립트 Box리스트에 추가
                //튜토리얼 설명을 모두 끝내면 가이드 화살표 드론에 생성
                
                break;
            case 2:
                MissionPanel.SetActive(true);
                //UIscripts.CountDown = 90.0f;
                MissionExplainText.text = "살아남아서 최대한 많은 스코어를 획득하세요.";
                TutorialArrow = Instantiate(TutorialArrow, DroneSpawn.transform.position, DroneSpawn.transform.rotation);
                ArrowOff(true);
                break;
        }
    }

    void Update()
    {

        if (SpawnBoxCount < 1)
        {
                //현재 맵에 gen된 상자의 수를 확인한다. 일정 수 이하면 박스를 랜덤한 맵 위치에 생성한다.
                //Vector3 BoxSpawnPos = new Vector3(Random.Range(-170.0f, 170.0f), 79.0f, Random.Range(-170.0f, 170.0f));
                GameObject obj = Instantiate(ItemBox, BoxSpPoint, Quaternion.identity);//랜덤한 위치에 박스 생성
                obj.name = "Box";
                DroneBoxSearcher.SendMessage("AddBoxList", obj);//Grab스크립트 Box리스트에 추가
                MiniMapMarkManager.SendMessage("BoxGened", obj);//미니맵에도 박스 추가
                SpawnBoxCount++;

            /*
                Vector3 ItemClusterSpawnPos = new Vector3(Random.Range(-170.0f, 170.0f), 79.0f, Random.Range(-170.0f, 170.0f));
                obj = Instantiate(ItemCluster, ItemClusterSpawnPos, Quaternion.identity);//랜덤한 위치에 코인생성
                obj.name = "ItemCluster";
            */
        }
    }
    //==============================미션 클리어[시작]====================================
    public void MissionEnd(int Rate, int getExp, int amountMoney, Dictionary<int, int> getPartID)
    {
        //============보상[시작]=====================================================
		StartCoroutine (MissionClear_To_DB (getExp, amountMoney, getPartID));
        //============보상[끝]==================================17.09.04 성민 최종수정
    }
    //==============================미션 클리어[끝]=============17.09.06 성민 최종수정

	IEnumerator MissionClear_To_DB(int getExp, int amountMoney, Dictionary<int, int> getPartID)
	{
		WWWForm form = new WWWForm ();
		form.AddField ("clearEXPPost", getExp);
		form.AddField ("clearMoneyPost", amountMoney);
		form.AddField ("userIDPost", PlayerDataManager.userID);

		WWW data = new WWW ("http://13.124.188.186/mission_clear.php", form);
		yield return data;

		string user_Data = data.text;
		if (!user_Data.Equals("")) {
            //PlayerDataManager.playerDataManager.AddExp(getExp);//현재 경험치
            //PlayerDataManager.playerDataManager.IncreaseMoney(amountMoney);//얻은 돈
            print("호출");
            PlayerDataManager.exp += getExp;
            PlayerDataManager.money += amountMoney;

        }

        // 미션 후 얻은 parts DB에 업데이트 및 클라이언트 상에서 계산
        foreach (KeyValuePair<int, int> getparts in getPartID)
        {
            // ----------- 얻은 parts playerdatamanager(클라이언트 데이터) 데이터로 저장 -----
            PlayerDataManager.ownParts[getparts.Key] += getPartID[getparts.Key];
            // -------------------------------------------------------------------------------
            print(getPartID[getparts.Key] + " " + getparts.Key);
            form.AddField("itemIDPost", getparts.Key);
            form.AddField("itemNumPost", PlayerDataManager.ownParts[getparts.Key]);
            form.AddField("userIDPost", PlayerDataManager.userID);

            data = new WWW("http://13.124.188.186/mission_clear_items.php", form);
            yield return data;
            print(data.text);
        }
	}

    public void GameEnd()
    {
        GameOver = true;//시간초과 게임 끝
        //PlayerDrone.GetComponent<Rigidbody>().isKinematic = true;
        //정산
        switch (StageLevel)
        {
            
            case 1:
                //미션 완료 검사
                {
                    int getRate, getExp, getMoney;
                    getRate = 3;
                    getExp = 25;
                    getMoney = 10;
                    UIManager.MissionEnd(getRate, MissionScore, getExp, getMoney, NowGetParts); //점수,exp, money
                    MissionEnd(getRate, getExp, getMoney, NowGetParts);
                    break;
                }
            case 2:
                //미션 완료 검사
                {
                    int getRate, getExp;
                    if (MissionScore >= 300)
                    {
                        getRate = 3;
                    }
                    else if (MissionScore >= 200)
                    {
                        getRate = 2;
                    }
                    else {
                        getRate = 1;
                    }
                    getExp = (int)(MissionScore * 0.1);
                    UIManager.MissionEnd(getRate, MissionScore, getExp, AmountMoney, NowGetParts); //점수,exp, money
                    MissionEnd(getRate, getExp, AmountMoney, NowGetParts);
                    break;
                }
        }
    }

    //네비게이션을 비활성화 하고, 박스 수신자를 랜덤하게 섞는다.
    public void ArrowOff(bool suffle)
    {
        TutorialArrow.SetActive(false);//화살표 비활성화
        if(suffle) BoxReceiver = ReceiverNPCs[Random.Range(0, ReceiverNPCs.Length - 1)];
    }

    public void ActiveArrowToDestination()
    {
        //튜토리얼 화살표를 재활성화 시키고 목적지를 향하게 한다.
        TutorialArrow.GetComponent<TutorialArrow>().ReTargeting(BoxReceiver);
        TutorialArrow.SetActive(true);
    }

    public void PlayerDataUpdate()
    {
        GameObject.Find("UI").SendMessage("PlayerDataUiUpdate", NowGetParts);
    }
    public void IncreaseScore(int amount)
    {
        UIManager.IncreaseScoreAni();
        MissionScore += amount;
        if (MissionScore == 50)
        {
            UIManager.AchivementActive("50 스코어 달성\n당신 초보는 아니군요!");
        }
    }

    public void IncreaseScore(int amount, int type)
    {
        UIManager.IncreaseScoreAni();
        MissionScore += amount;
        Achivement[type]++;
        switch (type)
        {
            case 0: break;
            case 1:
                if(Achivement[type] == 10) UIManager.AchivementActive("도시 청소 1단계");
                else if (Achivement[type] == 5) UIManager.AchivementActive("도시 청소 2단계");
                break;
            case 2:
                if (Achivement[type] == 5) UIManager.AchivementActive("자동차 홈런 1단계");
                else if (Achivement[type] == 2) UIManager.AchivementActive("자동차 홈런 2단계");
                break;
            case 3: break;
        }
    }

    public void MoneyPlus(int getMoney)
    {
        AmountMoney += getMoney;
        GameObject.Find("UI").SendMessage("PlayerNumCoinUpdate", AmountMoney.ToString());
    }
    public void AddParts(int[] PartIdList)
    {
        for (int i=0; i < PartIdList.Length; i++)
        {
            print(PartIdList[i]);
            //NowGetParts.Add(PartIdList[i], NowGetParts[i]++ );// i번 아이템을 +1 한다.
            NowGetParts[PartIdList[i]] = NowGetParts[PartIdList[i]] + 1;
        }
        //튜토리얼일 때, 바로 게임 종료시킴
        if(StageLevel == 1) GameEnd();
    }
    public void ExplainOk()
    {
        MissionPanel.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        UIManager.OnPause();
    }

    public void startMenu()
    {
        Application.LoadLevel("Menu");
    }

    public void ForceGameEnd()
    {
        Time.timeScale = 1;
        UIManager.PauseMenu.SetActive(false);
        GameEnd();
        //int getRate, getExp;
        //getRate = 3;
        //getExp = (int)(MissionScore * 0.1f);
        //UIManager.MissionEnd(getRate, MissionScore, getExp, AmountMoney, NowGetParts); //점수,exp, money
        //MissionEnd(getRate, getExp, AmountMoney, NowGetParts);
    }

    public void retrytGame()
    {
        print("게임 씬 호출");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}