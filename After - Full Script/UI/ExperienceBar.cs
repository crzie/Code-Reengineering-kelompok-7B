using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTextComponent;
    [SerializeField] private TextMeshProUGUI expTextComponent;
    private Slider slider;
    private PlayerData player;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        player = PlayerData.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        expTextComponent.text = string.Format("{0} ({1:F2}%)", player.GetExperience(), player.GetExperience() / (float)player.GetExperienceNeeded() * 100); 
        levelTextComponent.text = player.GetLevel().ToString();

        slider.value = GetEaseValue(slider.value, player.GetExperience() / (float)player.GetExperienceNeeded());
    }

    float GetEaseValue(float currValue, float targetValue)
    {
        if (Mathf.Abs(currValue - targetValue) <= 0.001f)
        {
            return targetValue;
        }

        float delta = (targetValue - currValue) * 0.01f;
        return Mathf.Clamp(currValue + delta, Mathf.Min(currValue, targetValue), Mathf.Max(currValue, targetValue));
    }
}
