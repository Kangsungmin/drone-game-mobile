
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class VirtualJS_Left : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

    private Image bgImg;
    private Image joystickImg;

    public Vector3 inputVector { set; get; }

    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();//내부 조이스틱
        inputVector = Vector3.zero;
    }
    public virtual void OnDrag(PointerEventData ped)
    {

        Vector2 pos = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            inputVector = new Vector3(x, 0, y);
            inputVector = (inputVector.magnitude > 1) ? inputVector.normalized : inputVector;//1보다 클 경우 처리

            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3), inputVector.z *
                (bgImg.rectTransform.sizeDelta.y / 3));//이미지를 움직임
            
            //실제 드론 움직임으로 변환하기 위해 적정 값 곱함.

        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)//조이스틱 떼었을때
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;//조이스틱 원점이동
    }

    public float Horizontal()
    {
        if (inputVector.x != 0) return inputVector.x;
        else return Input.GetAxis("Right");
    }

    public float Vertical()
    {
        if (inputVector.z != 0) return inputVector.z;
        else return Input.GetAxis("Forward");
    }
}
