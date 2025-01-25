using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using DG.Tweening;
using System.Data.SqlTypes;
using System.Linq;

public class UpgradeScript : MonoBehaviour
{
    [SerializeField] GameObject uiPanel;
    [SerializeField] Transform[] uiPos;

    [Header("업그레이드 요소")]
    public int[] levels;    // 1 bake, 2 move, 3 stove, 4
    public int bakeSpeed;
    public int moveSpeed;
    public int stoveLevel;
    int maxLevel;

    public GameObject[] Stoves;
    public Vector3 stoveScale = new Vector3(0.8f, 0.8f, 0.8f);

    [SerializeField] int[] costTable;

    [Header("Ui")]
    [SerializeField] Text[] texts;
    [SerializeField] Text[] costs;
    [SerializeField] GameObject[] upgradeDisable;
    private void Awake()
    {
        levels = Enumerable.Repeat(1,4).ToArray();
        bakeSpeed = 6;
        moveSpeed = 1;
        stoveLevel = 1;
        maxLevel = 3;
    }
    public void OnClickUpgradebakeSpeed(int i)      
    {
        if (levels[i]-1 < maxLevel && GameManager.instance.money >= costTable[i] * levels[i])
        {
            GameManager.instance.money -= costTable[i] * levels[i];
            levels[i]++;
            switch (i)
            {
                case 0:
                    bakeSpeed--;
                    break;
                case 1:
                    moveSpeed++;
                    break;
                case 2:
                    stoveLevel++;
                    StartCoroutine(EnableStove(Stoves[stoveLevel-2]));
                    break;
                case 3:
                    break;
            }
            UpgradeSync(i, levels[i], costTable[i] * levels[i]);
            GameManager.instance.MoneySync();
        }DisableBtn();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisableBtn();
            for(int i = 0; i < levels.Length; i++)
            {
                UpgradeSync(i, levels[i], costTable[i] * levels[i]);
            }
            UpgradeOn();
        }
    }
    public void UpgradeSync(int num, int level, int cost)
    {
            switch (level-1)
            {
                case 3:                 // 만렙이면 Max표시
                    texts[num].text = string.Format("Lv.Max");
                    costs[num].text = string.Format("Max");
                    break;
                default:                // 그외엔 정상작동
                    texts[num].text = string.Format("Lv.{0:F0}", level);
                    costs[num].text = string.Format("{0}", cost);
                    break;
            }
    }
    public void DisableBtn()
    {
        for (int i = 0; i < upgradeDisable.Length; i++)
        {
            if (GameManager.instance.money < costTable[i] * levels[i] || levels[i] > maxLevel)
                upgradeDisable[i].SetActive(true);
            else if (GameManager.instance.money >= costTable[i] * levels[i])
                upgradeDisable[i].SetActive(false);
        }
    }
    public void UpgradeOn()
    {
        uiPanel.transform.DOMove(uiPos[0].position, 0.3f).SetEase(Ease.OutCirc);
    }
    public void UpgradeOff()
    {
        uiPanel.transform.DOMove(uiPos[1].position, 0.3f).SetEase(Ease.InCirc);
    }
    IEnumerator EnableStove(GameObject stove)
    {
        UpgradeOff();
        GameManager.instance.cameraManager.vCam.LookAt = stove.transform;
        GameManager.instance.cameraManager.vCam.Follow = GameManager.instance.cameraManager.stoveEvent.transform;
        yield return new WaitForSeconds(1f);
        stove.SetActive(true);
        stove.transform.DOScale(stoveScale, 0.6f).SetEase(Ease.OutElastic);
        yield return new WaitForSeconds(0.8f);
        GameManager.instance.cameraManager.vCam.LookAt = GameManager.instance.player.transform;
        GameManager.instance.cameraManager.vCam.Follow = GameManager.instance.cameraManager.cam.transform;
    }
}