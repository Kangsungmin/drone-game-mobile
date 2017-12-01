using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
//using Facebook.Unity;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class Menu : MonoBehaviour {
    public GameObject LevelMenu;
    public SceneFader fader;

    public Text SpannerView, MoneyView, LevelView, NicknameView;

    //public Text Path;
    //public AudioClip backMusic;

    void Awake()
    {
        Screen.SetResolution(1280, 800, true);
        if (PlayerPrefs.GetInt("login_platform") == 1)
        {
            // facebok으로 로그인 한 상태
            //FB.Init();
        }
        else if (PlayerPrefs.GetInt("login_platform") == 2)
        {
            
            // google play로 로그인 한 상태
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = false;
            PlayGamesPlatform.Activate();
            
        }
    }

    void Start()
    {
        print(PlayerDataManager.userID + " " + PlayerDataManager.gameID + " " + PlayerDataManager.spanner_time);

        print("스페너 개수" + PlayerDataManager.spanner);
        if (PlayerDataManager.spanner == 10)
        {
            PlayerDataManager.spanner_time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
        else if (PlayerDataManager.spanner != 10)
        {
            Update_Spanner();
        }

        //AudioSource.PlayClipAtPoint(backMusic,transform.position);
        //화면 사이즈 적용
        
        Time.timeScale = 1;
        //Path.text = Application.dataPath;
        // Debug.Log("플레이어 레벨 : " + PlayerDataManager.level);
        // Debug.Log("경험치 : " + PlayerDataManager.exp);
        // Debug.Log("돈 : "+ PlayerDataManager.money);
    }

    public void Update()
    {
        MoneyView.text = PlayerDataManager.money.ToString();
        LevelView.text = PlayerDataManager.level.ToString();
        NicknameView.text = PlayerDataManager.gameID;
        SpannerView.text = PlayerDataManager.spanner.ToString() + "/10";
    }
    public void SingleplayBtn()
    {
        //Invoke("startGame", .1f);
        LevelMenu.SetActive(true);
    }
    public void SingleplayExit()
    {
        LevelMenu.SetActive(false);
    }
    public void GoShop()
    {
        fader.FadeTo("Shop");
    }

    public void Exit()
    {
        Invoke("doExit", .1f);
    }

    void doExit()
    {
        print("종료 버튼");
        Application.Quit();
    }

    // -------------------------- 스페너 업데이트 ----------------------------------
    void Update_Spanner()
    {
        print("playerdatamaanger의 스페너시간: " + PlayerDataManager.spanner_time);

        int year = int.Parse(PlayerDataManager.spanner_time.Substring(2, 2));
        int month = int.Parse(PlayerDataManager.spanner_time.Substring(5, 2));
        int date = int.Parse(PlayerDataManager.spanner_time.Substring(8, 2));
        int h = int.Parse(PlayerDataManager.spanner_time.Substring(11, 2));
        int m = int.Parse(PlayerDataManager.spanner_time.Substring(14, 2));
        int s = int.Parse(PlayerDataManager.spanner_time.Substring(17, 2));

        int time = h * 60 * 60 + m * 60 + s; // 시간을 초로 바꿈.

        string cur_datetime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        print("현재 시스템시간: " + cur_datetime);

        int cur_year = int.Parse(cur_datetime.Substring(2, 2));
        int cur_month = int.Parse(cur_datetime.Substring(5, 2));
        int cur_date = int.Parse(cur_datetime.Substring(8, 2));
        int cur_h = int.Parse(cur_datetime.Substring(11, 2));
        int cur_m = int.Parse(cur_datetime.Substring(14, 2));
        int cur_s = int.Parse(cur_datetime.Substring(17, 2));

        int cur_time = cur_h * 60 * 60 + cur_m * 60 + cur_s; // 현재 시간을 초로 바꿈

        // 1. 년도 비교
        // 2. 월 비교
        // 3. 일 비교
        // 4. 시간 비교

        if (cur_year > year || cur_month > month || cur_date > date)
        {
            // 날짜가 하루이상 지났으므로 스페너 풀로 채움
            print("full로 채워야함");
            StartCoroutine(Update_Spanner_DB(10));
        }
        else
        {
            // 시간 비교
            int addspanner = (cur_time - time) / 60; // 추가 가능한 스페너 수
            print("addspanner: " + addspanner);

            if (PlayerDataManager.spanner + addspanner >= 10)
            {
                print("full로 채워야함");
                StartCoroutine(Update_Spanner_DB(10));
            }
            else
            {
                StartCoroutine(Update_Spanner_DB(PlayerDataManager.spanner + addspanner));
                print("타이머 처음 호출, 남은시간: " + (float)(cur_time - time) % 60);
                StartCoroutine(Spanner_Timer(60f - (float)(cur_time - time) % 60));
            }
        }
        


    }

    IEnumerator Update_Spanner_DB(int spanner_num)
    {

        WWWForm form = new WWWForm();
        form.AddField("userIDPost", PlayerDataManager.userID);
        form.AddField("spannerNumPost", spanner_num);

        WWW data = new WWW("http://13.124.188.186/spanner_updater.php", form);
        yield return data;

        string user_Data = data.text;

        if (user_Data == "\n1")
        {
            print("에코 1받고 spanner 채움");
            PlayerDataManager.spanner = spanner_num;
            SpannerView.text = PlayerDataManager.spanner.ToString() + "/10";
        }
        else
        {
            Debug.Log("Spanner update failed...");
        }
    }

    IEnumerator Spanner_Timer(float delayTime)
    {
        print("딜레이시간초: " + delayTime);

        Debug.Log("Time: " + Time.time);
        yield return new WaitForSeconds(delayTime);

        StartCoroutine(Update_Spanner_DB(PlayerDataManager.spanner + 1)); // db에 스페너 개수 및 스페너 시간 설정.

        if (PlayerDataManager.spanner + 1 < 10)
        {
            StartCoroutine(Spanner_Timer(60));
        }

    }

    public void Logout()
    {

        googleexample ge = new googleexample();
        ge.LogOut();
    }

}
