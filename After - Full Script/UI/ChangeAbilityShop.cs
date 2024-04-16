using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAbilityShop : MonoBehaviour
{
    private const int ownedButtonCount = 8;
    private const int loadoutButtonCount = 4;

    [SerializeField] private Button loadoutButton;
    [SerializeField] private Button ownedButton;
    //[SerializeField] private Image loadoutIcon;
    //[SerializeField] private Image ownedIcon;
    [SerializeField] private Image toChange;
    [SerializeField] private Image toEquip;
    [SerializeField] private Sprite onButton;
    [SerializeField] private Sprite offButton;
    [SerializeField] private Button changeButton;
    [SerializeField] private GameObject goldIcon;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private GameObject loadoutLayout;
    [SerializeField] private GameObject ownedLayout;
    private int selectedIndex;
    private Ability toEquipAbility;

    private PlayerData player;

    private static ChangeAbilityShop instance;

    public static ChangeAbilityShop Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ChangeAbilityShop>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerData.Instance;
        Button buttonObject;

        for (int i = 0; i < ownedButtonCount; i++)
        {
            buttonObject = Instantiate(ownedButton);

            Ability ability = player.GetFromOwnedAbility(i);
            if(ability != null)
            {
                buttonObject.GetComponent<Image>().sprite = ability.icon;
            }

            buttonObject.transform.SetParent(ownedLayout.transform);

            buttonObject.onClick.AddListener(() =>
            {
                OnOwnedIconClick(ability);
            });
        }

        for(int i = 0; i < loadoutButtonCount; i++)
        {
            buttonObject = Instantiate(loadoutButton, new Vector3(), Quaternion.identity);

            Ability ability = player.GetFromOwnedAbility(i);
            if (ability != null)
            {
                buttonObject.GetComponentInChildren<Image>().sprite = ability.icon;
            }

            buttonObject.transform.SetParent(loadoutLayout.transform);

            int index = i;
            buttonObject.onClick.AddListener(() =>
            {
                OnLoadoutIconClick(index);
            });
        }
    }

    void OnOwnedIconClick(Ability ability)
    {
        if (ability != null)
        {
            toEquip.sprite = ability.icon;
            toEquipAbility = ability;

            if(toEquip.sprite != null && toChange.sprite != null)
            {
                changeButton.onClick.RemoveAllListeners();
                goldIcon.SetActive(true);
                priceText.gameObject.SetActive(true);

                if (!player.AbilityInLoadout(toEquipAbility))
                {
                    changeButton.GetComponent<Image>().sprite = onButton;

                    priceText.text = ability.Price.ToString();
                    if (player.EnoughGold(ability.Price))
                    {
                        priceText.color = new Color(0.73f, 0.64f, 0.13f); // yellow
                        changeButton.onClick.AddListener(OnChangeIconClick);
                    }
                    else
                    {
                        changeButton.GetComponent<Image>().sprite = offButton;
                        priceText.color = new Color(0.6f, 0, 0.05f); // red
                    }
                }
                else 
                {
                    changeButton.GetComponent<Image>().sprite = offButton;
                    priceText.text = "Already in the Loadout";
                    priceText.color = new Color(0.45f, 0.45f, 0.45f); // grey
                }
            }
        }
    }

    void OnLoadoutIconClick(int index)
    {
        Ability ability = player.GetFromLoadout(index);
        if (ability != null)
        {
            toChange.sprite = ability.icon;
            selectedIndex = index;

            if (toEquip.sprite != null && toChange.sprite != null)
            {
                changeButton.GetComponent<Image>().sprite = onButton;

                changeButton.onClick.RemoveAllListeners();
                changeButton.onClick.AddListener(OnChangeIconClick);
            }
        }
    }

    void OnChangeIconClick()
    {
        if (player.AbilityInLoadout(toEquipAbility)) return;
        if (!player.EnoughGold(toEquipAbility.Price)) return;

        player.SetLoadoutByIndex(selectedIndex, toEquipAbility);
        player.ReduceGold(toEquipAbility.Price);

        // refresh UI
        toChange.sprite = player.GetFromLoadout(selectedIndex).icon;
        changeButton.onClick.RemoveAllListeners();
        changeButton.GetComponent<Image>().sprite = offButton;
        priceText.text = "Already in the loadout";
        priceText.color = new Color(0.45f, 0.45f, 0.45f); // grey

        Refresh();
        LoadoutUI.Instance.Refresh();
    }

    public void Refresh() {
        for (int i = 0; i < loadoutLayout.transform.childCount; i++)
        {
            Ability ability = player.GetFromLoadout(i);
            Button button = loadoutLayout.transform.GetChild(i).GetComponent<Button>();
            if (ability != null)
            {
                button.GetComponentInChildren<Image>().sprite = ability.icon;
            }

            button.onClick.RemoveAllListeners();

            int index = i;
            button.onClick.AddListener(() =>
            {
                OnLoadoutIconClick(index);
            });
        }

        for(int i = 0; i < ownedLayout.transform.childCount; i++)
        {
            Ability ability = player.GetFromOwnedAbility(i);
            Button button = ownedLayout.transform.GetChild(i).GetComponent<Button>();
            if (ability != null)
            {
                button.GetComponent<Image>().sprite = ability.icon;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                OnOwnedIconClick(ability);
            });
        }


    }
}
