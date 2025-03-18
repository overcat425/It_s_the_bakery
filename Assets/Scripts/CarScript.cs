using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CarScript : MonoBehaviour
{
    public bool isMoving;       // �̵��ϰ� �ִ���
    public bool isRequesting;   // �մ��� �����ؼ� �ֹ��ϱ⵵ ���� �����ִ� ���̽� ����
    public int isFull;              // ����Ʈ �䱸���� ����
    bool isHorn;
    public int[] requires; // 0�� ���ӿ䱸��, 1�� ����ũ�䱸��
    public int[] getDesserts;   // ���� ����/����ũ
    public Stack<Transform>[] carStack = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };
    public Transform[] carBaskets;  // ����Ʈ ������Ʈ ���� ��ġ

    private NavMeshAgent navMesh;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        isMoving = true;
        isFull = 0;
        InitTakeOut();
    }
    private void Update()
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        IsFull();
        if(isRequesting && !isHorn)
        {
            isHorn = true;
            transform.LookAt(GameManager.instance.customerMoving.turn[2]);
            RandomHorn();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            CarFalse();
        }
    }
    void IsFull()   // �䱸 ����Ʈ ����
    {
        if (getDesserts[0] == requires[0] && getDesserts[1] == requires[1]) isFull++;
        if (isFull == 1)
        {
            GameManager.instance.thruMoneyManager.DropMoney(getDesserts[0], getDesserts[1]);
            GameManager.instance.customerMoving.CarsShiftForward();
            isFull++;
            SoundManager.instance.PlaySound(SoundManager.Effect.Counter);
        }
    }
    void CarFalse()
    {
        for (int i = 0; i < getDesserts.Length; i++)
        {
            int len = carStack[i].Count;
            for(int j = 0; j < len; j++)
            {
                Transform desserts = carStack[i].Pop();
                Destroy(desserts.gameObject);
            }getDesserts[i] = 0;
        }gameObject.SetActive(false);
    }
    void InitTakeOut()     // ����̺꽺�� �ֹ��� 0~4�ε� �Ѵ� 0�̸� ����
    {
        isRequesting = false;
        if (GameManager.instance.upgradeScript.stoveLevel < 3)
        {
            requires[0] = Random.Range(1, 5);
        }
        else if (GameManager.instance.upgradeScript.stoveLevel >= 3)
        {
            requires = requires.Select(x => Random.Range(1, 5)).ToArray();
        }
        if (requires[0] <= 0 && requires[1] <= 0) InitTakeOut();
    }
    void RandomHorn()
    {
        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0:
                SoundManager.instance.PlaySound(SoundManager.Effect.Horn);
                break;
            case 1:
                SoundManager.instance.PlaySound(SoundManager.Effect.Horn2);
                break;
        }
    }
}