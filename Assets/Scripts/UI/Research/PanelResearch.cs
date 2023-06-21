using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.Linq;

public class PanelResearch : UIPanel {
    public static PanelResearch instance;
    [SerializeField] Button btnExit;
    [SerializeField] Text txtResearchValues;
    [SerializeField] GameObject notifyFreeCustomer;
    [SerializeField] GameObject panelCustomerPack;
    [SerializeField] GameObject btnClosePanelCustomerPack;
    [SerializeField] Button btnFreeClaimCustomerInPack;
    [SerializeField] PanelResearchDetail panelResearchDetail;
    [SerializeField] PanelBuyResearchSlot panelBuyMoreSlot;

    [Header("===========Group Main===========")]
    bool IsBlockClose;
    [SerializeField] Transform listSlotParent;
    [SerializeField] UIFoodSlot researchSlotPref;
    public List<UIFoodSlot> listTechSlots = new List<UIFoodSlot>();
    public override void Awake() {
        instance = this;
        panelType = UIPanelType.PanelTechnology;
        base.Awake();
        InitData();
        btnExit.onClick.AddListener(OnClose);
    }
    private void OnEnable() {
        panelCustomerPack.SetActive(false);
        panelResearchDetail.gameObject.SetActive(false);
        panelBuyMoreSlot.gameObject.SetActive(false);
        IsBlockClose = false;
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.Research) StartCoroutine(ShowTut());
    }

    void InitData() {
        ResearchData researchData = ProfileManager.Instance.dataConfig.researchDataConfig;
        for (int i = 0; i < researchData.foodResearchs.Count; i++) {
            UIFoodSlot newResearSlot = Instantiate(researchSlotPref, listSlotParent);
            newResearSlot.InitDataResearch(researchData.foodResearchs[i].researchType);
            listTechSlots.Add(newResearSlot);
        }
    }

    private void Update() {
        notifyFreeCustomer.gameObject.SetActive(ProfileManager.PlayerData.researchManager.IsHasFreeCustomerCanWatched());
        ReloadData();
    }
    public void ShowDetail(ResearchType researchName) {
        panelResearchDetail.gameObject.SetActive(true);
        panelResearchDetail.ShowDetail(researchName);
    }
    public void ShowDetailUnlockSlot(EResearchSlotIndex index) {
        panelBuyMoreSlot.gameObject.SetActive(true);
        panelBuyMoreSlot.Init(index);
    }

    void OnClose() {
        if (IsBlockClose) return;
        SoundManager.instance.PlaySoundEffect(SoundID.TAB_SWITCH);
        if (Tutorials.instance.IsShow) {
            Tutorials.instance.FinishTutorial();
            Tutorials.instance.OnCloseTutorial();
            Tutorials.instance.ShowIntro(new List<string> {
             ProfileManager.Instance.dataConfig.GameText.GetTextByID(440)
            });
        }
        UIManager.instance.ClosePanelTech();

    }
    public void ReloadData() {
        txtResearchValues.text = ProfileManager.PlayerData.researchManager.researchValue.ToString();
        for (int i = 0; i < listTechSlots.Count; i++) {
            listTechSlots[i].LoadInfo();
        }
    }
    Transform FindFoodTrans(ResearchType type) {
        for (int i = 0; i < listTechSlots.Count; i++) {
            if (listTechSlots[i].researchName == type) return listTechSlots[i].transform;
        }
        return null;
    }

    /// <summary>
    /// ShowTut Improve  Reseach & Free Claim Customer
    /// </summary>
    bool isShowTut;
    IEnumerator ShowTut() {
        if (!isShowTut) {
            IsBlockClose = true;
            isShowTut = true;
            Tutorials.instance.OffBlocker();
            // intro why need to research food.
            Tutorials.instance.ShowIntro(new List<string> {
             ProfileManager.Instance.dataConfig.GameText.GetTextByID(20),
              ProfileManager.Instance.dataConfig.GameText.GetTextByID(229),
               ProfileManager.Instance.dataConfig.GameText.GetTextByID(230),
            });
            while (Tutorials.instance.IsShowIntro) {
                yield return new WaitForEndOfFrame();
            }
            // end intro. Show tut press research slot
            yield return new WaitForSeconds(0.5f);
            panelCustomerPack.SetActive(false);
            Transform slot = FindFoodTrans(ResearchType.CheeseBurger);
            Tutorials.instance.OnShow(slot);
            // Check Condition To Show Tut Free Claim Customer
            if (ProfileManager.PlayerData.researchManager.Free_Customer_NonAds <= 0)
                ProfileManager.PlayerData.researchManager.Free_Customer_NonAds = 1;
            // Wait user press in research slot
            while (!panelResearchDetail.gameObject.activeInHierarchy) {
                yield return new WaitForEndOfFrame();
            }
            Tutorials.instance.OffBlocker();
            // add sub action press btn improve research to listener improve action callback
            bool isPressImprove = false;
            panelResearchDetail.btnImprove.onClick.AddListener(() => {
                isPressImprove = true;
            });
            yield return new WaitForSeconds(0.8f);
            // show tut press btn improve research
            Tutorials.instance.OnShow(panelResearchDetail.btnImprove.transform);
            while (!isPressImprove) {
                yield return new WaitForEndOfFrame();
            }
            Tutorials.instance.OffBlocker();
            //show intro skip
            Tutorials.instance.ShowIntro(new List<string> {
             ProfileManager.Instance.dataConfig.GameText.GetTextByID(231)
            });
            while (Tutorials.instance.IsShowIntro) {
                yield return new WaitForEndOfFrame();
            }
            // end intro, //show tut press skip
            Tutorials.instance.OnShow(panelResearchDetail.btnFinish.transform);
            while (panelResearchDetail.btnFinish.gameObject.activeInHierarchy) {
                yield return new WaitForEndOfFrame();
            }
            Tutorials.instance.OffBlocker();
            // when user  press btn improve, show tut close improve
            Tutorials.instance.OnShow(panelResearchDetail.btnCloseDetail.transform);
            // wait user press close......
            while (panelResearchDetail.gameObject.activeInHierarchy) {
                yield return new WaitForEndOfFrame();
            }
            Tutorials.instance.OffBlocker();
            if (!ProfileManager.PlayerData.researchManager.IsHasFreeCustomerCanClaimed()) {
                IsBlockClose = false;
                Tutorials.instance.OnShow(btnExit.transform);
                yield break;
            }
            //show intro  claim customer reseach value.
            Tutorials.instance.ShowIntro(new List<string> {
             ProfileManager.Instance.dataConfig.GameText.GetTextByID(232)
            });
            while (Tutorials.instance.IsShowIntro) {
                yield return new WaitForEndOfFrame();
            }
            // end intro

            //show tut view customer reseach value pack. 
            Tutorials.instance.OnShow(txtResearchValues.transform);
            // wait user press show pack
            while (!panelCustomerPack.gameObject.activeInHierarchy) {
                yield return new WaitForEndOfFrame();
            }
            Tutorials.instance.OffBlocker();
            //show tut claim customer reseach value pack. 
            bool isPressClaim = false;
            btnFreeClaimCustomerInPack.onClick.AddListener(() => {
                isPressClaim = true;
            });
            yield return new WaitForSeconds(0.8f);
            Tutorials.instance.OnShow(btnFreeClaimCustomerInPack.transform);
            while (!isPressClaim) {
                yield return new WaitForEndOfFrame();
            }
            Tutorials.instance.OnShow(btnClosePanelCustomerPack.transform);
            while (panelCustomerPack.gameObject.activeInHierarchy) {
                yield return new WaitForEndOfFrame();
            }
            // claimed free.
            IsBlockClose = false;
            Tutorials.instance.OnShow(btnExit.transform);

        }
    }
}
