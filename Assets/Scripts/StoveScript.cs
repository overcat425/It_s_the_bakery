using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class StoveScript : MonoBehaviour
{
    PlayerHand playerHand;
    public enum StoveType { Donut, Cake }   // ����Ʈ ����
    public StoveType stoveType;

    [SerializeField] Transform[] dessertsPlace = new Transform[4];      // ����Ʈ ������Ʈ ���� ��ġ
    public Stack<Transform> dessertsStack = new Stack<Transform>(); //����Ʈ ����(��������)
    [SerializeField] GameObject[] dessertPrefab;        // ����Ʈ ������
    [SerializeField] GameObject dessertBasket;          // ����Ʈ ���̾��Ű
    public Transform[] playerBaskets;                  // �÷��̾� ���̾��Ű
    [SerializeField] GameObject placeTrans;

    [SerializeField] GameObject stoveStack;
    [SerializeField] GameObject[] stoveImg;
    int maxStove = 4;
    int speed;
    float timer;
    void Start()
    {
        playerHand = GameManager.instance.playerHand;
        for(int i = 0; i < dessertsPlace.Length; i++)
        {
            dessertsPlace[i] = placeTrans.transform.GetChild(i);
        }
        StartCoroutine(MakeDesserts(stoveType));
    }
    //private void LateUpdate()
    //{
    //    //stoveStack.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));
    //    //BurgerUi();
    //}
    IEnumerator MakeDesserts( StoveType type)   // ���� Ÿ�Կ� ���� ����Ʈ ����
    {
        GameObject prefab = type == StoveType.Donut ? dessertPrefab[0] : dessertPrefab[1];
        while (true)
        {
            speed = GameManager.instance.upgradeScript.bakeSpeed;   // ���׷��̵� ������ ����ӵ�
            yield return new WaitForSecondsRealtime(speed);
            if(dessertBasket.transform.childCount < maxStove)   // ������ �� �� ���°� �ƴϸ� ����Ʈ ����
            {
                GameObject dessert = Instantiate(prefab, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity, dessertBasket.transform); ;
                dessert.transform.DOJump(new Vector3(dessertsPlace[dessertBasket.transform.childCount - 1].position.x, dessertsPlace[dessertBasket.transform.childCount - 1].position.y, dessertsPlace[dessertBasket.transform.childCount - 1].position.z), 1f, 1, 0.2f).SetEase(Ease.OutQuad);     // ����Ʈ�� ��ǥ ������Ʈ�� �����ϴ� ����
                ItemData dessertItem = dessert.GetComponent<ItemData>(); ;     // ����Ʈ�� ������ ItemData���� �����ͼ�
                dessertsStack.Push(dessertItem.transform);                               // dessertsStack ���ÿ� Push()
            }
        }
    }
    //void BurgerUi()           // ���� ����Ʈ ����UI
    //{
    //    if (dessertsStack.Count > 0)
    //    {
    //        stoveStack.SetActive(true);
    //    }
    //    else if (dessertsStack.Count <= 0) stoveStack.SetActive(false);
    //}
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (dessertsStack.Count <= 0 || playerHand.isTrashHand) return;   // ���쿡 �ƹ��͵� ������ �۵�X
            GameManager.instance.player.Tuto(0);    // Ʃ�丮��
            MoveDessert();
        }
    }
    void MoveDessert()
    {
        int i = stoveType == StoveType.Donut ? 0 : 1;   // �����̸� 0, ����ũ�� 1
        float above = stoveType == StoveType.Donut ? 0.08f : 0.12f; // ����Ʈ�� ���ΰ���(�״� �뵵)
        timer += Time.deltaTime;
        if (timer > 0.08f && playerBaskets[i].childCount < playerHand.maxPlayerDesserts) // 0.8�� ����
        {
            SoundManager.instance.PlaySound(SoundManager.Effect.Click);
            Transform dessert = dessertsStack.Pop();    // ������ ����Ʈ�� Pop
            dessert.SetParent(playerBaskets[i]);            // �÷��̾��� �ڽ����� �ΰ�
            Vector3 pos = Vector3.up * playerHand.hands[i].Count * above;  //��ġ����
            dessert.DOLocalJump(pos, 1.0f, 0, 0.3f);    // ���쿡�� �÷��̾� ������ DOJump

            dessert.localRotation = Quaternion.identity;    // ȸ������ zero
            playerHand.hands[i].Push(dessert);        // �÷��̾� �������� Push

            playerHand.isDessertHand = true;    // �÷��̾ ����Ʈ ������� true
            timer = 0f;
        }
    }
}