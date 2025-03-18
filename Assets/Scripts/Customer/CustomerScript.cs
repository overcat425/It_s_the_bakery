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
    public bool isMoving;       // �̵��ϰ� �ִ���
    public bool isRequesting;   // �մ��� �����ؼ� �ֹ��ϱ⵵ ���� �����ִ� ���̽� ����
    public bool isEating;       // �¼��� �����ؼ� �Ա� ����
    public int isFull;              // ����Ʈ �䱸���� ����
    bool isGetting;
    public int[] requires; // 0�� ���ӿ䱸��, 1�� ����ũ�䱸��
    public int[] getDesserts;   // ���� ����Ʈ (��ġ)
    public Transform[] customerBaskets; // ����Ʈ ������Ʈ ���� ��ġ
    public int eatingTime;          // �Դ� �ð� ����
    private NavMeshAgent navMesh;   // �̵����
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
        if (isFull == 1)    // ����Ʈ �� �޾��� ��
        {
            GameManager.instance.moneyManager.DropMoney(getDesserts[0], getDesserts[1]);
            GameManager.instance.customerMoving.ShiftObjectsForward();
            isFull++;
            SoundManager.instance.PlaySound(SoundManager.Effect.Counter);
            GameManager.instance.player.Tuto(2);
            //GameManager.instance.customerMoving.Destination(gameObject, GameManager.instance.customerMoving.seats[0]);//destroyPoint);
        }
    }
    public void MoveOrNot()     // �ȱ�,���� �ִϸ��̼� ��Ʈ��
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        anim.SetBool("isWalk", isMoving);
    }
    void InitRequire()     // ��ǰ �䱸�� �޼ҵ� ; 0~3�ε� �Ѵ� 0�̸� ����
    {
        int stoveLevel = GameManager.instance.upgradeScript.stoveLevel;
        isRequesting = false;
        switch (stoveLevel)
        {
            case 3:         // ������ ��
                requires = requires.Select(x => Random.Range(1, 4)).ToArray();
                break;
            default:        // ���� �ƴ� ��
                requires[0] = Random.Range(1, 4);
                break;
        }
        if (requires[0] <= 0 && requires[1]<=0) InitRequire();
    }
    IEnumerator EatingTime()        // �Դ� ����
    {                                                                            
        CustomerMoving customerMoving = GameManager.instance.customerMoving;
        int index = customerMoving.seatObjects.IndexOf(gameObject);  // ���� �ڸ���ȣ�� �а�
        LookAtTable(index);                                          // ���̺��� �ٶ󺸵��� ��

        yield return StartCoroutine(SitDown()); // �ɴ� �ִϸ��̼�

        GameObject trash = GameManager.instance.customersPool.MakeBugy(6);  // ������ ������
        TransformTrash(trash);

        yield return StartCoroutine(StandUp());  // �Ͼ�� �ִϸ��̼�
        customerMoving.Destination(gameObject, customerMoving.destroyPoint); // �մ� ����
        customerMoving.seatObjects[index] = trash;      // �ڸ��� ������ �ΰ�
        chairScript.isTrash = true;
        EatDessertsAll();
        navMesh.isStopped = false;              // �̵��Ұ� ����
    }
    void TransformTrash(GameObject trash)
    {
        chairScript.chairDesserts[0].Push(trash.transform);
        trash.transform.localScale = Vector3.one;
        trash.transform.SetParent(chairScript.chairBasket[0], false);   // �����ڸ��� �������� �θ�� �����ϰ� ��ġ����
    }
    IEnumerator SitDown()
    {
        anim.SetTrigger("sit");
        yield return WaitForAnimation("StandToSit", 0.8f);
        anim.SetBool("isEating", true);         // �Դ� �ִϸ��̼� ����
        navMesh.isStopped = true;       // �Դ� ������ �̵� �Ұ�
        yield return new WaitForSeconds(eatingTime);    //�Դ� �ð�
    }
    IEnumerator StandUp()
    {
        anim.SetTrigger("stand");
        yield return WaitForAnimation("SitToStand", 1f);
        anim.SetBool("isEating", false);        // �Դ� �ִϸ��̼� ��
    }
    IEnumerator WaitForAnimation(string animation, float duration)
    {
        yield return new WaitUntil(() => //�ִϸ��̼� ���� ������ ��ٸ��� �޼ҵ�
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