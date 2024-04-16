using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    [SerializeField] private EnemyWeapon weapon;

    public void DisableWeapon()
    {
        weapon.isAttacking = false;
    }
    public void EnableWeapon()
    {
        weapon.isAttacking = true;
    }

}
