using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgimg;
    private Vector3 inputVector = Vector3.zero;
    private Vector2 StartPosition = Vector2.zero;
    public Transform Drone;

    private float rotX = 0f;
    private float rotY = 0f;
    private Vector3 origRot;

    public void Awake()
    {
        bgimg = GetComponent<Image>();
    }

    public void SetReference(GameObject[] Refs)
    {
        Drone = Refs[0].transform;
        origRot = Drone.eulerAngles;
        rotX = origRot.x;
        rotY = origRot.y;
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        StartPosition = ped.position;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 vec;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgimg.rectTransform, ped.position, ped.pressEventCamera, out vec))
        {
            //print(vec.x + " / "+bgimg.rectTransform.sizeDelta.x);
            vec = ped.position - StartPosition;
            vec.x = (vec.x / bgimg.rectTransform.sizeDelta.x);
            inputVector = new Vector3(4.5f * vec.x, 0, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

        }
    }


    public virtual void OnPointerUp(PointerEventData ped)
    {
        StartPosition = Vector2.zero;
        inputVector = Vector3.zero;
    }
    

    public float Horizontal()
    {
        if (inputVector.x != 0) return inputVector.x;
        else return Input.GetAxis("Horizontal");
    }

}
