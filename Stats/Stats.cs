using UnityEngine;

[CreateAssetMenu(menuName = "Stats")]
public class Stats : ScriptableObject
{
    public UnitType unitType;
    public int cost;
    public int reclaim;
    public int atk;
    public int range;
    public float cooldown;
    public int hp;
    public float speed;
    public VetStyle vetStyle;
    [TextArea(10, 15)] public string liberationInfo;
    public LiberationStats liberationStats;
}
