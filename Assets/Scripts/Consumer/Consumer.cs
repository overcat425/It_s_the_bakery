using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Consumer : MonoBehaviour
{
    public bool isMoving;       // 이동하고 있는지
    public bool isRequesting;   // 손님이 도착해서 주문하기도 전에 갖다주는 케이스 방지
    public int[] requires; // 0은 도넛요구량, 1은 케이크요구량
    public int[] getDesserts;   // 받은 디저트 (수치)
    public Transform[] baskets;  // 디저트 오브젝트 받을 위치

    public int isFull;              // 디저트 요구사항 충족

    protected NavMeshAgent navMesh;   // 이동경로
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
