using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ShopType
{
    Room,
    Item
}

public class ShopManager : MonoBehaviour
{
    [Header("Items")]
    [SerializeField]
    private int Cost;
    [SerializeField]
    private int MaxGamesNeeded;
    public int CurrentTier;
    [SerializeField]
    private int NeededRoomTier;
    [SerializeField]
    private Button BuyButton;
    [SerializeField]
    private int MaxTier;
    [SerializeField]
    private int WhichItem;
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI CostText;
    [SerializeField]
    private TextMeshProUGUI TierText;
    [SerializeField]
    private ShopManager[] Items;

    private Loader loader;
    [SerializeField]
    private GameObject WhichModel;
    public ModelChanger[] modelChanger;

    [SerializeField]
    private ShopType type;

    private void Awake()
    {
        loader = FindObjectOfType<Loader>();
        
    }


    void Start()
    {
        if (loader != null)
        {
            if (loader.GameIsLoaded)
            {
                LoadSavedData();
            }
            else
            {
                switch (WhichItem)
                {
                    case 0:
                        Cost = 3500;
                        Gamemanager.instance.TechicalCost = Cost;
                        break;
                    case 1:
                        Cost = 30000;
                        Gamemanager.instance.LawyerCost = Cost;
                        break;
                    case 2:
                        Cost = 2500;
                        Gamemanager.instance.PersonalCost = Cost;
                        break;
                    case 3:
                        MaxGamesNeeded = 3;
                        Gamemanager.instance.RoomCost = MaxGamesNeeded;
                        break;
                }

            }
        }
        else
        {
            switch (WhichItem)
            {
                case 0:
                    Cost = 3500;
                    Gamemanager.instance.TechicalCost = Cost;
                    break;
                case 1:
                    Cost = 30000;
                    Gamemanager.instance.LawyerCost = Cost;
                    break;
                case 2:
                    Cost = 2500;
                    Gamemanager.instance.PersonalCost = Cost;
                    break;
                case 3:
                    MaxGamesNeeded = 3;
                    Gamemanager.instance.RoomCost = MaxGamesNeeded;
                    break;
            }
        }

        switch (WhichItem)
        {
            case 0:
                CheckUpgrades();
                break;
            case 2:
                CheckUpgrades();
                break;
        }
        UpdateUI();
    }

    private void OnEnable()
    {
        switch (WhichItem)
        {
            case 0:
                CheckUpgrades();
                break;
            case 2:
                CheckUpgrades();
                break;
        }
        UpdateUI();
    }

    public void BuyItem()
    {
        if(CurrentTier >= MaxTier)
        {
            return;
        }
        else
        {
            if (Gamemanager.instance.currentMoney < Cost || Gamemanager.instance.GamesPublished < MaxGamesNeeded)
            {
                //Debug.Log("Not enough money to buy this item or not enough games published");
            }
            else
            {
                Gamemanager.instance.UpgradeResistance(WhichItem, Cost);
                CurrentTier++;
                for (int i = 0; i < modelChanger.Length; i++)
                {
                    modelChanger[i].CheckUpgrade();
                }
                switch (WhichItem)
                {
                    case 0:
                        Cost += 5000 * CurrentTier;
                        Gamemanager.instance.TechicalCost = Cost;
                        CheckUpgrades();
                        break;
                    case 2:
                        Cost += 500 * CurrentTier;
                        Gamemanager.instance.PersonalCost = Cost;
                        CheckUpgrades();
                        break;
                    case 3:
                        MaxGamesNeeded += 3 * CurrentTier;
                        Gamemanager.instance.RoomCost = MaxGamesNeeded;
                        for (int i = 0; i < Items.Length; i++)
                        {
                            Items[i].CheckUpgrades();
                            Items[i].UpdateUI();
                        }
                        break;
                    

                }
            }
            //UIPanel.SetActive(false);
            //ForwardButton.interactable = true;
            
        }
        UpdateUI();
    }

    public void UpdateUI()
    {
        switch (type)
        {
            case ShopType.Item:
                if(Gamemanager.instance.RoomTier >= NeededRoomTier)
                {
                    BuyButton.interactable = true;
                    if (CurrentTier < MaxTier)
                    {
                        CostText.text = Cost + "$";
                    }
                    else if (CurrentTier >= MaxTier)
                    {
                        CostText.text = "Max tier achieved";
                    }
                }
                else
                {
                    CostText.text = "Room tier " + NeededRoomTier + " required";
                    BuyButton.interactable = false;
                }
                break;
            case ShopType.Room:
                if (CurrentTier < MaxTier)
                {
                    if (Gamemanager.instance.GamesPublished < MaxGamesNeeded)
                    {
                        CostText.text = "Games: " + Gamemanager.instance.GamesPublished + " / " + MaxGamesNeeded;
                    }
                    else if(Gamemanager.instance.GamesPublished >= MaxGamesNeeded)
                    {
                        CostText.text = "Buy upgrade";
                    }
                }
                else if (CurrentTier >= MaxTier)
                {
                    CostText.text = "Max tier achieved";
                }
                break;

        }
        
        TierText.text = CurrentTier + "/" + MaxTier;
    }

    public void CheckUpgrades()
    {
        switch (CurrentTier)
        {
            case 0:
                NeededRoomTier = 1;
                break;
            case 1:
                NeededRoomTier = 2;
                break;
            case 2:
                NeededRoomTier = 3;
                break;
            case 3:
                NeededRoomTier = 4;
                break;
        }
    }

    private void LoadSavedData()
    {
        switch (WhichItem)
        {
            case 0:
                CurrentTier = Gamemanager.instance.TechicalTier;
                if (Gamemanager.instance.TechicalCost != 0)
                {
                    Cost = (int)Gamemanager.instance.TechicalCost;
                }
                else
                {
                    Cost = 3500;
                }
                break;
            case 1:
                CurrentTier = Gamemanager.instance.LegalTier;
                if (Gamemanager.instance.LawyerCost != 0)
                {
                    Cost = (int)Gamemanager.instance.LawyerCost;
                }
                else
                {
                    Cost = 30000;
                }
                break;
            case 2:
                CurrentTier = Gamemanager.instance.PersonalTier;
                if (Gamemanager.instance.PersonalCost != 0)
                {
                    Cost = (int)Gamemanager.instance.PersonalCost;
                }
                else
                {
                    Cost = 2500;
                }
                break;
            case 3:
                CurrentTier = Gamemanager.instance.RoomTier;
                if (Gamemanager.instance.RoomCost != 0)
                {
                    MaxGamesNeeded = Gamemanager.instance.RoomCost;
                }
                else
                {
                    MaxGamesNeeded = 3;
                }
                break;
        }
    }

    
}
