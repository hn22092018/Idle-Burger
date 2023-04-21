using SDK;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class QuestManager {
    List<Quest> listQuestsAvaiable = new List<Quest>();//
    public List<int> claimedID = new List<int>();
    public List<int> claimableID = new List<int>();
    [HideInInspector]
    public bool hasQuestClaimable = false;
    int IndexQuest;

    public void InitQuest(List<Quest> questsAvaiable) {
        IndexQuest = 0;
        listQuestsAvaiable = new List<Quest>(questsAvaiable);
        listQuestsAvaiable.Sort(SortQuestComparer);
        for (int i = listQuestsAvaiable.Count - 1; i >= 0; i--) {
            for (int j = 0; j < claimedID.Count; j++) {
                if (listQuestsAvaiable[i].questID == claimedID[j]) {
                    listQuestsAvaiable.Remove(listQuestsAvaiable[i]);
                    break;
                }
            }
        }
    }
    public int SortQuestComparer(Quest quest1, Quest quest2) {
        if (quest1.priority < quest2.priority) {
            return -1;
        } else if (quest1.priority > quest2.priority) {
            return 1;
        } else {
            if (quest1.questID < quest2.questID) {
                return -1;
            } else {
                return 1;
            }
        }
    }

    public Quest GetNextQuests() {
        if (listQuestsAvaiable.Count > IndexQuest) {
            IndexQuest++;
            if (!hasQuestClaimable) hasQuestClaimable = IsQuestClaimable(listQuestsAvaiable[IndexQuest - 1]);
            return listQuestsAvaiable[IndexQuest - 1];
        }
        return null;
    }

    /// <summary>
    /// Trigger Quest Has Done But Not Claim
    /// </summary>
    /// <param name="type"></param>
    /// <param name="level"></param>
    public void TriggerQuest(QuestType type, int level) {
        // Check in all quest list;
        for (int i = 0; i < listQuestsAvaiable.Count; i++) {
            if (listQuestsAvaiable[i].type == type
            && listQuestsAvaiable[i].level == level) {
                // Add this quest to done quest
                if (!claimableID.Contains(listQuestsAvaiable[i].questID)) {
                    claimableID.Add(listQuestsAvaiable[i].questID);
                    ProfileManager.PlayerData.IsMarkChangeQuest();
                    break;
                }
            }
        }

        if (!hasQuestClaimable) CheckAnyQuestClaimable();
    }

    /// <summary>
    /// Check Has Any Current Active Quest Enable Claim , If True Turn On Flag .
    /// </summary>
    public void CheckAnyQuestClaimable() {
        hasQuestClaimable = false;
        if (IndexQuest == 0) {
            for (int i = 0; i < 3; i++) {
                if (i < listQuestsAvaiable.Count) {
                    bool isDone = IsQuestClaimable(listQuestsAvaiable[i]);
                    bool isClaimed = IsQuestClaimed(listQuestsAvaiable[i]);
                    if (isDone && !isClaimed) {
                        hasQuestClaimable = true;
                        break;
                    }
                }

            }
        } else {
            for (int i = 0; i < IndexQuest; i++) {
                if (i < listQuestsAvaiable.Count) {
                    bool isDone = IsQuestClaimable(listQuestsAvaiable[i]);
                    bool isClaimed = IsQuestClaimed(listQuestsAvaiable[i]);
                    if (isDone && !isClaimed) {
                        hasQuestClaimable = true;
                        break;
                    }
                }

            }
        }
        
    }
 
    /// <summary>
    /// Claim quest. remove it in allQuestDoneList/currentActiveQuest/allQuests
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public void ClaimQuest(Quest quest) {
        // get reward
        if (quest.reward.type == ItemType.Gem) {
            ProfileManager.PlayerData.AddGem(quest.reward.amount);
            ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Earn_Quest, quest.reward.amount);
        } else if (quest.reward.type == ItemType.Cash) {
            ProfileManager.PlayerData.AddCash((float)quest.reward.amount);
        }
        if (!claimedID.Contains(quest.questID)) claimedID.Add(quest.questID);
        if (claimableID.Contains(quest.questID)) claimableID.Remove(quest.questID);
        CheckAnyQuestClaimable();
        ProfileManager.PlayerData.IsMarkChangeQuest();
    }

    public bool IsQuestClaimed(Quest quest) {
        for (int j = 0; j < claimedID.Count; j++) {
            if (claimedID[j] == quest.questID) {
                return true;
            }
        }
        return false;
    }
    public bool IsQuestClaimable(Quest quest) {
        for (int j = 0; j < claimableID.Count; j++) {
            if (claimableID[j] == quest.questID) {
                return true;
            }
        }
        return false;
    }

}
