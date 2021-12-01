using UnityEngine;
using TMPro;

public class DamagedVal : MonoBehaviour
{
    public GameObject canvas;
    public TMP_Text val;
    const float visibleTime = 0.5f;
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

    public void Show(int damage)
    {
        val.text = "-" + damage.ToString();
        canvas.SetActive(true);
        visibleTimer = visibleTime;
    }
}
