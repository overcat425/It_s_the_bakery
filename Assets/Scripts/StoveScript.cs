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
    public enum StoveType { Donut, Cake }   // 디저트 종류
    public StoveType stoveType;

    [SerializeField] Transform[] dessertsPlace = new Transform[4];      // 디저트 오브젝트 놓을 위치
    public Stack<Transform> dessertsStack = new Stack<Transform>(); //디저트 스택(정보저장)
    [SerializeField] GameObject[] dessertPrefab;        // 디저트 프리팹
    [SerializeField] GameObject dessertBasket;          // 디저트 하이어라키
    public Transform[] playerBaskets;                  // 플레이어 하이어라키
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
    IEnumerator MakeDesserts( StoveType type)   // 오븐 타입에 따라 디저트 생성
    {
        GameObject prefab = type == StoveType.Donut ? dessertPrefab[0] : dessertPrefab[1];
        while (true)
        {
            speed = GameManager.instance.upgradeScript.bakeSpeed;   // 업그레이드 가능한 생산속도
            yield return new WaitForSecondsRealtime(speed);
            if(dessertBasket.transform.childCount < maxStove)   // 오븐이 꽉 찬 상태가 아니면 디저트 생산
            {
                GameObject dessert = Instantiate(prefab, new Vector3(transform.position.x, -3f, transform.position.z), Quaternion.identity, dessertBasket.transform); ;
                dessert.transform.DOJump(new Vector3(dessertsPlace[dessertBasket.transform.childCount - 1].position.x, dessertsPlace[dessertBasket.transform.childCount - 1].position.y, dessertsPlace[dessertBasket.transform.childCount - 1].position.z), 1f, 1, 0.2f).SetEase(Ease.OutQuad);     // 디저트가 목표 오브젝트로 점프하는 연출
                ItemData dessertItem = dessert.GetComponent<ItemData>(); ;     // 디저트의 정보를 ItemData에서 가져와서
                dessertsStack.Push(dessertItem.transform);                               // dessertsStack 스택에 Push()
            }
        }
    }
    //void BurgerUi()           // 오븐 디저트 스택UI
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
            if (dessertsStack.Count <= 0 || playerHand.isTrashHand) return;   // 오븐에 아무것도 없으면 작동X
            GameManager.instance.player.Tuto(0);    // 튜토리얼
            MoveDessert();
        }
    }
    void MoveDessert()
    {
        int i = stoveType == StoveType.Donut ? 0 : 1;   // 도넛이면 0, 케이크면 1
        float above = stoveType == StoveType.Donut ? 0.08f : 0.12f; // 디저트별 세로간격(쌓는 용도)
        timer += Time.deltaTime;
        if (timer > 0.08f && playerBaskets[i].childCount < playerHand.maxPlayerDesserts) // 0.8초 간격
        {
            SoundManager.instance.PlaySound(SoundManager.Effect.Click);
            Transform dessert = dessertsStack.Pop();    // 오븐의 디저트를 Pop
            dessert.SetParent(playerBaskets[i]);            // 플레이어의 자식으로 두고
            Vector3 pos = Vector3.up * playerHand.hands[i].Count * above;  //위치설정
            dessert.DOLocalJump(pos, 1.0f, 0, 0.3f);    // 오븐에서 플레이어 손으로 DOJump

            dessert.localRotation = Quaternion.identity;    // 회전값은 zero
            playerHand.hands[i].Push(dessert);        // 플레이어 스택으로 Push

            playerHand.isDessertHand = true;    // 플레이어가 디저트 들고있음 true
            timer = 0f;
        }
    }
}