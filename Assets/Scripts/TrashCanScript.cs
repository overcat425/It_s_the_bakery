using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanScript : MonoBehaviour
{
    PlayerHand playerHand;
    public Transform trashBasket;
    float timer;
    void Start()
    {
        playerHand = GameManager.instance.playerHand;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerHand.isTrashHand) return;
            timer += Time.deltaTime;
            if (timer > 0.05f && playerHand.hands[0].Count > 0)
            {
                SoundManager.instance.PlaySound(SoundManager.Effect.Click);
                Transform trash = playerHand.hands[0].Pop();
                trash.SetParent(trashBasket);

                Vector3 pos = Vector3.zero;
                trash.DOLocalJump(pos, 1f, 0, 0.3f);
                trash.localRotation = Quaternion.identity;
                trash.gameObject.SetActive(false);
                timer = 0f;
            }
        }
    }
}
