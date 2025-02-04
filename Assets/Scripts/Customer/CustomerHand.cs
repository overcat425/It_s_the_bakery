using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHand : MonoBehaviour
{
    public bool isDessertHand;
    public bool isCarrying;
    public Stack<Transform>[] customerHands = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };
    Animator anim;
    float timer;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Carrying();
    }
    void Carrying()
    {
        isCarrying = customerHands[0].Count + customerHands[1].Count > 0 ? true : false;
        anim.SetBool("isCarryMove", isCarrying);
    }
}
