using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
    SceneFader Fader;
    GameObject ViewCamera, LeftButton, RightButton, BuyPanel, UsingInfo;
    public GameObject PlayerDataView, UpgradeButton, ChangeButton, ComingSoonPanel;
    GameObject[] AllModels;
    Text MoneyView, ModelPriceView;
    public Text partsView1, partsView2, partsView3, partsView4;
    int nowPoint;
    bool isMoving, isBuying;

	DroneModel thismodel;

	// Use this for initialization
	void Start () {
        Fader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
        ViewCamera = GameObject.Find("Main Camera");
        AllModels = GameObject.FindGameObjectsWithTag("Player"); //모든 드론 모델들을 가져온다.
        LeftButton = GameObject.Find("LeftBtn");
        RightButton = GameObject.Find("RightBtn");
        BuyPanel = GameObject.Find("BuyPanel");
        UsingInfo = GameObject.Find("Using");
        nowPoint = 0;
        MoneyView = GameObject.Find("MoneyAmount").GetComponent<Text>();
        ModelPriceView = GameObject.Find("ModelPrice").GetComponent<Text>();

        UsingInfo.SetActive(false);
        LeftButton.SetActive(false);
        isMoving = false;
        isBuying = false;
        RefreshData();
    }
    void Update()
    {
        //GUI 업데이트
        MoneyView.text = PlayerDataManager.money.ToString();
        partsView1.text = PlayerDataManager.ownParts[1].ToString();
        partsView2.text = PlayerDataManager.ownParts[2].ToString();
        partsView3.text = PlayerDataManager.ownParts[3].ToString();
        partsView4.text = PlayerDataManager.ownParts[4].ToString();

        thismodel = PlayerDataManager.Models[nowPoint];
        updateModelInfo(thismodel);

        if (CheckModelIs(thismodel))
        {
            BuyPanel.SetActive(false);
            if (PlayerDataManager.nowUsingModel.getID() == thismodel.getID())
            {
                UsingInfo.SetActive(true);
                UsingInfo.GetComponent<Text>().text = "사용중";
                UpgradeButton.SetActive(true);
                ChangeButton.SetActive(false); //현재 사용중
            }
            else
            {
                UsingInfo.SetActive(true);
                UsingInfo.GetComponent<Text>().text = "교체가능";
                UpgradeButton.SetActive(true);
                ChangeButton.SetActive(true); //보유하고 있지만 현재 사용중 아님.
            }
        }
        else
        {
            UsingInfo.SetActive(false);
            BuyPanel.SetActive(true);
            UpgradeButton.SetActive(false);
            ChangeButton.SetActive(false);
        }
            
		if (nowPoint > 2) {
			ComingSoonPanelOn ();
		} else ComingSoonPanelOff ();
    }
	
    IEnumerator BuyModel()
    {
        //PlayerData에 있는지 확인, 없으면 구매: 플레이어프리팹 추가 후 새로고침

		if (CheckModelIs (thismodel)) {
			//모델을 이미 보유하고 있음
		} else {//아이템 구매 
			WWWForm form = new WWWForm ();
			form.AddField ("userIDPost", PlayerDataManager.userID);
			form.AddField ("buyDronePost", thismodel.getID());

			WWW data = new WWW ("http://13.124.188.186/buy_drone.php", form);
			yield return data;

			string user_Data = data.text;

			
				//사는작업 성공 후 DB에 Update 성공한 상태.


				// ======= 현재 추가해야할 드론정보를 DB에서 가져와 내 소유모델에 추가 =========

				form.AddField("droneIDPost", thismodel.getID());

				data = new WWW("http://13.124.188.186/load_drone.php", form);
				yield return data;

				user_Data = data.text;

				DroneModel model = new DroneModel (int.Parse(GetDataValue(user_Data, "DroneID:")), 
					GetDataValue(user_Data, "Name:"), int.Parse(GetDataValue(user_Data, "Price:")));

				PlayerDataManager.ownModels.Add(model);

            // =============================================================================

        }
        isBuying = false;
    }

	string GetDataValue(string data, string index) {

		string value = data.Substring(data.IndexOf(index)+index.Length);

		//if (index != "Drone_Equip:") 
		value = value.Remove(value.IndexOf("|"));

		return value;
	}

    void updateModelInfo(DroneModel model)
    {
        //가격, 정보 업데이트
		ModelPriceView.text = model.getPrice().ToString();
    }

    bool CheckModelIs(DroneModel model)
    {
        //PlayerData에 있는지 확인
        for (int i = 0; i < PlayerDataManager.ownModels.Count; i++)
			if (PlayerDataManager.ownModels[i].getID() == model.getID()) return true;
        return false;
    }

    public void ThisModelBuy()//현재 모델 구매
    {
        
            thismodel = PlayerDataManager.Models[nowPoint];

            if (PlayerDataManager.money - thismodel.getPrice() >= 0)
            {
                if (!isBuying)
                {
                    isBuying = true;
                    PlayerDataManager.money -= thismodel.getPrice();
                    StartCoroutine(BuyModel());
                }
            }       
            else {
                Debug.Log("No money....");
            }
            
      
    }

    public void LeftShift()//좌로 이동
    {
        if (!isMoving)
        {
            isMoving = true;

            nowPoint--;


            RightButton.SetActive(true);
            
			if (nowPoint == 0)
                LeftButton.SetActive(false);
            
			ViewCamera.SendMessage("ShiftLeft");

            StartCoroutine("MovigLock");
        }

    }

    public void RightShift()//우로 이동
    {
        if (!isMoving)
        {
            isMoving = true;
            nowPoint++;
            LeftButton.SetActive(true);
            if (nowPoint == AllModels.Length-1)
                RightButton.SetActive(false);
            ViewCamera.SendMessage("ShiftRight");
            StartCoroutine("MovigLock");
        }

    }

    public void ChangeModel()
    {
        PlayerDataManager.nowUsingModel = thismodel;//현재 모델로 교체
		StartCoroutine (SelectModel()); // DB 업데이트
    }

	IEnumerator SelectModel()
	{
		WWWForm form = new WWWForm ();
		form.AddField ("userIDPost", PlayerDataManager.userID);
		form.AddField ("selectDronePost", thismodel.getID());

		WWW data = new WWW ("http://13.124.188.186/select_drone.php", form);
		yield return data;

		string user_Data = data.text;
	}

    public void RefreshData()//새로고침
    {
        Text BoxCount = PlayerDataView.transform.Find("BoxCount").GetComponent<Text>();
        BoxCount.text = PlayerDataManager.ownParts[1].ToString();
        Text PlasticCount = PlayerDataView.transform.Find("PlasticCount").GetComponent<Text>();
        PlasticCount.text = PlayerDataManager.ownParts[2].ToString();
        Text IronCount = PlayerDataView.transform.Find("IronCount").GetComponent<Text>();
        IronCount.text = PlayerDataManager.ownParts[3].ToString();
        Text AluminumCount = PlayerDataView.transform.Find("AluminumCount").GetComponent<Text>();
        AluminumCount.text = PlayerDataManager.ownParts[4].ToString();
    }

    public void Sell_Part(int itemID)
    {
        
            Part p = new Part(itemID);

            if (PlayerDataManager.ownParts[itemID] - 1 >= 0)
            {
                if (!isBuying)
                {
                    isBuying = true;
                    PlayerDataManager.money += p.getSellPrice();
                    PlayerDataManager.ownParts[itemID]--;
                    // 결과 DB로 전송
                    StartCoroutine(Sell_Part_DB(itemID, PlayerDataManager.ownParts[itemID]));

                }
            }
            else
            {
                Debug.Log("No items....");
            }

        

    }
    public void Buy_Part(int itemID)
    {
        
            Part p = new Part(itemID);

            if (PlayerDataManager.money - p.getBuyPrice() >= 0)
            {
                if (!isBuying)
                {
                    isBuying = true;
                    PlayerDataManager.money -= p.getBuyPrice();
                    PlayerDataManager.ownParts[itemID]++;
                    StartCoroutine(Sell_Part_DB(itemID, PlayerDataManager.ownParts[itemID]));

                }
            }
            else
            {
                Debug.Log("No money....");
            }

    }
    
    IEnumerator Sell_Part_DB(int itemID, int itemNum)
    {
        WWWForm form = new WWWForm();
        form.AddField("userIDPost", PlayerDataManager.userID);
        form.AddField("itemIDPost", itemID);
        form.AddField("itemNumPost", itemNum);
        form.AddField("userMoneyPost", PlayerDataManager.money);

        WWW data = new WWW("http://13.124.188.186/sell_items.php", form);
        yield return data;

        string user_Data = data.text;

        isBuying = false;
    }
    /*
    IEnumerator Buy_Part_DB(int itemID, int buyPrice)
    {

    }
    */
    public void GoBackMenu()
    {
        Fader.FadeTo("Menu");
    }
    public void ComingSoonPanelOn()
    {
        ComingSoonPanel.SetActive(true);
    }
    public void ComingSoonPanelOff()
    {
        ComingSoonPanel.SetActive(false);
    }
    IEnumerator MovigLock()
    {
        yield return new WaitForSeconds(1.05f);
        isMoving = false;
    }
}
