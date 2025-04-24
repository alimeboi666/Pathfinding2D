using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Scripting;

[Preserve]
public static class PlayerPersistentData
{
    private const string PREFIX_OwnedState = "OwnedState-";
    private const string PREFIX_Skin = "Skin";
    public const string LAST_APP_OPENED_TIME = "last-app-opened-time";
    public const string TODAY_ONLINE_TIME_SPENT = "today-online-time-spent";
    private const string FIRST_TIME_LOGIN = "first-login-time";
    private const string LAST_ONLINE_RESET_TIME = "last-online-reset-time";
    
    private static int GetIntFromList(string key, int index, int firstGivenNumber = 1)
    {
        string[] ss = PlayerPrefs.GetString(key, firstGivenNumber + "-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0").Split('-');
        return int.Parse(ss[index]);
    }
    
    private static void SetIntToList(string key, int index, int changedValue, int firstGivenNumber = 1)
    {
        string[] ss = PlayerPrefs.GetString(key, firstGivenNumber + "-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0-0").Split('-');
        ss[index] = changedValue.ToString();
        PlayerPrefs.SetString(key, string.Join("-", ss));
    }

    public static void Set_LastOnlineRewardResetTime() {
        var time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        PlayerPrefs.SetString(LAST_ONLINE_RESET_TIME, time);
        PlayerPrefs.Save();
    }
    
    public static DateTime Get_LastOnlineRewardResetTime() {
        if (!PlayerPrefs.HasKey(LAST_ONLINE_RESET_TIME))
        {
            // If there's no record of last reward collection, the reward is available
            Set_LastOnlineRewardResetTime();
            return DateTime.Now;
        }
        
        DateTime lastRewardDate = DateTime.Parse(PlayerPrefs.GetString(LAST_ONLINE_RESET_TIME));
        return lastRewardDate;
    }
    
    public static DateTime GetNextMidnight(DateTime dateTime)
    {
        // Add one day to the current date and set the time to 00:00:00
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(1);
    }
    
    /// <summary>
    /// returns if it's passed midnight into the next day since the player has finished their onine rewards
    /// </summary>
    /// <returns>can online reward</returns>
    public static bool Get_IsOnlineRewardAvailable() {
        if (!PlayerPrefs.HasKey(LAST_ONLINE_RESET_TIME))
        {
            // If there's no record of last reward collection, the reward is available
            return true;
        }

        DateTime lastRewardDate = DateTime.Parse(PlayerPrefs.GetString(LAST_ONLINE_RESET_TIME));
        DateTime nextDate = GetNextMidnight(lastRewardDate);

        // Check if the current date is different from the last reward collection date
        // var shouldReset = (today - lastRewardDate).TotalSeconds > 25;
        var shouldReset = (DateTime.Now - nextDate).TotalSeconds > 1f;
        return shouldReset;
    }
    
    public static void Set_TodayOnlineTimeSpent(float totalTime) {
        // totalTime += Get_TotalTimeSpent();
        PlayerPrefs.SetFloat(TODAY_ONLINE_TIME_SPENT, totalTime);
        PlayerPrefs.Save();
    }

    public static float Get_TodayOnlineTimeSpent() {
        return PlayerPrefs.GetFloat(TODAY_ONLINE_TIME_SPENT, 0);
        //PlayerPrefs.Save();
    }
    
    public static void Set_LastAppOpenedTime() {
        Debug.Log($"saving last time played");
        var savedTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        PlayerPrefs.SetString(LAST_APP_OPENED_TIME, savedTime);
        Debug.Log($"Set last played time: {savedTime}");
    }

    public static DateTime Get_LastAppOpenedTime() {
        Debug.Log($"getting last time played");
        string lastPlayedTimeStr = PlayerPrefs.GetString(LAST_APP_OPENED_TIME, "2023-08-10T00:00:00");
        Debug.Log($"Retrieved last played time string: {lastPlayedTimeStr}");

        if (DateTime.TryParseExact(lastPlayedTimeStr, "yyyy-MM-ddTHH:mm:ss", null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime lastPlayedTime))
        {
            Debug.Log($"Parsed last played time: {lastPlayedTime}");
            return lastPlayedTime;
        }
        else
        {
            Debug.LogError($"Failed to parse the last played time. String: {lastPlayedTimeStr}");
            return new DateTime(2023, 8, 10, 0, 0, 0); // Default value if parsing fails
        }
    }

    public static bool Get_IsFirstTimePlaying() {
        var isFirstTimePlaying = !PlayerPrefs.HasKey(LAST_APP_OPENED_TIME);
        if (isFirstTimePlaying) {
            PlayerPrefs.SetString(FIRST_TIME_LOGIN, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        return isFirstTimePlaying;
    }
    
    public static Switch Get_SettingOption(SettingOption settingOption)
    {
        return (Switch)PlayerPrefs.GetInt(settingOption.ToString(), 1);
    }

    public static void Set_SettingOption(SettingOption settingOption, Switch set)
    {
        PlayerPrefs.SetInt(settingOption.ToString(), (int) set);
    }

    public static int MaxLevel
    {
        get => PlayerPrefs.GetInt(nameof(MaxLevel), 0);

        set => PlayerPrefs.SetInt(nameof(MaxLevel), value);
    }

    public static OwnedState Get_OwnedStateSkinShop(int id)
    {
        return (OwnedState)GetIntFromList(PREFIX_OwnedState + PREFIX_Skin, id, 2);
    }

    public static void Set_OwnedStateSkinShop(int id, OwnedState ownedState)
    {
        SetIntToList(PREFIX_OwnedState + PREFIX_Skin, id, (int) ownedState, 2);
    }

    public static int Skin
    {
        get => PlayerPrefs.GetInt(PREFIX_Skin, 0);

        set => PlayerPrefs.SetInt(PREFIX_Skin, value);
    }
}
