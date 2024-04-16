using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Player player;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        player = Player.Instance;
        player.InputAction.Player.BasicAttack.performed += BasicAttack;
    }

    private void BasicAttack(InputAction.CallbackContext context)
    {
        if(player.IsCombat)
        {
            animator.SetBool("isClicking", true);
        }
    }
}
