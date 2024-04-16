using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerData
{
    private long Level = 1;
    private long Gold = 10000;
    private float MaxHealth = 1000f;
    private float Health;
    private float Attack = 50f;
    private long Experience = 0;
    private long ExperienceNeeded = 500;
    private float MaxHealthIncreaseModifier = 0.1f;
    private float AttackIncreaseModifier = 0.1f;
    private float ExperienceIncreaseModifier = 0.25f;
    public float HealthRegenerationCooldown { get { return 3f; } }

    private const int MaxLoadoutSize = 4;
    public float LastTakeDamage { get; private set; }

    private List<Ability> OwnedAbilities = new List<Ability>();
    private List<Ability> Loadout = new List<Ability>();

    public float healthRegenTimer = 0;

    private static PlayerData instance;
    public static PlayerData Instance
    {
        get {
            if (instance == null)
            {
                instance = new PlayerData();
            }
            return instance;
        }
    }

    private PlayerData()
    {
        FullHealth();
    }

    public long GetLevel()
    {
        return Level;
    }

    public void UpgradeLevel()
    {
        Level++;
        MaxHealth *= 1 + MaxHealthIncreaseModifier;
        Attack *= 1 + AttackIncreaseModifier;
        Experience -= ExperienceNeeded;
        ExperienceNeeded = (long) (ExperienceNeeded * (1 + ExperienceIncreaseModifier));
        Health = MaxHealth;
        CreatePopup("LEVEL UP!", 1f, 0.92f, 0);
    }

    public void AddGold(long gold)
    {
        this.Gold += gold;
        CreatePopup("+" + gold + " gold", 1f, 0.92f, 0);
    }

    public void ReduceGold(long gold)
    {
        this.Gold -= gold;
    }

    public bool EnoughGold(long gold)
    {
        return this.Gold >= gold;
    }

    public long GetGold()
    {
        return Gold;
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public void FullHealth()
    {
        Health = MaxHealth;
    }

    public float GetHealth()
    {
        return Health;
    }

    public void RegenerateHealth()
    {
        this.Health += (MaxHealth) * 0.01f;
        Health = Mathf.Min(Health, MaxHealth);
    }

    public void ReduceHealth(float damage)
    {
        this.Health -= damage;
        LastTakeDamage = Time.time;
        CreatePopup(((int)damage).ToString(), 1, 0, 0);

        if(Health < 0)
        {
            Health = 0;
            Player.Instance.Die();
        }
    }

    public float GetAttack()
    {
        return Attack;
    }

    public void AddExperience(long experience)
    {
        this.Experience += experience;
        CreatePopup("+" + experience + " exp", 0.03f, 0.85f, 0.98f);

        while (this.Experience > ExperienceNeeded)
        {
            UpgradeLevel();
        }
    }

    public long GetExperience()
    {
        return Experience;
    }

    public long GetExperienceNeeded()
    {
        return ExperienceNeeded;
    }

    public void AddOwnedAbility(Ability ability)
    {
        this.OwnedAbilities.Add(ability);
    }

    public bool HasAbility(Ability ability)
    {
        return OwnedAbilities.Contains(ability);
    }

    public Ability GetFromOwnedAbility(int index)
    {
        if (index >= OwnedAbilities.Count) return null;

        return this.OwnedAbilities[index];
    }

    public void AddToLoadout(Ability ability)
    {
        if (Loadout.Count >= MaxLoadoutSize) return;

        this.Loadout.Add(ability);
    }

    public Ability GetFromLoadout(int index)
    {
        if(index >= Loadout.Count) return null;

        return this.Loadout[index];
    }
    
    public void SetLoadoutByIndex(int index, Ability ability)
    {
        Loadout[index] = ability;
    }

    public bool AbilityInLoadout(Ability ability)
    {
        return Loadout.Contains(ability);
    }

    private void CreatePopup(string text, float r, float g, float b)
    {
        PopUpGenerator.Instance.CreatePopup(Player.Instance.transform.position, text, new Color(r, g, b));
    }
}
