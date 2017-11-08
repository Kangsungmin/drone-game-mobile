using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class UIAdManager : MonoBehaviour {

    public Button _BtnUnityAds;
    public Text SpannerView;


    ShowOptions _ShowOpt = new ShowOptions();

    void Awake()
    {
        Advertisement.Initialize("1560964", true);
        _ShowOpt.resultCallback = OnAdsShowResultCallBack;
        UpdateButton();
    }

    void OnAdsShowResultCallBack(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
			StartCoroutine(Update_Spanner_DB(PlayerDataManager.spanner + 1));
        }
    }

    void UpdateButton()
    {
        _BtnUnityAds.interactable = Advertisement.IsReady();
        _BtnUnityAds.GetComponentInChildren<Text>().text
            = "광고보고 스패너 충전하기";
    }

    public void OnBtnUnityAds()
    {
        print("광고클릭");
        Advertisement.Show(null, _ShowOpt);
    }

    void Update() { UpdateButton(); }

	IEnumerator Update_Spanner_DB (int spanner_num) {

		WWWForm form = new WWWForm();
		form.AddField("userIDPost", PlayerDataManager.userID);
		form.AddField ("spannerNumPost", spanner_num);

		WWW data = new WWW("http://13.124.188.186/spanner_updater_AD.php", form);
		yield return data;

		string user_Data = data.text;

		if (user_Data == "\n1") {
			print("에코 1받고 spanner 채움");
			PlayerDataManager.spanner = spanner_num;
            SpannerView.text = PlayerDataManager.spanner.ToString() + "/10";
        } else {
			Debug.Log ("Spanner update failed...");
		}
	}

}
