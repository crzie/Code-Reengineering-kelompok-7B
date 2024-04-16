using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AreaWarning : MonoBehaviour
{
    [SerializeField] private Button cancelButton;
    [SerializeField] private EnterButton confirmButton;
    [SerializeField] private TextMeshProUGUI areaTextComponent;
    [SerializeField] private TextMeshProUGUI levelTextComponent;

    void Start()
    {
        Debug.Log("start");
        cancelButton.onClick.AddListener(Close);
        //confirmButton.onClick.AddListener(Confirm);
    }

    public void Close()
    {
        Debug.Log("close");
        gameObject.SetActive(false);
        Player.Instance.EnableAction();
        Player.Instance.EnableCamera();
    }

    //public void Confirm()
    //{

    //}

    public void ShowWarning(string areaName, int lowRecommendedLevel, int highRecommendedLevel, string destinationName)
    {
        gameObject.SetActive(true);
        Player.Instance.DisableAction();
        Player.Instance.DisableCamera();

        areaTextComponent.text = areaName;
        levelTextComponent.text = string.Format("(Recommended Level: {0}-{1})", lowRecommendedLevel, highRecommendedLevel);
        confirmButton.destinationScene = destinationName;
    }
}
