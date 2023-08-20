using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameState
{
    Preproduction,
    Production,
    Release
}

public enum Compatibility
{
    Good,
    OK,
    Bad
}

public class GameCreator : MonoBehaviour
{
    //This class acts as a game creator and a controller of the current state of production of the said game.


    private Genre genre;
    private SubGemre subGenre;
    private Theme theme;
    private string Name;

    public GameState currentState;
    public Compatibility currentCompatibility;

    float decreaseSales;


    [SerializeField]
    private Text CompabilityText;

    [SerializeField]
    private GameObject MainUIPanel;
    [SerializeField]
    private GameObject GameCreatorPanel;
    [SerializeField]
    private Button forwardButton;
    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private Text GameNameText;

    [HideInInspector]
    public int weeks;
    [HideInInspector]
    public int weeksLeft;
    public int DaysNeeded;
    public int CurrentProgress;

    private Gamemanager GM;

    void Start()
    {
        GM = FindObjectOfType<Gamemanager>();
        Name = "";
        GameNameText.gameObject.SetActive(false);
        UpdateUI();
    }

    public void SetTheme(Theme th)
    {
        theme = th;
        CheckCompability();
        //Debug.Log("This game's current theme is " + theme.Name);
    }

    public void SetSubGenre(SubGemre sub)
    {
        subGenre = sub;
        CheckCompability();
        //Debug.Log("This game's current sub genre is " + subGenre.Name);
    }

    public void SetGenre(Genre gen)
    {
        genre = gen;
        CheckCompability();
        CompabilityText.gameObject.SetActive(true);
        CompabilityText.text = "Time needed to develop: " + genre.WeeksNeeded + " weeks";
        //Debug.Log("This game's current genre is " + gen.Name);
    }

    public void SetName(string letters)
    {
        UpdateUI();
        Name = letters;
    }

    public void CheckCompability()
    {
        if(genre == null && subGenre == null && theme == null)
        {
            Debug.LogError("Not all values are set");
        }
        else
        {
            for (int i = 0; i < subGenre.GoodThemes.Length; i++)
            {
                if(theme == subGenre.GoodThemes[i])
                {
                    //Debug.Log("This theme has good compability with this sub genre");
                    currentCompatibility = Compatibility.Good;
                }
                else
                {
                    //Debug.Log("This theme does not exist in this list");
                }
            }
            for (int i = 0; i  < subGenre.OkThemes.Length; i++)
            {
                if(theme == subGenre.OkThemes[i])
                {
                    //Debug.Log("This theme has ok compability with this sub genre");
                    currentCompatibility = Compatibility.OK;
                }
                else
                {
                    //Debug.Log("This theme does not exist in this list");
                }
            }
            for (int i = 0; i < subGenre.BadThemes.Length; i++)
            {
                if(theme == subGenre.BadThemes[i])
                {
                    //Debug.Log("This theme has bad compability with this sub genre");
                    currentCompatibility = Compatibility.Bad;
                }
                else
                {
                    //Debug.Log("This theme does not exist in this list");
                }
            }


            switch (currentCompatibility)
            {
                case Compatibility.Good:
                    decreaseSales = 0;
                    break;
                case Compatibility.Bad:
                    decreaseSales = 0.55f;
                    break;
                case Compatibility.OK:
                    decreaseSales = 0.33f;
                    break;
            }
        }
    }

    public void UpdateUI()
    {
        if (genre != null && subGenre != null && theme != null)
        {
            GameNameText.text = Name + "\n" + genre.Name + "/" + subGenre.Name + "/" + theme.Name;
        }
        if(CurrentProgress < 0)
        {
            CurrentProgress = 0;
        }

        if(CurrentProgress > DaysNeeded)
        {
            CurrentProgress = DaysNeeded;
        }

        if(currentState == GameState.Preproduction)
        {
            forwardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Create";
            progressSlider.maxValue = 0;
            progressSlider.value = 0;
        }
        else if(currentState == GameState.Production)
        {
            forwardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Work";
            progressSlider.maxValue = DaysNeeded;
            progressSlider.value = CurrentProgress;
            
        }
        else if(currentState == GameState.Release)
        {
            forwardButton.GetComponentInChildren<TextMeshProUGUI>().text = "Release";
            progressSlider.maxValue = DaysNeeded;
            progressSlider.value = CurrentProgress;
        }
    }

    public void GoForward()
    {
        if(currentState == GameState.Preproduction)
        {
            CreateGame();
        }
        else if(currentState == GameState.Production && CurrentProgress < DaysNeeded)
        {
            //Debug.Log("Working on game");
            
        }
        else if(currentState == GameState.Production && CurrentProgress >= DaysNeeded)
        {
            currentState = GameState.Release;
            //Debug.Log("Releasing game");
        }
        else if(currentState == GameState.Release)
        {
            string reason = "Sales: ";
            GM.CalculateEarnedMoney(decreaseSales,reason);
            GM.GamesPublished++;
            DifficultyManager.instance.ChangeDifficulty();
            ResetEverything();
            //Debug.Log("Game is done and selling");
        }

    }

    private void CreateGame()
    {
        MainUIPanel.SetActive(false);
        GameCreatorPanel.SetActive(true);
    }

    public void StartWorking()
    {
        if (genre == null || subGenre == null || theme == null || Name == string.Empty)
        {
            Debug.LogWarning("WTF? You cannot work on a game if you don't have the needed things!");
        }
        else
        {
            MainUIPanel.SetActive(true);
            GameCreatorPanel.SetActive(false);
            GameNameText.gameObject.SetActive(true);
            currentState = GameState.Production;
            weeksLeft = genre.WeeksNeeded;
            DaysNeeded = weeksLeft * 7;
            UpdateUI();
        }
    }
    
    public void GainWeeks(int amount)
    {
        weeks += amount;
        if (amount > 0)
        {
            CurrentProgress += amount * 7;
        }
        UpdateUI();
    }

    public void DecreaseWeeks(int amount)
    {
        weeks -= amount;
        if (amount > 0)
        {
            CurrentProgress -= amount * 7;
        }
        UpdateUI();
    }
    

    public void ResetEverything()
    {
        currentState = GameState.Preproduction;
        weeksLeft = 0;
        DaysNeeded = 0;
        CurrentProgress = 0;
        Name = "";
        genre = null;
        subGenre = null;
        theme = null;
        GameNameText.gameObject.SetActive(false);
        GM.ResetGameUI();
        UpdateUI();
    }
}
