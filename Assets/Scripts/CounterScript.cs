using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterScript : MonoBehaviour
{
    public GameObject requireBurgers;   // 손님이 요구하는 버거 Ui
    public GameObject[] burgerImg;      // 손님이 요구하는 버거 수 이미지
    public int require;
    private void Awake()
    {
        requireBurgers.SetActive(true);
    }
    private void Update()
    {
        if(GameManager.instance.customerObjects.Count >= 1)IsFirstObject();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            RefreshRequires();
        }
    }
    void IsFirstObject()        // 차례 된 손님의 햄버거 요구량 전시
    {
        GameObject yourTurn = GameManager.instance.customerObjects[0];
        CustomerScript customerScript = yourTurn.GetComponent<CustomerScript>();
        if (requireBurgers.activeSelf)
        {
            requireBurgers.transform.position = Camera.main.WorldToScreenPoint(yourTurn.transform.position + new Vector3(0, 3f, 0));
            for (int i = 0; i < customerScript.burgerRequire; i++)
            {
                burgerImg[i].SetActive(true);
            }
        }
    }
    public void RefreshRequires()
    {
        for (int i = 0; i < 5; i++)
        {
            burgerImg[i].SetActive(false);
        }
    }
}
