using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public bool isDessertHand;  // 디저트를 들고 있는가
    public bool isTrashHand;    // 쓰레기를 들고 있는가
    public bool isCarrying;
    public int maxPlayerDesserts;
    public Stack<Transform>[] playerHands  = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };

    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        maxPlayerDesserts = GameManager.instance.player.maxPlayerDesserts;
        HandControll();
        Carrying();
    }
    void HandControll()
    {
        if (playerHands[0].Count <= 0 && playerHands[1].Count <= 0)
        {
            isDessertHand = false;
            isTrashHand = false;
        }
        isCarrying = isDessertHand || isTrashHand;
    }
    void Carrying()     // 플레이어 물건 운반 여부
    {
        Player player = GameManager.instance.player;
        if (!isCarrying)
        {
            anim.SetBool("isCarry", false); anim.SetBool("isCarryMove", false);
            anim.SetBool("isWalk", player.moveVec != Vector3.zero);
        }
        else if (isCarrying)
        {
            anim.SetBool("isCarry", player.moveVec == Vector3.zero);
            anim.SetBool("isCarryMove", player.moveVec != Vector3.zero);
        }
    }
}