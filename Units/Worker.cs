using UnityEngine;
using UnityEngine.AI;

public class Worker : Unit
{
    const float gatherTime = 0.7f;
    float gatherTimer = gatherTime;

    public WrkrState state;
    public NavMeshAgent agent;

    public Vector3 destination;
    public override void Init(ItemControl ic)
    {
        base.Init(ic);
        state = WrkrState.FindResources;
        agent.speed = Speed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (state == WrkrState.Attacking)
            return;

        if (state == WrkrState.FindResources)
        {
            if (other.GetComponent<Resource>())
            {
                state = WrkrState.Gather;
                agent.SetDestination(transform.position + RandomCircleXZ());
            }
        }

        if (state == WrkrState.ReturnToDepo)
        {
            if (other.GetComponent<Depo>())
            {
                state = WrkrState.Deposit;
                agent.SetDestination(transform.position + RandomCircleXZ());
            }
        }
    }

    public override void Update()
    {
        switch (state)
        {
            case WrkrState.FindResources:
                FindResources();
                break;

            case WrkrState.Gather:
                Gather();
                break;

            case WrkrState.ReturnToDepo:
                ReturnToDepo();
                break;

            case WrkrState.Deposit:
                Deposit();
                break;

            case WrkrState.Attacking:
                Attacking();
                break;
        }

        if (target != null)
        {
            state = WrkrState.Attacking;
        }
        else
        {
            if (state == WrkrState.Attacking)
            {
                state = WrkrState.FindResources;
            }
        }
        base.Update();
    }

    void Deposit()
    {
        gatherTimer -= Time.deltaTime;
        if (gatherTimer <= 0)
        {
            Overseer.Instance.Earn(Team, Atk);
            Reclaim = 0;
            gatherTimer = gatherTime;
            state = WrkrState.FindResources;
        }
    }

    void ReturnToDepo()
    {
        agent.SetDestination(Overseer.Instance.teamDict[Team]._depo.position + RandomCircleXZ());
    }

    void FindResources()
    {
        agent.SetDestination(Overseer.Instance.teamDict[Team]._resources.position + RandomCircleXZ());
    }

    public override void Attacking()
    {
        agent.SetDestination(new Vector3(transform.position.x, 0, transform.position.z));
        base.Attacking();
    }

    void Gather()
    {
        gatherTimer -= Time.deltaTime;
        if (gatherTimer <= 0)
        {
            Reclaim = Atk;
            gatherTimer = gatherTime;
            state = WrkrState.ReturnToDepo;
        }
    }

    public override void Liberate()
    {
        if (liberated)
        {
            Debug.Log($"{Name}{GetInstanceID()} Already liberated");
            return;
        }

        Overseer.Instance.Earn(Team, 10);
        agent.speed += liberationStats.speed;
        base.Liberate();
    }

    Vector3 RandomCircleXZ()
    {
        Vector2 tempVec = Random.insideUnitCircle;
        return new Vector3(tempVec.x, 0, tempVec.y);
    }
}

public enum WrkrState
{
    FindResources,
    Gather,
    ReturnToDepo,
    Deposit,
    Attacking
}
