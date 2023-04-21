using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "TutorialConfig", menuName = "ScriptableObjects/TutorialConfig", order = 1)]
public class TutorialConfig : ScriptableObject {
    public List<TutorialStep> steps;
    public void Load() {
        foreach (var step in steps) {
            step.CheckFinish();
        }
    }
    public List<TutorialStep> GetTutorialSteps() {
        return new List<TutorialStep>(steps.Where(x => !x.IsFinish).ToList());
    }
    public List<TutorialStep> GetTutorialStepsDefault() {
        return new List<TutorialStep>(steps);
    }

}
[System.Serializable]
public class TutorialStep {
    [GUIColor(1, 0.8f, 0, 1)]
    public int ID;
    public TutorialStepID stepID;
    public int levelRequire;
    public List<int> textLocalizeIds;
    public bool IsTutNonUI;
    [ShowIf("IsTutNonUI")]
    public Vector3 cameraPos;
    [ShowIf("IsTutNonUI")]
    public Vector3 blockerCenterPos;
    [ReadOnly]
    public bool IsFinish;
    public void CheckFinish() {
        if (levelRequire != ProfileManager.PlayerData.GetSelectedWorld()) return;
        IsFinish = PlayerPrefs.GetInt("FinishTutorial_" + stepID + "_" + ID) == 1;
        switch (stepID) {
            case TutorialStepID.BuildTable:
                if (GameManager.instance.IsUnlockSmallTable(0)) FinishTut();
                break;
            case TutorialStepID.UpgradeTable:
                if (GameManager.instance.IsUnlockSmallTable(0) && GameManager.instance.SmallTablesRoom[0].GetLevelItem(3) > 0) {
                    FinishTut();
                }
                break;
            case TutorialStepID.BuildPower:
                if (GameManager.instance.IsUnlockPowerRoom()) {
                    FinishTut();
                }
                break;
            case TutorialStepID.BuildRestroom:
                if (GameManager.instance.IsUnlockRestroom(0)) {
                    FinishTut();
                }
                break;
          
            case TutorialStepID.ActiveMarketingCampaign:
                if (ProfileManager.PlayerData.GetMarketingManager().IsPassTutorial) {
                    FinishTut();
                }
                break;
            case TutorialStepID.AdBoost:
                if (ProfileManager.PlayerData.GetAdBoostManager().IsPassTutorial) {
                    FinishTut();
                }
                break;
            case TutorialStepID.Research:
                if (ProfileManager.PlayerData.researchManager.researchSave.Where(x => x.researchName == ResearchName.FrenchFries).ToList().Count > 1) {
                    FinishTut();
                }
                break;
        }
    }
    public void FinishTut() {
        Debug.Log("Finish Tutorial " + stepID);
        PlayerPrefs.SetInt("FinishTutorial_" + stepID + "_" + ID, 1);
        IsFinish = true;
    }
    [Button]
    public void ResetTutorial() {
        Debug.Log("Reset Tutorial " + stepID);
        PlayerPrefs.SetInt("FinishTutorial_" + stepID + "_" + ID, 0);
    }
    public bool IsEnoughConditionShow() {
        if (IsFinish) return false;
        if (stepID == TutorialStepID.Research) {
            int valueNeedToResearch = 20;
            valueNeedToResearch = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(ResearchName.FrenchFries).CalulateReseachPrice(0);
            if (ProfileManager.PlayerData.researchManager.researchSave.Where(x => x.researchName == ResearchName.FrenchFries).ToList().Count == 0 &&
                  ProfileManager.PlayerData.researchManager.researchValue >= valueNeedToResearch) {
                return true;
            }
            return false;
        }

        if (levelRequire != ProfileManager.PlayerData.GetSelectedWorld()) return false;
        BigNumber price = 0;
        int energy = 0;
        switch (stepID) {
            case TutorialStepID.BuildTable:
                price = GameManager.instance.buildData.GetBuildCashPrice(RoomID.Table1);
                if (GameManager.instance.IsEnoughCash(price) && !GameManager.instance.IsUnlockSmallTable(0))
                    return true;
                break;
            case TutorialStepID.UpgradeTable:
                if (GameManager.instance.IsUnlockSmallTable(0) && GameManager.instance.SmallTablesRoom[0].GetLevelItem(0) > 0) {
                    return false;
                }
                price = GameManager.instance.SmallTablesRoom[0].GetUpgradePriceItem(0);
                if (GameManager.instance.IsUnlockSmallTable(0) && GameManager.instance.IsEnoughCash(price)) return true;
                break;
            
            case TutorialStepID.BuildPower:
                if (GameManager.instance.IsUnlockPowerRoom()) {
                    return false;
                }
                price = GameManager.instance.buildData.GetBuildCashPrice(RoomID.Power);
                if (GameManager.instance.IsEnoughCash(price))
                    return true;
                break;
            case TutorialStepID.BuildRestroom:
                if (GameManager.instance.IsUnlockRestroom(0)) {
                    return false;
                }
                price = GameManager.instance.buildData.GetBuildCashPrice(RoomID.Restroom);
                energy = GameManager.instance.buildData.GetBuildEnergy(RoomID.Restroom);
                if (GameManager.instance.IsEnoughCash(price) && GameManager.instance.IsEnoughEnergy(energy))
                    return true;
                break;
            
            case TutorialStepID.ActiveMarketingCampaign:
                if (!GameManager.instance.IsUnlockSmallTable(1)) return false;
                if (!ProfileManager.PlayerData.GetMarketingManager().IsPassTutorial) {
                    return true;
                }
                break;
            case TutorialStepID.AdBoost:
                if (!GameManager.instance.IsUnlockSmallTable(1)) return false;
                if (!ProfileManager.PlayerData.GetAdBoostManager().IsPassTutorial) {
                    return true;
                }
                break;
            case TutorialStepID.SelectNewWorld:
                if (ProfileManager.PlayerData.GetTotalStarEarned() >= ProfileManager.Instance.dataConfig.worldDataAsset.GetDataByLevel(2).starNeededToUnlock) return true;
                break;
            case TutorialStepID.ClaimQuest:
                if (ProfileManager.PlayerData.GetQuestManager().hasQuestClaimable) {
                    return true;
                }
                break;
        }
        return false;
    }

}
public enum TutorialStepID {
    BuildVIP,
    BuildTable = 1,
    UpgradeTable,
    HireStaff,
    BuildPower,
    BuildRestroom,
    BuildClean,
    ActiveMarketingCampaign,
    AdBoost,
    WakeupStaff,
    SelectNewWorld,
    ClaimQuest,
    Research,
    None
}