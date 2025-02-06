using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public Text tutorialText;
    public ScriptData scriptData;
    int currentIndex;
    bool init;

    public enum TextType { Office = 9, Drive = 10}
    private void Start()
    {
        tutorialText.gameObject.SetActive(true);
        currentIndex = 0;
        StartCoroutine("TextMoving");
        StartCoroutine(TutorialText(currentIndex));
    }
    public IEnumerator TextEffect(TextType textType)
    {
        tutorialText.text = scriptData.scriptTexts[(int)textType].text;
        float time = Time.time;
        float distance = 20f;
        tutorialText.gameObject.SetActive(true);
        while (Time.time - time < 5f)
        {
            TextEvent(distance);
            TextEvent(-distance);
        }
        tutorialText.gameObject.SetActive(false);
        yield return null;
    }
    void TextEvent(float yPos)
    {
        tutorialText.transform.DOMoveY(tutorialText.transform.position.y + yPos, 1f).SetEase(Ease.InOutSine).WaitForCompletion();
    }
    IEnumerator TutorialText(int index)
    {
        tutorialText.text = scriptData.scriptTexts[index].text;
        yield return null;
    }
    public void ShowNextText()
    {
        currentIndex++;
        if(currentIndex < 8)
        {
            StartCoroutine(TutorialText(currentIndex));
        }
        else if(currentIndex == 8)
        {
            StartCoroutine("TextEnd");
        }
    }
    IEnumerator TextMoving()
    {
        while (true)
        {
            yield return tutorialText.transform.DOMoveY(tutorialText.transform.position.y + 20, 1f).SetEase(Ease.InOutSine).WaitForCompletion();
            yield return tutorialText.transform.DOMoveY(tutorialText.transform.position.y - 20, 1f).SetEase(Ease.InOutSine).WaitForCompletion();
        }
    }
    IEnumerator TextEnd()
    {
        StartCoroutine(TutorialText(currentIndex));
        yield return new WaitForSeconds(6f);
        tutorialText.gameObject.SetActive(false);
    }
}
