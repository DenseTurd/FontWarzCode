using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public KeyCode key;
    public Unit unit;
    public new string name;
    public int team;
    public Stats stats;
    [TextArea(1, 3)] public string additional;
}
