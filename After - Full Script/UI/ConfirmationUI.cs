using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ConfirmationUI : MonoBehaviour
{
    [SerializeField] protected Button cancelButton;
    [SerializeField] protected Button confirmButton;

    public void Awake()
    {
        cancelButton.onClick.AddListener(Close);
        confirmButton.onClick.AddListener(Confirm);
    }

    protected void Close()
    {
        Debug.Log("well");
        gameObject.SetActive(false);
        Player.Instance.EnableAction();
        Player.Instance.EnableCamera();
    }

    protected void Activate()
    {
        gameObject.SetActive(true);
        Player.Instance.DisableAction();
        Player.Instance.DisableCamera();
    }

    public abstract void Confirm();
}
