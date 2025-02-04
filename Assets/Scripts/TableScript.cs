using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    public Stack<Transform>[] tableDesserts = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };
    public Transform[] tableBasket = new Transform[2];
    public GameObject[] donutsPrefab;
    public GameObject[] cakePrefab;
    float timer;
    private void OnTriggerEnter(Collider other)     // 자리에 앉는 오브젝트가 가지고있는 디저트 수를 받아서 식탁에 내려놓음
    {
        GameObject cust = other.gameObject;
        CustomerHand customerHand = cust.GetComponent<CustomerHand>();
        CustomerScript customerScript = cust.GetComponent<CustomerScript>();
        if(other.gameObject.CompareTag("Customer")&&customerScript.isEating == false)
        {
            //GameManager.instance.player.DessertsUi(customerScript.getDesserts, donutsPrefab, cakePrefab);
            StartCoroutine(EatFood(customerScript.getDesserts, customerScript.eatingTime));


            while (customerHand.customerHands[0].Count > 0 || customerHand.customerHands[1].Count > 0)
            {
                for (int i = 0; i < customerHand.customerHands.Length; i++)
                {
                    if (customerHand.customerHands[i].Count > 0)
                    {
                        float above = i == 0 ? 0.08f : 0.12f;
                        Transform dessert = customerHand.customerHands[i].Pop();
                        dessert.SetParent(tableBasket[i]);

                        Vector3 pos = Vector3.up * tableDesserts[i].Count * above;
                        dessert.DOLocalJump(pos, 1f, 0, 0.3f);
                        dessert.localRotation = Quaternion.identity;
                        tableDesserts[i].Push(dessert);
                    }
                }
            }
        }
    }

    IEnumerator EatFood(int[] desserts, int time)
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
                        //donutsPrefab[j].SetActive(false);
                        dessert = tableDesserts[k].Pop();
                        Destroy(dessert.gameObject);
                        break;
                    case 1:
                        yield return new WaitForSeconds(time / (desserts[0] + desserts[1]));
                        //cakePrefab[j].SetActive(false);
                        dessert = tableDesserts[k].Pop();
                        Destroy(dessert.gameObject);
                        break;
                }
            }
        }
    }
}
