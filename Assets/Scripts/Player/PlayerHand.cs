using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public bool isDessertHand;
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
        Carrying();
    }
    void Carrying()
    {
        isCarrying = playerHands[0].Count + playerHands[1].Count > 0 ? true : false;
        if (!isCarrying)
        {
            anim.SetBool("isCarry", false); anim.SetBool("isCarryMove", false);
            anim.SetBool("isWalk", GameManager.instance.player.moveVec != Vector3.zero);
        }
        else if (isCarrying)
        {
            anim.SetBool("isCarry", GameManager.instance.player.moveVec == Vector3.zero);
            anim.SetBool("isCarryMove", GameManager.instance.player.moveVec != Vector3.zero);
        }
    }
}