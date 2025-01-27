using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GaugeScript : MonoBehaviour
{
    float currentGauge;
    float maxGauge;
    float gaugeSpeed;
    [SerializeField] GameObject gaugeObject;

    TextMeshPro tmp;
    bool isPlayerIn;
    Collider coll;
    [SerializeField] GameObject content;
    Vector3 officeScale = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        currentGauge = 0f;
        maxGauge = 500f;
        gaugeSpeed = 1f;
        coll = GetComponent<Collider>();
        tmp = GetComponentInChildren<TextMeshPro>();
        isPlayerIn = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) isPlayerIn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerIn = false;
    }
    void Start()
    {
        StartCoroutine("SpawnGauge");
    }
    IEnumerator SpawnGauge()
    {
        while (true)
        {
            if (isPlayerIn)
            {
                if(GameManager.instance.money > 0)
                {
                    GameManager.instance.money -= 1;
                    currentGauge += gaugeSpeed;
                    gaugeObject.transform.Translate(Vector3.up * (Time.fixedDeltaTime/20f));
                    gaugeObject.transform.localScale = new Vector3(1,currentGauge/500, 1);
                    GameManager.instance.MoneySync();
                    if (currentGauge >= maxGauge)
                    {
                        isPlayerIn=false;
                        coll.enabled = false;
                        currentGauge = maxGauge;
                        StartCoroutine("OfficeOn");
                    }
                }
                tmp.text = (maxGauge - currentGauge).ToString();
            }
            yield return null;
        }
    }
    IEnumerator OfficeOn()
    {
        GameManager.instance.cameraManager.CamCtrl(content.transform, GameManager.instance.cameraManager.officeEvent.transform);
        yield return new WaitForSeconds(0.7f);
        content.transform.DOScale(officeScale, 1.2f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.cameraManager.CamCtrl(GameManager.instance.player.transform, GameManager.instance.cameraManager.cam.transform);
        Destroy(gameObject);
    }
}
