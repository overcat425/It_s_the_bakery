using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : Hand
{
    public bool isTrashHand;    // 쓰레기를 들고 있는가
    public int maxPlayerDesserts;
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        maxPlayerDesserts = GameManager.instance.player.maxPlayerDesserts;
        HandControll();
        Carrying();
    }
    void HandControll()
    {
        if (hands[0].Count <= 0 && hands[1].Count <= 0)
        {
            isDessertHand = false;
            isTrashHand = false;
        }
        isCarrying = isDessertHand || isTrashHand;
    }
    protected override void Carrying()     // 플레이어 물건 운반 여부
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