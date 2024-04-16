using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatsMenu : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI levelTextComponent;
    [SerializeField] private TextMeshProUGUI healthTextComponent;
    [SerializeField] private TextMeshProUGUI attackTextComponent;
    [SerializeField] private TextMeshProUGUI goldTextComponent;

    private static StatsMenu instance;

    public float lastOpen = 0;
    public float lastClose = 0;

    public static StatsMenu Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StatsMenu>();
            }
            return instance;
        }
    }

    public bool IsActive
    {
        get; private set;
    }

    private void Start()
    {
        instance = Instance;
        Activate();
    }

    private void Update()
    {
        if(IsActive && Input.GetKeyDown(KeyCode.C) && Time.time - lastOpen > 0.5f)
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        Refresh();
        gameObject.SetActive(true);
        IsActive = true;

        Player.Instance.DisableAction();
        Player.Instance.DisableCamera();
        lastOpen = Time.time;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        IsActive = false;

        Player.Instance.EnableAction();
        Player.Instance.EnableCamera();
        lastClose = Time.time;
    }

    private void Refresh()
    {
        PlayerData player = PlayerData.Instance;

        levelTextComponent.text = player.GetLevel().ToString();
        healthTextComponent.text = player.GetMaxHealth().ToString("F2");
        attackTextComponent.text = player.GetAttack().ToString("F2");
        goldTextComponent.text = player.GetGold().ToString();
    }
}
