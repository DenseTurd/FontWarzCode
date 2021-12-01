using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggerChecks : MonoBehaviour
{
    #region Instance
    public static StaggerChecks Instance { get; private set; }

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

    public List<Unit> units;

    int index;

    bool initialised;

    public void Init()
    {
        units = new List<Unit>();
        initialised = true;
    }

    public void AddUnit(Unit u)
    {
        units.Add(u);
    }

    public void RemoveUnit(Unit u)
    {
        units.Remove(u);
    }

    void Update()
    {
        if (initialised)
            Checks();
    }

    void Checks()
    {
        if (units.Count < 1)
        {
            Debug.Log("no units for checks");
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            if (index > units.Count -1)
            {
                index = 0;
            }
            units[index].CheckForThings();
            index++;
        }



        if (index > units.Count -1)
        {
            index = 0;
        }
    }
}
