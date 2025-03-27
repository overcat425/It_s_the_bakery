using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isDessertHand;
    protected bool isCarrying;
    public Stack<Transform>[] hands = new Stack<Transform>[2] { new Stack<Transform>(), new Stack<Transform>() };
    protected Animator anim;
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }
    protected virtual void Carrying()
    {
    }
}
