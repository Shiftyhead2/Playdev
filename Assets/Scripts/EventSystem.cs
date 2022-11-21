using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventSystem : MonoBehaviour
{
    private UI_manager UI_Manager;
    private Gamemanager gamemanager;
    private DifficultyManager difficultyManager;
    private GameCreator game;
    private BadEvent[] badEvents;
    private GoodEvent[] goodEvents;
    private GoodEvent currentGoodEvent;
    private BadEvent currentBadEvent;
    [SerializeField]
    private GameObject eventAudio;
    
    private float chance = 0f;
    [Range(0.1f, 1f)]
    [Tooltip("The lower this value is the higher are the chances of getting a good event. Opposite: the higher this value is the higher chances are of getting a bad event.")]
    public float luckFactor;
    private bool hasInteracted = false;

    public static bool EventBeingFired = false;

    private Loader loader;

    void Awake()
    {
        difficultyManager = this.gameObject.GetComponent<DifficultyManager>();
        LoadAllEvents();
    }

    void Start()
    {
        EventBeingFired = false;
        UI_Manager = FindObjectOfType<UI_manager>();
        gamemanager = FindObjectOfType<Gamemanager>();
        game = FindObjectOfType<GameCreator>();
        loader = FindObjectOfType<Loader>();
    }

    //There is a chance of an event firing every single day. That chance will increase if an event hasn't been fired by 10%. 
    //When an event fires the chance resets to 10%.
    public void FireEvent()
    {
        if (Gamemanager.GameOver)
        {
            return;
        }

        float eventChance = Random.value;
        //Debug.Log("event chance = " + (eventChance * 100f) + "%");
        bool isFired = (eventChance <= chance);
        if (isFired)
        {
            TypeOfFiredEvent();
            //Debug.Log("Event has been fired");
            chance = 0f;
        }
        else
        {
            if (chance < 1f)
            {
                chance += 0.1f;
                
                //Debug.Log("Event failed to fire , current chances to fire a event are " + currentChance + "%");
            }else if(chance >= 1f)
            {
                chance = 1f;
                
                //Debug.Log("Event failed to fire , current chances to fire a event are " + currentChance + "%");
            }
            
        }
    }

    //Which event will fire is based on the luck factor of the player. If the value is higher there is a higher chance of the player 
    //getting a bad event. If the value is lower there is a higher chance of the player getting a good event.
    //luck must be lower than the luck factor to fire a bad event and higher than the luck factor to fire a good event.
    //if luck is equal to the luck factor(which is extremely rare) then nothing happens.
    private void TypeOfFiredEvent()
    {
        if (Gamemanager.GameOver)
        {
            return;
        }

        float luck = Random.value;
        //Debug.Log("Current luck is at: " + (luck * 100f) + "% of " + (luckFactor * 100f) + "% luck factor.");
        if(luck < luckFactor)
        {
            //Debug.Log("Bad event has been fired");
            SetupBadEvent();
            EventBeingFired = true;
        }
        else if(luck > luckFactor)
        {
            //Debug.Log("Good event has been fired");
            SetupGoodEvent();
            EventBeingFired = true;
        }
        else if(luck == luckFactor)
        {
            //Debug.Log("Nothing happened on that day. Returning");
            return;
        }
        eventAudio.GetComponent<AudioSource>().Play();
    }

    private void SetupGoodEvent()
    {
        currentGoodEvent = goodEvents[Random.Range(0, goodEvents.Length)];
        UI_Manager.SetUpEventUI(currentGoodEvent.description, currentGoodEvent.solutions);
    }

    private void SetupBadEvent()
    {
        currentBadEvent = badEvents[Random.Range(0, badEvents.Length)];
        UI_Manager.SetUpEventUI(currentBadEvent.description, currentBadEvent.solutions);
    }

    public void Solution(int solution)
    {
        if (Gamemanager.GameOver)
        {
            if(solution == 1)
            {
                //Restart scene
                if (loader != null)
                {
                    loader.GameIsLoaded = false;
                }
                //Gamemanager.GameOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if(solution == 2)
            {
                //Go back to menu
                if (loader != null)
                {
                    loader.GameIsLoaded = false;
                }
                //Gamemanager.GameOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }

        if (Gamemanager.GameStart)
        {
            if(solution == 1)
            {
                Gamemanager.GameStart = false;
                UI_Manager.CloseEventUI();
            }
        }
        
        if (!hasInteracted)
        {
            if (currentGoodEvent != null && solution == 1)
            {
                int GainedWeeks = Random.Range(currentGoodEvent.minWeeksGained, currentGoodEvent.maxWeeksGained);
                int GainedMoney = Random.Range(currentGoodEvent.minMoneyGained, currentGoodEvent.maxMoneyGained);
                string[] solutions = { "OK" };
                string description = currentGoodEvent.solutionTexts[0] + "You gained " + GainedWeeks + " weeks and " + GainedMoney + " money!";
                string reason = "Event: ";
                gamemanager.CalculateEventEarnedMoney(GainedMoney, reason);
                game.GainWeeks(GainedWeeks);
                UI_Manager.SetUpEventUI(description, solutions);
                currentGoodEvent = null;
                hasInteracted = true;
            }
            else if (currentGoodEvent != null && solution == 2)
            {
                string[] solutions = { "OK" };
                string description = currentGoodEvent.solutionTexts[1];
                UI_Manager.SetUpEventUI(description, solutions);
                currentGoodEvent = null;
                hasInteracted = true;
            }

            if (currentBadEvent != null && solution == 1)
            {
                float luck = Random.value;
                //Debug.Log("Chances to fight off the event are at " + (luck * 100f) + "%");
                CalculateSuccess(luck, currentBadEvent);
                if (currentBadEvent.succeded)
                {
                    string[] solutions = { "OK" };
                    string description = currentBadEvent.solutionTexts[0];
                    UI_Manager.SetUpEventUI(description, solutions);
                    currentBadEvent.ResetBools();
                    currentBadEvent = null;
                    hasInteracted = true;
                }
                else
                {
                    if (currentBadEvent.type == EventType.Personal)
                    {
                        int lostDays = Random.Range(currentBadEvent.minDaysLost, currentBadEvent.maxDaysLost);
                        string[] solutions = { "OK" };
                        string description = currentBadEvent.solutionTexts[1] + "You lost " + lostDays + " days.";
                        UI_Manager.SetUpEventUI(description, solutions);
                        gamemanager.IncreaseDaysEvent(lostDays);
                        currentBadEvent.ResetBools();
                        currentBadEvent = null;
                        hasInteracted = true;
                    }
                    else if(currentBadEvent.type == EventType.Publisher)
                    {
                        string[] solutions = { "OK" };
                        string description = currentBadEvent.solutionTexts[1];
                        UI_Manager.SetUpEventUI(description, solutions);
                        game.ResetEverything();
                        currentBadEvent.ResetBools();
                        currentBadEvent = null;
                        hasInteracted = true;
                    }
                    else
                    {
                        int LostWeeks = Random.Range(currentBadEvent.minWeeksLost, currentBadEvent.maxWeeksLost);
                        int LostMoney = Random.Range(currentBadEvent.minMoneyLost, currentBadEvent.maxMoneyLost);
                        string[] solutions = { "OK" };
                        string description = currentBadEvent.solutionTexts[1] + "You lost " + LostWeeks + " weeks and " + LostMoney + " money!";
                        string reason = "Event: -";
                        UI_Manager.SetUpEventUI(description, solutions);
                        game.DecreaseWeeks(LostWeeks);
                        gamemanager.CalculateDecreasedMoney(LostMoney, reason);
                        currentBadEvent = null;
                        hasInteracted = true;
                    }
                }
            }
            else if (currentBadEvent != null && solution == 2)
            {
                if (currentBadEvent.type == EventType.Personal)
                {
                    int lostDays = Random.Range(currentBadEvent.minDaysLost, currentBadEvent.maxDaysLost);
                    string[] solutions = { "OK" };
                    string description = currentBadEvent.solutionTexts[1] + "You lost " + lostDays + " days.";
                    UI_Manager.SetUpEventUI(description, solutions);
                    gamemanager.IncreaseDaysEvent(lostDays);
                    currentBadEvent.ResetBools();
                    currentBadEvent = null;
                    hasInteracted = true;
                }
                else if (currentBadEvent.type == EventType.Publisher)
                {
                    string[] solutions = { "OK" };
                    string description = currentBadEvent.solutionTexts[1];
                    UI_Manager.SetUpEventUI(description, solutions);
                    game.ResetEverything();
                    currentBadEvent.ResetBools();
                    currentBadEvent = null;
                    hasInteracted = true;
                }
                else
                {
                    int LostWeeks = Random.Range(currentBadEvent.minWeeksLost, currentBadEvent.maxWeeksLost);
                    int LostMoney = Random.Range(currentBadEvent.minMoneyLost, currentBadEvent.maxMoneyLost);
                    string[] solutions = { "OK" };
                    string description = currentBadEvent.solutionTexts[1] + "You lost " + LostWeeks + " weeks and " + LostMoney + " money!";
                    string reason = "Event: -";
                    UI_Manager.SetUpEventUI(description, solutions);
                    game.DecreaseWeeks(LostWeeks);
                    gamemanager.CalculateDecreasedMoney(LostMoney, reason);
                    currentBadEvent = null;
                    hasInteracted = true;
                }
            }
        }
        else if(hasInteracted && solution == 1)
        {
            UI_Manager.CloseEventUI();
            hasInteracted = false;
            EventBeingFired = false;
        }
    }

    public void LoadAllEvents()
    {
        badEvents = new BadEvent[0];
        goodEvents = new GoodEvent[0];

        switch (difficultyManager.diffuculty)
        {
            case GameDiffuculty.LOW:
                luckFactor = 0.5f;
                badEvents = Resources.LoadAll<BadEvent>("Bad Events/LOW");
                goodEvents = Resources.LoadAll<GoodEvent>("Good Events/LOW");
                break;
            case GameDiffuculty.MEDIUM:
                luckFactor = 0.6f;
                badEvents = Resources.LoadAll<BadEvent>("Bad Events/MEDIUM");
                goodEvents = Resources.LoadAll<GoodEvent>("Good Events/MEDIUM");
                break;
            case GameDiffuculty.HIGH:
                luckFactor = 0.7f;
                badEvents = Resources.LoadAll<BadEvent>("Bad Events/HIGH");
                goodEvents = Resources.LoadAll<GoodEvent>("Good Events/HIGH");
                break;
        }
        
    }

    public void CalculateSuccess(float luck, BadEvent bad)
    {
        float maxLuck = bad.chanceOfSuccess;
        //Debug.Log("Correct max luck is " + maxLuck);

        switch (bad.type)
        {
            case EventType.Malware:
                maxLuck -= Gamemanager.instance.TechicalResistance;
                //Debug.Log("Actual max luck is " + maxLuck);
                break;
            case EventType.Hacker:
                maxLuck -= Gamemanager.instance.TechicalResistance;
                //Debug.Log("Actual max luck is " + maxLuck);
                break;
            case EventType.Legal:
                maxLuck -= Gamemanager.instance.LegalResistance;
                //Debug.Log("Actual max luck is " + maxLuck);
                break;
            case EventType.Personal:
                maxLuck -= Gamemanager.instance.PersonalResistance;
                //Debug.Log("Actual max luck is " + maxLuck);
                break;
        }

        if (luck < maxLuck)
        {
            bad.succeded = false;
        }
        else if (luck >= maxLuck)
        {
            bad.succeded = true;
        }
    }
}
