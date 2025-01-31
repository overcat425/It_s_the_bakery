using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GaugeScript : MonoBehaviour
{
    public GameObject prop;
    public enum Gauge { Door, Hall, Stove, Counter, Office, Drive }
    public Gauge type;
    [SerializeField] GameObject customerStart;
    public GameObject nextSpawn;
    float currentGauge;
    float maxGauge;
    float gaugeSpeed;
    [SerializeField] GameObject gaugeObject;

    TextMeshPro tmp;
    bool isPlayerIn;
    Collider coll;

    Vector3 scaleBigger = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        currentGauge = 0f;
        gaugeSpeed = 1f;
        coll = GetComponent<Collider>();
        tmp = GetComponentInChildren<TextMeshPro>();
        isPlayerIn = false;
        switch (type)
        {
            case Gauge.Door:
            case Gauge.Hall:
                maxGauge = 50f;
                break;
            case Gauge.Stove:
            case Gauge.Counter:
                maxGauge = 100f;
                break;
            case Gauge.Office:
                maxGauge = 500f;
                break;
            case Gauge.Drive:
                maxGauge = 1000f;
                break;
        }
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
                    gaugeObject.transform.Translate(Vector3.up * (Time.fixedDeltaTime/(20f * (maxGauge/500))));
                    gaugeObject.transform.localScale = new Vector3(1,currentGauge/maxGauge, 1);
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
        CameraManager cameraManager = GameManager.instance.cameraManager;
        TextScript textScript = GameManager.instance.textScript;
        switch (type)
        {
            case Gauge.Door:
            case Gauge.Hall:
                prop.transform.DOScale(scaleBigger, 0.8f).SetEase(Ease.OutElastic);
                break;
            case Gauge.Stove:
                prop.SetActive(true);
                prop.transform.DOScale(GameManager.instance.upgradeScript.stoveScale, 0.8f).SetEase(Ease.OutElastic);
                break;
            case Gauge.Counter:
                prop.transform.DOScale(scaleBigger, 1f).SetEase(Ease.OutElastic);
                customerStart.SetActive(true);
                break;
            case Gauge.Office:
                cameraManager.CamCtrl(prop.transform, cameraManager.eventCams[1].transform);
                yield return new WaitForSeconds(0.7f);
                prop.transform.DOScale(scaleBigger, 1.2f).SetEase(Ease.OutElastic);
                yield return new WaitForSeconds(1.5f);
                cameraManager.CamCtrl(GameManager.instance.player.transform, cameraManager.cam.transform);
                StartCoroutine(textScript.TextEffect(TextScript.TextType.Office));
                break;
            case Gauge.Drive:
                prop.transform.DOScale(scaleBigger, 1.2f).SetEase(Ease.OutElastic);
                StartCoroutine(textScript.TextEffect(TextScript.TextType.Drive));
                GameManager.instance.customerMoving.isThruEnable = true;
                break;
        }gameObject.transform.localScale = Vector3.zero;
        GameManager.instance.tutorialScript.NextPosition();
        GameManager.instance.textScript.ShowNextText();
        nextSpawn.SetActive(true);
        Destroy(gameObject, 7f);
    }
}