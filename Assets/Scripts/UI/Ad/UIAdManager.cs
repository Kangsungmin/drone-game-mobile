using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.Advertisements;

public class UIAdManager : MonoBehaviour {
    public UIManager ui_manager;
    public Button _BtnUnityAds;
    ShowOptions _ShowOpt = new ShowOptions();
    
    void Awake()
    {
        Advertisement.Initialize("1560964", true);
        _ShowOpt.resultCallback = OnAdsShowResultCallBack;
        UpdateButton();
    }

    void OnAdsShowResultCallBack(ShowResult result)//광고 완료 후 호출
    {
        ui_manager.GoLeaderBoard();
    }

    void UpdateButton()
    {
        _BtnUnityAds.interactable = Advertisement.IsReady(); //광고 준비되면 버튼 활성
    }

    public void OnBtnUnityAds()//버튼 안에 삽입
    {
        Advertisement.Show(null, _ShowOpt);
    }

    void Update() { UpdateButton(); }

    

}
