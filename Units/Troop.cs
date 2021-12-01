using UnityEngine;
using UnityEngine.AI;
public class Troop : Unit
{
    public TroopState state;
    public NavMeshAgent agent;

    public Vector3 destination;
    public override void Init(ItemControl ic)
    {
        base.Init(ic);
        state = TroopState.Move;
        agent.speed = Speed;
    }

    public override void Update()
    {
        switch (state)
        {
            case TroopState.Move:
                Move();
                break;

            case TroopState.Attacking:
                Attacking();
                break;
        }

        if (target != null)
        {
            state = TroopState.Attacking;
        }
        else
        {
            if (state == TroopState.Attacking)
            {
                state = TroopState.Move;
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
}

public enum TroopState
{
    Move,
    Attacking
}
