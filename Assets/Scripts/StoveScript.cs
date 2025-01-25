using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class StoveScript : MonoBehaviour
{
    UpgradeScript upgradeScript;
    public enum StoveType { Donut, Cake }
    public StoveType stoveType;

    //public Queue<int> burgerQueue = new Queue<int>(); // 굳이 큐를 써야하는가......?????????
    public int stoveDesserts;
    [SerializeField] GameObject stoveStack;
    [SerializeField] GameObject[] stoveImg;
    int maxStove = 4;
    int speed;
    void Start()
    {
        StartCoroutine("MakeBurger");
    }
    private void LateUpdate()
    {
        stoveStack.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));
        BurgerUi();
    }
    IEnumerator MakeBurger()
    {
        while (true)
        {
            speed = GameManager.instance.upgradeScript.bakeSpeed;
            yield return new WaitForSecondsRealtime(speed);
            if (stoveDesserts < maxStove)
            {
                stoveDesserts++;
                stoveImg[stoveDesserts - 1].SetActive(true);
                //AddQueue(burger);
            }
        }
    }
    //void AddQueue(int burger)
    //{
    //    if (burgerQueue.Count < maxStoveBurger) burgerQueue.Enqueue(burger);
    //    burgers[burgerQueue.Count-1].SetActive(true);
    //}
    void BurgerUi()
    {
        if (stoveDesserts > 0)
        {
            stoveStack.SetActive(true);
        }
        else if (stoveDesserts <= 0) stoveStack.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            for(int  i = 0; i< maxStove; i++)
            {
                if (i < stoveDesserts)
                {
                    stoveImg[i].SetActive(true);
                }else stoveImg[i].SetActive(false);
            }
        }
    }
}
