using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoneyManager : MonoBehaviour
{
    public Stack<Transform> moneyStack = new Stack<Transform>();
    public Transform[] moneyPlace = new Transform[8];
    public GameObject placeMoney;
    void Start()
    {
        for(int i = 0; i < moneyPlace.Length; i++)
        {
            moneyPlace[i] = placeMoney.transform.GetChild(i);
        }
    }
    public void DropMoney(int donut, int cake)
    {
        for (int i = 0; i < donut*3 + cake*6; i++)
        {
            GameObject money = GameManager.instance.customersPool.MakeBugy(5);  // �� ������
            money.transform.position = new Vector3(moneyPlace[moneyStack.Count % 8].position.x,
            moneyPlace[moneyStack.Count % 8].position.y +((moneyStack.Count / 8)*0.07f),
            moneyPlace[moneyStack.Count % 8].position.z);
            /* �� �������� 1�� ���� 2x4�� 8���̸�, x��ǥ�� z��ǥ�� 8�� ���� �������� ����
             1~8���� �ݺ��ϰ�,  y��ǥ�� 8�� ���� �� ���� �����. */

            ItemData moneyItem = money.GetComponent<ItemData>();
            moneyStack.Push(moneyItem.transform);
        }
    }
}
