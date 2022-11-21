using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int SavedDay;
    public int SavedWeeks;
    public float SavedRent;
    public float SavedBillsPercentage;
    public bool SavedLawyer;
    public float SavedMoney;
    public int SavedDaysToWeek;
    public int SavedDaysToMonth;

    public float SavedLawyerCost;
    public float SavedPersonalCost;
    public float SavedTechicalCost;


    public int SavedTechicalTier;
    public float SavedTechicalResistance;
    public int SavedLegalTier;
    public float SavedLegalResistance;
    public int SavedPersonalTier;
    public float SavedPersonalResistance;
    public int SavedRoomTier;

    public int SavedPublishedGames;
    public int SavedRoomCost;


    public PlayerData(Gamemanager gamemanager)
    {
        SavedDay = gamemanager.Day;
        SavedWeeks = gamemanager.WeeksTotal;
        SavedRent = gamemanager.rent;
        SavedBillsPercentage = gamemanager.billsPercentage;
        SavedLawyer = gamemanager.HasLawyer;
        SavedMoney = gamemanager.currentMoney;
        SavedDaysToWeek = gamemanager.DaysToWeek;
        SavedDaysToMonth = gamemanager.DaysToMonth;

        SavedLawyerCost = gamemanager.LawyerCost;
        SavedPersonalCost = gamemanager.PersonalCost;
        SavedTechicalCost = gamemanager.TechicalCost;

        SavedTechicalTier = gamemanager.TechicalTier;
        SavedTechicalResistance = gamemanager.TechicalResistance;
        SavedLegalTier = gamemanager.LegalTier;
        SavedLegalResistance = gamemanager.LegalResistance;
        SavedPersonalTier = gamemanager.PersonalTier;
        SavedPersonalResistance = gamemanager.PersonalResistance;
        SavedRoomTier = gamemanager.RoomTier;

        SavedPublishedGames = gamemanager.GamesPublished;
        SavedRoomCost = gamemanager.RoomCost;

       }
         
}
