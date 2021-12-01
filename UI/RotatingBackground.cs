using UnityEngine;

public class RotatingBackground : MonoBehaviour
{
    float T;
    public float speed = 30;

    void Update()
    {
        T += Time.deltaTime * speed;

        if (T > 360) T -= 360;

        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, T);
        
    }
}
