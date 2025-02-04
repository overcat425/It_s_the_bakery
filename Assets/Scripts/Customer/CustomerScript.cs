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
    //public Transform homeTrans;
    public bool isMoving;       // 이동하고 있는지
    public bool isRequesting;   // 손님이 도착해서 주문하기도 전에 갖다주는 케이스 방지
    public bool isEating;       // 좌석에 도착해서 먹기 시작
    public int isFull;              // 디저트 요구사항 충족
    bool isCarrying;            // 디저트 들고있는지 애니메이션여부
    bool isGetting;
    public int[] requires; // 0은 도넛요구량, 1은 케이크요구량
    public int[] getDesserts;   // 받은 도넛/케이크
    public Transform[] customerBaskets;
    //[SerializeField] GameObject[] donutsPrefab;
    //[SerializeField] GameObject[] cakePrefab;
    public int eatingTime;
    private NavMeshAgent navMesh;   // 이동경로
    Rigidbody rigid;
    public Animator anim;

    float timer;
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
        eatingTime = Random.Range(25, 35);
    }
    void Update()
    {
        MoveOrNot();
        if (getDesserts[0] == requires[0] && getDesserts[1] == requires[1]) isFull++;
        if (isFull == 1)
        {
            //StartCoroutine("HoldDesserts");
            GameManager.instance.customerMoving.ShiftObjectsForward();
            isFull++;
            SoundManager.instance.PlaySound(SoundManager.Effect.Counter);
            //GameManager.instance.customerMoving.Destination(gameObject, GameManager.instance.customerMoving.seats[0]);//destroyPoint);
        }
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
        if (requires[0] <= 0 && requires[1]<=0) InitRequire();
    }
    IEnumerator EatingTime()        // 먹는 동작
    {                                                                             // 앉을 자리번호 선택
        int index = GameManager.instance.customerMoving.seatObjects.IndexOf(gameObject);
        LookAtTable(index);//SitOn(index);
        yield return StartCoroutine("SitDown"); // 앉는 애니메이션
        anim.SetBool("isEating", true);         // 먹는 애니메이션 시작
        navMesh.isStopped = true;
        yield return new WaitForSeconds(eatingTime);    //먹는 시간
        yield return StartCoroutine("StandUp");  // 일어나는 애니메이션
        anim.SetBool("isEating", false);        // 먹는 애니메이션 끝
        GameManager.instance.customerMoving.Destination(gameObject, GameManager.instance.customerMoving.destroyPoint); // 손님 퇴장
        GameManager.instance.customerMoving.seatObjects[index] = null;
        for (int i = 0; i < getDesserts.Length; i++)
        {
            getDesserts[i] = 0;
        }
        navMesh.isStopped = false;              // 먹는 동안은 이동 불가
    }
    IEnumerator SitDown()
    {
        anim.SetTrigger("sit");
        yield return WaitForAnimation("StandToSit", 0.8f);
    }
    IEnumerator StandUp()
    {
        anim.SetTrigger("stand");
        yield return WaitForAnimation("SitToStand", 1f);
    }
    IEnumerator WaitForAnimation(string animation, float duration)
    {
        yield return new WaitUntil(() => //애니메이션 끝날 때까지 기다리는 메소드
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            return state.IsName(animation) && state.normalizedTime >= duration;
        });
    }
    //IEnumerator HoldDesserts()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    GameManager.instance.player.DessertsUi(getDesserts, donutsPrefab, cakePrefab);
    //}
    //void SitOn(int i)
    //{
    //    for (int k = 0; k < getDesserts.Length; k++)
    //    {
    //        for (int j = 0; j < getDesserts[k]; j++)
    //        {
    //            switch (k)
    //            {
    //                case 0:
    //                    donutsPrefab[j].SetActive(false);
    //                    break;
    //                case 1:
    //                    cakePrefab[j].SetActive(false);
    //                    break;
    //            }
    //        }
    //    }
    //    LookAtTable(i);
    //}
    void LookAtTable(int i)
    {
        switch (i%2)
        {
            case 0:
                gameObject.transform.LookAt(GameManager.instance.customerMoving.turn[0]);
                break;
            case 1:
                gameObject.transform.LookAt(GameManager.instance.customerMoving.turn[1]);
                break;
        }
    }
}