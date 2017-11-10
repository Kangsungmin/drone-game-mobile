using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    bool LoadDialog = false;
    string Result_DB="";

    void Start()
    {
        StartCoroutine(Load_Data());
        StartCoroutine(DBLoad());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator DBLoad()
    {
        LoadDialog = true;
        do
        {
            yield return null;
        } while (Result_DB.Equals(""));
        LoadDialog = false;

        //씬전환
        SceneManager.LoadScene("Menu");
    }

    private void OnGUI()
    {
        if (LoadDialog)
        {

        }
    }


    // 게임 접속 시 DB에서 데이터를 클라이언트로 업데이트
    IEnumerator Load_Data()
    {
        PlayerDataManager.userID = PlayerPrefs.GetString("ID");
        print("load data --- " + PlayerDataManager.userID);
        // -------닉네임, 돈, 레벨, 경험치 정보 로드-----------------
        // ---------------------------------------------------------
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", PlayerDataManager.userID);

        WWW data = new WWW("http://13.124.188.186/load_data.php", form);
        yield return data;

        string user_Data = data.text;
        print(user_Data);

        PlayerDataManager.gameID = GetDataValue(user_Data, "gameID:");
        PlayerDataManager.money = int.Parse(GetDataValue(user_Data, "Money:"));
        PlayerDataManager.level = int.Parse(GetDataValue(user_Data, "Level:"));
        PlayerDataManager.exp = int.Parse(GetDataValue(user_Data, "Experience:"));
        PlayerDataManager.spanner = int.Parse(GetDataValue(user_Data, "Spanner_Num:"));
        PlayerDataManager.spanner_time = GetDataValue(user_Data, "Spanner_Time:");

        //--------------------------------------------------------

        // -----------------------------------------------------------


        // -----------현재 사용 드론 로드-------------------------------
        // -------------------------------------------------------------
        int drone_equip = int.Parse(GetDataValue(user_Data, "Drone_Equip:"));

        form.AddField("droneIDPost", drone_equip);

        data = new WWW("http://13.124.188.186/load_drone.php", form);
        yield return data;

        user_Data = data.text;

        PlayerDataManager.nowUsingModel = new DroneModel(int.Parse(GetDataValue(user_Data, "DroneID:")),
            GetDataValue(user_Data, "Name:"), int.Parse(GetDataValue(user_Data, "Price:")));

        //-------------------------------------------------------------

        // ------소유 드론목록 로드--------------------------------------
        // --------------------------------------------------------------
        form.AddField("userIDPost", PlayerDataManager.userID);

        data = new WWW("http://13.124.188.186/load_user_drone.php", form);
        yield return data;

        user_Data = data.text;
        user_Data = user_Data.Replace("\n", "");
        print(user_Data);
        // 1,2,-1 이런식으로 return값 되있음.

        string[] ids = user_Data.Split(',');
        //Log.text += "보유리스트 길이 : "+ids.Length+"\n";
        for (int i = 0; i < ids.Length - 1; i++)
        {
            print("droneid: " + ids[i]);
            form.AddField("droneIDPost", ids[i]);

            data = new WWW("http://13.124.188.186/load_drone.php", form);
            yield return data;

            user_Data = data.text;
            print(user_Data);

            DroneModel model = new DroneModel(int.Parse(GetDataValue(user_Data, "DroneID:")),
                GetDataValue(user_Data, "Name:"), int.Parse(GetDataValue(user_Data, "Price:")));
            PlayerDataManager.ownModels.Add(model);
        }

        //-----------------------------------------------------------------

        // --------전체 드론목록 로드-------------------------------------
        // ---------------------------------------------------------------

        for (int i = 0; i <= 4; i++)
        {
            form.AddField("droneIDPost", i);
            data = new WWW("http://13.124.188.186/load_drone.php", form);
            yield return data;

            user_Data = data.text;

            DroneModel model = new DroneModel(int.Parse(GetDataValue(user_Data, "DroneID:")),
                GetDataValue(user_Data, "Name:"), int.Parse(GetDataValue(user_Data, "Price:")));

            PlayerDataManager.Models.Add(model);
        }

        // ---------------------------------------------------------------

        // -------------------------------파츠 로드--------------------------------

        form.AddField("userIDPost", PlayerDataManager.userID);
        data = new WWW("http://13.124.188.186/load_user_items.php", form);
        yield return data;

        user_Data = data.text;

        string[] items_num = user_Data.Split(',');

        for (int i=1; i<=5; i++)
        {
            PlayerDataManager.ownParts[i] = int.Parse(items_num[i - 1]);
        }

        // ------------------------------------------------------------------

        print("loadmanager00");
        print(PlayerDataManager.userID + " " + PlayerDataManager.gameID + " " + PlayerDataManager.money + " " + PlayerDataManager.exp + " " + PlayerDataManager.nowUsingModel.getTitle());

        Result_DB = "1";
    }

    string GetDataValue(string data, string index)
    {

        string value = data.Substring(data.IndexOf(index) + index.Length);

        //if (index != "Drone_Equip:") 
        value = value.Remove(value.IndexOf("|"));

        return value;
    }
}

