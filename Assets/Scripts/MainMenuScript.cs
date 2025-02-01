using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    public void OnClickStart()
    {
        buttons.SetActive(false);
        LoadingScript.Instance.LoadScene("Bakery");
    }
}
