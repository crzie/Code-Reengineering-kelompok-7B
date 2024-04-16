using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoebus : BossEnemy
{
    public float FlyingRange = 25f;

    // Start is called before the first frame update
    void Start()
    {
        BaseSpeed = 3f;
        RotationSpeed = 0.05f;
        WalkSpeedModifier = 1f;
        WalkRange = 10f;
        WalkCooldown = 3f;
        AggroRange = 15f;
        ChaseSpeedModifier = 1.5f;
        ChaseCooldown = 0.5f;
        GeneralAttackCooldown = 2.5f;
        TakeDamageCooldown = 0.5f;
        AttackRange = 3f;

        MinLevel = 80;
        MaxLevel = 80;
        Level = (long)Random.Range(MinLevel, MaxLevel);
        MaxHealth = 1000;
        Attack = 100;
        ExperienceDrop = 200;
        GoldDrop = 200;
        HealthModifier = 0.06f;
        AttackModifier = 0.06f;
        ExperienceDropModifier = 0.075f;
        GoldDropModifier = 0.075f;
        //MaxHealth *= Mathf.Pow(1 + HealthModifier, Level - 1);
        //Health = MaxHealth;
        //Attack *= Mathf.Pow(1 + AttackModifier, Level - 1);
        //ExperienceDrop = (long)(ExperienceDrop * Mathf.Pow(1 + ExperienceDropModifier, Level - 1));
        //GoldDrop *= (long)(GoldDrop * Mathf.Pow(1 + GoldDropModifier, Level - 1));

        Initialize();
    }
}
