using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public float maxGauge = 100f;
    float currentGauge;
    float currentPos;
    // Start is called before the first frame update
    void Start()
    {
        currentGauge = 0f;
        currentPos = -0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime);
    }
}
