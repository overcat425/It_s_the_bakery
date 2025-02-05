using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class TutorialScript : MonoBehaviour
{
    public Transform[] target;
    public GameObject notice;
    public Image tutoIcon;
    float screenWidth;
    float screenHeight;

    public int num;
    void Start()
    {
        notice.SetActive(true);
        num = 0;
        screenWidth = Screen.width - 30f;
        screenHeight = Screen.height - 30f;
        StartCoroutine("NoticeMoving");
        NextPosition();
    }
    void LateUpdate()
    {
        OutOfScreen();
        Rotating();
        if (num >= 9)
        {
            notice.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    public void NextPosition()
    {
        if (num >= 9) return;
        notice.transform.position = target[num].position + new Vector3(0, 2.5f, 0);
        num++;
    }
    void Rotating()
    {
        notice.transform.Rotate(0, 100f * Time.deltaTime, 0);
    }
    IEnumerator NoticeMoving()
    {
        while (true)
        {
            yield return notice.transform.DOMoveY(notice.transform.position.y + 1, 1f).SetEase(Ease.InOutSine).WaitForCompletion();
            yield return notice.transform.DOMoveY(notice.transform.position.y - 1, 1f).SetEase(Ease.InOutSine).WaitForCompletion();
        }
    }
    void OutOfScreen()
    {
        if (notice.GetComponent<Renderer>().isVisible)
        {
            tutoIcon.gameObject.SetActive(false);
        }
        else if (!notice.GetComponent<Renderer>().isVisible)
        {
            tutoIcon.gameObject.SetActive(true);
            Vector3 tutoPos = Camera.main.WorldToScreenPoint(notice.transform.position);
            tutoPos.x = Mathf.Clamp(tutoPos.x, 30f, screenWidth);
            tutoPos.y = Mathf.Clamp(tutoPos.y, 30f, screenHeight);
            tutoIcon.transform.position = new Vector3(tutoPos.x, tutoPos.y, 0f);
        }
    }
}
