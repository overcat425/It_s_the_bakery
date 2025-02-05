using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairScript : MonoBehaviour
{
    public Stack<Transform>[] chairDesserts = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };
    public Transform[] chairBasket = new Transform[2];
    private void Start()
    {
        for (int i = 0; i < chairBasket.Length; i++)
        {
            chairBasket[i] = transform.GetChild(i);     // 의자 당 디저트 놓을 곳 할당
        }
    }
    private void OnTriggerEnter(Collider other)     // 자리에 앉는 오브젝트가 가지고있는 디저트 수를 받아서 식탁에 내려놓음
    {
        GameObject cust = other.gameObject;
        CustomerHand customerHand = cust.GetComponent<CustomerHand>();
        CustomerScript customerScript = cust.GetComponent<CustomerScript>();
        if(other.gameObject.CompareTag("Customer")&&customerScript.isEating == false)
        {
            StartCoroutine(EatFood(customerScript.getDesserts, customerScript.eatingTime));


            while (customerHand.customerHands[0].Count > 0 || customerHand.customerHands[1].Count > 0)
            {
                for (int i = 0; i < customerHand.customerHands.Length; i++)
                {
                    if (customerHand.customerHands[i].Count > 0)
                    {
                        float above = i == 0 ? 0.08f : 0.12f;
                        Transform dessert = customerHand.customerHands[i].Pop();
                        dessert.SetParent(chairBasket[i]);

                        Vector3 pos = Vector3.up * chairDesserts[i].Count * above;
                        dessert.DOLocalJump(pos, 1f, 0, 0.3f);
                        dessert.localRotation = Quaternion.identity;
                        chairDesserts[i].Push(dessert);
                    }
                }
            }
        }
    }
    IEnumerator EatFood(int[] desserts, int time)   // 손님이 먹으면서 디저트오브젝트 소멸
    {
        Transform dessert;
        for (int k = 0; k < desserts.Length; k++)
        {
            for (int j = desserts[k]-1; j >=0 ; j--)
            {
                switch (k)
                {
                    case 0:
                        yield return new WaitForSeconds(time / (desserts[0]+ desserts[1]));
                        dessert = chairDesserts[k].Pop();
                        Destroy(dessert.gameObject);
                        break;
                    case 1:
                        yield return new WaitForSeconds(time / (desserts[0] + desserts[1]));
                        dessert = chairDesserts[k].Pop();
                        Destroy(dessert.gameObject);
                        break;
                }TableMoney();
            }
        }
    }
    //void TableMoney()
    //{
    //    TableScript tableScript = GetComponentInParent<TableScript>();
    //    int rand = Random.Range(0, 4);
    //    for (int i = 0; i < rand; i++)
    //    {
    //        GameObject money = GameManager.instance.customersPool.MakeBugy(5);
    //        money.transform.position = new Vector3(tableScript.tableMoneyPlace[tableScript.tableMoneyStack.Count % 8].position.x, tableScript.tableMoneyPlace[tableScript.tableMoneyStack.Count % 8].position.y + ((tableScript.tableMoneyStack.Count / 8) * 0.07f), tableScript.tableMoneyPlace[tableScript.tableMoneyStack.Count % 8].position.z);
    //        ItemData moneyItem = money.GetComponent<ItemData>();
    //        tableScript.tableMoneyStack.Push(moneyItem.transform);

    //    }
    //}
    void TableMoney()
    {
        MoneyManager moneyManager = GetComponentInParent<MoneyManager>();
        int rand = Random.Range(0, 4);
        for (int i = 0; i < rand; i++)
        {
            GameObject money = GameManager.instance.customersPool.MakeBugy(5);
            money.transform.position = new Vector3(moneyManager.moneyPlace[moneyManager.moneyStack.Count % 8].position.x, moneyManager.moneyPlace[moneyManager.moneyStack.Count % 8].position.y + ((moneyManager.moneyStack.Count / 8) * 0.07f), moneyManager.moneyPlace[moneyManager.moneyStack.Count % 8].position.z);
            ItemData moneyItem = money.GetComponent<ItemData>();
            moneyManager.moneyStack.Push(moneyItem.transform);

        }
    }
}