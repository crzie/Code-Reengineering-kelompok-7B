using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : MainEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        BaseSpeed = 2f;
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

        MinLevel = 1;
        MaxLevel = 20;
        Level = (long) Random.Range(MinLevel, MaxLevel);
        MaxHealth = 1000;
        Attack = 100;
        ExperienceDrop = 100;
        GoldDrop = 100;
        HealthModifier = 0.1f;
        AttackModifier = 0.1f;
        ExperienceDropModifier = 0.1f;
        GoldDropModifier = 0.1f;

        Initialize();
    }
}
