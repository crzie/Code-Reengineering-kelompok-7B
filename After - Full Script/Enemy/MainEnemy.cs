using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainEnemy : Enemy
{
    public EnemySpawner spawner;

    public override void Die()
    {
        animator.SetTrigger("die");
        GetComponent<Collider>().enabled = false;
        spawner.NotifyDeath();
        Destroy(gameObject, 3f);
    }
}
