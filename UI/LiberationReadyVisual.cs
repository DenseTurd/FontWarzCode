using UnityEngine;
using TMPro;

public class LiberationReadyVisual : MonoBehaviour
{
    public TMP_Text text;
    const float timerTime = 0.5f;
    float timer = timerTime;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            text.enabled = !text.enabled;
            timer = timerTime;
        }
    }
}
