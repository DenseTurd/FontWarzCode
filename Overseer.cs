using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overseer : MonoBehaviour
{
    #region Instance
    public static Overseer Instance { get; private set; }

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

    public Transform spawn0;
    public Transform spawn1;

    public Transform depo0;
    public Transform depo1;

    public Transform resources0;
    public Transform resources1;

    public Transform turretSpawn0;
    public Transform turretSpawn1;
    public Dictionary<int, Transform> turretSpawnDict;

    public Unit com0;
    public Unit com1;
    public Item com0Item;
    public Item com1Item;
    public Transform comLoc0;
    public Transform comLoc1;

    public Dictionary<int, Transform> spawnDict;

    public UI ui;
    public ScreenSpaceHealthBar com0Health;
    public ScreenSpaceHealthBar com1Health;

    public Dictionary<int, Team> teamDict;

    public Unit worker0;
    public Unit worker1;
    public Item worker0Item;
    public Item worker1Item;

    public List<AIPlayer> aIPlayers;
    [Range(0, 2)]
    public int ais;

    int difficulty = 2;

    public void SetAIs(Slider slider)
    {
        ais = (int)slider.value;
    }

    public void SetDifficulty(Slider slider)
    {
        difficulty = (int)slider.value;
    }

    public void Init()
    {
        StaggerChecks.Instance.Init();

        Unit c0 = Instantiate(com0, comLoc0);
        Unit c1 = Instantiate(com1, comLoc1);

        teamDict = new Dictionary<int, Team>
        {
            { 0, new Team(spawn0, depo0, resources0, c0, comLoc0) },
            { 1, new Team(spawn1, depo1, resources1, c1, comLoc1) } 
        };
        turretSpawnDict = new Dictionary<int, Transform>
        {
            { 0, turretSpawn0 },
            { 1, turretSpawn1 }
        };

        SetupComs();
        SpawnWorkers();

        if (ais > 0)
        {
            aIPlayers[1].Init(difficulty);
            teamDict[1].isAi = true;

            if (ais > 1) 
            { 
                aIPlayers[0].Init(difficulty);
                teamDict[0].isAi = true;
            }
        }

        ui.gameObject.SetActive(true);
        ui.Init();
        Liberation.Instance.Init();
        KeyboardThings.Instance.Init();
    }

    void SetupComs()
    {
        SetupCom(0);
        SetupCom(1);
    }

    void SetupCom(int team)
    {
        Team comTeam = teamDict[team];
        Unit comm = comTeam._com;

        ItemControl ic = new ItemControl();
        ic.item = team == 0 ? com0Item : com1Item;

        comm.Init(ic);
    }

    void SpawnWorkers()
    {
        SpawnWorker(worker0, 0);
        SpawnWorker(worker0, 0);
        SpawnWorker(worker1, 1);
        SpawnWorker(worker1, 1);
    }

    void SpawnWorker(Unit wrkr, int team)
    {
        var unit = Instantiate(wrkr, Instance.teamDict[team]._spawn);

        ItemControl ic = new ItemControl();
        ic.item = team == 0 ? worker0Item : worker1Item;

        unit.Init(ic);
        unit.transform.localPosition = new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(-0.5f, 0.5f));
    }

    public void Earn(int team, int amount)
    {
        teamDict[team].monies += amount;
        ui.SetMonies();
        Sound.Guy.Earn(team);
    }

    public void Spend(int team, int amount)
    {
        teamDict[team].monies -= amount;
        ui.SetMonies();
    }
}
