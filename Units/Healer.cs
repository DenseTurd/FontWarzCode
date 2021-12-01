using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

public class Healer : Unit
{
    public HealState state;
    public NavMeshAgent agent;

    public Vector3 destination;

    public List<Unit> healTargets;
    public Unit healTarget;

    public int healVal;
    float castTimer;
    public override void Init(ItemControl ic)
    {
        base.Init(ic);
        healVal = Atk;
        state = HealState.Move;
        agent.speed = Speed;
    }

    public override void CheckForThings()
    {
        if (healTarget != null)
            return;

        healTargets = new List<Unit>();
        Collider[] thingsInRange = Physics.OverlapBox(transform.position, new Vector3(Range, Range, 50));
        foreach (Collider collider in thingsInRange)
        {
            //if (debugs) Debug.Log($"HealCheck found {collider.gameObject.name}{collider.gameObject.GetInstanceID()}");
            Unit unit = collider.GetComponent<Unit>();
            if (unit)
            {
                if (unit is Turret) continue;

                if (unit.Team == Team)
                {
                    if (unit.hp < unit.MaxHp)
                    {
                        healTargets.Add(unit);
                        if (debugs) Debug.Log($"{unit.name}{unit.GetInstanceID()} needs healing");
                    }
                }
            }
        }

        if (healTargets.Count > 0)
        {
            int lowestHp = 10000;
            foreach (Unit friend in healTargets)
            {
                if (friend.hp < lowestHp)
                {
                    healTarget = friend;
                    lowestHp = friend.hp;
                }
            }
            if (debugs) Debug.Log($"{healTarget}{healTarget.gameObject.GetInstanceID()} is heal target");
        }

        base.CheckForThings();
    }

    public override void Update()
    {
        switch (state)
        {
            case HealState.Move:
                Move();
                break;

            case HealState.Attacking:
                Attacking();
                break;

            case HealState.Healing:
                Heal();
                break;
        }

        if (healTarget != null)
        {
            state = HealState.Healing;
        }
        else
        {
            if (target != null)
            {
                state = HealState.Attacking;
            }
            else 
            {
                if (state == HealState.Attacking)
                    state = HealState.Move;
            }
        }
        base.Update();
    }

    public override void Attacking()
    {
        agent.SetDestination(new Vector3(transform.position.x, 0, transform.position.z));
        base.Attacking();
    }

    void Move()
    {
        agent.SetDestination(Overseer.Instance.teamDict[EnemyTeam]._comLoc.position);
    }

    void Heal()
    {
        agent.SetDestination(new Vector3(transform.position.x, 0, transform.position.z));

        castTimer -= Time.deltaTime;
        if (castTimer <= 0)
        {
            if (healTarget != null)
            {
                if (debugs) Debug.Log($"Healed {healTarget.name}{healTarget.gameObject.GetInstanceID()}");
                TryApplyHot();
                healTarget.Heal(Atk * 2);
                castTimer = Cooldown;
                healTarget = null;
                Sound.Guy.Heal();
            }
            state = HealState.Move;
        }
    }

    void TryApplyHot()
    {
        if (liberated)
        {
            healTarget.ApplyHot(Atk);
        }
    }
}

public enum HealState
{
    Move,
    Attacking,
    Healing
}
