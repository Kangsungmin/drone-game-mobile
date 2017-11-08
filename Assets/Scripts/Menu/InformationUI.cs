using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationUI : MonoBehaviour {
    public GameObject AddSpannerPanel, OptionPanel;
	// Use this for initialization
	void Awake () {
        AddSpannerPanel.SetActive(false);
        OptionPanel.SetActive(false);
    }
	
    public void OnOptionPanel()
    {
        OptionPanel.SetActive(true);
    }
    public void OffOptionPanel()
    {
        OptionPanel.SetActive(false);
    }
    public void OnAddSpanner()
    {
        if (PlayerDataManager.spanner <10)
        {
            AddSpannerPanel.SetActive(true);
        }
    }
    public void OffAddSpanner()
    {
        AddSpannerPanel.SetActive(false);
    }
}
