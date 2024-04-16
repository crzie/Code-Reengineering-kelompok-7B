using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject ArmSword;
    [SerializeField] private GameObject ThighSword;

    public void StopDashing()
    {
        animator.SetBool("isDashing", false);
    }

    public void DisableAction()
    {
        Player.Instance.DisableAction();
    }

    public void EnableAction()
    {
        Player.Instance.EnableAction();
    }

    public void ActivateArmSword()
    {
        ArmSword.SetActive(true);
        ThighSword.SetActive(false);
    }

    public void ActivateThighSword()
    {
        ThighSword.SetActive(true);
        ArmSword.SetActive(false);
    }

    public void ClearClick()
    {
        animator.SetBool("isClicking", false);
    }

    public void SetComboCount(int count)
    {
        ArmSword.GetComponent<Sword>().ComboCount = count;
    }

    public void EnableDamage()
    {
        ArmSword.GetComponent<Sword>().isAttacking = true;
    }

    public void DisableSword()
    {
        ArmSword.GetComponent<Sword>().isAttacking = false;
    }

    public void ActivateSword()
    {
        ClearClick();
        EnableDamage();
    }

    public void ClearLandingAnimation()
    {
        animator.SetBool("isLightLand", false);
        animator.SetBool("isHardLand", false);
        animator.SetBool("isRoll", false);
    }
}
