using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField] float speed;
    float hor;
    float ver;
    bool runKey;
    Vector3 moveVec;
    Animator anim;

    //public Queue<int> playerQueue = new Queue<int>(); // 생각해보니까 굳이 큐로 써야되는가...??????????
    public int[] playerDesserts = { 0, 0 };  // 0이 Donut, 1이 Cake
    [SerializeField] GameObject[] playerInven;
    [SerializeField] GameObject playerUi;
    [SerializeField] GameObject[] donutsImg;
    [SerializeField] GameObject[] cakeImg;
    public int maxPlayerDesserts = 5;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        StartCoroutine("PlayerDesserts");
    }
    void Update()
    {
        Inputs();
        Move();
    }
    private void LateUpdate()
    {
        playerUi.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));
        DessertsUi();
    }
    IEnumerator PlayerDesserts()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
        }
    }
    void Inputs()
    {
        hor = Input.GetAxisRaw("Horizontal");
        ver = Input.GetAxisRaw("Vertical");
        runKey = Input.GetButton("Run");
        //jumpKey = Input.GetButtonDown("Jump");
    }
    void Move()
    {
        moveVec = new Vector3(hor, 0, ver).normalized;
        transform.position += moveVec * speed * Time.deltaTime * (runKey?1.5f:1f);
        anim.SetBool("isWalk", moveVec != Vector3.zero);
        if(moveVec != Vector3.zero)anim.SetBool("isRun", runKey);
        if (moveVec == Vector3.zero) anim.SetBool("isRun", false);
        transform.LookAt(moveVec+transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        StoveScript stoveScript = collision.gameObject.GetComponent<StoveScript>();
        switch (collision.gameObject.tag)
        {
            case "Stove":
                if (stoveScript.stoveDesserts > 0)
                {
                    while (playerDesserts[0] < maxPlayerDesserts)
                    {
                        stoveScript.stoveDesserts--; playerDesserts[0]++;
                        if (stoveScript.stoveDesserts <= 0) break;
                    }
                }
                break;
            case "CakeStove":
                if (stoveScript.stoveDesserts > 0)
                {
                    if (stoveScript.stoveDesserts > 0)
                    {
                        while (playerDesserts[1] < maxPlayerDesserts)
                        {
                            stoveScript.stoveDesserts--; playerDesserts[1]++;
                            if (stoveScript.stoveDesserts <= 0) break;
                        }
                    }
                }
                break;
            case "Trash":
                for (int i = 0; i < maxPlayerDesserts; i++)
                {
                    donutsImg[i].SetActive(false);
                    cakeImg[i].SetActive(false);
                    if (i < 2) playerDesserts[i] = 0;
                }
                break;
            case "Counter":
                if (GameManager.instance.customerObjects.Count <= 0) return;
                CustomerScript customerScript = GameManager.instance.customerObjects[0].GetComponent<CustomerScript>();
                for (int i = 0; i < playerDesserts.Length; i++)
                {
                    int req = customerScript.requires[i];
                    if (req == 0 || customerScript.isRequesting == false) continue;
                    while (playerDesserts[i] > 0)
                    {
                        switch (i)
                        {
                            case 0:
                                donutsImg[playerDesserts[i] - 1].SetActive(false);
                                GameManager.instance.GetMoney(GameManager.instance.donutCost);
                                break;
                            case 1:
                                cakeImg[playerDesserts[i] - 1].SetActive(false);
                                GameManager.instance.GetMoney(GameManager.instance.cakeCost);
                                break;
                        }
                        playerDesserts[i]--;
                        req--;
                        if (req <= 0) break;
                    }
                    customerScript.requires[i] = req;
                }
                break;
        }
    }
    void DessertsUi()
    {
        for(int i = 0; i < playerInven.Length; i++)
        {
            if (playerDesserts[i] > 0)
            {
                playerInven[i].SetActive(true);
            }
            else if (playerDesserts[i] <= 0) playerInven[i].SetActive(false);

            for (int j = 0; j < playerDesserts[i]; j++)
            {
                switch (i)
                {
                    case 0:
                        donutsImg[j].SetActive(true);
                    break;
                        case 1:
                        cakeImg[j].SetActive(true);
                    break;
                }
            }
        }
    }
}