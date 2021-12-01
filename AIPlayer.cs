using System;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public List<Item> items;
    List<ItemControl> ics;

    public AIState state;

    public bool initialised;
    bool spawnRanged;
    int randomTroopSpawnIndex;

    public float stateChangeTime = 5f;
    float stateChangeTimer;

    public int myTeam = 1;
    public string teamStr;

    float clickCooldown;

    public bool tellMeAboutIt;

    const float comDamageCheatCooldown = 1f;
    float comDamageCheatTimer;

    bool accruing;

    const int worker = 0;
    const int melee = 1;
    const int ranged = 2;
    const int healer = 3;
    const int tank = 4;
    const int speedy = 5;
    const int turret = 6;
    bool canTurret = true;

    Dictionary<int, string> unitDict;

    Unit selectedUnit;

    int difficulty;
    public void Init(int diff)
    {
        difficulty = diff;
        if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Difficulty set: {difficulty}\"");
        unitDict = new Dictionary<int, string> 
        {
            { worker, "Worker" }, 
            { melee, "Melee" }, 
            { ranged, "Ranged" }, 
            { healer, "Healer" }, 
            { tank, "Tank" }, 
            { speedy, "Speedy" },
            { turret, "Turret" }
        };
        ics = new List<ItemControl>();
        foreach (var it in items)
        {
            ItemControl ic = new ItemControl();
            ic.item = it;
            ics.Add(ic);
        }

        stateChangeTimer = stateChangeTime;

        RandomTroopSpawnIndex();

        teamStr = myTeam == 0 ? "TheLiberation" : "ComicRevolt";

        initialised = true;
    }

    void Act(SitRep sitRep)
    {
        Cheat(sitRep);

        if (sitRep.enemyArmyVal == 0 && sitRep.enemyWorkers > 2)
        {
            if (UnityEngine.Random.Range(0, 3) == 0)
            {
                if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"You wanna eco? I'll show you eco!\"");
                state = AIState.Eco;
                return;
            }
            else
            {
                if (Overseer.Instance.teamDict[myTeam].monies >= ics[speedy].item.stats.cost)
                {
                    Spawn(speedy);
                }
            }
        }


        if (sitRep.myWorkers > 5)
        {
            if (sitRep.enemyArmyVal < Overseer.Instance.teamDict[myTeam].monies)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"I'm switching to agressive to spawn a larger army than you! BITCH!\"");
                    state = AIState.Aggressive;
                    return;
                }

            }

            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"I'm accruing monies\"");
                state = AIState.AccrueMoines;
                return;
            }
        }

        if (sitRep.myWorkers > 8)
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"I'm accruing monies\"");
                state = AIState.AccrueMoines;
                return;
            }
            else
            {
                if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"I got loads of workers, Time to get agressive!\"");
                state = AIState.Aggressive;
                return;
            }
        }

        if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"I don't know what to do, gonna do some thing random...\"");
        RandomState();
    }

    public void UnitTakingDamage(Unit u)
    {
        if (!initialised) return;
        if (difficulty == 0) return;

        if (u.Team == myTeam)
        {
            if (u is Worker)
            {
                if (state == AIState.Defensive)
                    return;

                if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"My workers are taking damage! Switching to defense mode.\"");
                state = AIState.Defensive;
                ResetTimer();
            }
            if (u is Com)
            {

                if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"My com is taking damage\"");
                state = AIState.Defensive;
                ResetTimer();

                if (comDamageCheatTimer <= 0)
                {
                    if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Cheating\"");
                    Overseer.Instance.Earn(myTeam, 1);
                    SitRep sitRep = SitRep();
                    Cheat(sitRep);
                    comDamageCheatTimer = comDamageCheatCooldown;
                }
            }
        }
    }

    void RandomState()
    {
        int i = UnityEngine.Random.Range(0, 3);
        if (i == 0)
        {
            RandomTroopSpawnIndex();
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Engaging Agression!\"");
            state = AIState.Aggressive;
        }
        else if (i == 1)
        {
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Eco fest\"");
            state = AIState.Eco;
        }
        else
        {
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Break time...\"");

            state = AIState.None;
        }
    }

    void Cheat(SitRep sitRep)
    {

        if (sitRep.myWorkers == 0)
        {
            Overseer.Instance.Earn(myTeam, 5);
            InstaClick();
            Spawn(worker);
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"I got no workers. I get a free one\"");
        }

        if (difficulty != 2) return;

        if (sitRep.enemyWorkers > sitRep.myWorkers * 2)
        {
            Overseer.Instance.Earn(myTeam, 5);
            InstaClick();
            Spawn(worker);
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"You got loads of workers. I get a free one\"");
        }

        if (sitRep.myArmyVal == 0 && sitRep.enemyArmyVal > 4)
        {
            Overseer.Instance.Earn(myTeam, 4);
            InstaClick();
            Spawn(melee);
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"I got no army. I get a free melee\"");
            return;
        }

        if (sitRep.myArmyVal < 12 && sitRep.enemyArmyVal > 16)
        {
            Overseer.Instance.Earn(myTeam, 7);
            InstaClick();
            Spawn(ranged);
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"You got a larger army. I get a free ranged\"");
            return;
        }

        if (sitRep.myArmyVal < 17 && sitRep.enemyArmyVal > 28)
        {
            Overseer.Instance.Earn(myTeam, 4);
            InstaClick();
            Spawn(ranged);
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"You still got a larger army. I get a free ranged\"");
            return;
        }
    }

    void Aggressive()
    {
        SitRep sitRep = SitRep();
        if (sitRep.myArmyVal == 0)
        {
            CrackSquad();
        }

        AntiTurret(sitRep);
        FullHouse();
        TankAndHealer();
        CrackSquad();

        if (Overseer.Instance.teamDict[myTeam].monies >= ics[randomTroopSpawnIndex].item.stats.cost)
        {
            if (sitRep.enemyTurretActive) return;
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Random troop: {unitDict[randomTroopSpawnIndex]}\"");
            InstaClick();
            Spawn(randomTroopSpawnIndex);
            RandomTroopSpawnIndex();
        }
    }

    void AntiTurret(SitRep sitRep)
    {
        if (difficulty == 0) return;
        if (sitRep.enemyTurretActive)
        {
            if (Overseer.Instance.teamDict[myTeam].monies >=
            ics[ranged].item.stats.cost +
            ics[ranged].item.stats.cost +
            ics[melee].item.stats.cost +
            ics[healer].item.stats.cost)
            {
                if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Anti turret squad\"");
                InstaClick();
                Spawn(ranged);
                InstaClick();
                Spawn(ranged);
                InstaClick();
                Spawn(melee);
                InstaClick();
                Spawn(healer);
            }
        }
    }

    void CrackSquad()
    {
        if (Overseer.Instance.teamDict[myTeam].monies >= ics[ranged].item.stats.cost + ics[melee].item.stats.cost)
        {
            InstaClick();
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Crack squad!\"");
            Spawn(ranged);
            InstaClick();
            Spawn(melee);
        }
    }

    void FullHouse()
    {
        if (difficulty == 0) return;
        if (Overseer.Instance.teamDict[myTeam].monies >= 
            ics[ranged].item.stats.cost + 
            ics[melee].item.stats.cost + 
            ics[tank].item.stats.cost + 
            ics[healer].item.stats.cost)
        {
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Full house\"");
            InstaClick();
            Spawn(ranged);
            InstaClick();
            Spawn(melee);
            InstaClick();
            Spawn(tank);
            InstaClick();
            Spawn(healer);
        }
    }    

    void TankAndHealer()
    {
        if (difficulty == 0) return;
        if (Overseer.Instance.teamDict[myTeam].monies >= ics[tank].item.stats.cost + ics[healer].item.stats.cost)
        {

            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"Tank and healer\"");
            InstaClick();
            Spawn(tank);
            InstaClick();
            Spawn(healer);
        }
    }

    void Defensive()
    {
        SitRep sitRep = SitRep();
        if (sitRep.enemyArmyVal > 10 && sitRep.enemyMeleeAttackers > 1)
        {
            if (canTurret)
            {
                InstaClick();
                Spawn(turret);
                canTurret = false;
            }
        }

        if (sitRep.myArmyVal > sitRep.myHealersVal)
        {
            if (Overseer.Instance.teamDict[myTeam].monies >= ics[healer].item.stats.cost)
            {
                Spawn(healer);
            }
            return;
        }

        if (Overseer.Instance.teamDict[myTeam].monies >= ics[tank].item.stats.cost)
            Spawn(tank);

        if (Overseer.Instance.teamDict[myTeam].monies >= ics[melee].item.stats.cost)
            Spawn(melee);

        if (Overseer.Instance.teamDict[myTeam].monies >= ics[ranged].item.stats.cost)
            Spawn(ranged);
    }

    void Eco()
    {
        if (Overseer.Instance.teamDict[myTeam].monies >= ics[worker].item.stats.cost)
        {
            Spawn(worker);
        }
    }

    void AccrueMonies()
    {
        if (!accruing)
        {
            stateChangeTimer = stateChangeTime * 2f;
            accruing = true;
        }
    }

    void Update()
    {
        if (!initialised)
            return;

        switch (state)
        {
            case AIState.None:
                RandomState();
                break;

            case AIState.Eco:
                Eco();
                break;

            case AIState.Defensive:
                Defensive();
                break;

            case AIState.Aggressive:
                Aggressive();
                break;

            case AIState.AccrueMoines:
                AccrueMonies();
                break;

            default:
                break;
        }

        stateChangeTimer -= Time.deltaTime;
        if (stateChangeTimer <= 0)
        {
            SitRep sitRep = SitRep();
            Act(sitRep);
            ResetTimer();
            Useliberation(sitRep);
        }

        CloseLiberationPanel();

        comDamageCheatTimer -= Time.deltaTime;
    }

    float libPanelCloseTime;
    void CloseLiberationPanel()
    {
        libPanelCloseTime -= Time.deltaTime;
        if (libPanelCloseTime <= 0)
        {
            libPanelCloseTime = UnityEngine.Random.Range(0.9f, 2.3f);

            if (selectedUnit != null)
                if (Selector.selected == selectedUnit) 
                    Selector.DeSelect();

            Liberation.Instance.HidePanel(myTeam);
        }
    }

    void Useliberation(SitRep sitRep)
    {
        List<Unit> eligibleUnits = new List<Unit>();
        foreach (var u in StaggerChecks.Instance.units)
        {
            if (u.Team == myTeam)
            {
                if (u is Com) continue;

                if (u is Worker) 
                {
                    if (sitRep.myArmyVal > 14) continue; // Don't choose workers if we have a decent army

                    if (sitRep.myWorkers > 8) continue; // Don't choose workers if we have a bunch

                    if (UnityEngine.Random.Range(0, 4) > 0) continue; // only add a quarter of the workers, favor the action!
                }

                eligibleUnits.Add(u);
            }
        }

        if (eligibleUnits.Count == 0) return;

        int index = UnityEngine.Random.Range(0, eligibleUnits.Count - 1);
        selectedUnit = eligibleUnits[UnityEngine.Random.Range(0, eligibleUnits.Count - 1)];
        if (Selector.selected == null)
        {
            Selector.Select(selectedUnit);
            Liberation.Instance.Liberate();
        }
        else
        {
            Liberation.Instance.Liberate(selectedUnit);
        }
        //Liberation.Instance.ShowPanel(eligibleUnits[UnityEngine.Random.Range(0, eligibleUnits.Count - 1)]);
    }

    void Spawn(int index)
    {
        if (ClickReady())
        {
            if (tellMeAboutIt) Debug.Log($"{teamStr}AI: \"{unitDict[index]} spawn\"");
            ics[index].OnClick();
            accruing = false;
        }
    }

    bool ClickReady()
    {
        clickCooldown -= Time.deltaTime;
        if (clickCooldown <= 0)
        {
            clickCooldown = UnityEngine.Random.Range(0.2f, 0.9f);
            return true;
        }
        else
        {
            return false;
        }
    }

    void InstaClick()
    {
        clickCooldown = 0;
    }

    void ResetTimer()
    {
        stateChangeTimer = stateChangeTime;
    }

    SitRep SitRep()
    {
        SitRep outRep = new SitRep();

        foreach (Unit u in StaggerChecks.Instance.units)
        {
            if (u.Team == myTeam)
            {
                if (u is Troop)
                    outRep.myArmyVal += u.Cost;

                if (u is Worker)
                    outRep.myWorkers++;

                if (u is Healer)
                    outRep.myHealersVal += u.Cost;

                if (u is Turret)
                    outRep.myTurretActive = true;
            }
            else
            {
                if (u is Troop)
                    outRep.enemyArmyVal += u.Cost;

                if (u is Worker)
                    outRep.enemyWorkers++;

                if (u.Type == UnitType.Melee || u.Type == UnitType.Speedy || u.Type == UnitType.Tank)
                    outRep.enemyMeleeAttackers++;

                if (u is Turret)
                    outRep.enemyTurretActive = true;
            }
        }
        return outRep;
    }

    void RandomTroopSpawnIndex()
    {
        randomTroopSpawnIndex = UnityEngine.Random.Range(1, 6);
    }
}

public enum AIState
{
    None,
    Eco,
    Defensive,
    Aggressive,
    AccrueMoines
}

public struct SitRep
{
    public int myWorkers;
    public int myArmyVal;
    public int myHealersVal;
    public bool myTurretActive;
    public int enemyArmyVal;
    public int enemyWorkers;
    public int enemyMeleeAttackers;
    public bool enemyTurretActive;
}
