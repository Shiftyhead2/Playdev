using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sub genre",menuName = "New sub genre" , order = 55)]
public class SubGemre : ScriptableObject
{
    public string Name;
    public Theme[] GoodThemes;
    public Theme[] OkThemes;
    public Theme[] BadThemes;
}
