using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElemental : MainEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        BaseSpeed = 4f;
        RotationSpeed = 0.05f;
        WalkSpeedModifier = 0.35f;
        WalkRange = 10f;
        WalkCooldown = 3f;
        AggroRange = 15f;
        ChaseSpeedModifier = 0.75f;
        ChaseCooldown = 0.5f;
        GeneralAttackCooldown = 2f;
        TakeDamageCooldown = 0.5f;
        AttackRange = 3f;

        MinLevel = 40;
        MaxLevel = 50;
        Level = (long) Random.Range(MinLevel, MaxLevel);
        MaxHealth = 7000;
        Attack = 1500;
        ExperienceDrop = 2400;
        GoldDrop = 600;
        HealthModifier = 0.15f;
        AttackModifier = 0.2f;
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
