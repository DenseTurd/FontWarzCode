using UnityEngine;

public class Reclaim : MonoBehaviour
{
    public int val;

    private void Start()
    {
        float scale = 1 + (0.5f * val);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
