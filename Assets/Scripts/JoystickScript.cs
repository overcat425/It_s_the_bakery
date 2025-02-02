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
        radius = 45f;
    }
    public void OnPointerDown(PointerEventData eventData)   // 터치 시
    {
        joystick.position = eventData.position;
        joystick.gameObject.SetActive(true);
        startPos = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.gameObject.SetActive(false);
        handle.anchoredPosition = Vector2.zero;
        inputVec = Vector2.zero;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dir = eventData.position - startPos;
        float dist = Mathf.Min(dir.magnitude, radius);
        joystickPos = startPos + dir.normalized * dist;
        handle.position = joystickPos;
        inputVec = dir.normalized * (dist / radius);  // 0~1로 정규화시킴
    }
}