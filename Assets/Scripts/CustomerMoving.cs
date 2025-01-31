using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CustomerMoving : MonoBehaviour
{
    [Header("목적지")]
    [SerializeField] Transform spawnPoint;
    public Transform destroyPoint;
    [SerializeField] Transform carSpawnPoint;
    public Transform carDestroyPoint;

    [Header("보행자")]
    public List<Transform> counters = new List<Transform>();
    public List<GameObject> customerObjects = new List<GameObject>();
    public List<Transform> seats = new List<Transform>();
    public List<GameObject> seatObjects = new List<GameObject>();

    [Header("드라이브스루")]
    public List<Transform> thru = new List<Transform>();
    public List<GameObject> carObjects = new List<GameObject>();
    public bool isThruEnable;

    public Transform[] turn;
    [SerializeField] GameObject noSeat;

    private void OnEnable()
    {
        StartCoroutine("CustomersComing");
    }
    private void Update()
    {
        if (isThruEnable)
        {
            StartCoroutine("CarsComing");
            isThruEnable = false;
        }
    }
    private void LateUpdate()
    {
        noSeat.transform.position = Camera.main.WorldToScreenPoint(counters[0].transform.position + new Vector3(0, 2f, 0));
    }
    public void Destination(GameObject cust, Transform dest)
    {                                       // 손님들 목적지 설정
        NavMeshAgent agent = cust.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(dest.position);
        }
    }
    public void ShiftObjectsForward()    // 손님들 앞으로 한칸씩 이동
    {
        if (customerObjects.Count > 0&& IsThereSeat()>0 )
        {
            noSeat.SetActive(false);
            GameObject firstObject = customerObjects[0];
            FindSeat(firstObject);
            customerObjects.RemoveAt(0);  // 첫 번째 오브젝트를 리스트에서 제거
            for (int i = 0; i < customerObjects.Count; i++)
            {                   //  앞쪽으로 한 칸씩 이동(위치)
                Destination(customerObjects[i], counters[i]);
            }
            if (customerObjects.Count > 0 && counters.Count > customerObjects.Count)
            {           // 앞쪽으로 한 칸씩 이동(리스트번호)
                Destination(customerObjects[customerObjects.Count - 1], counters[customerObjects.Count - 1]);
            }
        }else if(IsThereSeat() <= 0) {
            StartCoroutine("NoSeat");
        }
    }
    public void CarsShiftForward()
    {
        if (carObjects.Count > 0)
        {
            GameObject firstObject = carObjects[0];
            Destination(firstObject, carDestroyPoint);
            carObjects.RemoveAt(0);
            for (int i = 0; i < carObjects.Count; i++)
            {
                Destination(carObjects[i], thru[i]);
            }
            if(carObjects.Count > 0 && thru.Count > carObjects.Count)
            {
                Destination(carObjects[carObjects.Count-1], thru[carObjects.Count-1]);
            }
        }
    }
    public void FindSeat(GameObject cust)   // 좌석 찾아서 이동하는 메소드
    {
        for (int i = 0; i < seats.Count; i++)
        {
            if (seatObjects[i] == null)
            {
                seatObjects[i] = cust;
                Destination(seatObjects[i], seats[i]);
                break;
            }
        }
    }
    IEnumerator CustomersComing()       // 손님 생성 메소드
    {
        while (true)
        {
            float rand = Random.Range(3, 7);        // 3~6초 랜덤쿨타임
            yield return new WaitForSeconds(rand);
            if (customerObjects.Count < 8)
            {
                GameObject cust = GameManager.instance.customersPool.MakeBugy(0);
                cust.transform.position = spawnPoint.position;
                customerObjects.Add(cust);
                Destination(cust, counters[customerObjects.Count - 1]);
            }
        }
    }
    public IEnumerator CarsComing()
    {
        while (true)
        {
            float rand1 = Random.Range(5, 11);        // 5~10초 랜덤쿨타임
            int rand2 = Random.Range(1, 5);      // 1~4 랜덤프리팹(차종류)
            yield return new WaitForSeconds(rand1);
            if (carObjects.Count < 4)
            {
                GameObject car = GameManager.instance.customersPool.MakeBugy(rand2);
                car.transform.position = carSpawnPoint.position;
                carObjects.Add(car);
                Destination(car, thru[carObjects.Count - 1]);
            }
        }
    }
    int IsThereSeat()       // 앉을 자리 있는지 확인
    {
        int seat = 0;
        for (int i = 0; i < seats.Count; i++)
        {
            if (seatObjects[i] == null) seat++;
        }return seat;
    }
    IEnumerator NoSeat()
    {
        noSeat.SetActive(true);
        yield return new WaitForSeconds(3f);
        ShiftObjectsForward();
    }
}
