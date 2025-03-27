using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHand : Hand
{
    float timer;
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        Carrying();
    }
    protected override void Carrying()
    {
        isCarrying = hands[0].Count + hands[1].Count > 0 ? true : false;
        anim.SetBool("isCarryMove", isCarrying);
    }
}
