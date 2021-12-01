using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fonts : MonoBehaviour
{
    public TMP_FontAsset liberation;
    public TMP_FontAsset comic;
    Dictionary<int, TMP_FontAsset> fontDict;
    public static Fonts Instance { get; private set; }

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

        fontDict = new Dictionary<int, TMP_FontAsset> { { 0, liberation }, { 1, comic } };
    }

    public static TMP_FontAsset Get(int team)
    {
        return Instance.InstanceGet(team);
    }

    TMP_FontAsset InstanceGet(int team)
    {
        return fontDict[team];
    }
}
