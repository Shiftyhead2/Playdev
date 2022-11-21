using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_manager : MonoBehaviour
{
    //This class controls the UI. 

    [Header("Game creator UI")]
    [SerializeField]
    private TMP_Dropdown GenreDropDown;
    [SerializeField]
    private TMP_Dropdown SubGenreDropDown;
    [SerializeField]
    private TMP_Dropdown ThemeDropDown;
    [Header("Game UI")]
    [SerializeField]
    private TextMeshProUGUI DaysText;
    [SerializeField]
    private TextMeshProUGUI MoneyText;
    [SerializeField]
    private InputField NameInputField;
    [SerializeField]
    private Animator CashFlowAnimator;
    [SerializeField]
    private Text CashFlowReasonText;
    [SerializeField]
    private Image CashFlowImage;
    [SerializeField]
    private Sprite IncomeSprite;
    [SerializeField]
    private Sprite OutComeSprite;
   

    [Header("Event UI")]
    #region Events
    [SerializeField]
    private GameObject EventUI;
    [SerializeField]
    private Text EventDescText;
    [SerializeField]
    private Button[] ReactionButtons;
    [SerializeField]
    private Button ForwardDayButton;
    #endregion

    private SubGemre[] subs;

    private Gamemanager GM;
    private GameCreator game;

    private int CurrentGenre;
    private int CurrentSubGenre;
    private int CurrentTheme;

    void Start()
    {

        GM = FindObjectOfType<Gamemanager>();
        game = FindObjectOfType<GameCreator>();
        ForwardDayButton.interactable = true;
        CheckCurrentGenre(0);
        CheckCurrentTheme(0);
    }

    public void UpdateGenreDropDown(Genre[] genres)
    {
        GenreDropDown.ClearOptions();

        List<string> listGenres = new List<string>();
        for (int i = 0; i < genres.Length; i++)
        {
            string name = genres[i].Name;
            listGenres.Add(name);
        }
        GenreDropDown.AddOptions(listGenres);
        GenreDropDown.RefreshShownValue();
    }

    private void UpdateSubGenreDropDown(SubGemre[] subGenres)
    {
        //Debug.Log("Updating sub genres");
        SubGenreDropDown.ClearOptions();

        List<string> listSubGenres = new List<string>();
        for (int i = 0; i < subGenres.Length; i++)
        {
            subs = subGenres;
            string name = subGenres[i].Name;
            listSubGenres.Add(name);
        }
        SubGenreDropDown.AddOptions(listSubGenres);
        SubGenreDropDown.RefreshShownValue();
    }

    public void UpdateThemeDropdown(Theme[] themes)
    {
        ThemeDropDown.ClearOptions();

        List<string> listThemes = new List<string>();
        for (int i = 0; i < themes.Length; i++)
        {
            string name = themes[i].Name;
            listThemes.Add(name);
        }
        ThemeDropDown.AddOptions(listThemes);
        ThemeDropDown.RefreshShownValue();
    }

    public void CheckCurrentGenre(int CurrentIndex)
    {
        CurrentGenre = CurrentIndex;
        GenreDropDown.value = CurrentIndex;
        UpdateSubGenreDropDown(GM.allGenres[CurrentGenre].SubGenres);
        CheckCurrentSubGenre(CurrentGenre);
        game.SetGenre(GM.allGenres[CurrentGenre]);

        GenreDropDown.RefreshShownValue();
    }

    public void CheckCurrentTheme(int CurrentIndex)
    {
        CurrentTheme = CurrentIndex;
        ThemeDropDown.value = CurrentIndex;
        game.SetTheme(GM.allThemes[CurrentTheme]);
        ThemeDropDown.RefreshShownValue();
    }

    public void CheckCurrentSubGenre(int CurrentIndex)
    { 
        CurrentSubGenre = CurrentIndex;
        SubGenreDropDown.value = CurrentIndex;
        game.SetSubGenre(subs[CurrentSubGenre]);
        SubGenreDropDown.RefreshShownValue();
    }

    public void UpdateDaysText(int day)
    {
        DaysText.text = day.ToString();
    }



    public void UpdateMoneyText(float money)
    {
        if (money < 1000)
        {
            MoneyText.text = string.Format("{0:0}", money);
        } else if (money >= 1000 && money < 1000000)
        {
            money = money / 1000;
            MoneyText.text = string.Format("{0:0.0}", money) + "k";
        } else if(money >= 1000000 && money < 1000000000)
        {
            money = money / 1000000;
            MoneyText.text = string.Format("{0:0.0}", money) + "m";
        } else if(money >= 1000000000)
        {
            money = money / 1000000000;
            MoneyText.text = string.Format("{0:0.0}", money) + "b";
        }

        if(money < -1000)
        {
            money = money / 1000;
            MoneyText.text = string.Format("{0:0.0}", money) + "k";
        }

    }

    public void UpdateGameName()
    {
        game.SetName(NameInputField.text);
    }

    public void ResetEverything()
    {
        NameInputField.text = string.Empty;
        CheckCurrentGenre(0);
        CheckCurrentSubGenre(0);
        CheckCurrentTheme(0);
    }

    public void SetCashFlowAnimation(string reason, float cashFlow, Color textColor)
    {
        if(textColor.Compare(Color.green))
        {
            CashFlowImage.overrideSprite = IncomeSprite;
        }else if (textColor.Compare(Color.red))
        {
            CashFlowImage.overrideSprite = OutComeSprite;
        }

        if (cashFlow != 0)
        {
            CashFlowAnimator.CrossFadeInFixedTime("CashPopUp_Open", 0.01f);
            CashFlowReasonText.text = reason + "" + string.Format("{00:0}", cashFlow) + "$";
            CashFlowAnimator.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    public void SetUpEventUI(string desc, string[] solutions)
    {
        EventUI.SetActive(true);
        EventDescText.text = desc;
        for (int i = 0; i < solutions.Length; i++)
        {
            ReactionButtons[i].transform.parent.gameObject.SetActive(true);
            ReactionButtons[i].GetComponent<TextMeshProUGUI>().text = solutions[i];
        }

        for (int i = solutions.Length; i < ReactionButtons.Length; i++)
        {
            ReactionButtons[i].transform.parent.gameObject.SetActive(false);
        }
        ForwardDayButton.interactable = false;
     }

    public void CloseEventUI()
    {
        EventUI.SetActive(false);
        if (Gamemanager.GameOver != true)
        {
            ForwardDayButton.interactable = true;
        }
    }
}
