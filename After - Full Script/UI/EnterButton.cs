using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterButton : MonoBehaviour
{
    [SerializeField] public string destinationScene;
    [SerializeField] private LoadingScreen loadingScreen;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(EnterScene);
    }

    private void EnterScene()
    {
        loadingScreen.GoToScene(destinationScene);
    }
}
