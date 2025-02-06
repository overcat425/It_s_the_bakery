using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterDisplay : MonoBehaviour // 상품 계산 스크립트
{
    PlayerHand playerHand;
    public Stack<Transform>[] disPlayDesserts = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };
    public Transform[] counterBasket = new Transform[2];

    [SerializeField] GameObject placeTrans;
    float timer;

    private void Start()
    {
        playerHand = GameManager.instance.playerHand;
        for (int i = 0; i < counterBasket.Length; i++) {
            counterBasket[i] = placeTrans.transform.GetChild(i);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerHand.isDessertHand) return;
            GameManager.instance.player.Tuto(1);
            timer += Time.deltaTime;
            if (timer > 0.08f && (playerHand.playerHands[0].Count > 0 || playerHand.playerHands[1].Count > 0))
            {
                MoveDessert();
                timer = 0f;
            }
        }
    }
    void MoveDessert()
    {
        SoundManager.instance.PlaySound(SoundManager.Effect.Click);
        for (int i = 0; i < playerHand.playerHands.Length; i++)
        {
            if (playerHand.playerHands[i].Count > 0)
            {
                float above = i == 0 ? 0.08f : 0.12f;
                Transform dessert = playerHand.playerHands[i].Pop();
                dessert.SetParent(counterBasket[i]);

                Vector3 pos = Vector3.up * disPlayDesserts[i].Count * above;
                dessert.DOLocalJump(pos, 1f, 0, 0.3f);
                dessert.localRotation = Quaternion.identity;
                disPlayDesserts[i].Push(dessert);
            }
        }
    }
}