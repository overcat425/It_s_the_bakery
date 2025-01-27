using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [Header("인스턴스")]
    public static GameManager instance;
    public UpgradeScript upgradeScript;
    public CustomerScript customerScript;
    public CustomersPool customersPool;
    public CustomerMoving customerMoving;
    public CounterScript counterScript;
    public CameraManager cameraManager;
    public Player player;

    [SerializeField] ItemData[] itemData;
    [SerializeField] Text moneyText;
    public int money;
    public int donutCost;
    public int cakeCost;

    [Header("일시정지 UI")]
    [SerializeField] GameObject pauseUi;
    private void Awake()        // 싱글톤
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
        money = 0;
        for (int i = 0; i < itemData.Length; i++)
        {
            switch (itemData[i].itemType)
            {
                case ItemData.ItemType.Doughnut:
                    donutCost = 50;
                    break;
                case ItemData.ItemType.Cake:
                    cakeCost = 100;
                    break;
            }
        }
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