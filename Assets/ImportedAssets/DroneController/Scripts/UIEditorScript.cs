using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIEditorScript : MonoBehaviour
{
	private CameraScript cs;
	void Update(){
		cs = GetComponent<CameraScript> ();
		if (GameObject.FindGameObjectsWithTag ("Player").Length > 1) {
			foreach (GameObject g in cs.canvasSelectButtons) {
				g.SetActive (true);
			}
			foreach (GameObject g in cs.canvasExitButtons) {
				g.SetActive (true);
			}
		} else {
			foreach (GameObject g in cs.canvasSelectButtons) {
				g.SetActive (false);
			}
			foreach (GameObject g in cs.canvasExitButtons) {
				g.SetActive (false);
			}
		}
	}
}
