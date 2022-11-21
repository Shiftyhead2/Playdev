using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Techical,
    Personal,
    Room
}

public class ModelChanger : MonoBehaviour
{
    public UpgradeType type;
    public GameObject[] Objects;

    void Start()
    {
        CheckUpgrade();
    }


   public void CheckUpgrade()
    {
        switch (type)
        {
            case UpgradeType.Techical:
                if(Gamemanager.instance.TechicalTier == 1)
                {
                    Objects[0].SetActive(true);
                    Objects[1].SetActive(false);
                    Objects[2].SetActive(false);
                    Objects[3].SetActive(false);
                }else if(Gamemanager.instance.TechicalTier == 2)
                {
                    Objects[0].SetActive(false);
                    Objects[1].SetActive(true);
                    Objects[2].SetActive(false);
                    Objects[3].SetActive(false);
                }
                else if(Gamemanager.instance.TechicalTier == 3)
                {
                    Objects[0].SetActive(false);
                    Objects[1].SetActive(false);
                    Objects[2].SetActive(true);
                    Objects[3].SetActive(false);
                }
                else if (Gamemanager.instance.TechicalTier == 4)
                {
                    Objects[0].SetActive(false);
                    Objects[1].SetActive(false);
                    Objects[2].SetActive(false);
                    Objects[3].SetActive(true);
                }
                break;
           case UpgradeType.Room:
                if (Gamemanager.instance.RoomTier == 1)
                {
                    Objects[0].SetActive(true);
                    if (Objects.Length > 1)
                    {
                        Objects[1].SetActive(false);
                        if (Objects.Length > 2)
                        {
                            Objects[2].SetActive(false);
                        }
                        if (Objects.Length > 3)
                        {
                            Objects[3].SetActive(false);
                        }
                    }
                }
                else if (Gamemanager.instance.RoomTier == 2)
                {
                    Objects[0].SetActive(false);
                    if (Objects.Length > 2)
                    {
                        Objects[1].SetActive(true);
                        Objects[2].SetActive(false);
                        if (Objects.Length > 3)
                        {
                            Objects[3].SetActive(false);
                        }
                    }
                }
                else if (Gamemanager.instance.RoomTier == 3)
                {
                    Objects[0].SetActive(false);
                    if (Objects.Length > 3)
                    {
                        Objects[1].SetActive(false);
                        Objects[2].SetActive(true);
                        Objects[3].SetActive(false);
                    }
                }
                else if (Gamemanager.instance.RoomTier == 4)
                {
                    Objects[0].SetActive(false);
                    if (Objects.Length > 1)
                    {
                        Objects[1].SetActive(false);
                        if (Objects.Length > 2)
                        {
                            Objects[2].SetActive(false);
                        }
                        if (Objects.Length > 3)
                        {
                            Objects[3].SetActive(true);
                        }
                    }
                }
                break;
            case UpgradeType.Personal:
                if (Gamemanager.instance.PersonalTier == 1)
                {
                    Objects[0].SetActive(true);
                    Objects[1].SetActive(false);
                    Objects[2].SetActive(false);
                    if (Objects.Length > 3)
                    {
                        Objects[3].SetActive(false);
                    }
                }
                else if (Gamemanager.instance.PersonalTier == 2)
                {
                    Objects[0].SetActive(false);
                    Objects[1].SetActive(true);
                    Objects[2].SetActive(false);
                    if (Objects.Length > 3)
                    {
                        Objects[3].SetActive(false);
                    }
                }
                else if (Gamemanager.instance.PersonalTier == 3)
                {
                    Objects[0].SetActive(false);
                    Objects[1].SetActive(false);
                    Objects[2].SetActive(true);
                    if (Objects.Length > 3)
                    {
                        Objects[3].SetActive(false);
                    }
                }
                else if (Gamemanager.instance.PersonalTier == 4)
                {
                    Objects[0].SetActive(false);
                    Objects[1].SetActive(false);
                    if (Objects.Length > 3)
                    {
                        Objects[2].SetActive(false);
                        Objects[3].SetActive(true);
                    }
                    
                }
                break;



        }
    }
}
