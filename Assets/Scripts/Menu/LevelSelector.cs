using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
    public SceneFader fader;
    public Text SpannerView;

    public void Select(string Name)//버튼 클릭시.
    {
        if (PlayerDataManager.spanner > 0)
        {
            string MapName, Level;
            int index = Name.IndexOf(".");
            MapName = Name.Substring(0, index);
            Level = Name.Substring(index + 1, 2);
            //씬데이터에 저장
            SceneData.sceneData.LoadStage(MapName, Level);
            StartCoroutine(Update_Spanner_DB(PlayerDataManager.spanner - 1)); // 스페너 감소
            fader.FadeTo(MapName);
        }
       
    }

	IEnumerator Update_Spanner_DB (int spanner_num) {

		WWWForm form = new WWWForm();
		form.AddField("userIDPost", PlayerDataManager.userID);
		form.AddField ("spannerNumPost", spanner_num);

		WWW data = new WWW("http://13.124.188.186/spanner_updater.php", form);
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
