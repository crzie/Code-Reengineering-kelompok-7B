using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int ComboCount;
    public bool isAttacking = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && isAttacking)
        {
            other.GetComponent<MainEnemy>().TakeDamage(PlayerData.Instance.GetAttack() * Mathf.Sqrt(ComboCount));
        }
    }
}
