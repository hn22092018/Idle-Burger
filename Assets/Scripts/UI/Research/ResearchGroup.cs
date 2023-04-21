using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResearchGroup : MonoBehaviour {
    [SerializeField] CanvasGroup bgCanvas;
    [SerializeField] Transform listSlotParent;
    [SerializeField] RectTransform contentRect;
    [SerializeField] ResearchSlot researchSlotPref;
    [SerializeField] ResearchWrap researchFoodWrap;
    public List<ResearchWrap> researchFoodWraps = new List<ResearchWrap>();
    public List<ResearchSlot> listTechSlots = new List<ResearchSlot>();
    ResearchManager researchManager;
    public GroupResearchName researchGroupName;
    private void Awake() {
        researchManager = ProfileManager.Instance.playerData.researchManager;
    }
    public virtual void InitData(GroupResearchData groupResearch) {
        researchGroupName = groupResearch.researchGroupName;
        for (int i = 0; i < groupResearch.researches.Count; i++) {
            int indexOfWrap = GetIndexOfFoodWrap(groupResearch.researches[i].researchType);
            if (indexOfWrap < 0)
                indexOfWrap = CreateNewWrap(groupResearch.researches[i].researchType);
            ResearchSlot newResearSlot = researchFoodWraps[indexOfWrap].InitSlot(groupResearch.researches[i], researchSlotPref);
            listTechSlots.Add(newResearSlot);
        }
    }
    public void ReloadData() {
        for (int i = 0; i < listTechSlots.Count; i++) {
            listTechSlots[i].ChangeNameResearch();
            if (!researchManager.IsUnlockResearch(listTechSlots[i].researchName)) listTechSlots[i].Lock();
            else if (researchManager.IsMaxLevel(listTechSlots[i].researchName)) listTechSlots[i].UpgradeDoneMode();
            else if (researchManager.CheckCurrentResearch(listTechSlots[i].researchName)) listTechSlots[i].UpgradeMode();
            else if (!researchManager.IsMaxLevel(listTechSlots[i].researchName) && researchManager.IsEnoughResearchValue(listTechSlots[i].researchName) && !researchManager.IsMaxResearcherWorking()) {
                listTechSlots[i].CanUpgradeMode();
            } else if (researchManager.GetLevelByName(listTechSlots[i].researchName) <= 0) listTechSlots[i].LevelZeroMode();
            else listTechSlots[i].NormalMode();
            if (PanelResearch.instance.GetCurrentResearchNameOnDetail() == listTechSlots[i].researchName)
                listTechSlots[i].OnSelect();
            else listTechSlots[i].OnDeSelect();
        }
        LayoutRebuilder.MarkLayoutForRebuild(contentRect);
    }

    public void OnOpen() {
        gameObject.SetActive(true);
        bgCanvas.DOFade(0, 1f);
    }
    public void OnClose() {
        gameObject.SetActive(false);
        bgCanvas.alpha = 1f;
    }
    int GetIndexOfFoodWrap(ResearchType researchMap) {
        for (int i = 0; i < researchFoodWraps.Count; i++) {
            if (researchMap == researchFoodWraps[i].researchDepend)
                return i;
        }
        return -1;
    }
    int CreateNewWrap(ResearchType researchDepend) {
        ResearchWrap newResearchFoodWrap = Instantiate(researchFoodWrap, listSlotParent);
        newResearchFoodWrap.researchDepend = researchDepend;
        researchFoodWraps.Add(newResearchFoodWrap);
        return researchFoodWraps.Count - 1;
    }
    public Transform GetResearchSlotTran(ResearchName name) {
        foreach(var slot in listTechSlots) {
            if (slot.researchName == name) return slot.transform;
        }
        return null;
    }
}
