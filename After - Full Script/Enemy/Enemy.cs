using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    public float BaseSpeed;
    public float RotationSpeed;
    public float WalkSpeedModifier;
    public float WalkRange;
    public float WalkCooldown;
    public float AggroRange;
    public float ChaseSpeedModifier;
    public float ChaseCooldown;
    public float GeneralAttackCooldown;
    public float TakeDamageCooldown;
    public float AttackRange;

    public long MinLevel;
    public long MaxLevel;
    public long Level;
    public float MaxHealth;
    public float Health;
    public float Attack;
    public long ExperienceDrop;
    public long GoldDrop;
    public float HealthModifier;
    public float AttackModifier;
    public float ExperienceDropModifier;
    public float GoldDropModifier;

    public float LastAttack = 0;
    public float LastTakingDamage = 0;

    public Vector2 spawnPoint;
    [SerializeField] protected Animator animator;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI levelTextComponent;
    private void Update()
    {
        healthBar.value = Health / MaxHealth;

        long levelDifference = this.Level - PlayerData.Instance.GetLevel();

        if (levelDifference <= -4)
        {
            levelTextComponent.color = Color.white;
        }
        else if (levelDifference <= 3)
        {
            levelTextComponent.color = Color.green;
        }
        else if (levelDifference <= 9)
        {
            levelTextComponent.color = Color.yellow;
        }
        else
        {
            levelTextComponent.color = Color.red;
        }
    }

    protected void Initialize()
    {
        MaxHealth *= Mathf.Pow(1 + HealthModifier, Level - 1);
        Health = MaxHealth;
        Attack *= Mathf.Pow(1 + AttackModifier, Level - 1);
        ExperienceDrop = (long)(ExperienceDrop * Mathf.Pow(1 + ExperienceDropModifier, Level - 1));
        GoldDrop *= (long)(GoldDrop * Mathf.Pow(1 + GoldDropModifier, Level - 1));

        spawnPoint = new Vector2(transform.position.x, transform.position.z);
        levelTextComponent.text = "Lv. " + Level;
    }

    public bool AttackInCooldown
    {
        get
        {
            return Time.time - LastAttack < GeneralAttackCooldown;
        }
    }

    public bool TakingDamageInCooldown
    {
        get
        {
            return Time.time - LastTakingDamage < TakeDamageCooldown;
        }
    }

    public void TakeDamage(float damage)
    {
        if (TakingDamageInCooldown) return;

        Health -= damage;
        PopUpGenerator.Instance.CreatePopup(transform.position, ((int)damage).ToString(), Color.white);

        if (Health <= 0)
        {
            Die();
            RewardPlayer();
        }
        else
        {
            LastTakingDamage = Time.time;
            animator.SetTrigger("takeDamage");
        }
    }

    public abstract void Die();

    public void RewardPlayer()
    {
        PlayerData.Instance.AddExperience(this.ExperienceDrop);
        PlayerData.Instance.AddGold(this.GoldDrop);
    }
}
