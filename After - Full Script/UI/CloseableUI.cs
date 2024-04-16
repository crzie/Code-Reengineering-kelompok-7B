using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseableUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;

    void Start()
    {
        closeButton.onClick.AddListener(Close);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }
    private void Close()
    {
        gameObject.SetActive(false);
        Player.Instance.EnableAction();
        Player.Instance.EnableCamera();
    }
}
