using UnityEngine;

public class Jumple : MonoBehaviour
{
    float T;
    float tau = Mathf.PI * 2;
    float rate;
    void Start()
    {
        rate = Random.Range(1.8f, 3.3f);
    }

    void Update()
    {
        T += Time.deltaTime * tau * rate;

        float vertOffset = Mathf.Sin(T);
        vertOffset /= 3;
        vertOffset = vertOffset < 0 ? 0 : vertOffset;

        transform.position = new Vector3(transform.position.x, vertOffset, transform.position.z);
    }
}
