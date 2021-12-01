using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wibble : MonoBehaviour
{
    float T;
    const float tau = Mathf.PI * 2;

    float rate;
    float horMagnitude;
    float vertMagnitude;

    private void Start()
    {
        rate = Random.Range(0.5f, 3f);
        horMagnitude = Random.Range(0.05f, 0.2f);
        vertMagnitude = Random.Range(0.05f, 0.25f);
    }

    void Update()
    {
        T += Time.deltaTime * tau * rate;
        if (T < tau) T -= tau;

        float horScale = Mathf.Sin(T);
        horScale *= horMagnitude;

        float vertScale = Mathf.Cos(T);
        vertScale *= vertMagnitude;

        float x, y, z;
        x = 1 + horScale;
        y = 1 + vertScale;
        z = x;

        transform.localScale = new Vector3(x, y, z);
    }
}
