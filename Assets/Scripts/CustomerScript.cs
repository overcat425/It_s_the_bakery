using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class CustomerScript : MonoBehaviour
{
    public Transform homeTrans;
    public bool isMoving;

    private NavMeshAgent navMesh;
    Rigidbody rigid;
    Animator anim;
    public int burgerRequire;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        isMoving = true;
    }
    private void OnEnable()
    {
        burgerRequire = Random.Range(1, 6);
    }
    void Update()
    {
        MoveOrNot();
        if (burgerRequire <= 0)
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
}
