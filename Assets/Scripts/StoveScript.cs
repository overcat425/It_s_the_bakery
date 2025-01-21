using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveScript : MonoBehaviour
{
    [SerializeField] Player player;
    public Queue<int> burgerQueue = new Queue<int>();
    [SerializeField] GameObject burgerStack;
    [SerializeField] GameObject[] burgers;
    int maxStoveBurger = 4;
    void Start()
    {
        StartCoroutine("MakeBurger");
    }
    private void LateUpdate()
    {
        burgerStack.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));
        BurgerUi();
    }
    IEnumerator MakeBurger()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            int burger = burgerQueue.Count + 1;
            AddQueue(burger);
        }
    }
    void AddQueue(int burger)
    {
        if (burgerQueue.Count < maxStoveBurger) burgerQueue.Enqueue(burger);
        burgers[burgerQueue.Count-1].SetActive(true);
    }
    void BurgerUi()
    {
        if (burgerQueue.Count > 0)
        {
            burgerStack.SetActive(true);
        }
        else if (burgerQueue.Count <= 0) burgerStack.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for(int  i = 0; i< maxStoveBurger; i++)
            {
                if (i < burgerQueue.Count)
                {
                    burgers[i].SetActive(true);
                }else burgers[i].SetActive(false);
            }
        }
    }
}
