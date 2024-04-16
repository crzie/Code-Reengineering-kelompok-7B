using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public bool isAttacking = false;
    [SerializeField] private MainEnemy enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAttacking)
        {
            PlayerData.Instance.ReduceHealth(enemy.Attack);
            Debug.Log(PlayerData.Instance.GetHealth());
        }
    }
}
