public struct Tip 
{
    public TipType _type;
    public string _name;
    public UnitType _unitType;
    public int _team;
    public int _cost;
    public int _vetLevel;
    public int _atk;
    public float _cooldown;
    public int _range;
    public int _maxHp;
    public int _hp;
    public float _speed;
    public int _reclaim;
    public VetStyle _vetStyle;
    public string _additional;

    public Tip(TipType type, string name, UnitType unitType, int team, int cost, int vetLevel, int atk, float cooldown, int range, int maxHp, int hp, float speed, int reclaim, VetStyle vetStyle, string additional)
    {
        _type = type;
        _name = name;
        _unitType = unitType;
        _team = team;
        _cost = cost;
        _vetLevel = vetLevel;
        _atk = atk;
        _cooldown = cooldown;
        _range = range;
        _maxHp = maxHp;
        _hp = hp;
        _speed = speed;
        _reclaim = reclaim;
        _vetStyle = vetStyle;
        _additional = additional;
    }
}

public enum TipType
{
    UI,
    GameObject
}
