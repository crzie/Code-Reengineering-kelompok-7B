using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthElemental : MainEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        BaseSpeed = 4f;
        RotationSpeed = 0.05f;
        WalkSpeedModifier = 0.225f;
        WalkRange = 10f;
        WalkCooldown = 3f;
        AggroRange = 15f;
        ChaseSpeedModifier = 0.5f;
        ChaseCooldown = 0.5f;
        GeneralAttackCooldown = 2f;
        TakeDamageCooldown = 0.5f;
        AttackRange = 3f;

        MinLevel = 25;
        MaxLevel = 35;
        Level = (long) Random.Range(MinLevel, MaxLevel);
        MaxHealth = 4000;
        Attack = 650;
        ExperienceDrop = 1000;
        GoldDrop = 300;
        HealthModifier = 0.25f;
        AttackModifier = 0.1f;
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
