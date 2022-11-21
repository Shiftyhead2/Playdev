using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    //Gamemanager controls everything related to the game such as moving forward the days, counting days till a week or a month,
    //controlling the cash flow etc.
    public static Gamemanager instance;

    [Header("Game stuff")]
    public Genre[] allGenres;
    public SubGemre[] allSubGenres;
    public Theme[] allThemes;

    private UI_manager UImanager;
    private GameCreator game;
    private EventSystem eventSystem;

    
    public int GamesPublished;

    public float currentMoney = 0f;
    [Header("Game manager stuff")]
    [HideInInspector]
    public int Day = 0;
    [HideInInspector]
    public int DaysToWeek;
    [HideInInspector]
    public int DaysToMonth;
    [HideInInspector]
    public int WeeksTotal = 0;
    public float rent = 500f;
    public float billsPercentage = 0.35f;
    [HideInInspector]
    public float LawyerPay;
    [HideInInspector]
    public bool HasLawyer;
    public string OpenGameDialogue;
    public string[] OpenGameButtonSolutions;

    [Header("Upgrade stuff")]
    public float TechicalResistance;
    public float LegalResistance;
    public float PersonalResistance;
    public ShopManager LawyerItem;

    
    public int TechicalTier;
    public int LegalTier;
    public int PersonalTier;
    public int RoomTier;

    [HideInInspector]
    public float LawyerCost;
    [HideInInspector]
    public float PersonalCost;
    [HideInInspector]
    public float TechicalCost;
    [HideInInspector]
    public int RoomCost; // in games published

    public static bool GameOver = false;
    public static bool GameStart = true;
    [HideInInspector]
    public float GameOverThreshold = -10000f;

    private Loader loader;

    

    private void Awake()
    {
        instance = this;
        UImanager = FindObjectOfType<UI_manager>();
        game = FindObjectOfType<GameCreator>();
        eventSystem = GetComponent<EventSystem>();
        loader = FindObjectOfType<Loader>();
        if (loader != null)
        {
            if (loader.GameIsLoaded)
            {
                GameStart = false;
                LoadPlayerData();
            }
            else
            {
                if (GameStart == false)
                {
                    GameStart = true;
                }
                UImanager.SetUpEventUI(OpenGameDialogue, OpenGameButtonSolutions);
            }
        }
        LoadAllGenres();
        LoadAllSubGenres();
        LoadAllThemes();
    }

    void Start()
    {

        GameOver = false;
        UImanager.UpdateGenreDropDown(allGenres);
        UImanager.UpdateThemeDropdown(allThemes);
        UImanager.UpdateDaysText(Day);
        UImanager.UpdateMoneyText(currentMoney);
        InvokeRepeating("CheckIfGameOver", 1f, 1f);
        InvokeRepeating("CheckIfHasLawyer", 1f, 1f);
    }

    private void CheckIfGameOver()
    {
        if(currentMoney <= GameOverThreshold)
        {
            GameOver = true;
            string[] solutions = { "Restart Game", "Back to menu" };
            string desc = "You went bankrupt";
            UImanager.SetUpEventUI(desc, solutions);
        }
    }

    private void CheckIfHasLawyer()
    {
        if(LawyerItem.CurrentTier == 1 && !HasLawyer)
        {
            LawyerPay = 5.0f;
            HasLawyer = true;
        }

        if (HasLawyer)
        {
            if(currentMoney < 0f)
            {
                HasLawyer = false;
                LawyerPay = 0f;
                LawyerItem.CurrentTier = 0;
                LegalTier--;
                LawyerItem.UpdateUI();
            }
        }
        
    }

    private void LoadAllGenres()
    {
        allGenres = Resources.LoadAll<Genre>("Genres");  
    }

    private void LoadAllSubGenres()
    {
        allSubGenres = Resources.LoadAll<SubGemre>("SubGenres");
    }

    private void LoadAllThemes()
    {
        allThemes = Resources.LoadAll<Theme>("Themes");
    }

    public void ForwardDay()
    {
        if (GameOver || EventSystem.EventBeingFired)
        {
            return;
        }

        if(DaysToWeek >= 7 && game.currentState == GameState.Production)
        {
            SavePlayerData();
            game.weeks++;
            game.UpdateUI();
            DaysToWeek = 0;
            WeeksTotal++;
            
        }
        else if(DaysToWeek < 7 && game.currentState == GameState.Production)
        {
            DaysToWeek++;
        }
        else if(game.currentState == GameState.Preproduction || game.currentState == GameState.Release)
        {
            DaysToWeek = 0;
        }

        if (game.currentState == GameState.Production || game.currentState == GameState.Release)
        {
            Day++;
            game.CurrentProgress++;
            game.UpdateUI();
            UImanager.UpdateDaysText(Day);
        }
        if(DaysToMonth >= 30 && (game.currentState == GameState.Production || game.currentState == GameState.Release))
        {
            //Debug.Log("Pay rent and bills");
            int cost = (int)(rent + ((billsPercentage + LawyerPay) * 100f));
            CalculateDecreasedMoney(cost, "Bills: -");
            DaysToMonth = 0;   
            SavePlayerData();
        }
        else if((game.currentState == GameState.Production || game.currentState == GameState.Release) && DaysToMonth < 30)
        {
            DaysToMonth++;
        }

        if(game.currentState == GameState.Production)
        {
            eventSystem.FireEvent();
        }
    }

    public void CalculateEarnedMoney(float decrease,string actualReason)
    {
        int random;
        string reason = actualReason;
        float DecreaseAmount = decrease * 100f;

        if (decrease == 0f)
        {
            switch (DifficultyManager.instance.diffuculty)
            {
                case GameDiffuculty.LOW:
                    random = Random.Range(1500, 3500);
                    currentMoney += random;
                    UImanager.UpdateMoneyText(currentMoney);
                    UImanager.SetCashFlowAnimation(reason, random, Color.green);
                    break;
                case GameDiffuculty.MEDIUM:
                    random = Random.Range(4000, 6500);
                    currentMoney += random;
                    UImanager.UpdateMoneyText(currentMoney);
                    UImanager.SetCashFlowAnimation(reason, random, Color.green);
                    break;
                case GameDiffuculty.HIGH:
                    random = Random.Range(7250, 12000);
                    currentMoney += random;
                    UImanager.UpdateMoneyText(currentMoney);
                    UImanager.SetCashFlowAnimation(reason, random, Color.green);
                    break;
            }

            
        }
        else
        {
            switch (DifficultyManager.instance.diffuculty)
            {
                case GameDiffuculty.LOW:
                    random = Random.Range(1500, 3500);
                    int EarnedMoney = (int)(random / DecreaseAmount);
                    currentMoney += EarnedMoney;
                    UImanager.UpdateMoneyText(currentMoney);
                    UImanager.SetCashFlowAnimation(reason, EarnedMoney, Color.green);
                    break;
                case GameDiffuculty.MEDIUM:
                    random = Random.Range(4000, 6500);
                    EarnedMoney = (int)(random / DecreaseAmount);
                    currentMoney += EarnedMoney;
                    UImanager.UpdateMoneyText(currentMoney);
                    UImanager.SetCashFlowAnimation(reason, EarnedMoney, Color.green);
                    break;
                case GameDiffuculty.HIGH:
                    random = Random.Range(7250, 12000);
                    EarnedMoney = (int)(random / DecreaseAmount);
                    currentMoney += EarnedMoney;
                    UImanager.UpdateMoneyText(currentMoney);
                    UImanager.SetCashFlowAnimation(reason, EarnedMoney, Color.green);
                    break;

            }
            
        }
    }

    public void CalculateEventEarnedMoney(int amount, string reason)
    {
        currentMoney += amount;
        UImanager.UpdateMoneyText(currentMoney);
        UImanager.SetCashFlowAnimation(reason, amount,Color.green);
    }

    public void CalculateDecreasedMoney(float decreaseAmount, string reason)
    {
        currentMoney -= decreaseAmount;
        UImanager.UpdateMoneyText(currentMoney);
        UImanager.SetCashFlowAnimation(reason, decreaseAmount,Color.red);
    }

    public void IncreaseDaysEvent(int amount)
    {
        Day += amount;
        DaysToMonth += amount;
        DaysToWeek += amount;
        if(DaysToMonth >= 30)
        {
            int cost = (int)(rent + ((billsPercentage + LawyerPay) * 100f));
            CalculateDecreasedMoney(cost, "Rent + bills: -");
            DaysToMonth = 0; 
                SavePlayerData();
        }
        if(DaysToWeek >= 7)
        {
            DaysToWeek = 0;
            WeeksTotal++;
                SavePlayerData();
        }
        UImanager.UpdateDaysText(Day);
    }

    public void ResetGameUI()
    {
        UImanager.ResetEverything();
    }

    public void UpgradeResistance(int which, int upgradeCost)
    {
            string reason = "Upgrades:-";
            CalculateDecreasedMoney(upgradeCost, reason);
            switch (which)
            {
                case 0:
                    //Techical
                    TechicalResistance += 0.1f;
                    billsPercentage += 1.0f;
                    TechicalTier++;
                    break;
                case 1:
                    //Legal
                    LegalResistance += 0.2f;
                    LegalTier++;
                    break;
                case 2:
                    //Personal
                    PersonalResistance += 0.1f;
                    PersonalTier++;
                    break;
                case 3:
                    RoomTier++;
                    rent += 500f;
                    break;

            }
    }

   

    public void SavePlayerData()
    {
        if (GameOver)
        {
            return;
        }
        else
        {
            SaveSystem.Save(this);
        }
    }

    private void LoadPlayerData()
    {
       PlayerData data = SaveSystem.LoadPlayer();
        currentMoney = data.SavedMoney;
        Day = data.SavedDay;
        WeeksTotal = data.SavedWeeks;
        rent = data.SavedRent;
        billsPercentage = data.SavedBillsPercentage;
        HasLawyer = data.SavedLawyer;
        DaysToWeek = data.SavedDaysToWeek;
        DaysToMonth = data.SavedDaysToMonth;

        LawyerCost = data.SavedLawyerCost;
        PersonalCost = data.SavedPersonalCost;
        TechicalCost = data.SavedTechicalCost;

        PersonalTier = data.SavedPersonalTier;
        PersonalResistance = data.SavedPersonalResistance;
        TechicalTier = data.SavedTechicalTier;
        TechicalResistance = data.SavedTechicalResistance;
        LegalTier = data.SavedLegalTier;
        LegalResistance = data.SavedLegalResistance;
        RoomTier = data.SavedRoomTier;
        RoomCost = data.SavedRoomCost;


        GamesPublished = data.SavedPublishedGames;
    }

    public void BackToMenu()
    {
        if (loader != null)
        {
            loader.GameIsLoaded = false;
        }
        SavePlayerData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
