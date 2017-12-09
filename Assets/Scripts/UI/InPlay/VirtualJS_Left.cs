
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJS_Left : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image bgimg;
    private Image stickimg;
    private Vector3 inputVector = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        bgimg = GetComponent<Image>();
        stickimg = transform.GetChild(0).GetComponent<Image>();

    }


    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgimg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgimg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgimg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x, pos.y, 0);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            stickimg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgimg.rectTransform.sizeDelta.x / 3), inputVector.y * (bgimg.rectTransform.sizeDelta.y / 3));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        stickimg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0) return inputVector.x;
        else return Input.GetAxis("Right");
    }

    public float Vertical()
    {
        if (inputVector.y != 0) return inputVector.y;
        else return Input.GetAxis("Forward");
    }
}
