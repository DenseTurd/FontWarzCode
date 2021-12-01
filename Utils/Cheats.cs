using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    bool cheats = false;

    void Update()
    {
        if (!cheats) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Overseer.Instance.Earn(0, 100);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Overseer.Instance.Earn(1, 100);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Liberation.Instance.timer0 = 0;
            Liberation.Instance.timer1 = 0;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
