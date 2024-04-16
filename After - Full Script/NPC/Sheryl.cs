using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sheryl : NPC
{
    public override void Interact()
    {
        UI.SetActive(true);
        Player.Instance.DisableAction();
        Player.Instance.DisableCamera();
    }
}
