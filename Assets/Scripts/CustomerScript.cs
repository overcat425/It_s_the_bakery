using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CustomerScript : MonoBehaviour
{
    public Transform homeTrans;
    public bool isMoving;
    public bool isRequesting;   // 손님이 도착해서 주문하기도 전에 갖다주는 케이스 방지
    public int[] requires; // 0은 도넛요구량, 1은 케이크요구량

    private NavMeshAgent navMesh;
    Rigidbody rigid;
    public Animator anim;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        isMoving = true;
        InitRequire();
    }
    void Update()
    {
        MoveOrNot();
        if (requires[0] <= 0 && requires[1]<=0)
        {
            GameManager.instance.customerMoving.ShiftObjectsForward();
            requires[0]++; // 둘다 00인 상태면 리스트에 있는걸 싹다 빼버림..
            GameManager.instance.customerMoving.Destination(gameObject, GameManager.instance.customerMoving.destroyPoint);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            gameObject.SetActive(false);
        }
    }
    public void MoveOrNot()     // 걷기,정지 애니메이션 컨트롤
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        anim.SetBool("isWalk", isMoving);
    }
    void InitRequire()     // 상품 요구량 메소드 ; 0~3인데 둘다 0이면 리롤
    {
        isRequesting = false;
        if (GameManager.instance.upgradeScript.stoveLevel < 3)
        {
            requires[0] = Random.Range(1, 4);
        }
        else if (GameManager.instance.upgradeScript.stoveLevel >= 3)
        {
            requires = requires.Select(x => Random.Range(1, 4)).ToArray();
        }
        if (requires[0] == 0 && requires[1]==0) InitRequire();
    }
}