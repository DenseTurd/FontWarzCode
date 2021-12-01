using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public SpriteRenderer bar;
    float width;
    void Start()
    {
        bar = GetComponent<SpriteRenderer>();
        width = transform.localScale.x;
    }

    public void UpdateBar(float percentHealth)
    {
        transform.localScale = new Vector3(Mathf.Max(width * percentHealth, 0.1f), 1, 1);
        bar.color = Color.Lerp(Color.red, Color.green, percentHealth);
    }
}
