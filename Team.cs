using UnityEngine;
public class Team 
{
    public bool isAi;
    public int monies;
    public int kills;
    public Transform _spawn;
    public Transform _depo;
    public Transform _resources;
    public Unit _com;
    public Transform _comLoc;
    public bool turreted;

    public Team(Transform spawn, Transform depo, Transform resources, Unit comander, Transform comLoc)
    {
        monies = 10;
        _spawn = spawn;
        _depo = depo;
        _resources = resources;
        _comLoc = comLoc;
        _com = comander;
    }
}
