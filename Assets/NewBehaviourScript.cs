using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FB.Init ();
		print ("nickname is " + PlayerPrefs.GetString ("gameID"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Logout() {
		PlayerPrefs.DeleteAll ();
		FB.LogOut ();
		SceneManager.LoadScene ("flogintest");
	}
}
