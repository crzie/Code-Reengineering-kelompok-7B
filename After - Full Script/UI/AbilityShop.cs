using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityShop : MonoBehaviour
{
    [SerializeField] private GameObject gameDatas;
    [SerializeField] private GameObject templateButton;
    [SerializeField] private Sprite offButton;
    [SerializeField] private Sprite onButton;

    [SerializeField] private GameObject nameTextObject;
    [SerializeField] private GameObject descriptionTextObject;
    [SerializeField] private Image imageObject;
    [SerializeField] private GameObject priceIconObject;
    [SerializeField] private GameObject priceTextObject;
    [SerializeField] private GameObject unchosenTextObject;
    [SerializeField] private GameObject buyButtonObject;

    private List<Ability> abilityList;
    private PlayerData player;

    private static AbilityShop instance;

    public static AbilityShop Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AbilityShop>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = Instance;
        player = PlayerData.Instance;

        GameObject abilityButton;
        abilityList = gameDatas.GetComponent<AbilityManager>().AbilityList;

        foreach (Ability ability in abilityList)
        {
            abilityButton = Instantiate(templateButton, transform);

            if (ability.icon != null)
            {
                // assign onclick here too
                abilityButton.GetComponent<Image>().sprite = ability.icon;

                abilityButton.GetComponent<Button>().onClick.AddListener(() => 
                {
                    OnIconClick(ability);
                });
            }
        }
    }

    private void OnIconClick(Ability ability)
    {
        nameTextObject.GetComponent<TextMeshProUGUI>().text = ability.Name;
        descriptionTextObject.GetComponent<TextMeshProUGUI>().text = "Description\n\n" + ability.Description;
        imageObject.sprite = ability.icon;

        if (!player.HasAbility(ability))
        {
            priceTextObject.GetComponent<TextMeshProUGUI>().text = ability.Price.ToString();
        }
        else
        {
            priceTextObject.GetComponent<TextMeshProUGUI>().text = "Already Unlocked";
        }

        Color newColor = imageObject.color;
        newColor.a = 1;
        imageObject.color = newColor;

        unchosenTextObject.SetActive(false);
        priceIconObject.SetActive(true);
        nameTextObject.SetActive(true);
        descriptionTextObject.SetActive(true);
        priceTextObject.SetActive(true);

        buyButtonObject.GetComponent<Button>().onClick.RemoveAllListeners();

        if (player.EnoughGold(ability.Price) && !player.HasAbility(ability))
        {
            buyButtonObject.GetComponent<Image>().sprite = onButton;
            priceTextObject.GetComponent<TextMeshProUGUI>().color = new Color(0.73f, 0.64f, 0.13f); // yellow

            buyButtonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                Buy(ability);
            });
        }
        else if(!player.HasAbility(ability))
        {
            buyButtonObject.GetComponent<Image>().sprite = offButton;
            priceTextObject.GetComponent<TextMeshProUGUI>().color = new Color(0.6f, 0, 0.05f); // red
        }
        else 
        {
            buyButtonObject.GetComponent<Image>().sprite = offButton;
            priceTextObject.GetComponent<TextMeshProUGUI>().color = new Color(0.45f, 0.45f, 0.45f); // grey
        }
    }

    public void Buy(Ability ability)
    {
        if(player.EnoughGold(ability.Price))
        {
            player.AddOwnedAbility(ability);
            player.AddToLoadout(ability);
            player.ReduceGold(ability.Price);

            // refresh UI after successful buying
            buyButtonObject.GetComponent<Button>().onClick.RemoveAllListeners();
            buyButtonObject.GetComponent<Image>().sprite = offButton;
            priceTextObject.GetComponent<TextMeshProUGUI>().text = "Already Unlocked";
            priceTextObject.GetComponent<TextMeshProUGUI>().color = new Color(0.45f, 0.45f, 0.45f); // grey
        }

        Refresh();
        LoadoutUI.Instance.Refresh();
        //ChangeAbilityShop.Instance.Refresh();
    }

    public void Refresh()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Ability ability = abilityList[i];
            if (ability.icon != null)
            {
                transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnIconClick(ability);
                });
            }
        }

    }
}
