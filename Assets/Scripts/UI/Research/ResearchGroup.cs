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
    public List<ResearchSlot> listTechSlots = new List<ResearchSlot>();
    ResearchManager researchManager;
    private void Awake() {
        researchManager = ProfileManager.Instance.playerData.researchManager;
    }
    public virtual void InitData(List<Research> list) {
        for (int i = 0; i < list.Count; i++) {
            ResearchSlot newResearSlot = Instantiate(researchSlotPref, listSlotParent);
            newResearSlot.InitDataResearch(list[i].researchType);
            listTechSlots.Add(newResearSlot);
        }
        ReloadData();
    }
    public void ReloadData() {
        researchManager = ProfileManager.Instance.playerData.researchManager;
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

    public Transform GetResearchSlotTran(ResearchType name) {
        foreach (var slot in listTechSlots) {
            if (slot.researchName == name) return slot.transform;
        }
        return null;
    }
}
