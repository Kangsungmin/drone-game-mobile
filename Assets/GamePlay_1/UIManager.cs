using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    public Drone Player;
    public Environment environment;
    public GameObject MainHuman;

    //========스코어 변수==========
    public Text ScoreText, MoneyText, AchivementText;
    public Animator ScoreAni, AchiveAni;
    //========스코어 변수==========

    //========게임종료 변수===========
    public Text ResultScore, ResultTime;
    //========게임종료 변수===========

    public GameObject GameEndPanel, ShopPanel, MissionPanelButtons, PauseMenu, GaugeUI, LiftButtonBG, LiftButton, BatteryChangeButton;
    public Image LeftJoystick, RightJoystick, FuelBar, MainHPBar;
    public GameObject LiftButtonAni;
    private float Fuel = 0.0f, MaxFuel= 0.0f, Main_HP = 100.0f, Main_MaxHP = 100.0f;

    private void Awake()
    {
        BatteryChangeButton.SetActive(false);
    }

    public void SetReference(GameObject[] Refs)//Environment에서 호출
    {
        Player = Refs[0].GetComponent<Drone>();
        MainHuman = Refs[1];

        GameObject[] tempRefs = new GameObject[3];
        tempRefs[0] = Refs[0];//Drone Object
        tempRefs[1] = Refs[0].transform.Find("Claw").gameObject;//Claw
        tempRefs[2] = LiftButtonBG;//리프팅 버튼 Ani
        LiftButton.SendMessage("SetReference", tempRefs);//드론, Claw, GrabModeCtrl

        tempRefs = new GameObject[1];
        tempRefs[0] = Refs[0];
        BatteryChangeButton.transform.GetChild(0).SendMessage("SetReference", tempRefs);
    }

    void Start () {
    }

    // Update is called once per frame
    void Update()
    {
        if (!Environment.GameOver)
        {
            ScoreText.text = environment.MissionScore.ToString();
            MoneyText.text = environment.AmountMoney.ToString();
            Fuel = Player.Fuel;
            MaxFuel = Player.Max_Fuel;
            FuelBar.fillAmount = (float)Fuel / (float)MaxFuel;
            if (Fuel < 20) FuelBar.color = new Color32(255, 66, 66, 208);
            else FuelBar.color = new Color32(255, 255, 169, 208);

            Main_HP = environment.Main_HP;
            Main_MaxHP = environment.Main_MaxHP;
            MainHPBar.fillAmount = (float)Main_HP / (float)Main_MaxHP;
            if (Main_HP < 20) MainHPBar.color = new Color32(255, 66, 66, 208);
            else MainHPBar.color = new Color32(214, 214, 214, 255);

            //MainHuman과 Drone의 거리를 재서 배터리 교체버튼 활성화 여부 결정
            if (Vector3.Distance(MainHuman.transform.position, Player.transform.position) < 3.0f) BatteryChangeButton.SetActive(true);
            else BatteryChangeButton.SetActive(false);
        }
    }

    public void BuyItem(string name)
    {
        switch (name)
        {
            case "Drink":
                if (environment.AmountMoney >= 50)
                {
                    environment.AmountMoney -= 50;
                    environment.Main_HP += 20.0f;
                }
                break;
            case "Box":
                if (environment.AmountMoney >= 20)
                {
                    environment.AmountMoney -= 20;
                    Player.SendMessage("SpawnItem", name);
                }
                break;
            case "Concrete":
                if (environment.AmountMoney >= 50)
                {
                    environment.AmountMoney -= 50;
                    Player.SendMessage("SpawnItem", name);
                }
                break;
            case "Container":
                if (environment.AmountMoney >=5)
                {
                    environment.AmountMoney -= 5;
                    Player.SendMessage("SpawnItem", name);
                }
                break;

        }
    }
    public void ShopCall()
    {
        if(ShopPanel.activeSelf) ShopPanel.SetActive(false);
        else ShopPanel.SetActive(true);

    }
    public void IncreaseScoreAni()
    {
        ScoreAni.Play("IncreaseScore", 0, 0.0f);
    }
    

    public void OnPause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinuePlay()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ForceEnd()
    {
        environment.GameEnd();
    }

    public void GameEnd(int getScore, Dictionary<int, int> getPartID)
    {
        
        GameEndPanel.SetActive(true);
        ResultScore.text = getScore.ToString();

    }

    public void ExitGamePlay()
    {
        SceneManager.LoadScene("Menu");
    }

}
