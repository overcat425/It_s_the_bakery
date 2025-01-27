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

    //[SerializeField] Transform spawnPoint;
    //public Transform destroyPoint;
    //public List<Transform> counters = new List<Transform>();
    //public List <GameObject> customerObjects = new List <GameObject>();
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
    //void Start()
    //{
    //    StartCoroutine("CustomersComing");
    //}
    //public void Destination(GameObject cust, Transform dest)
    //{                                       // 손님들 목적지 설정
    //    NavMeshAgent agent = cust.GetComponent<NavMeshAgent>();
    //    if (agent != null)
    //    {
    //        agent.SetDestination(dest.position);
    //    }
    //}
    //public void ShiftObjectsForward()    // 손님들 앞으로 한칸씩 이동
    //{
    //    if (customerObjects.Count > 0)
    //    {
    //        GameObject firstObject = customerObjects[0];
    //        customerObjects.RemoveAt(0);  // 첫 번째 오브젝트를 리스트에서 제거
    //        //firstObject.SetActive(false);
    //        for (int i = 0; i < customerObjects.Count; i++)
    //        {                   //  앞쪽으로 한 칸씩 이동(위치)
    //            Destination(customerObjects[i], counters[i]);
    //        }
    //        if (customerObjects.Count > 0 && counters.Count > customerObjects.Count)
    //        {           // 앞쪽으로 한 칸씩 이동(리스트번호)
    //            Destination(customerObjects[customerObjects.Count - 1], counters[customerObjects.Count-1]);
    //        }
    //    }
    //}
    //IEnumerator CustomersComing()       // 손님 생성 메소드
    //{
    //    while (true)
    //    {
    //        float rand = Random.Range(3, 7);        // 3~6초 랜덤쿨타임
    //        yield return new WaitForSeconds(rand);
    //        if(customerObjects.Count < 8)
    //        {
    //            GameObject enemy = customersPool.MakeBugy(0);
    //            enemy.transform.position = spawnPoint.position;
    //            customerObjects.Add(enemy);
    //            Destination(enemy, counters[customerObjects.Count-1]);
    //        }
    //    }
    //}
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