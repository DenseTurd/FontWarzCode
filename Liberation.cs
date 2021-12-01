using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Liberation : MonoBehaviour
{
    #region Instance
    public static Liberation Instance { get; private set; }

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

    Unit unit;

    public GameObject panel0;
    public Image icon0;
    public Button button0;
    public TMP_Text text0;
    public RectTransform cooldownVisual0;
    public GameObject readyVisual0;
    public GameObject liberatedSash0;

    public GameObject panel1;
    public Image icon1;
    public Button button1;
    public TMP_Text text1;
    public RectTransform cooldownVisual1;
    public GameObject readyVisual1;
    public GameObject liberatedSash1;

    Dictionary<int, GameObject> panelDict;
    Dictionary<int, Image> iconDict;
    Dictionary<int, TMP_Text> textDict;
    Dictionary<int, GameObject> sashDict;

    const float cooldown = 90;
    [HideInInspector] public float timer0;
    [HideInInspector] public float timer1;

    bool initialised;

    public void Init()
    {
        panelDict = new Dictionary<int, GameObject> { { 0, panel0 }, { 1, panel1 } };
        iconDict = new Dictionary<int, Image> { { 0, icon0 }, { 1, icon1 } };
        textDict = new Dictionary<int, TMP_Text> { { 0, text0 }, { 1, text1 } };
        sashDict = new Dictionary<int, GameObject> { { 0, liberatedSash0 }, { 1, liberatedSash1 } };

        Button.ButtonClickedEvent clickedEvent = new Button.ButtonClickedEvent();
        clickedEvent.AddListener(Liberate);
        button0.onClick = clickedEvent;
        button1.onClick = clickedEvent;

        timer0 = cooldown;
        timer1 = cooldown;

        initialised = true;
    }

    void Update()
    {
        if (!initialised) return;

        timer0 -= Time.deltaTime;
        timer1 -= Time.deltaTime;
        if (timer0 < 0)
        {
            readyVisual0.SetActive(true);
            timer0 = 0;
        }
        if (timer1 < 0)
        {
            readyVisual1.SetActive(true);
            timer1 = 0;
        }

        cooldownVisual0.localScale = new Vector3(cooldownVisual0.localScale.x, timer0 / cooldown, cooldownVisual0.localScale.z);
        cooldownVisual1.localScale = new Vector3(cooldownVisual1.localScale.x, timer1 / cooldown, cooldownVisual1.localScale.z);
    }

    public void ShowPanel(Unit u)
    {
        unit = u;
        iconDict[u.Team].sprite = u.Icon;
        sashDict[u.Team].SetActive(u.liberated);
        textDict[u.Team].text = u.LiberationInfo;
        textDict[u.Team].font = Fonts.Get(u.Team);
        panelDict[u.Team].SetActive(true);
        Debug.Log(unit.Name);
    }

    public void HidePanel()
    {
        panel0.SetActive(false);
        panel1.SetActive(false);
    }

    public void HidePanel(int team)
    {
        if (team == 0) panel0.SetActive(false);
        if (team == 1) panel1.SetActive(false);
    }

    public void Liberate()
    {
        //Debug.Log("Starting liberate");
        if (!Ready(unit.Team)) return;
        //Debug.Log("Passed ready test");
        if (unit)
        {
            //Debug.Log("got a unit");
            if (unit.liberated)
            {
                Debug.Log($"{unit.Name} already liberated");
                return;
            }
            unit.Liberate();
            sashDict[unit.Team].SetActive(unit.liberated);
            Sound.Guy.Liberate();
        }
        unit = null;
    }

    public void Liberate(Unit u)
    {
        Unit unitToRemember = null;
        if (unit != null) unitToRemember = unit; 

        unit = u;
        Liberate();

        if (unitToRemember != null) unit = unitToRemember;
    }

    public bool Ready(int team)
    {
        if (team == 0)
        {
            if (timer0 <= 0)
            {
                readyVisual0.SetActive(false);
                timer0 = cooldown;
                return true;
            }
        }
        if (team == 1)
        {
            if (timer1 <= 0)
            {
                readyVisual1.SetActive(false);
                timer1 = cooldown;
                return true;
            }
        }
        return false;
    }

    public float GetCharge(int team)
    {
        return 1 - ((team == 0 ? timer0 : timer1) / cooldown);
    }
}
