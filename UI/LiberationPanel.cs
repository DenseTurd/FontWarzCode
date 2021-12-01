using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiberationPanel : MonoBehaviour
{
    #region Instance
    public static LiberationPanel Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    
}
