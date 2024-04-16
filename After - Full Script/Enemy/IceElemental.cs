using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceElemental : MainEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        BaseSpeed = 3f;
        RotationSpeed = 0.05f;
        WalkSpeedModifier = 0.35f;
        WalkRange = 10f;
        WalkCooldown = 3f;
        AggroRange = 15f;
        ChaseSpeedModifier = 0.6f;
        ChaseCooldown = 0.5f;
        GeneralAttackCooldown = 2f;
        TakeDamageCooldown = 0.5f;
        AttackRange = 3f;

        MinLevel = 35;
        MaxLevel = 45;
        Level = (long) Random.Range(MinLevel, MaxLevel);
        MaxHealth = 5500;
        Attack = 1000;
        ExperienceDrop = 1800;
        GoldDrop = 550;
        HealthModifier = 0.15f;
        AttackModifier = 0.15f;
        ExperienceDropModifier = 0.2f;
        GoldDropModifier = 0.2f;
        //MaxHealth *= Mathf.Pow(1 + HealthModifier, Level - 1);
        //Health = MaxHealth;
        //Attack *= Mathf.Pow(1 + AttackModifier, Level - 1);
        //ExperienceDrop = (long)(ExperienceDrop * Mathf.Pow(1 + ExperienceDropModifier, Level - 1));
        //GoldDrop *= (long)(GoldDrop * Mathf.Pow(1 + GoldDropModifier, Level - 1));

        Initialize();
    }
}
