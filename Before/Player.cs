using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public bool IsCombat
    {
        get; private set;
    }

    private CinemachineFreeLook freelook;
    [SerializeField] private Animator animator;
    [SerializeField] private StatsMenu statsMenu;
    [SerializeField] private NPC[] npcList;
    [SerializeField] private LoadingScreen loadingScreen;
    private Vector3 spawnPosition;

    public PlayerInputAction InputAction
    {
        get; private set;
    }

    public bool InInteractRange
    {
        get
        {
            foreach (NPC npc in npcList)
            {
                if(npc.InInteractRange) return true;
            }

            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        InputAction = new PlayerInputAction();
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerCombat>().enabled = true;
        freelook = FindObjectOfType<CinemachineFreeLook>();

        //PlayerData.Instance.FullHealth();
        spawnPosition = transform.position;

        InputAction.Player.Enable();
        InputAction.Player.CombatToggle.performed += ToggleCombat;
        InputAction.Player.StatsMenu.performed += ShowStats;
    }

    // Update is called once per frame
    void Update()
    {
        RegenerateHealth();
    }

    private void ToggleCombat(InputAction.CallbackContext context)
    {
        if (CanToggleCombat)
        {
            IsCombat = !IsCombat;
            if(IsCombat)
            {
                animator.SetTrigger("UnsheathSword");
            }
            else
            {
                animator.SetTrigger("SheathSword");
            }
        }
    }

    private void ShowStats(InputAction.CallbackContext context)
    {
        if (InInteractRange) return;

        if(Time.time - statsMenu.lastClose > 0.5f) statsMenu.Activate();
    }

    public void DisableAction()
    {
        InputAction.Player.Disable();
    }

    public void EnableAction()
    {
        InputAction.Player.Enable();
    }

    public void DisableCamera()
    {
        freelook.enabled = false;
    }

    public void EnableCamera()
    {
        freelook.enabled = true;
    }

    public bool CanToggleCombat
    {
        get
        {
            return GetComponent<PlayerMovement>().IsGrounded;
        }
    }

    public void RegenerateHealth()
    {
        if (Time.time - PlayerData.Instance.LastTakeDamage < PlayerData.Instance.HealthRegenerationCooldown) return;

        PlayerData.Instance.healthRegenTimer += Time.deltaTime;
        if(PlayerData.Instance.healthRegenTimer > 1)
        {
            PlayerData.Instance.RegenerateHealth();
            PlayerData.Instance.healthRegenTimer -= 1;
        }
    }

    public void Die()
    {
        animator.SetTrigger("die");
        DisableAction();
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);

        loadingScreen.GoToScene("MainScene");
        EnableAction();
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;
        animator.SetTrigger("alive");
        PlayerData.Instance.FullHealth();
        //transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }

    public void CheatSpeed()
    {
        GetComponent<PlayerMovement>().CheatSpeed();
    }

}
