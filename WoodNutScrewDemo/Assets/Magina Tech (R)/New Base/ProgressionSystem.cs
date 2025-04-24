using System;
using System.Collections;
using System.Collections.Generic;
using Hung.GameData;
using Hung.StatSystem;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public record QuestInfo
{
    public string type;
    public string id_target;
    public int required;
    public string prize;
}

public record QuestStat: IDataHolder<QuestData>, IEffectable, IAmount
{
    [field:SerializeField] public QuestData info { get; private set; }

    [field: SerializeField] public GameData targetData { get; private set; }

    [field: SerializeField] public Amount Amount { get; private set; }

    [field: SerializeField] public IResource Prize { get; private set; }

    public StatChanging OnStatEffected { get; set; }

    float[] IStat<Amount>.rankingValues { get; set; }

    Amount IStat<Amount>.GetStat() => Amount;

    public QuestStat(QuestData info, GameData targetData, Amount amount, IResource prize)
    {
        this.info = info;
        this.targetData = targetData;
        this.Amount = amount;
        this.Prize = prize;
    }

    public void StartQuest()
    {
        if (!info.Core.trackData)
        {
            info.Core.Reset(targetData);
            new AmountUp(-Amount.current).Apply(this);
        }       
    }

    public void GoToQuest()
    {
        info.Core.Goto(targetData);
    }
}

[CreateAssetMenu(menuName = "Hung/SO Singleton/ProgressionSystem")]
public class ProgressionSystem : SerializedScriptableObject
{
    public int index 
    {
        get => PlayerPrefs.GetInt("progression-quest", 0); 
        private set => PlayerPrefs.SetInt("progression-quest", value); 
    }

    [field: ListDrawerSettings(ShowIndexLabels = true), SerializeField] public List<QuestStat> quests { get; private set; }


    Action<int, QuestStat> _onQuestChange;
    public event Action<int, QuestStat> OnQuestChange
    {
        add 
        {
            value(index, GetQuest(index));
            _onQuestChange += value; 
        }
        remove 
        { 
            _onQuestChange -= value; 
        }
    }

    public void LoadQuests(List<QuestInfo> infos)
    {
        Debug.Log("Progression Quest: Loading");
        quests = new List<QuestStat>();
        GameData targetData;
        foreach (QuestInfo info in infos)
        {
            //Debug.Log("Try to load quest: " + info.type);
            if (DataCollection.TryGetDataByNameID(info.type, out QuestData questData))
            {
                DataCollection.TryGetDataByNameID(info.id_target, out targetData);
                var amount = new Amount(info.required);
                amount.overvalue = true;
                
                var questStat = new QuestStat(questData, targetData, amount, CurrencyParser.Parse(info.prize));
                quests.Add(questStat);
                             
                //Debug.Log("Load quest: " + info.type + " successfully");
            }
        }
    }

    public void AssignQuests()
    {
        foreach(var quest in quests)
        {
            quest.info.Core.AssignListener(quest, quest.targetData);
        }
    }

    QuestStat GetQuest(int index)
    {
        if (index >= quests.Count) return null;
        return quests[index];
    }

    public void NextQuest()
    {
        index++;
        var nextQuest = GetQuest(index);
        if (nextQuest != null)
        {
            _onQuestChange?.Invoke(index, nextQuest);
            nextQuest.StartQuest();
        }
       
    }
}
