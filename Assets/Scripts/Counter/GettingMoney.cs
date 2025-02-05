using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingMoney : MonoBehaviour
{
    public enum CounterType { Counter, Thru }
    public CounterType type;

    CounterDisplay disPlay;
    public bool isSelling;
    public bool isGetting;
    public bool isThruing;
    [SerializeField] SpriteRenderer spriteRenderer;
    private void Start()
    {
        switch (type)
        {
            case CounterType.Counter:
                disPlay = GameManager.instance.counterDisplay;
                break;
            case CounterType.Thru:
                disPlay = GameManager.instance.thruDisplay;
                break;
        }
    }
    private void Update()
    {
        GetMoney();
    }
    void GetMoney()
    {
        switch (type)
        {
            case CounterType.Counter:
                if (isSelling && !isGetting && GameManager.instance.customerMoving.customerObjects.Count > 0) StartCoroutine("GetDessert");
                break;
            case CounterType.Thru:
                if (isThruing && !isGetting && GameManager.instance.customerMoving.carObjects.Count > 0) StartCoroutine("GetThru");
                break;
        }
    }
    IEnumerator GetDessert()
    {
        isGetting = true;
        GameObject yourTurn = GameManager.instance.customerMoving.customerObjects[0];
        CustomerHand customerHand = yourTurn.GetComponent<CustomerHand>();
        CustomerScript customerScript = yourTurn.GetComponent<CustomerScript>();

        for (int i = 0; i < customerHand.customerHands.Length; i++)
        {
            if (customerHand.customerHands[i].Count < customerScript.requires[i]&& disPlay.disPlayDesserts[i].Count >0 &&customerScript.isRequesting)
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
    IEnumerator GetThru()
    {
        isGetting = true;
        GameObject yourTurn = GameManager.instance.customerMoving.carObjects[0];
        CarScript carScript = yourTurn.GetComponent<CarScript>();

        for (int i = 0; i < carScript.carStack.Length; i++)
        {
            if (carScript.carStack[i].Count < carScript.requires[i] && disPlay.disPlayDesserts[i].Count > 0 && carScript.isRequesting)
            {
                float above = i == 0 ? 0.08f : 0.13f;
                Transform dessert = disPlay.disPlayDesserts[i].Pop();
                dessert.SetParent(carScript.carBaskets[i]);

                Vector3 pos = Vector3.up * carScript.carStack[i].Count * above;
                dessert.DOLocalJump(pos, 1f, 0, 0.3f);
                dessert.localRotation = Quaternion.identity;
                carScript.carStack[i].Push(dessert);
                carScript.getDesserts[i]++;
            }
        }
        yield return new WaitForSeconds(0.2f);
        isGetting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case CounterType.Counter:
                    isSelling = true;
                    break;
                case CounterType.Thru:
                    isThruing = true;
                    break;
            }
            spriteRenderer.DOColor(Color.cyan, 0.3f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case CounterType.Counter:
                    isSelling = false;
                    break;
                case CounterType.Thru:
                    isThruing = false;
                    break;
            }
            spriteRenderer.DOColor(Color.white, 0.3f);
        }
    }
}
