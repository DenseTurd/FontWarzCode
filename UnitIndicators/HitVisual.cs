using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVisual : MonoBehaviour
{
    public float showTime = 0.5f;
    float showTimer;
    public GameObject visual;
    Vector3 visualOriginalPos;

    void Start()
    {
        visualOriginalPos = visual.transform.localPosition;
    }
    public void Go()
    {
        visual.SetActive(true);
        showTimer = showTime;
        RandomPos();
    }

    void RandomPos()
    {
        visual.transform.localPosition = new Vector3(visualOriginalPos.x + Random.Range(-0.5f, 0.5f), 
                                                visualOriginalPos.y + Random.Range(-1f, 0.5f), 
                                                visualOriginalPos.z);
    }

    void Update()
    {
        if (showTimer > 0)
        {
            showTimer -= Time.deltaTime;
            if (showTimer <= 0)
            {
                visual.SetActive(false);
                visual.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 4) * 90));
            }
        }
    }
}
