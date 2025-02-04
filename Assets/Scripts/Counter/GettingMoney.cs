using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingMoney : MonoBehaviour
{
    CounterDisplay disPlay;
    public bool isSelling;
    public bool isGetting;
    [SerializeField] SpriteRenderer spriteRenderer;
    private void Start()
    {
        disPlay = GameManager.instance.counterDisplay;
    }
    private void Update()
    {
        if (isSelling&& !isGetting&& GameManager.instance.customerMoving.customerObjects.Count > 0
            && !(disPlay.disPlayDesserts[0].Count== 0 && disPlay.disPlayDesserts[1].Count==0)) StartCoroutine("GetDessert");
    }
    IEnumerator GetDessert()
    {
        isGetting = true;
        GameObject yourTurn = GameManager.instance.customerMoving.customerObjects[0];
        CustomerHand customerHand = yourTurn.GetComponent<CustomerHand>();
        CustomerScript customerScript = yourTurn.GetComponent<CustomerScript>();

        for (int i = 0; i < customerHand.customerHands.Length; i++)
        {
            if (customerHand.customerHands[i].Count < customerScript.requires[i]&&customerScript.isRequesting)
            {
                float above = i == 0 ? 0.08f : 0.13f;
                Transform dessert = disPlay.disPlayDesserts[i].Pop();
                dessert.SetParent(customerScript.customerBaskets[i]);

                Vector3 pos = Vector3.up * customerHand.customerHands[i].Count *above;
                dessert.DOLocalJump(pos, 1f, 0, 0.3f);
                dessert.localRotation = Quaternion.identity;
                customerHand.customerHands[i].Push(dessert);
                customerScript.getDesserts[i]++;
            }
        }
        yield return new WaitForSeconds(0.5f);
        isGetting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSelling = true;
            spriteRenderer.DOColor(Color.cyan, 0.3f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSelling = false;
            spriteRenderer.DOColor(Color.white, 0.3f);
        }
    }
}
