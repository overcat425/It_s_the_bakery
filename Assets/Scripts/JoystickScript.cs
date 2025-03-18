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
    public void OnPointerDown(PointerEventData eventData)   // 터치 다운
    {
        joystick.position = eventData.position; // 터치된 좌표를 받아
        joystick.gameObject.SetActive(true);        // 조이스틱 true
        startPos = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData) // 터치 업
    {
        joystick.gameObject.SetActive(false);
        handle.anchoredPosition = Vector2.zero;
        inputVec = Vector2.zero;
    }
    public void OnDrag(PointerEventData eventData)  // 터치 후 드래그
    {
        Vector2 dir = eventData.position - startPos;
        float dist = Mathf.Min(dir.magnitude, radius);
        joystickPos = startPos + dir.normalized * dist;
        handle.position = joystickPos;
        inputVec = dir.normalized * (dist / radius);  // 0~1로 정규화시킴
    }
}