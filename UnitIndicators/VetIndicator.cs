using UnityEngine;

public class VetIndicator : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 2f, transform.position.z);

        if (transform.position.y > 4)
        {
            Destroy(gameObject);
        }
    }
}
