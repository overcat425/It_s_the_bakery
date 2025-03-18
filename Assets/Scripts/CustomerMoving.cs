using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CustomerMoving : MonoBehaviour
{
    [Header("������")]
    [SerializeField] Transform spawnPoint;
    public Transform destroyPoint;
    [SerializeField] Transform carSpawnPoint;
    public Transform carDestroyPoint;

    [Header("������")]
    public List<Transform> counters = new List<Transform>();    // �����ڰ� �� ���� ��ġ
    public List<GameObject> customerObjects = new List<GameObject>();   // �� ���ִ� ������
    public List<Transform> seats = new List<Transform>();               // �����ڰ� ���� �ڸ���ġ
    public List<GameObject> seatObjects = new List<GameObject>();   // �ɾ��ִ� ������

    [Header("����̺꽺��")]
    public List<Transform> thru = new List<Transform>();        // �ڵ����� �� ������ ��ġ
    public List<GameObject> carObjects = new List<GameObject>();    // �� ���ִ� �ڵ���
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
    {                                       // �մԵ� ������ ����
        NavMeshAgent agent = cust.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(dest.position);
        }
    }
    public void ShiftObjectsForward()    // �� �� �մԵ� ������ ��ĭ�� �̵�
    {
        if (customerObjects.Count > 0&& IsThereSeat()>0 )
        {
            noSeat.SetActive(false);
            GameObject firstObject = customerObjects[0];        // ù��° ������Ʈ ���� ��
            FindSeat(firstObject);  // �¼����� �̵���Ű��
            Shift();    // ��ĭ�� ������ �̵�
        }else if(IsThereSeat() <= 0)    // �ڸ� ������ �ڸ����� ��ǳ�� ����
        {
            StartCoroutine("NoSeat");
        }
    }
    public void FindSeat(GameObject cust)   // �����¼� ã�Ƽ� �̵��ϴ� �޼ҵ�
    {
        int rand = Random.Range(0, seats.Count);
        if (seatObjects[rand] == null)
        {
            seatObjects[rand] = cust;
            Destination(seatObjects[rand], seats[rand]);
        }
        else if (seatObjects[rand] != null) FindSeat(cust);
    }
    void Shift()
    {
        customerObjects.RemoveAt(0);  // ù ��° ������Ʈ�� ����Ʈ���� ����
        for (int i = 0; i < customerObjects.Count; i++)
        {                   //  �������� �� ĭ�� �̵�(��ġ)
            Destination(customerObjects[i], counters[i]);
        }
        if (customerObjects.Count > 0 && counters.Count > customerObjects.Count)
        {           // �������� �� ĭ�� �̵�(����Ʈ��ȣ)
            Destination(customerObjects[customerObjects.Count - 1], counters[customerObjects.Count - 1]);
        }
    }
    IEnumerator CustomersComing()       // �մ� ���� �޼ҵ�
    {
        while (true)
        {
            float rand = Random.Range(5, 11);        // 5~10�� ������Ÿ��
            yield return new WaitForSeconds(rand);
            if (customerObjects.Count < 8)
            {
                GameObject cust = GameManager.instance.customersPool.MakeBugy(0); // ������Ʈ ������ ����
                cust.transform.position = spawnPoint.position;
                customerObjects.Add(cust);                                      // ������Ʈ Ǯ�� ����Ʈ�� ���
                Destination(cust, counters[customerObjects.Count - 1]); // ī���ͷ� ���� �ֹ��ϵ��� ����
            }
        }
    }
    int IsThereSeat()       // ���� �ڸ� �ִ��� Ȯ��
    {
        int seat = 0;
        for (int i = 0; i < seats.Count; i++)
        {
            if (seatObjects[i] == null) seat++;
        }return seat;
    }
    IEnumerator NoSeat()    // ���� �ڸ��� ������ �ڸ��� ����� UI ǥ��
    {
        noSeat.SetActive(true);
        yield return new WaitForSeconds(3f);
        ShiftObjectsForward();
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
    public IEnumerator CarsComing()
    {
        while (true)
        {
            float rand1 = Random.Range(5, 11);        // 5~10�� ������Ÿ��
            int rand2 = Random.Range(1, 5);      // 1~4 ����������(������)
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
}
