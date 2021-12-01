using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ToolTip : MonoBehaviour
{
    #region Instance
    public static ToolTip Instance { get; private set; }
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

    public static bool visible;

    public GameObject panel;
    public TMP_Text text;
    public RectTransform tipRect;

    Dictionary<int, string> teamDict = new Dictionary<int, string> { { 0, "The Liberation" }, { 1, "Comic Revolt" } };
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    void FormatTip(Tip tip)
    {
        if (tip._type == TipType.UI) UIFormat(tip);
        if (tip._type == TipType.GameObject) GameObjectFormat(tip);
        ResizeRect();
    }

    void ResizeRect()
    {
        tipRect.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight);
    }

    void UIFormat(Tip tip)
    {
        text.font = Fonts.Get(tip._team);

        text.text = tip._name + "\n" +
        "Type: " + tip._unitType.ToString() + "\n" +
        "Team: " + teamDict[tip._team] + "\n" +
        "Cost: " + tip._cost + "\n" +
        "Attack: " + tip._atk + "\n" +
        "Cooldown: " + tip._cooldown + "\n" +
        "Range: " + tip._range + "\n" +
        "Hp: " + tip._hp + "\n" +
        "Speed: " + tip._speed + "\n" +
        "Reclaim: " + tip._reclaim + "\n" +
        "Vet style: " + tip._vetStyle.ToString() + "\n" +
        tip._additional;
    }


    void GameObjectFormat(Tip tip)
    {
        text.font = Fonts.Get(tip._team);

        text.text = tip._name + "\n" +
        "Type: " + tip._unitType.ToString() + "\n" +
        "Team: " + teamDict[tip._team] + "\n" +
        "Vet level: " + tip._vetLevel + "\n" +
        "Attack: " + tip._atk + "\n" +
        "Cooldown: " + tip._cooldown + "\n" +
        "Range: " + tip._range + "\n" +
        "Hp: " + tip._hp + "/" + tip._maxHp + "\n" +
        "Speed: " + tip._speed + "\n" +
        "Reclaim: " + tip._reclaim + "\n" +
        "Vet style: " + tip._vetStyle.ToString() + "\n" +
        tip._additional;
    }

    public static void Show(Tip tip)
    {
        Instance.ShowMe(tip);
    }

    public void ShowMe(Tip tip)
    {
        visible = true;
        FormatTip(tip);
        panel.SetActive(true);
    }

    public static void Hide()
    {
        visible = false;
        Instance.HideMe();
    }

    public void HideMe()
    {
        panel.SetActive(false);
    }
}
