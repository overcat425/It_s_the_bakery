using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class AutoDoorScript : MonoBehaviour
{
    bool isActivate;
    public GameObject[] door;
    [SerializeField]float[] openPoint;
    [SerializeField] float[] closePoint;
    void Start()
    {
        isActivate = false;
        openPoint = new float[] { -2.7f, 1.3f };
        closePoint = new float[] { -1.4f, 0f };
        StartCoroutine("DoorOpen");
    }
    IEnumerator DoorOpen()
    {
        while (true)
        {
            if (isActivate)
            {
                CloseDoor(door[0], openPoint[0]); // 180도로 열리기 때문
                OpenDoor(door[1], openPoint[1]);
            }
            else if (!isActivate)
            {
                OpenDoor(door[0], closePoint[0]);
                CloseDoor(door[1], closePoint[1]);
            }
            yield return null;
        }
    }
    void OpenDoor(GameObject door, float point)
    {
        if (door.transform.position.z < point)
        {
            door.transform.Translate(0, 0, 0.05f);
        }
    }
    void CloseDoor(GameObject door, float point)
    {
        if (door.transform.position.z > point)
        {
            door.transform.Translate(0, 0, -0.05f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isActivate = true;
        SoundManager.instance.PlaySound(SoundManager.Effect.Customer);
    }
    private void OnTriggerExit(Collider other)
    {
        isActivate = false;
    }
}
