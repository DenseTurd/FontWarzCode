using UnityEngine;
using UnityEngine.UI;

public class ScreenSpaceHealthBar : MonoBehaviour
{
    public Image bar;
    float width;
    void Start()
    {
        bar = GetComponent<Image>();
        width = transform.localScale.x;
    }

    public void UpdateBar(float percentHealth)
    {
        transform.localScale = new Vector3(Mathf.Max(width * percentHealth, 0.1f), 1, 1);
        bar.color = Color.Lerp(Color.red, Color.green, percentHealth);
    }
}
