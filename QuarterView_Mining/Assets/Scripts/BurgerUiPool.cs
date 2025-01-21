using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerUiPool : MonoBehaviour
{
    List<GameObject>[] burgerPool;
    public GameObject[] burgerImg;
    private void Awake()
    {
        burgerPool = new List<GameObject>[burgerImg.Length];
        for (int i = 0; i< burgerImg.Length; i++)
        {
            burgerPool[i] = new List<GameObject>();
        }
    }
    public GameObject MakeBugy(int i)
    {
        GameObject active = null;
        foreach (GameObject item in burgerPool[i])
        {
            if (!item.activeSelf)
            {
                active = item;
                active.SetActive(true);
                break;
            }
        }
        if (!active)
        {
            active = Instantiate(burgerImg[i], transform);
            burgerPool[i].Add(active);
        } return active;
    }
}
