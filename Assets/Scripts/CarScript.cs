using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CarScript : MonoBehaviour
{
    public bool isMoving;       // 이동하고 있는지
    public bool isRequesting;   // 손님이 도착해서 주문하기도 전에 갖다주는 케이스 방지
    public int isFull;              // 디저트 요구사항 충족
    public int[] requires; // 0은 도넛요구량, 1은 케이크요구량
    public int[] getDesserts;   // 받은 도넛/케이크
    private NavMeshAgent navMesh;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        isMoving = true;
        isFull = 0;
        InitTakeOut();
    }
    private void Update()
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        if (getDesserts[0] == requires[0] && getDesserts[1] == requires[1]) isFull++;
        if (isFull == 1)
        {
            GameManager.instance.customerMoving.CarsShiftForward();
            isFull++;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            for (int i = 0; i < getDesserts.Length; i++)
            {
                getDesserts[i] = 0;
            }
            gameObject.SetActive(false);
        }
    }
    void InitTakeOut()     // 드라이브스루 주문량 0~4인데 둘다 0이면 리롤
    {
        isRequesting = false;
        if (GameManager.instance.upgradeScript.stoveLevel < 3)
        {
            requires[0] = Random.Range(1, 5);
        }
        else if (GameManager.instance.upgradeScript.stoveLevel >= 3)
        {
            requires = requires.Select(x => Random.Range(1, 5)).ToArray();
        }
        if (requires[0] <= 0 && requires[1] <= 0) InitTakeOut();
    }
}
