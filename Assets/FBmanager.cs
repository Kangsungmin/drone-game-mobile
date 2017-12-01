using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class FBmanager : MonoBehaviour {

    List<string> Perms = new List<string>() { "public_profile", "email", "user_friends" };
	string userid;
	string usernickname;
	string registration_checkURL = "http://13.124.188.186/registration_check.php";
	string duplication_checkURL = "http://13.124.188.186/duplication_check.php";
	string register_idURL = "http://13.124.188.186/register_id.php";
	string LoginURL = "http://13.124.188.186/Login.php";

	public Text txt;
	public Text txt_nickname;
	public InputField input_nickame;

	public Canvas nicknamecanvas;

    

    void Awake()
    {
        Screen.SetResolution(1280, 800, true);
        //FB.Init(InitCompleteCallback,UnityCallbackDelegate);
        
        // googleplay part start
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
        // googleplay part finish
    }

    public void LoginButton(int login_number)
    {
        // facebook과 googleplay 각각 로그아웃 방법이 다르기 때문에
        // 어떤 방식으로 로그인 했는지 저장.
        PlayerPrefs.SetInt("login_platform", login_number);

        if (login_number == 1)
        {
            /*
            // facebook 로그인 버튼 클릭 시
            if (!FB.IsLoggedIn)
            {
                FB.LogInWithReadPermissions(Perms, LoginCallback);

            }
            else
            {
                Debug.LogError("user is logging");
                txt.text = "User is logging";
            }*/
        }
        else if (login_number == 2)
        {
            // google play 로그인 버튼 클릭 시
            Social.localUser.Authenticate(signInCallback);
        }
    }

	IEnumerator Connect_Login() {
        print(userid);
		WWWForm form = new WWWForm();
		form.AddField("userIDPost", userid);

		WWW data = new WWW(registration_checkURL, form);
		yield return data;

		string user_Data = data.text;

		if (user_Data == "\nNot Registered!") {
			print ("no id");

			// 닉네임 입력창을 띄워 입력받고 중복확인하는 과정
			nicknamecanvas.enabled = true;
		} else {

			PlayerPrefs.SetString ("ID", GetDataValue (user_Data, "ID:"));

			SceneManager.LoadScene ("LoadingScene");
		}


	}

	string GetDataValue(string data, string index) {

		string value = data.Substring(data.IndexOf(index)+index.Length);

		if (index != "Drone_Equip:") 
			value = value.Remove(value.IndexOf("|"));

		return value;
	}

	public void start_duplication_check() {
		print ("dupcheck!!!!!");
		StartCoroutine (duplication_check ());
	}
	public void start_register_id() {
		print ("recheck!!!!");
	//	StartCoroutine (register_id ());
	}

	IEnumerator duplication_check() {
        print(input_nickame.text);
		WWWForm form = new WWWForm();
		form.AddField("userNicknamePost", input_nickame.text);

		WWW data = new WWW(duplication_checkURL, form);
		yield return data;

		string user_Data = data.text;
		print (user_Data);

		if (user_Data == "\nThis nickname is already registered.") {
			txt_nickname.text = "이미 존재하는 닉네임입니다.";
		} else {
			usernickname = input_nickame.text;
			nicknamecanvas.enabled = false;
			StartCoroutine (register_id ());
			StopCoroutine (duplication_check ());
		}

	}
	IEnumerator register_id() {
		print (usernickname);
		WWWForm form = new WWWForm();
		form.AddField ("userIDPost", userid);
		form.AddField("userNicknamePost", usernickname);

		WWW data = new WWW(register_idURL, form);
		yield return data;

		string user_Data = data.text;
		print (user_Data);

		PlayerPrefs.SetString ("ID", GetDataValue (user_Data, "ID:"));

		SceneManager.LoadScene ("LoadingScene");
	}

    #region callback
    /*
    private void LoginCallback(IResult result)
    {
        if (result.Cancelled)
        {
            Debug.LogError("User Cancelled");
			txt.text = "User Cancelled";
        }
        else
        {
            Debug.Log("Login Successfully");
			txt.text = "Login Successfully";
			FB.API ("/me?fields=first_name", HttpMethod.GET, DisplayText);
        }
    }

	private void DisplayText(IResult result) 
	{ 
		userid = (string)result.ResultDictionary ["id"];
		txt.text += "Hi," + result.ResultDictionary ["first_name"] + userid;
		StartCoroutine (Connect_Login());
		//PlayerPrefs.SetString ("userID", userid);
		//PlayerPrefs.Save ();
		//string aaa = PlayerPrefs.GetString ("userID");
		//print (aaa);
		//SceneManager.LoadScene("a");
	}
    private void InitCompleteCallback()
    {
        if (FB.IsInitialized)
        {
            Debug.Log("Successfully init");
			txt.text = "Successfully init";
        }
        else
        {
            Debug.LogError("Failed Init");
			txt.text = "Failed Init";
        }
    }
    private void UnityCallbackDelegate(bool isUnity)
    {
        if (isUnity)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }*/
    
    private void signInCallback(bool success)
    {
        if (success)
        {
            userid = Social.localUser.id;
            txt.text = "Hi, " + userid;
            StartCoroutine(Connect_Login());
        }
        else
            txt.text = "SignIn Fail!!";
    }
    
    #endregion
}
