using UnityEngine;
using TMPro;

public class HealedVal : MonoBehaviour
{
    public GameObject canvas;
    public TMP_Text val;
    const float visibleTime = 0.4f;
    float visibleTimer;


    void Update()
    {
        if (visibleTimer > 0)
        {
            visibleTimer -= Time.deltaTime;
            if (visibleTimer <= 0)
            {
                canvas.SetActive(false);
            }
        }
    }

    public void Show(int healing)
    {
        if (this == null) return;
        val.text = "+" + healing.ToString();
        canvas.SetActive(true);
        visibleTimer = visibleTime;
    }
}
