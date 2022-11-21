using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameDiffuculty
{
    LOW,
    MEDIUM,
    HIGH
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public GameDiffuculty diffuculty;
    private EventSystem eventSystem;

    void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        eventSystem = this.gameObject.GetComponent<EventSystem>();
        ChangeDifficulty();
    }

    public void ChangeDifficulty()
    {
        if(Gamemanager.instance.GamesPublished < 6)
        {
            diffuculty = GameDiffuculty.LOW;
        }
        else if(Gamemanager.instance.GamesPublished >= 6 && Gamemanager.instance.GamesPublished < 15)
        {
            diffuculty = GameDiffuculty.MEDIUM;
        }
        else if(Gamemanager.instance.GamesPublished >= 15)
        {
            diffuculty = GameDiffuculty.HIGH;
        }

        eventSystem.LoadAllEvents();
    }
}
