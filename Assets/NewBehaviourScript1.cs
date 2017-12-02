using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class NewBehaviourScript1 : MonoBehaviour
{

    public GameObject InternetNotWorkPopup;

    bool mAuthenticating = false;

    string userID = "";

    private void Awake()
    {
        PushStarter ps = new PushStarter();

        Screen.SetResolution(1280, 800, true);
        InternetNotWorkPopup.SetActive(false);
    }
    // Use this for initialization
    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //인터넷 연결 안됨
            InternetNotWorkPopup.SetActive(true);
        }
        else
        {
            if (PlayerPrefs.HasKey("ID"))
            {
                print("user is logging");

                userID = PlayerPrefs.GetString("ID");
                LogIn();
            }
            else
            {
                SceneManager.LoadScene("flogintest");
            }
        }

    }


    public void LogIn()
    {
        if (Social.Active.localUser.authenticated || mAuthenticating)
        {

            Debug.LogError("Ignoring repeated call to LogIn().");
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

        mAuthenticating = true;
        Social.localUser.Authenticate(SignInCallback);


        Debug.Log("clicked:LogIn End");

    }

    private void SignInCallback(bool success, string p_val)
    {
        string templog = string.Format("SignInCallback: {0}, {1}, {2}"
            , success
            , p_val
            , Social.localUser);

        print(templog);

        mAuthenticating = false;
        if (success)
        {
            if (Social.localUser == null)
            {
                Debug.Log("Login Error");
                Application.Quit();
            }
            else
            {
                Debug.Log("Success!!");

                SceneManager.LoadScene("LoadingScene");
            }
        }
        else
        {
            Debug.Log("Login Fail ");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}