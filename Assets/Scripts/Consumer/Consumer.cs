using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Consumer : MonoBehaviour
{
    public bool isMoving;       // �̵��ϰ� �ִ���
    public bool isRequesting;   // �մ��� �����ؼ� �ֹ��ϱ⵵ ���� �����ִ� ���̽� ����
    public int[] requires; // 0�� ���ӿ䱸��, 1�� ����ũ�䱸��
    public int[] getDesserts;   // ���� ����Ʈ (��ġ)
    public Transform[] baskets;  // ����Ʈ ������Ʈ ���� ��ġ

    public int isFull;              // ����Ʈ �䱸���� ����

    protected NavMeshAgent navMesh;   // �̵����
    // Start is called before the first frame update
    protected virtual void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }
    protected virtual void OnEnable()
    {
        isMoving = true;
        isFull = 0;
    }
    protected virtual void CheckIsFull()
    {
        if (getDesserts[0] == requires[0] && getDesserts[1] == requires[1]) isFull++;
    }
}
