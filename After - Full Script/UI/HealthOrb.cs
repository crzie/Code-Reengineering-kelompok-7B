using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthOrb : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthTextComponent;
    private Slider slider;
    private PlayerData player;

    private void Start()
    {
        slider = GetComponent<Slider>();
        player = PlayerData.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        healthTextComponent.text = ((long) player.GetHealth()).ToString();
        //slider.value = player.GetHealth() / player.GetMaxHealth();

        slider.value = GetEaseValue(slider.value, player.GetHealth() / player.GetMaxHealth());
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
