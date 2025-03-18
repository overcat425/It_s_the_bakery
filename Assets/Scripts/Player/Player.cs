using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    public float speed;
    public bool[] isTuto = new bool[4];  // 0:Stove 1:DisPlay 2:Sell 3:Money
    public Vector2 inputVec;
    public Vector3 moveVec;
    float camY;

    public int[] playerDesserts = { 0, 0 };  // 0�� Donut, 1�� Cake
    [SerializeField] GameObject[] donutsPrefab;
    [SerializeField] GameObject[] cakePrefab;
    public int maxPlayerDesserts;

    float timer;
    void Start()
    {
        maxPlayerDesserts = 5;
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Move();
    }
    //void OnMove(InputValue value)
    //{
    //    //inputVec = value.Get<Vector2>();
    //}
    void Move()
    {
        camY = Camera.main.transform.eulerAngles.y;   // ī�޶� -45 ȸ���� ���� �ذ�å
        Quaternion camRot = Quaternion.Euler(0f, camY, 0f);   // ī�޶��� Y�ุ ���� ��,
        speed = (2 + GameManager.instance.upgradeScript.moveSpeed * 0.3f);
        inputVec = GameManager.instance.joystickScript.inputVec;
        moveVec = camRot *  new Vector3(inputVec.x, 0, inputVec.y);   // �÷��̾��� �̵����� ������
        transform.position += moveVec * speed * Time.deltaTime;
        transform.LookAt(moveVec+transform.position);
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    StoveScript stoveScript = collision.gameObject.GetComponent<StoveScript>();
    //    switch (collision.gameObject.tag)
    //    {
    //        case "Stove":
    //            DessertsPickUp(stoveScript, 0);
    //            break;
    //        case "CakeStove":
    //            DessertsPickUp(stoveScript, 1);
    //            break;
    //        case "Trash":
    //            for (int i = 0; i < maxPlayerDesserts; i++)
    //            {
    //                donutsPrefab[i].SetActive(false);
    //                cakePrefab[i].SetActive(false);
    //                if (i < 2) playerDesserts[i] = 0;
    //            }
    //            break;
    //    }
    //}
    //void DessertsPickUp(StoveScript stoveScript, int i)
    //{
    //    if (stoveScript.stoveDesserts <= 0) return;
    //    if (i == 0) Tuto(0);
    //    while (playerDesserts[i] < maxPlayerDesserts && stoveScript.stoveDesserts > 0)
    //    {
    //        stoveScript.stoveDesserts--;
    //        playerDesserts[i]++;
    //        SoundManager.instance.PlaySound(SoundManager.Effect.Click);
    //    }
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Counter")){
    //        if (GameManager.instance.customerMoving.customerObjects.Count <= 0) return; //�մ��� �������� �������� ����
    //        CustomerScript customerScript = GameManager.instance.customerMoving.customerObjects[0].GetComponent<CustomerScript>();
    //        GetMoneyPast(customerScript.requires, customerScript.getDesserts, customerScript.isRequesting);
    //    }
    //    if (other.CompareTag("Thru"))
    //    {
    //        if (GameManager.instance.customerMoving.carObjects.Count <= 0) return;
    //        CarScript carScript = GameManager.instance.customerMoving.carObjects[0].GetComponent<CarScript>();
    //        GetMoneyPast(carScript.requires, carScript.getDesserts, carScript.isRequesting);
    //    }
    //}
    //void GetMoneyPast(int[] requires, int[] getDesserts, bool isRequesting) {
    //    for (int i = 0; i < playerDesserts.Length; i++)
    //    {
    //        if(requires[i] <= getDesserts[i] || isRequesting == false) continue;
    //        while (playerDesserts[i] > 0)
    //        {
    //            HandDessert(i);
    //            playerDesserts[i]--;
    //            getDesserts[i]++;
    //            if (requires[i] <= getDesserts[i]) break;
    //        }if (i == 0) Tuto(1);
    //    }
    //    GameManager.instance.upgradeScript.DisableBtn();
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))   // �� ȹ��
        {
            MoneyManager moneyManager = other.GetComponentInParent<MoneyManager>();
            int stackCount = moneyManager.moneyStack.Count;
            if (stackCount > 0)
            {
                StartCoroutine(PlayerGetMoney(stackCount, moneyManager));
            }
        }
    }
    IEnumerator PlayerGetMoney(int stackCount, MoneyManager moneyManager)
    {
        SoundManager.instance.PlaySound(SoundManager.Effect.Click);
        for (int i = 0; i < stackCount; i++)
        {
            Transform money = moneyManager.moneyStack.Pop();    // money ���ÿ��� Pop
            GameManager.instance.GetMoney(10);          // �� ȹ��
            money.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.01f);     // �� ȹ�� ����
        }
        Tuto(3);
    }
    //void HandDessert(int i)
    //{
    //    switch (i)
    //    {
    //        case 0:
    //            donutsPrefab[playerDesserts[i] - 1].SetActive(false);
    //            GameManager.instance.GetMoney(GameManager.instance.donutCost);
    //            break;
    //        case 1:
    //            cakePrefab[playerDesserts[i] - 1].SetActive(false);
    //            GameManager.instance.GetMoney(GameManager.instance.cakeCost);
    //            break;
    //    }
    //}
    //public void DessertsUi(int[] desserts, GameObject[] donuts, GameObject[] cake)
    //{
    //    GameObject[][] dessertObjects = { donuts, cake }; // 0: ����, 1: ����ũ
    //    for (int i = 0; i < desserts.Length; i++)
    //    {
    //        int limit = Mathf.Min(desserts[i], dessertObjects[i].Length);
    //        for (int j = 0; j < limit; j++)
    //        {
    //            dessertObjects[i][j].SetActive(true);
    //        }
    //    }
    //}
    public void Tuto(int i)     // Ʃ�丮��
    {
        if (isTuto[i] == false)
        {
            GameManager.instance.tutorialScript.NextPosition();
            GameManager.instance.textScript.ShowNextText();
            isTuto[i] = true;
        }
    }
}