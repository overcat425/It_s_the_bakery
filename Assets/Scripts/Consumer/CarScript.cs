using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class CarScript : Consumer
{
    bool isHorn;
    public Stack<Transform>[] carStack = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        InitTakeOut();
    }
    private void Update()
    {
        isMoving = navMesh.remainingDistance <= 0.05f ? false : true;
        CheckIsFull();
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
    protected override void CheckIsFull()   // 요구 디저트 충족
    {
        base.CheckIsFull();
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
    void InitTakeOut()     // 드라이브스루 주문량 0~4인데 둘다 0이면 리롤
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