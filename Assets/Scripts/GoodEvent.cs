using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Good Event", menuName = "New good event" , order = 60)]
public class GoodEvent : ScriptableObject
{
    public string description;
    public string[] solutions;
    public string[] solutionTexts;

    public int minWeeksGained;
    public int maxWeeksGained;
    public int minMoneyGained;
    public int maxMoneyGained;
}
