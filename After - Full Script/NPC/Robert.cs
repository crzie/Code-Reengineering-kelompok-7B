using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robert : NPC
{
    public override void Interact()
    {
        UI.SetActive(true);
        UI.GetComponent<ChangeAbilityShop>().Refresh();
        Player.Instance.DisableAction();
        Player.Instance.DisableCamera();
    }
}
