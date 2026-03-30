using System;
using UnityEngine;
public static class GameConstants 
{
        public static string SteamWishlistUrl = "https://store.steampowered.com/app/4197270/Black_Hole_Farmageddon/"; 
        public static string FeedbackUrl = "https://forms.gle/wMvuht9Uigvaeabm9";

        public static uint steamworksAppId = 4197270;

        public static int campaignChickensToConsume = 750;
        public static int campaignPigsToConsume = 2500;
        public static int campaignSheepToConsume = 4000        ;
        public static int campaignCowsToConsume = 10000;
        public static int campaignChicksToConsume = 100000;
        
        public static bool isPlaytestBuild = false; // TODO - SET THIS TO FALSE WHEN RELEASING LIVE VERSIONS
}