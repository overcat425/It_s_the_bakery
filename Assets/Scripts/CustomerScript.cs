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

    private NavMeshAgent navMesh;
    Rigidbody rigid;
    public Animator anim;
    public int[] requires; // 0은 도넛요구량, 1은 케이크요구량
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        isMoving = true;
    }
    private void OnEnable()
    {
        InitRequire();
    }
    void Update()
    {
        MoveOrNot();
        if (requires[0] <= 0 && requires[1]<=0)
        {
            GameManager.instance.ShiftObjectsForward();
            gameObject.SetActive(false);
        }
    }
    public void MoveOrNot()     // 걷기,정지 애니메이션 컨트롤
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        anim.SetBool("isWalk", isMoving);
    }
    void InitRequire()
    {
        isRequesting = false;
        requires = requires.Select(x => Random.Range(1, 4)).ToArray();
        if (requires[0] == 0 && requires[1]==0) InitRequire();
    }
}
