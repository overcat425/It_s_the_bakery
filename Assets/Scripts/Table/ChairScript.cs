using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ChairScript : MonoBehaviour
{
    PlayerHand playerHand;
    public Stack<Transform>[] chairDesserts = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };
    public Transform[] chairBasket = new Transform[2];
    public bool isTrash;

    public Transform[] playerBaskets;
    [SerializeField] int chairNum;
    private void Start()
    {
        playerHand = GameManager.instance.playerHand;
        for (int i = 0; i < chairBasket.Length; i++)
        {
            chairBasket[i] = transform.GetChild(i);     // ���� �� ����Ʈ ���� �� �Ҵ�
        }
    }
    private void OnTriggerEnter(Collider other)     // �ڸ��� �ɴ� ������Ʈ�� �������ִ� ����Ʈ ���� �޾Ƽ� ��Ź�� ��������
    {
        GameObject cust = other.gameObject;
        if(other.CompareTag("Customer"))//&&customerScript.isEating == false)
        {
            DropDessertsToDesk(cust);
        }
        if (other.CompareTag("Player") && isTrash && !playerHand.isDessertHand)
        {
            CleanTrash();
        }
    }
    void DropDessertsToDesk(GameObject cust)
    {
        CustomerScript customerScript = cust.GetComponent<CustomerScript>();
        CustomerHand customerHand = cust.GetComponent<CustomerHand>();
        StartCoroutine(EatFood(customerScript.requires, customerScript.eatingTime));
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
    IEnumerator EatFood(int[] desserts, int time)   // �մ��� �����鼭 ����Ʈ������Ʈ �Ҹ�
    {
        float eatingTime = time / (desserts[0] + desserts[1]);
        for (int k = 0; k < desserts.Length; k++)
        {
            for (int j = desserts[k]-1; j >=0 ; j--)
            {
                yield return new WaitForSeconds(eatingTime);
                EraseDesserts(k);
                TableMoney();
            }
        }
    }
    void EraseDesserts(int i)
    {
        Transform dessert = chairDesserts[i].Pop();
        Destroy(dessert.gameObject);
    }
    void TableMoney()       // �մ��� ���̺� �� �������� �ΰ��� �޼ҵ�
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
    void CleanTrash()
    {
        SoundManager.instance.PlaySound(SoundManager.Effect.Click);
        Transform trash = chairDesserts[0].Pop();
        trash.SetParent(playerBaskets[0]);
        Vector3 pos = Vector3.up * playerHand.playerHands[0].Count * 0.13f;
        trash.DOLocalJump(pos, 1f, 0, 0.3f);
        trash.localRotation = Quaternion.identity;
        playerHand.playerHands[0].Push(trash);
        playerHand.isTrashHand = true;
        isTrash = false;
        GameManager.instance.customerMoving.seatObjects[chairNum] = null;
    }
}