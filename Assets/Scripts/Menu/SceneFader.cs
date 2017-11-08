using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneFader : MonoBehaviour {
    public Image img;
	public bool FadeEnd;
	private void Awake()
	{
		FadeEnd = false;
	}
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)//씬을 불러온다.
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t>0f)
        {
            t -= Time.deltaTime;
            img.color = new Color(0f,0f,0f,t); //t는 투명도
            yield return 0;
        }
		FadeEnd = true;
        gameObject.SetActive(false);
    }
    IEnumerator FadeOut(string scene)
    {
        
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            img.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
        SceneManager.LoadScene(scene);
    }
}
