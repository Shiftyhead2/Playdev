using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    Malware,
    Hacker,
    Legal,
    Personal,
    Publisher
}

[CreateAssetMenu(fileName = "Bad Event", menuName = "New bad event" , order = 65)]
public class BadEvent : ScriptableObject
{
    public string description;
    public string[] solutions;
    public string[] solutionTexts;
    public bool succeded = false;
    public EventType type;

    [Range(0.1f, 1f)]
    public float chanceOfSuccess;
    float CorrectChance;

    public int minWeeksLost;
    public int maxWeeksLost;
    public int minMoneyLost;
    public int maxMoneyLost;
    public int minDaysLost;
    public int maxDaysLost;

    public void ResetBools()
    {
        succeded = false;
    }
}
