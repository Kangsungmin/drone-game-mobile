using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript1 : MonoBehaviour {
    public GameObject InternetNotWorkPopup;
    public googleexample GoogleManager;
    private void Awake()
    {
        PushStarter ps = new PushStarter();
#if UNITY_EDITOR
        PlayerPrefs.SetString("ID", "g10730980630015563674");
#endif

        Screen.SetResolution(1280, 800, true);
        InternetNotWorkPopup.SetActive(false);
    }
    // Use this for initialization
    void Start () {
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

                GoogleManager.LogIn();
            }
            else
            {
                SceneManager.LoadScene("flogintest");
            }
        }


	}
    public void QuitGame()
    {
        Application.Quit();
    }
		
}
