using UnityEngine;
using UnityEngine.AI;
public class Com : Unit
{
    public ComState state = ComState.Idle;
    NavMeshAgent agent;
    public Wibble wibble;
    public Jumple jumple;
    public GameObject eyes;
    public ScreenSpaceHealthBar screenSpaceHealth;
    public GameObject rubble;

    public override void Init(ItemControl ic)
    {
        agent = GetComponent<NavMeshAgent>();
        base.Init(ic);
        screenSpaceHealth = Team == 0 ? Overseer.Instance.com0Health : Overseer.Instance.com1Health;
    }

    public override void Ded()
    {
        Debug.Log($"Team {Team}'s com is down. Team {EnemyTeam} wins!");
        foreach (var ai in Overseer.Instance.aIPlayers) ai.initialised = false;
        lowerCase.SetActive(false);
        capitol.SetActive(false);
        rubble.SetActive(true);
        rubble.transform.SetParent(null);
        Ref.ImDefeated(Team);
        CamShake.Shake(2, 10);
        base.Ded();
    }

    public override void Update()
    {
        switch (state)
        {
            case ComState.Attacking:
                Attacking();
                break;

            case ComState.Idle:
                break;

            case ComState.Move:
                Move();
                break;
        }

        if (target != null)
        {
            state = ComState.Attacking;
        }
        else
        {
            if (state == ComState.Attacking)
            {
                if (liberated)
                {
                    state = ComState.Move;
                }
                else
                {
                    state = ComState.Idle;
                }
            }
        }
        base.Update();
    }

    void Move()
    {
        agent.SetDestination(Overseer.Instance.teamDict[EnemyTeam]._comLoc.position);
    }

    public override void Attack()
    {
        agent.SetDestination(new Vector3(transform.position.x, 0, transform.position.z));
        base.Attack();
    }

    public override void Liberate()
    {
        if (liberated) return;

        agent.speed += liberationStats.speed;
        eyes.SetActive(true);
        wibble.enabled = true;
        jumple.enabled = true;
        state = ComState.Move;

        base.Liberate();
    }

    public override bool TakeDamage(Unit assailant, int damage)
    {
        CamShake.Shake(0.6f, ((float)hp / (float)MaxHp) * 8);
        bool result = base.TakeDamage(assailant, damage);
        screenSpaceHealth.UpdateBar((float)hp / (float)MaxHp);
        return result;
    }

    public override void Heal(int healing)
    {
        base.Heal(healing);
        screenSpaceHealth.UpdateBar((float)hp / (float)MaxHp);
    }

    public override void Vet()
    {
        base.Vet();
        screenSpaceHealth.UpdateBar((float)hp / (float)MaxHp);
    }
}

public enum ComState
{
    Attacking,
    Idle,
    Move
}
