using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour {
    public static SceneData sceneData;

    public static string MapName;
    public static string SceneLevelName;

    void Awake()
    {
        sceneData = transform.GetComponent<SceneData>();
    }

    //============================스테이지 호출[시작]===============================
    public void LoadStage(string mapName, string sceneName) 
    {
        MapName = mapName;
        SceneLevelName = sceneName;
        
    }
    //============================스테이지 호출[끝]=================================

}
