using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string Name { get; set; }
    public UnitType Type { get; set; }
    public int Team { get; set; }
    public int EnemyTeam { get; set; }
    public int Reclaim { get; set; }
    public int Atk { get; set; }
    public int Range { get; set; }
    public int MaxHp { get; set; }
    public int hp;
    public float Speed { get; set; }

    public VetStyle vetStyle;
    public int vetLevel;
    public string Additional { get; set; }
    public string LiberationInfo { get; set; }
    public LiberationStats liberationStats;
    public bool liberated;

    public Unit target;
    public float Cooldown { get; set; }
    float cooldownTimer;

    public int Cost { get; set; }
    public Sprite Icon { get; set; }

    [Space][Header("Visuals")]
    public AttackVisual atkVisual;
    public HitVisual hitVisual;
    public HealthBar healthBar;
    public DamagedVal damagedVal;
    public HealedVal healedVal;
    public GameObject selectionBox;
    public GameObject rainbow;
    public GameObject shades;
    public GameObject lowerCase;
    public GameObject capitol;

    [Space][Header("Prefabs")]
    public Reclaim reclaimPrefab;
    public VetIndicator vetIndicatorPrefab;

    [HideInInspector] public bool showingTooltip;

    public virtual void Init(ItemControl ic)
    {
        StaggerChecks.Instance.AddUnit(this);

        SetStats(ic);
        //Debug.Log($"{name}{GetInstanceID()} Init. Team:{Team}, Reclaim:{Reclaim} Atk:{Atk}, Hp:{MaxHp}");
    }

    void SetStats(ItemControl ic)
    {
        Name = ic.item.name;
        Icon = ic.item.icon;
        Team = ic.item.team;
        Additional = ic.item.additional;

        Type = ic.item.stats.unitType;
        Reclaim = ic.item.stats.reclaim;
        Atk = ic.item.stats.atk;
        Range = ic.item.stats.range;
        Cooldown = ic.item.stats.cooldown;
        MaxHp = ic.item.stats.hp;
        Speed = ic.item.stats.speed;
        vetStyle = ic.item.stats.vetStyle;
        Cost = ic.item.stats.cost;
        LiberationInfo = ic.item.stats.liberationInfo;
        liberationStats = ic.item.stats.liberationStats;

        hp = MaxHp;
        EnemyTeam = Team == 0 ? 1 : 0;
    }

    public virtual void CheckForThings()
    {
        if (target != null) return;
        if (this == null) return;

        Collider[] thingsInRange = Physics.OverlapBox(transform.position, new Vector3(Range, Range, 50));
        foreach (Collider collider in thingsInRange)
        {
            //if (debugs) Debug.Log($"Check found {collider.gameObject.name}{collider.gameObject.GetInstanceID()}");
            Unit u = collider.GetComponent<Unit>();
            if (u)
            {
                if (u.Team == EnemyTeam)
                {
                    if (u.stealth)
                        if (!TryReveal(u)) 
                            continue;

                    target = u;
                    if (debugs) Debug.Log($"Targeting {u.name}{u.GetInstanceID()}");
                    return;
                }
            }
            target = null;

            Reclaim rclaim = collider.GetComponent<Reclaim>();
            if (rclaim)
            {
                if (Mathf.Abs(rclaim.transform.position.x - transform.position.x) < 1)
                {
                    if (debugs) Debug.Log($"Reclaim pickup {rclaim.val}");
                    Overseer.Instance.Earn(Team, rclaim.val);
                    Destroy(rclaim.gameObject);
                }
            }
        }

        debugs = false;
    }

    bool TryReveal(Unit u)
    {
        if (Type == UnitType.HQ || Type == UnitType.Worker)
        {
            //Debug.Log("Revealed");
            u.shades.SetActive(false);
            u.stealth = false;
            return true;
        }

        return false;
    }

    public virtual void Attacking()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            cooldownTimer = Cooldown;
            Attack();
        }
    }

    public virtual void Attack()
    {
        if (target == null) return;

        atkVisual.Go();

        TrySplash();

        //Debug.Log($"{name}{GetInstanceID()} attacks {Target.name}{Target.GetInstanceID()} for {Atk} damage");
        if (!target.TakeDamage(this, AtkMod()))
        {
            //Debug.Log($"{name}{GetInstanceID()} defeated {Target.name}{Target.GetInstanceID()} clearing target");
            Vet();
            target = null;
        }

        if (Range > 3)
        {
            Sound.Guy.Ranged();
        }
        else
        {
            Sound.Guy.Melee();
        }
    }

    int AtkMod()
    {
        if (Type == UnitType.Melee || Type == UnitType.HQ)
        {
            if (liberated)
            {
                if (UnityEngine.Random.Range(0, 5) == 0)
                {
                    Debug.Log($"{Name}{GetInstanceID()} crit");
                    Heal(Atk);
                    return Atk * 3;
                }
            }
        }
        return Atk;
    }

    public virtual void Vet()
    {
        if (vetLevel > 4) return;

        Instantiate(vetIndicatorPrefab, transform);
        if (vetStyle == VetStyle.Hp)
        {
            if (vetLevel < 2)
                MaxHp += 2;

            Heal(1);
        }
        if (vetStyle == VetStyle.Atk)
        {
            if (vetLevel < 2)
                Atk++;
        }

        if (vetLevel < 2)
            MaxHp += 1;

        Heal(1);
        healthBar.UpdateBar((float)hp / (float)MaxHp);
        if (showingTooltip) FreshTip();

        vetLevel++;

        transform.localScale *= 1.2f;

        Sound.Guy.Vet();
    }

    public virtual void Heal(int healing)
    {
        healedVal.Show(healing);
        hp += healing;
        if (hp > MaxHp)
        {
            hp = MaxHp;
        }

        healthBar.UpdateBar((float)hp / (float)MaxHp);
        if (showingTooltip) FreshTip();
    }

    const float hotTickTime = 1.2f;
    float hotTickTimer;
    int healStore;
    public void ApplyHot(int healing)
    {
        healStore += healing;
    }
    public virtual void Update()
    {
        Hot();
    }
    void Hot()
    {
        hotTickTimer -= Time.deltaTime;
        if (hotTickTimer <= 0)
        {
            hotTickTimer = hotTickTime;
            if (healStore > 0)
            {
                //Debug.Log("Hot heal");
                Heal(1);
                healStore--;
            }
        }
    }

    public virtual bool TakeDamage(Unit assailant, int damage)
    {
        foreach (var ai in Overseer.Instance.aIPlayers) ai.UnitTakingDamage(this);
        if (TryReflect(assailant, damage)) return true;

        hitVisual.Go();
        damagedVal.Show(damage);
        Sound.Guy.TakeDamage();
        CamShake.Shake(0.2f + damage/10, damage/3);

        hp -= damage;
        if (hp < 0)
        {
            Ded();
            return false;
        }

        healthBar.UpdateBar((float)hp/(float)MaxHp);
        if (showingTooltip) FreshTip();

        return true;
    }

    bool TryReflect(Unit assailant, int damage)
    {
        if (Type == UnitType.Tank)
            if (liberated)
                return Reflect(assailant, damage);

        return false;
    }

    bool Reflect(Unit assailant, int damage)
    {
        if (UnityEngine.Random.Range(0, 4) == 0)
        {
            //Debug.Log("Reflect");
            assailant.TakeDamage(this, damage);
            Sound.Guy.Reflect();
            return true;
        }

        return false;
    }

    void TrySplash()
    {
        if (Type == UnitType.Ranged)
            if (liberated)
                target.Splash();
    }

    public void Splash()
    {
        //Debug.Log("Splash");

        //int splashDamage = Mathf.FloorToInt((float)Atk / 2); Was overpowered
        Collider[] thingsInSplashRange = Physics.OverlapBox(transform.position, new Vector3(4, 4, 50));
        foreach (Collider collider in thingsInSplashRange)
        {
            Unit u = collider.GetComponent<Unit>();
            if (u)
            {
                if (u.Team == Team)
                {
                    u.TakeDamage(this, 1);
                }
            }
        }
    }

    public virtual void Ded()
    {
        if (showingTooltip) ToolTip.Hide();
        if (Selector.selected == this) Selector.DeSelect();
        //Debug.Log($"{name}{GetInstanceID()} ded");
        StaggerChecks.Instance.RemoveUnit(this);
        Reclaim r = Instantiate(reclaimPrefab);
        r.transform.position = this.transform.position;
        r.val = Reclaim;
        Sound.Guy.Ded();
        Destroy(gameObject);
    }

    public virtual void Liberate()
    {
        if (liberated)
        {
            Debug.Log($"{Name}{GetInstanceID()} Already liberated");
            return;
        }

        StatIncrease();
        Heal(MaxHp);

        liberated = true;
        rainbow.SetActive(true);
        lowerCase.SetActive(false);
        capitol.SetActive(true);
        Debug.Log($"{Name}{GetInstanceID()} Liberated");

        TryStealth();
    }

    [HideInInspector] public bool stealth;
    void TryStealth()
    {
        if (Type == UnitType.Speedy)
            if (liberated)
            {
                shades.SetActive(true);
                stealth = true;
            }
    }

    void StatIncrease()
    {
        Atk += liberationStats.atk;
        Range += liberationStats.range;
        Cooldown += liberationStats.cooldown;
        MaxHp += liberationStats.hp;
        // speed is onlly set for units with a navMeshAgent
    }

    public void OnMouseEnter()
    {
        FreshTip();
    }

    public void OnMouseDown()
    {
        Selector.Select(this);
    }

    public void FreshTip()
    {
        Tip tip = new Tip(TipType.GameObject, Name, Type, Team, Cost, vetLevel, Atk, Cooldown, Range, MaxHp, hp, Speed, Reclaim, vetStyle, Additional);
        ToolTip.Show(tip);
        showingTooltip = true;
    }

    public void OnMouseExit()
    {
        ToolTip.Hide();
        showingTooltip = false;
    }

    public bool debugs;
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Range * 2, Range * 2, 50));
    }
#endif
}

public enum VetStyle
{
    None,
    Hp,
    Atk
}
