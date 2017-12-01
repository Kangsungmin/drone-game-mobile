using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {
    const int EXIT = -1, DIE = 0, IDLE = 1, ATTACKED = 2;

    public DroneCamera D_CAM;
    GameObject PlayerDrone, Enemy;
    public GameObject MainHuman;
    //======UI GameObject=======
    public GameObject moveJoystickLeft, moveJoystickRight;
    public UIManager ui_Manager;
    //======UI GameObject=======

    //======Env variable========
    public static bool GameOver = false;
    public static int EnemyCount = 0;
    //======Env variable========
    //======Main Human Variable=======
    private int MainState = IDLE;
    public float Main_HP = 100.0f, Main_MaxHP = 100.0f;
    //======Main Human Variable=======

    public int AmountMoney=0, MissionScore=0;
    public Dictionary<int, int> NowGetParts = new Dictionary<int, int>();//지금까지 얻은 아이템

    public Transform DroneSpawn, EnemySpawn;
    private void Awake()
    {
        DroneSpawn = transform.Find("DroneSpawnPoint");
        if (PlayerDataManager.nowUsingModel != null) PlayerDrone = Resources.Load("Prefabs/Drones/Drone_" + PlayerDataManager.nowUsingModel.getTitle()) as GameObject;
        else PlayerDrone = Resources.Load("Prefabs/Drones/Drone_Beginner") as GameObject;
        PlayerDrone = Instantiate(PlayerDrone, DroneSpawn.position, DroneSpawn.localRotation);
        GameObject[] tempRefs = new GameObject[4];//Env, UI, 조이스틱L,R
        tempRefs[0] = gameObject;
        tempRefs[1] = ui_Manager.gameObject;
        tempRefs[2] = moveJoystickLeft;
        tempRefs[3] = moveJoystickRight;
        PlayerDrone.SendMessage("SetReference", tempRefs);//파라미터 : 게임오브젝트배열

        tempRefs = new GameObject[2];
        tempRefs[0] = PlayerDrone;
        tempRefs[1] = MainHuman;
        ui_Manager.SendMessage("SetReference", tempRefs);



        //================아이템 변수 초기화==================
        NowGetParts.Add(1, 0);
        NowGetParts.Add(2, 0);
        NowGetParts.Add(3, 0);
        NowGetParts.Add(4, 0);
        NowGetParts.Add(5, 0);
        //================아이템 변수 초기화==================

        //===========================적 생성========================================
        //첫 번째 웨이브
        float[] pas = new float[4];
        pas[0] = 1.0f;//Enemy 종류
        pas[1] = 1.0f;//시간
        pas[2] = 2.0f;//적 수
        pas[3] = 4.0f;//남은 웨이브 호출
        StartCoroutine(EnemyWave(pas));

        //두번째 웨이브
        pas = new float[4];
        pas[0] = 2.0f;//Enemy 종류
        pas[1] = 80.0f;//시간
        pas[2] = 3.0f;//적 수
        pas[3] = 5.0f;//남은 웨이브 호출
        StartCoroutine(EnemyWave(pas));
        //===========================적 생성========================================

        //===========================장애물 생성====================================

        //===========================장애물 생성====================================
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //적의 수를 확인하고 리스폰 한다.

        switch (MainState)
        {
            case EXIT:
                break;
            case DIE:
                GameEnd();
                MainState = EXIT;
                break;
            case IDLE:
 
                break;
            case ATTACKED:
                
                break;
        }
    }

    IEnumerator EnemyWave(float[] pas)
    {
        yield return new WaitForSeconds(pas[1]);
        switch ((int)pas[0])
        {
            case 1:
                Enemy = Resources.Load("Prefabs/Enemy/Enemy_kid") as GameObject;
                break;
            case 2:
                Enemy = Resources.Load("Prefabs/Enemy/Enemy_adult") as GameObject;
                break;
            case 3: break;
            case 4: break;
        }
        
        GameObject[] tempRefs = new GameObject[2];
        tempRefs[0] = gameObject;
        tempRefs[1] = MainHuman;
        int NumofEnemy = (int) pas[2];
        for (int i = 0; i < NumofEnemy; i++)
        {
            float temp_x = Random.Range(-90.0f, 90.0f);
            int sign = Random.value < .5 ? -1 : 1;
            float temp_z = sign * Mathf.Sqrt(8100.0f - Mathf.Pow(temp_x, 2.0f) ); //원의 방정식, x를 랜덤하게 설정 y는 원의 반지름(90)에 의해 자동으로 결정
            Vector3 temp = new Vector3(temp_x, 1, temp_z);

            Enemy = Instantiate(Enemy, temp, Quaternion.identity);
            EnemyCount++;
            Enemy.SendMessage("SetReference", tempRefs);
        }
        pas[1] = 15.0f;
        pas[2] += 2;
        pas[3]--;//다음 웨이브 호출 가능 수 감소
        if(pas[3] > 1)
        {
            StartCoroutine(EnemyWave(pas));
        }
        
    }

    public void IncreaseScore(int amount, int type)//점수와 업적
    {
        MissionScore += amount;
    }

    public void IncreaseMoney(int amount)
    {
        AmountMoney += amount;
    }

    public void AttackMain(float damage)
    {
        if (MainState > DIE)
        {
            MainState = ATTACKED;
            Main_HP -= damage;
            if (Main_HP <= 0.0f) MainState = DIE;
        }
    }

    public void GameEnd()
    {
        PlayerDrone.GetComponent<Drone>().GameOver = true;
        PlayerDrone.GetComponent<Drone>().DronePowerOn = false;
        PlayerDrone.GetComponent<Drone>().DroneAnimator.enabled = false;
        //서버 통신
        StartCoroutine(MissionClear_To_DB(MissionScore, NowGetParts));
        //UI 표시
        ui_Manager.GameEnd(MissionScore, NowGetParts);

        googleexample ge = new googleexample();
        ge.ShowLeaderBoardUI((long)MissionScore, "CgkIhNHUuPIJEAIQBg");
    }

    IEnumerator MissionClear_To_DB(int getScore, Dictionary<int, int> getPartID)
    {
        int getExp = 0, amountMoney=0;
        WWWForm form = new WWWForm();
        form.AddField("clearEXPPost", getExp);
        form.AddField("clearMoneyPost", amountMoney);
        form.AddField("userIDPost", PlayerDataManager.userID);

        WWW data = new WWW("http://13.124.188.186/mission_clear.php", form);
        yield return data;

        string user_Data = data.text;
        if (!user_Data.Equals(""))
        {
            PlayerDataManager.exp += getExp;
            PlayerDataManager.money += amountMoney;
            PlayerDataManager.exp += getExp;
            PlayerDataManager.money += amountMoney;
        }
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
        }
    }


}
