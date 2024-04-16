using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private const float inputGracePeriod = 2f;

    private string currentString;
    private List<CheatInstance> cheatList;
    private float lastInputTime;
    private bool isTyping;

    [SerializeField] private GameObject cheatPopupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentString = "";
        cheatList = new List<CheatInstance>
        {
            new("njus", AddGold),
            new("tobsttpa", UpLevel),
            new("depdepdep", UpSpeed),
            new("dmg", ()  => { PlayerData.Instance.ReduceHealth(10000); })
        };
    }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            currentString += c;
            lastInputTime = Time.time;
            isTyping = true;

            if (CheckCheat(currentString))
            {
                currentString = "";
                return;
            }
        }

        if(isTyping && Time.time - inputGracePeriod > lastInputTime)
        {
            currentString = "";
            isTyping = false;
        }
    }

    private bool CheckCheat(string input)
    {
        bool clearInput = true;
        foreach (CheatInstance cheat in cheatList)
        {
            if(input.Equals(cheat.Keyword))
            {
                cheat.CheatEffect();
                ManagePopup(cheat.Keyword);
                return true;
            }
            else if(cheat.Keyword.StartsWith(input))
            {
                clearInput = false;
            }
        }

        if(clearInput) currentString = "";

        return false;
    }

    private void ManagePopup(string keyword)
    {
        TextMeshProUGUI textComponent = GetTextComponent("Cheatcode");
        if (textComponent != null) textComponent.text = keyword.ToUpper();

        GameObject popup = Instantiate(cheatPopupPrefab);

        //await Task.Delay(3000);
        
        Destroy(popup, 3f);
    }

    private TextMeshProUGUI GetTextComponent(string name)
    {
        TextMeshProUGUI[] textComponents = cheatPopupPrefab.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI c in textComponents)
        {
            if(c.name == name)
            {
                return c;
            }
        }

        return null;
    }

    // cheat effects
    private void AddGold()
    {
        long gold = 1000000;
        PlayerData.Instance.AddGold(gold);
    }

    private void UpLevel()
    {
        long xp = 1000000;
        PlayerData.Instance.AddExperience(xp);
    }

    private void UpSpeed()
    {
        Player.Instance.CheatSpeed();
    }
}
