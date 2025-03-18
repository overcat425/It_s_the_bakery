using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CustomerScript : MonoBehaviour
{
    ChairScript chairScript;
    //public Transform homeTrans;
    public bool isMoving;       // 이동하고 있는지
    public bool isRequesting;   // 손님이 도착해서 주문하기도 전에 갖다주는 케이스 방지
    public bool isEating;       // 좌석에 도착해서 먹기 시작
    public int isFull;              // 디저트 요구사항 충족
    bool isGetting;
    public int[] requires; // 0은 도넛요구량, 1은 케이크요구량
    public int[] getDesserts;   // 받은 디저트 (수치)
    public Transform[] customerBaskets; // 디저트 오브젝트 받을 위치
    public int eatingTime;          // 먹는 시간 랜덤
    private NavMeshAgent navMesh;   // 이동경로
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
        isEating = false;
        isMoving = true;
        isFull = 0;
        InitRequire();
        eatingTime = Random.Range(20, 30);
    }
    void Update()
    {
        MoveOrNot();
        CheckIsFull();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destroy"))
        {
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Seat")&&isEating==false)
        {
            StartCoroutine("EatingTime");
            isEating= true;
            chairScript = other.GetComponent<ChairScript>();
        }
    }
    void CheckIsFull()
    {
        if (getDesserts[0] == requires[0] && getDesserts[1] == requires[1]) isFull++;
        if (isFull == 1)    // 디저트 다 받았을 때
        {
            GameManager.instance.moneyManager.DropMoney(getDesserts[0], getDesserts[1]);
            GameManager.instance.customerMoving.ShiftObjectsForward();
            isFull++;
            SoundManager.instance.PlaySound(SoundManager.Effect.Counter);
            GameManager.instance.player.Tuto(2);
            //GameManager.instance.customerMoving.Destination(gameObject, GameManager.instance.customerMoving.seats[0]);//destroyPoint);
        }
    }
    public void MoveOrNot()     // 걷기,정지 애니메이션 컨트롤
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        anim.SetBool("isWalk", isMoving);
    }
    void InitRequire()     // 상품 요구량 메소드 ; 0~3인데 둘다 0이면 리롤
    {
        int stoveLevel = GameManager.instance.upgradeScript.stoveLevel;
        isRequesting = false;
        switch (stoveLevel)
        {
            case 3:         // 만렙일 때
                requires = requires.Select(x => Random.Range(1, 4)).ToArray();
                break;
            default:        // 만렙 아닐 때
                requires[0] = Random.Range(1, 4);
                break;
        }
        if (requires[0] <= 0 && requires[1]<=0) InitRequire();
    }
    IEnumerator EatingTime()        // 먹는 동작
    {                                                                            
        CustomerMoving customerMoving = GameManager.instance.customerMoving;
        int index = customerMoving.seatObjects.IndexOf(gameObject);  // 앉은 자리번호를 읽고
        LookAtTable(index);                                          // 테이블을 바라보도록 함

        yield return StartCoroutine(SitDown()); // 앉는 애니메이션

        GameObject trash = GameManager.instance.customersPool.MakeBugy(6);  // 쓰레기 프리팹
        TransformTrash(trash);

        yield return StartCoroutine(StandUp());  // 일어나는 애니메이션
        customerMoving.Destination(gameObject, customerMoving.destroyPoint); // 손님 퇴장
        customerMoving.seatObjects[index] = trash;      // 자리에 쓰레기 두고감
        chairScript.isTrash = true;
        EatDessertsAll();
        navMesh.isStopped = false;              // 이동불가 해제
    }
    void TransformTrash(GameObject trash)
    {
        chairScript.chairDesserts[0].Push(trash.transform);
        trash.transform.localScale = Vector3.one;
        trash.transform.SetParent(chairScript.chairBasket[0], false);   // 먹은자리를 쓰레기의 부모로 설정하고 위치설정
    }
    IEnumerator SitDown()
    {
        anim.SetTrigger("sit");
        yield return WaitForAnimation("StandToSit", 0.8f);
        anim.SetBool("isEating", true);         // 먹는 애니메이션 시작
        navMesh.isStopped = true;       // 먹는 동안은 이동 불가
        yield return new WaitForSeconds(eatingTime);    //먹는 시간
    }
    IEnumerator StandUp()
    {
        anim.SetTrigger("stand");
        yield return WaitForAnimation("SitToStand", 1f);
        anim.SetBool("isEating", false);        // 먹는 애니메이션 끝
    }
    IEnumerator WaitForAnimation(string animation, float duration)
    {
        yield return new WaitUntil(() => //애니메이션 끝날 때까지 기다리는 메소드
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            return state.IsName(animation) && state.normalizedTime >= duration;
        });
    }
    void LookAtTable(int i)
    {
        Transform[] turn = GameManager.instance.customerMoving.turn;
        gameObject.transform.LookAt(turn[i % 2]);
    }
    void EatDessertsAll()
    {
        for (int i = 0; i < getDesserts.Length; i++)
        {
            getDesserts[i] = 0;
        }
    }
}