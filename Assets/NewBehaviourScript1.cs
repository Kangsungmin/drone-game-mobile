using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript1 : MonoBehaviour {
    public GameObject InternetNotWorkPopup;
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
                SceneManager.LoadScene("LoadingScene");
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
