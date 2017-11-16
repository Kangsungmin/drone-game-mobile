using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class UIscripts : MonoBehaviour {
    GameObject Player;
    Playenv PlayEnvironment;
    //=====튜토리얼 변수=======//
    public GameObject TutorialPanel, TutorialButton;
    public int TutorialCount;
    //=====튜토리얼 변수=======//

    //=====스코어, 업적 변수=========//
    public Text ScoreText, AchivementText;
    public Animator ScoreAni, AchiveAni;
    //=====스코어 변수=========//

    public SceneFader SceneFadeManager;
    public GameObject PauseMenu;
    public GameObject EndViewPanel, EndDataViewPanel, MissionPanelButtons;
    public GameObject RatingView;
    public Image fireBtn, joystickLeft, joystickRight;
    //private float health = 100.0f, maxHealth = 100.0f;
    private float fuel = 100.0f, maxFuel = 100.0f;
    public Image fuelBar;

    
    public Text MissionClearEnglish, MissionClearKorean;
    //스톱워치
    bool MissonEnd;
    //public static Stopwatch stopwatch;
    //public static float CountDown = -1.0f;
    public Text NumBoxView, NumCoinView, NumPlasticView, NumIronView, NumAluminumView, NumSilverView;
    IEnumerator corutine;
    void Awake()
    {
        MissonEnd = false;
    }
    void Start ()
    {
		Player = GameObject.FindGameObjectWithTag ("Player");
        PlayEnvironment = GameObject.Find("PlayEnvironment").GetComponent<Playenv>();
        //healthBar = transform.Find("HP").Find("HP_bar").GetComponent<Image>();
        
        //현재 튜토리얼이면 튜토리얼 패널 활성화
        if (PlayEnvironment.GetComponent<Playenv>().StageLevel == 1)
        {
            TutorialPanel.SetActive(true);
            TutorialPanel.transform.GetChild(0).gameObject.SetActive(true);
            TutorialPanel.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine("WaitSceneFade");//씬페이드 끝날때 까지 대기
        }
    }
    // Update is called once per frame
    void Update () {
        if (!Playenv.GameOver)
        {
            ScoreText.text = PlayEnvironment.MissionScore.ToString();

            fuel = Player.GetComponent<Drone>().Fuel;
            maxFuel = Player.GetComponent<Drone>().Max_Fuel;
            fuelBar.fillAmount = (float)fuel / (float)maxFuel;
            if (MissonEnd)//미션 종료시
            {
                //Player.GetComponent<Rigidbody>().isKinematic = true;//드론 멈춤
                Player.GetComponent<Drone>().DronePowerOn = false;
            }
        }
        else//게임 오버 시,
        {

        }

    }

    public void MissionEnd(int Rate, int endScore, int getExp, int amountMoney, Dictionary<int, int> getPartsList)
    {
        //게임 결과창 애니메이션
        /*
         * 백그라운드 활성화
         * Mission & Subtitle 활성화 
         * Clear & 패널 활성화 (점차)
         */
        EndDataViewPanel.SetActive(true);
        EndViewPanel.GetComponent<Animator>().SetBool("Active", true);
        object[] parms = new object[2]{ MissionPanelButtons, 2.3f };
        corutine = UiActiveReserve(parms);
        StartCoroutine(corutine);//미션 버튼 2.5초 후 활성화
        //RatingView.GetComponent<Rating>().SetRate(Rate);//별 개수
        
        //============보상출력[시작]=================================================
        RatingView.GetComponent<Rating>().SetRate(Rate);//별 개수
        EndViewPanel.GetComponent<MissionReward>().ViewUpdate(SceneData.SceneLevelName, endScore, amountMoney, PlayerDataManager.level, getExp, getPartsList);//돈과 파츠는 현재 획득한것을 보여준다.
        //============보상출력[끝]==============================17.11.06 성민 최종수정
        MissonEnd = true;
    }

    public void PlayerDataUiUpdate(Dictionary<int, int> ItemList)
    {
        NumBoxView.text = ItemList[1].ToString();
        NumPlasticView.text = ItemList[2].ToString();
        NumIronView.text = ItemList[3].ToString();
        NumAluminumView.text = ItemList[4].ToString();
        NumSilverView.text = ItemList[5].ToString();
    }
    public void PlayerNumBoxUpdate(string val)
    {
        NumBoxView.text = val;
    }
    public void PlayerNumCoinUpdate(string val)
    {
        NumCoinView.text = val;
    }

    //UiActiveReserve(object[] parms) : 게임오브젝트를 예약하여 해당시간 후에 활성화시키는 함수
    //ㄴ>첫번째 인자(parms[0]) : 게임오브젝트, 두번째 인자(parms[1]) : 시간
    IEnumerator UiActiveReserve(object[] parms)
    {
        yield return new WaitForSeconds((float)parms[1]);
        GameObject reservedObject = (GameObject) parms[0];
        reservedObject.SetActive(true);
    }

    public void IncreaseScoreAni()
    {
        //ScoreAni.SetBool("INC", true);
        ScoreAni.Play("IncreaseScore", 0, 0.0f);
        //ScoreAni.SetBool("INC", false);
    }

    public void AchivementActive(string t)
    {
        AchivementText.gameObject.SetActive(true);
        AchivementText.text = t;
        AchiveAni.Play("AchivementText", 0, 0.0f);
    }
    

    public void OnPause()
    {
        PauseMenu.SetActive(true);
    }

    
    public void ContinuePlay()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    IEnumerator WaitSceneFade()
    {
        while (!SceneFadeManager.FadeEnd)
        {
            yield return true;
        }
        Time.timeScale = 0;
    }

    public void TutorialNext()
    {
        TutorialCount++;
        if (TutorialCount == 1)
        {
            //조이스틱 가이드를 끄고 게임을 진행한다.
            TutorialPanel.transform.GetChild(0).gameObject.SetActive(false);
            TutorialPanel.transform.GetChild(1).gameObject.SetActive(false);
            TutorialButton.SetActive(false);
            Time.timeScale = 1;
        }
        else if (TutorialCount == 2)
        {
            //박스리프트 조작 가이드를 켜고 게임을 중지시킨다.
            TutorialPanel.transform.GetChild(2).gameObject.SetActive(true);
            TutorialButton.SetActive(true);
            Time.timeScale = 0;
        }
        else if (TutorialCount == 3)
        {
            //박스리프트 조작 가이드를 끄고 게임을 재 진행한다.
            TutorialPanel.transform.GetChild(2).gameObject.SetActive(false);
            TutorialButton.SetActive(false);
            PlayEnvironment.SendMessage("ActiveArrowToDestination");//튜토리얼 가이드 화살표를 활성화 시킨다.
            Time.timeScale = 1;
        }
    }

}
