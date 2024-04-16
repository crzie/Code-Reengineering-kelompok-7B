using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    protected Player player;
    [SerializeField] protected GameObject UI;

    private const float rotationSpeed = 0.01f;
    private const float interactRange = 5f;

    private float distanceToPlayer;
    private Vector3 direction;

    protected virtual void Start()
    {
        StartCoroutine(InitializePlayer());
    }

    protected virtual void Update()
    {
        if (player == null) return;

        distanceToPlayer = Vector3.Distance(player.gameObject.transform.position, transform.position);
        direction = player.gameObject.transform.position - transform.position;
        direction.y = 0;

        if (InInteractRange)
        {
            RotateToPlayer();

            if (Input.GetKeyDown(KeyCode.E) && !player.IsCombat)
            {
                Interact();
            }
        }
    }

    private void RotateToPlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
    }

    private IEnumerator InitializePlayer()
    {
        yield return null;

        player = Player.Instance;
    }

    public abstract void Interact();

    public bool InInteractRange
    {
        get
        {
            return distanceToPlayer < interactRange;
        }
    }
}
