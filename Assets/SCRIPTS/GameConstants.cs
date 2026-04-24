using System;
using UnityEngine;
public static class GameConstants 
{
        public static string SteamWishlistUrl = "https://store.steampowered.com/app/4197270/Black_Hole_Farmageddon/"; 
        public static string FeedbackUrl = "https://forms.gle/WGR6EoDaMo4JoMmH9";

        public static uint steamworksAppId = 4197270;

        public static int campaignChickensToConsume = 750;
        public static int campaignPigsToConsume = 2500;
        public static int campaignSheepToConsume = 4000        ;
        public static int campaignCowsToConsume = 10000;
        public static int campaignChicksToConsume = 100000;

        public static int maxVolatility = 50;

        // Colors
        public static Color whiteColor = Color.white;
        public static Color lightGreyColor = new Color32(197, 197, 197, 255);
        
        public static Color greenColor = new Color32(119, 255, 65, 255);
        public static Color redColor = new Color32(255, 65, 74, 255);
        
        public static Color commonTierColor = new Color32(23, 121, 8, 255);
        public static Color rareTierColor = new Color32(6, 182, 219, 255);
        public static Color epicTierColor = new Color32(163, 0, 255, 255);
        public static Color legendaryTierColor = new Color32(255, 219, 0, 255);
        
        public static bool isPlaytestBuild = true; // TODO - SET THIS TO FALSE WHEN RELEASING LIVE VERSIONS
        public static bool isDevHacksEnabled = false; // TODO - SET THIS TO FALSE WHEN RELEASING LIVE/PLAYTEST VERSIONS
}