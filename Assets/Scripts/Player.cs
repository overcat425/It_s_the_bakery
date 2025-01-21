using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public StoveScript stoveScript;

    Rigidbody rigid;
    [SerializeField] float speed;
    float hor;
    float ver;
    bool runKey;
    Vector3 moveVec;
    Animator anim;

    public Queue<int> playerQueue = new Queue<int>();
    [SerializeField] GameObject playerStack;
    [SerializeField] GameObject[] burgers;
    public int maxPlayerBurger = 5;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        StartCoroutine("PlayerBurgers");
    }
    void Update()
    {
        Inputs();
        Move();
    }
    private void LateUpdate()
    {
        playerStack.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3f, 0));
        BurgerUi();
    }
    IEnumerator PlayerBurgers()
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
        transform.LookAt(moveVec+transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Stove")
        {
            StoveScript stoveScript = collision.gameObject.GetComponent<StoveScript>();
            while (playerQueue.Count < maxPlayerBurger)
            {
                int burger = stoveScript.burgerQueue.Dequeue();
                playerQueue.Enqueue(burger);
                if (stoveScript.burgerQueue.Count <= 0) break;
            }
        }
        if (collision.gameObject.tag == "Trash")
        {
            for (int i = 0; i < maxPlayerBurger; i++)
            {
                burgers[i].SetActive(false);
            }
            playerQueue.Clear();
        }
        if (collision.gameObject.tag == "Counter")
        {
            CustomerScript customerScript = GameManager.instance.customerObjects[0].GetComponent<CustomerScript>();
            int req = customerScript.burgerRequire;
            while(playerQueue.Count > 0)
            {
                burgers[playerQueue.Count-1].SetActive(false);
                playerQueue.Dequeue();
                req--;
                if(req <= 0) break;
            }
            customerScript.burgerRequire = req;
        }
    }
    void BurgerUi()
    {
        if (playerQueue.Count > 0)
        {
            playerStack.SetActive(true);
        }
        else if (playerQueue.Count <= 0) playerStack.SetActive(false);
        for (int i = 0; i < playerQueue.Count; ++i)
        {
            burgers[i].SetActive(true);
        }
    }
}