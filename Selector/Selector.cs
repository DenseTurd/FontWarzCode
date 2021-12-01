using UnityEngine;

public class Selector : MonoBehaviour
{
    #region Instance
    public static Selector Instance { get; private set; }

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

    public static Unit selected;

    public void InstanceSelect(Unit unit)
    {
        InstanceDeSelect();
        selected = unit;
        selected.selectionBox.SetActive(true);
        SetLiberationPanel(unit);
    }

    void SetLiberationPanel(Unit unit)
    {
        Liberation.Instance.ShowPanel(unit);
        //Debug.Log($"{unit.Name} info");
    }

    public void InstanceDeSelect()
    {
        //Debug.Log("DeSelect");
        if (selected) selected.selectionBox.SetActive(false);
        selected = null;
        Liberation.Instance.HidePanel();
    }

    public static void Select(Unit unit)
    {
        Instance.InstanceSelect(unit);
        Sound.Guy.Select();
    }

    public static void DeSelect()
    {
        Instance.InstanceDeSelect();
    }
}
