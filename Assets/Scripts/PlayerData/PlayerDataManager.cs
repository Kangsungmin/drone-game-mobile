using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager playerDataManager { get; private set; }//
    //public DroneDatabase modelDatabase;
	public static string userID { get; set; }
	public static string gameID { get; set; }
	public static int spanner { get; set; }
	public static string spanner_time { get; set; }
	public static int money { get; set; }
	public static int level { get; set; }
	public static int exp { get; set; }
	public static List<DroneModel> Models = new List<DroneModel>();//모든 모델 정보 저장
	public static List<DroneModel> ownModels = new List<DroneModel>();
    public static Dictionary<int, int> ownParts = new Dictionary<int, int>(); //<Key, value>
    public static DroneModel nowUsingModel;
 

    


	//===========돈, 경험치 수정[시작]==============
	public void IncreaseMoney(int value)
	{
        money += value;
		PlayerPrefs.SetInt("money", money);
	}

	public void DecreaseMoney(int value)
	{
		money -= value;
		PlayerPrefs.SetInt("money", money);
	}

	public void AddExp(int value)
	{
		exp += value;

		//렙업이 가능한 지 검사
		if (exp >= 100)
		{
			exp -= 100;
			level++;
		}
		PlayerPrefs.SetInt("level", level);
		PlayerPrefs.SetInt("exp", exp);
	}
	//===========돈, 경험치 수정[끝]================

	void Awake()
    {
        playerDataManager = this;
    }

    void Start()
	{
        playerDataManager = new PlayerDataManager();
    }
		

}
