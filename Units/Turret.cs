using UnityEngine;
public class Turret : Unit
{
    public TurretState state;

    public override void Init(ItemControl ic)
    {
        base.Init(ic);
        state = TurretState.Idle;
        Overseer.Instance.teamDict[Team].turreted = true;
    }

    public override void Update()
    {
        switch (state)
        {
            case TurretState.Idle:
                break;

            case TurretState.Attacking:
                Attacking();
                break;
        }

        if (target != null)
        {
            state = TurretState.Attacking;
        }
        else
        {
            if (state == TurretState.Attacking)
            {
                state = TurretState.Idle;
            }
        }

        base.Update();
    }

    public override void Attack()
    {
        if (target == null) return;

        if (liberated) Heal(Atk);

        base.Attack();
    }

    public override void Vet()
    {
        if (vetLevel > 9) return;

        MaxHp++;
        Heal(target.MaxHp / 4);
        vetLevel++;
        if (showingTooltip) FreshTip();

        base.Vet();
    }

    void Special()
    {
        Debug.Log("No Special yet");
    }
}

public enum TurretState
{
    Idle,
    Attacking
}
