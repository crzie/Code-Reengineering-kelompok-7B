using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpGenerator : MonoBehaviour
{
    [SerializeField] private GameObject popupTemplate;

    public static PopUpGenerator Instance { get; private set; }
    public static Color ColorBlue { get { return new Color(0.03f, 0.85f, 0.98f, 1); } }
    public static Color ColorYellow { get { return new Color(1f, 0.92f, 0, 1); } }
    
    void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    CreatePopup(Player.Instance.transform.position, "1000000", Color.yellow);
        //}
    }

    public void CreatePopup(Vector3 position, string text, Color color)
    {
        Vector3 randomizer = Random.insideUnitSphere * 2;

        GameObject popup = Instantiate(popupTemplate, position + randomizer, Quaternion.identity);
        TextMeshProUGUI textComponent = popup.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.color = color;

        Destroy(popup, 1f);
    }
}
