using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVisual : MonoBehaviour
{
    public float showTime = 0.5f;
    float showTimer;
    public GameObject visual;
    public void Go()
    {
        visual.SetActive(true);
        showTimer = showTime;
    }

    void Update()
    {
        if (showTimer > 0)
        {
            showTimer -= Time.deltaTime;
            if (showTimer <= 0)
            {
                visual.SetActive(false);
            }
        }
    }
}
