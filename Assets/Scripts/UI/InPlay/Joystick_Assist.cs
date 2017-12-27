using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick_Assist : MonoBehaviour {
    private Image image;
    public Canvas parent;
    public bool Right = false;
	// Use this for initialization
	void Start () {
        image = this.GetComponent<Image>();
	}

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent.transform as RectTransform, Input.mousePosition, parent.worldCamera, out pos);
            if (pos.x < -410 && pos.y < -150)
            {
                image.rectTransform.anchoredPosition = pos;
            }
        }
    }
}
