using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class googleexample : MonoBehaviour
{

    bool mAuthenticating = false;

    public Text txt;

    string userid;
    string usernickname;
    string registration_checkURL = "http://13.124.188.186/registration_check.php";
    string duplication_checkURL = "http://13.124.188.186/duplication_check.php";
    string register_idURL = "http://13.124.188.186/register_id.php";
    string LoginURL = "http://13.124.188.186/Login.php";

    public Text txt_nickname;
    public InputField input_nickame;

    public Canvas nicknamecanvas;

    private void Start()
    {
        

    }

    private void Update()
    {

    }

    public void LogIn()
    {
        if (Social.Active.localUser.authenticated || mAuthenticating)
        {
            txt.text = "Ignoring repeated call to LogIn().";
            Debug.Log("Ignoring repeated call to LogIn().");

            return;
        }

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            //.EnableSavedGames()
            .Build();

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        Debug.Log("clicked:LogIn");


        string defaultleaderboard = GPGSIds.leaderboard_scoreboard;
        ((PlayGamesPlatform)Social.Active).SetDefaultLeaderboardForUI(defaultleaderboard);
        PlayGamesPlatform.Instance.SetDefaultLeaderboardForUI(defaultleaderboard);
        mAuthenticating = true;
        Social.localUser.Authenticate(SignInCallback);

        Debug.Log("clicked:LogIn End");


    }

    private void SignInCallback(bool success)
    {


        string templog = string.Format("SignInCallback: {0}, {1}"
            , success
            , Social.localUser);

        print(templog);

        mAuthenticating = false;
        if (success)
        {
            if (Social.localUser == null)
            {
                txt.text = "Login Error";
                Application.Quit();
            }
            else
            {
                userid = Social.localUser.id;
                txt.text = "Success!!";

                StartCoroutine(Connect_Login());
            }
        }
        else
        {
            txt.text = "Login Fail";

        }
    }

    /*
    public void ShowLeaderBoardUI(string ID)
    {
        if (Social.Active.localUser.authenticated)
        {
            setScore(10, ID);
            ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(ID);
        }
        else
        {
            txt.text = "Not Login";
        }
    }

    protected void setScore(long score, string ID)
    {
        Social.ReportScore(score, ID, isSetScore);


    }
    */

    IEnumerator Connect_Login()
    {
        txt.text = "connect login, " + userid + "aa";
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", userid);

        WWW data = new WWW(registration_checkURL, form);
        yield return data;

        string user_Data = data.text;
        txt.text = "userdata: " + user_Data;
        if (user_Data == "\nNot Registered!")
        {
            print("no id");

            // 닉네임 입력창을 띄워 입력받고 중복확인하는 과정
            nicknamecanvas.enabled = true;
        }
        else
        {

            PlayerPrefs.SetString("ID", GetDataValue(user_Data, "ID:"));

            SceneManager.LoadScene("LoadingScene");
        }


    }

    string GetDataValue(string data, string index)
    {

        string value = data.Substring(data.IndexOf(index) + index.Length);

        if (index != "Drone_Equip:")
            value = value.Remove(value.IndexOf("|"));

        return value;
    }

    public void start_duplication_check()
    {
        print("dupcheck!!!!!");
        StartCoroutine(duplication_check());
    }
    public void start_register_id()
    {
        print("recheck!!!!");
        //   StartCoroutine (register_id ());
    }

    IEnumerator duplication_check()
    {
        print(input_nickame.text);
        WWWForm form = new WWWForm();
        form.AddField("userNicknamePost", input_nickame.text);

        WWW data = new WWW(duplication_checkURL, form);
        yield return data;

        string user_Data = data.text;
        print(user_Data);

        if (user_Data == "\nThis nickname is already registered.")
        {
            txt_nickname.text = "이미 존재하는 닉네임입니다.";
        }
        else
        {
            usernickname = input_nickame.text;
            nicknamecanvas.enabled = false;
            StartCoroutine(register_id());
            StopCoroutine(duplication_check());
        }

    }
    IEnumerator register_id()
    {
        print(usernickname);
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", userid);
        form.AddField("userNicknamePost", usernickname);

        WWW data = new WWW(register_idURL, form);
        yield return data;

        string user_Data = data.text;
        print(user_Data);

        PlayerPrefs.SetString("ID", GetDataValue(user_Data, "ID:"));

        SceneManager.LoadScene("LoadingScene");
    }
}