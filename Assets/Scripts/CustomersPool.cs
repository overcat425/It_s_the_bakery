using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomersPool : MonoBehaviour
{
    public List<GameObject>[] customerPool;
    public GameObject[] customers;
    private void Awake()
    {
        customerPool = new List<GameObject>[customers.Length];
        for (int i = 0; i< customers.Length; i++)
        {
            customerPool[i] = new List<GameObject>();
        }
    }
    public GameObject MakeBugy(int i)
    {
        GameObject active = null;
        foreach (GameObject item in customerPool[i])
        {
            if (!item.activeSelf)
            {
                active = item;
                active.SetActive(true);
                break;
            }
        }
        if (!active)
        {
            active = Instantiate(customers[i], transform);
            customerPool[i].Add(active);
        } return active;
    }
}
