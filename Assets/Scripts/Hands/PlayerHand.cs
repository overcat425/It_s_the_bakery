using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : Hand
{
    public bool isTrashHand;    // �����⸦ ��� �ִ°�
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
    protected override void Carrying()     // �÷��̾� ���� ��� ����
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