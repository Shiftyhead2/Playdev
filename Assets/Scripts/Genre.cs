using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Genre",menuName = "New genre" , order = 60)]
public class Genre : ScriptableObject
{
    public string Name;
    public SubGemre[] SubGenres;
    public int WeeksNeeded;
}
