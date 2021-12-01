using UnityEngine;
using UnityEngine.UI;

public class WinLoseCanvas : MonoBehaviour
{
    #region Instance
    public static WinLoseCanvas Instance { get; set; }
    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    #endregion

    public Button again;
    public GameObject txtTheLiberation;
    public GameObject txtComicRevolt;

    bool initialised;
    const float timerTime = 3f;
    float timer;
    public void Init(int losingTeam)
    {
        initialised = true;
        timer = timerTime;
        if (losingTeam == 0) txtComicRevolt.SetActive(true);
        if (losingTeam == 1) txtTheLiberation.SetActive(true);
    }

    void Update()
    {
        if (!initialised) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
            again.gameObject.SetActive(true);
    }
}
