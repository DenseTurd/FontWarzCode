using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemControl : MonoBehaviour
{
    public Image icon;
    public TMP_Text key;
    public TMP_Text costTxt;
    public Item item;
    Vector3 checkPos;
    public Button button;

    public void Init(Item itm)
    {
        item = itm;
        icon.sprite = item.icon;
        costTxt.font = Fonts.Get(item.team);
        costTxt.text = item.stats.cost.ToString();
        key.font = Fonts.Get(item.team);
        SortKeyText();
        button = GetComponent<Button>();
    }

    private void SortKeyText()
    {
        string str = item.key.ToString();
        if (item.key == KeyCode.BackQuote) str = "'";
        if (item.key == KeyCode.Quote) str = "#";
        if (item.key == KeyCode.Semicolon) str = ";";
        key.text = "(" + str + ")";
    }

    public void OnClick()
    {
        if (item.unit is Turret)
        {
            if (Overseer.Instance.teamDict[item.team].turreted) return;

            Spawn(Overseer.Instance.turretSpawnDict[item.team].position);
            Overseer.Instance.Spend(item.team, item.stats.cost);
            ToolTip.Hide();
            if (button) button.interactable = false;
            Sound.Guy.Spawn(item.team);
            return;
        }

        if (Overseer.Instance.teamDict[item.team].monies >= item.stats.cost)
        {
            Spawn(FindClearSpawn(item.team));
            Overseer.Instance.Spend(item.team, item.stats.cost);
            Sound.Guy.Spawn(item.team);
        }
    }

    void Spawn(Vector3 spawnPos)
    {
        var unit = Instantiate(item.unit, spawnPos, Quaternion.Euler(Vector3.zero), Overseer.Instance.teamDict[item.team]._spawn);
        unit.Init(this);
    }

    public void MouseEnter()
    {
        Tip tip = new Tip(
            TipType.UI, 
            item.name, 
            item.stats.unitType, 
            item.team, 
            item.stats.cost, 
            0, 
            item.stats.atk, 
            item.stats.cooldown, 
            item.stats.range, 
            item.stats.hp, 
            item.stats.hp, 
            item.stats.speed,
            item.stats.reclaim, 
            item.stats.vetStyle, 
            item.additional);
        ToolTip.Show(tip);    
    }

    public void MouseExit()
    {
        ToolTip.Hide();    
    }

    Vector3 FindClearSpawn(int team)
    {
        bool clear = true;
        Vector3 randomPos = Vector3.zero;
        for (int i = 0; i < 5; i++)
        {
            float area = 1 + (0.25f * i);
            randomPos = Overseer.Instance.teamDict[team]._spawn.position + new Vector3(Random.Range(-area, area), 0, Random.Range(-area * 3, area));
            checkPos = randomPos;
            Collider[] potentialColliders = Physics.OverlapSphere(randomPos, 0.5f);
            foreach (var collider in potentialColliders)
            {
                Unit u = collider.GetComponent<Unit>();
                if (u)
                {
                    clear = false;
                    break;
                }
            }
            if (clear)
            {
                //Debug.Log("found clear spawn");
                return randomPos;
            }
            clear = true;
        }
        //Debug.Log("Couldn't find a clear position to spawn unit, hoping for the best :D");
        return randomPos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(checkPos, 0.5f);
    }
#endif
}
