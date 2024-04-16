using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minotaur : MainEnemy
{
    // Start is called before the first frame update
    void Start()
    {
        BaseSpeed = 5f;
        RotationSpeed = 0.05f;
        WalkSpeedModifier = 0.225f;
        WalkRange = 7.5f;
        WalkCooldown = 3f;
        AggroRange = 12.5f;
        ChaseSpeedModifier = 1.1f;
        ChaseCooldown = 0.5f;
        GeneralAttackCooldown = 2f;
        TakeDamageCooldown = 0.5f;
        AttackRange = 2.75f;

        MinLevel = 50;
        MaxLevel = 60;
        Level = (long)Random.Range(MinLevel, MaxLevel);
        MaxHealth = 1000;
        Attack = 100;
        HealthModifier = 0.1f;
        AttackModifier = 0.1f;
        MaxHealth *= Mathf.Pow(1 + HealthModifier, Level - 1);
        Health = MaxHealth;
        Attack *= Mathf.Pow(1 + AttackModifier, Level - 1);

        Initialize();
    }
}
