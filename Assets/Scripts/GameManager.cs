using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("인스턴스")]
    public static GameManager instance;

    [Header("스크립트")]
    #region
    public UpgradeScript upgradeScript;
    public CustomerScript customerScript;
    public CustomersPool customersPool;
    public CustomerMoving customerMoving;
    public CounterScript counterScript;
    public CounterDisplay counterDisplay;
    public CounterDisplay thruDisplay;
    public CameraManager cameraManager;
    public GaugeScript gaugeScript;
    public TextScript textScript;
    public TutorialScript tutorialScript;
    public ScriptData scriptData;
    public JoystickScript joystickScript;
    public Player player;
    public PlayerHand playerHand;
    public GettingMoney gettingMoney;
    public MoneyManager moneyManager;
    public MoneyManager thruMoneyManager;
    #endregion

    [SerializeField] Text moneyText;
    public int money;
    public int donutCost;
    public int cakeCost;
    public bool isCounter;
    public bool isDriveThru;

    [Header("일시정지 UI")]
    [SerializeField] GameObject pauseUi;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
        Costing();
    }
    public void GetMoney(int cost)      // 손님 계산도와드리겠읍니다
    {
        money += cost;
        MoneySync();
    }
    void Costing()      // 소지금 + 상품별 금액 초기화
    {
        money = 9999;
        donutCost = 30;
        cakeCost = 60;
    }
    public void MoneySync()
    {
        moneyText.text = string.Format("{0}", money);
    }
    public void PauseUiToggle()
    {
        if (pauseUi.activeSelf) { pauseUi.SetActive(false);
        }else if (pauseUi.activeSelf==false) pauseUi.SetActive(true);
    }
}