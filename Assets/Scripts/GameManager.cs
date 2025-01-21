using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CustomerScript customerScript;
    public CustomersPool customersPool;
    public CounterScript counterScript;
    public Player player;

    [SerializeField] Transform spawnPoint;
    public List<Transform> counters = new List<Transform>();
    public List <GameObject> customerObjects = new List <GameObject>();
    private void Awake()        // 싱글톤
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(instance.gameObject);
        }
    }
    void Start()
    {
        StartCoroutine("CustomersComing");
    }
    public void Destination(GameObject cust, Transform dest)
    {
        NavMeshAgent agent = cust.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.SetDestination(dest.position);
        }
    }
    public void ShiftObjectsForward()
    {
        if (customerObjects.Count > 0)
        {
            GameObject firstObject = customerObjects[0];
            customerObjects.RemoveAt(0);  // 첫 번째 오브젝트를 리스트에서 제거
            firstObject.SetActive(false);
            for (int i = 0; i < customerObjects.Count; i++)
            {                   //  앞쪽으로 한 칸씩 이동(위치)
                Destination(customerObjects[i], counters[i]);
            }
            if (customerObjects.Count > 0 && counters.Count > customerObjects.Count)
            {           // 앞쪽으로 한 칸씩 이동(리스트번호)
                Destination(customerObjects[customerObjects.Count - 1], counters[customerObjects.Count-1]);
            }
        }
    }
    IEnumerator CustomersComing()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(customerObjects.Count < 8)
            {
                GameObject enemy = customersPool.MakeBugy(0);
                enemy.transform.position = spawnPoint.position;
                customerObjects.Add(enemy);
                Destination(enemy, counters[customerObjects.Count-1]);
            }
        }
    }
}
