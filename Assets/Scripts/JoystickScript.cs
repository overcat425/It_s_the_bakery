using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform joystick;
    public RectTransform handle;
    Vector2 startPos;
    Vector2 joystickPos;
    public Vector2 inputVec;
    float radius;

    Vector2 GetInputVector() => inputVec;
    void Start()
    {
        radius = 50f;
    }
    public void OnPointerDown(PointerEventData eventData)   // ��ġ �ٿ�
    {
        joystick.position = eventData.position; // ��ġ�� ��ǥ�� �޾�
        joystick.gameObject.SetActive(true);        // ���̽�ƽ true
        startPos = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData) // ��ġ ��
    {
        joystick.gameObject.SetActive(false);
        handle.anchoredPosition = Vector2.zero;
        inputVec = Vector2.zero;
    }
    public void OnDrag(PointerEventData eventData)  // ��ġ �� �巡��
    {
        Vector2 dir = eventData.position - startPos;
        float dist = Mathf.Min(dir.magnitude, radius);
        joystickPos = startPos + dir.normalized * dist;
        handle.position = joystickPos;
        inputVec = dir.normalized * (dist / radius);  // 0~1�� ����ȭ��Ŵ
    }
}