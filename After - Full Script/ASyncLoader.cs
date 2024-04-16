using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ASyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    //[SerializeField] private Scene currentScene;

    [SerializeField] private Slider loadingSlider;
    
    public void GoToScene(string sceneName)
    {
        loadingScreen.SetActive(true);

        Input.simulateMouseWithTouches = false;
        Input.multiTouchEnabled = false;

        StartCoroutine(ExecuteLoadScene(sceneName));
    }

    private IEnumerator ExecuteLoadScene(string sceneName)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        while(!loadOperation.isDone) 
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }

        Input.simulateMouseWithTouches = true;
        Input.multiTouchEnabled = true;
    }
}
