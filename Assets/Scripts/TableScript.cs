using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    public GameObject[] donutsPrefab;
    public GameObject[] cakePrefab;
    private void OnTriggerEnter(Collider other)
    {
        GameObject cust = other.gameObject;
        CustomerScript customerScript = cust.GetComponent<CustomerScript>();
        if(other.gameObject.CompareTag("Customer")&&customerScript.isEating == false)
        {
            GameManager.instance.player.DessertsUi(customerScript.getDesserts, donutsPrefab, cakePrefab);
            StartCoroutine(EatFood(customerScript.getDesserts, customerScript.eatingTime));
        }
    }
    IEnumerator EatFood(int[] desserts, int time)
    {
        for (int k = 0; k < desserts.Length; k++)
        {
            for (int j = desserts[k]-1; j >=0 ; j--)
            {
                switch (k)
                {
                    case 0:
                        yield return new WaitForSeconds(time / (desserts[0]+ desserts[1]));
                        donutsPrefab[j].SetActive(false);
                        break;
                    case 1:
                        yield return new WaitForSeconds(time / (desserts[0] + desserts[1]));
                        cakePrefab[j].SetActive(false);
                        break;
                }
            }
        }
    }
}
