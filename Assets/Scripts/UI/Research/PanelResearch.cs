using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using System.Linq;

public class PanelResearch : UIPanel {
    public static PanelResearch instance;
    [SerializeField] Transform groupMainParent;
    [SerializeField] RectTransform tabParent;
    [SerializeField] ResearchGroup researchGroupPref;
    [SerializeField] ResearchTab researchTabPref;
    [SerializeField] Button btnExit;
    [SerializeField] ScrollRect scroll;
    [SerializeField] Text txtResearchValues;
    [SerializeField] Text txtResearchCount;
    [SerializeField] GameObject notifyFreeCustomer;
    [SerializeField] GameObject panelCustomerPack;
    [SerializeField] GameObject btnClosePanelCustomerPack;
    [SerializeField] Button btnFreeClaimCustomerInPack;
    [Header("===========Animation===========")]
    public CanvasGroup canvasGroup;
    public AnimationCurve animShowUp;
    public AnimationCurve animClose;
    [SerializeField] Transform transformMove;
    public float speed;
    public float offSet;
    Vector3 start, end;
    bool open;
    float timeAnim;
    [Header("===========Group Main===========")]
    public List<ResearchGroup> researchGroupMains = new List<ResearchGroup>();
    public List<ResearchTab> researchTabs = new List<ResearchTab>();
    ResearchTab currenTab;
    ResearchTab lastTab;
    [SerializeField] PanelResearchDetail panelResearchDetail;
    bool IsBlockClose;

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
        for (int i = 0; i < researchTabs.Count; i++) {
            researchTabs[i].IsBlock = false;
        }
        IsBlockClose = false;
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.Research) StartCoroutine(ShowTut());
    }

    void InitData() {
        ResearchData researchData = ProfileManager.Instance.dataConfig.researchDataConfig;
        for (int i = 0; i < groupMainParent.childCount; i++)
            groupMainParent.GetChild(i).gameObject.SetActive(false);
        ResearchGroup newTechGroup = Instantiate(researchGroupPref, groupMainParent);
        newTechGroup.InitData(researchData.foodResearchs);
        researchGroupMains.Add(newTechGroup);
    }
    private void Update() {
        notifyFreeCustomer.gameObject.SetActive(ProfileManager.PlayerData.researchManager.IsHasFreeCustomerCanWatched());
        if (open) {
            AnimOpen();
        }
        ReloadData();
        LayoutRebuilder.ForceRebuildLayoutImmediate(tabParent);
    }
    public void ShowDetail(ResearchType researchName) {
        panelResearchDetail.ShowDetail(researchName);
    }
    public ResearchType GetCurrentResearchNameOnDetail() {
        return panelResearchDetail.GetCurrentResearchNameOnDetail();
    }
    public void OnOpen() {
        transformMove.position = new Vector3(transformMove.position.x, transformMove.position.y - offSet);
        start = transformMove.position;
        end = new Vector3(transformMove.position.x, transformMove.position.y + offSet);
        open = true;
        timeAnim = 0;
    }
    void AnimOpen() {
        if (timeAnim < animShowUp.keys[animShowUp.length - 1].time) {
            transformMove.position = Vector3.Lerp(start, end, timeAnim);
            scroll.content.anchoredPosition = Vector2.Lerp(scroll.content.anchoredPosition, Vector2.zero, timeAnim);
            canvasGroup.alpha = Mathf.Lerp(0, 1, timeAnim);
            timeAnim += Time.deltaTime * speed;
        } else {
            transformMove.position = end;
            open = false;
            scroll.content.anchoredPosition = new Vector2(0, 0);
            canvasGroup.alpha = 1;
        }
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
        for (int i = 0; i < researchGroupMains.Count; i++)
                researchGroupMains[i].ReloadData();
        txtResearchValues.text = ProfileManager.PlayerData.researchManager.researchValue.ToString();
        int onUpgradeRemain = ProfileManager.PlayerData.researchManager.currentResearchs.Count;
        onUpgradeRemain = ProfileManager.PlayerData.researchManager.maxResearchCount - onUpgradeRemain;
        txtResearchCount.text = onUpgradeRemain.ToString() + "/" + ProfileManager.PlayerData.researchManager.maxResearchCount.ToString();
    }
    public void OnSelectTab(ResearchTab tab) {
        if (currenTab != null)
            lastTab = currenTab;
        researchGroupMains[tab.index].OnOpen();
        currenTab = tab;
        ResetTab();
    }
    public void OnDeSelectTab() { ResetTab(); }
    private void ResetTab() {
        for (int i = 0; i < researchGroupMains.Count; i++) {
            if (researchTabs[i] == currenTab)
                continue;
            researchGroupMains[i].OnClose();
            if (lastTab == researchTabs[i]) researchTabs[i].OnDeselect(currenTab.OnSelect);
            else researchTabs[i].OnDeselect();
        }
        if (lastTab == null)
            currenTab.OnSelect();
    }
    /// <summary>
    /// ShowTut Improve  Reseach & Free Claim Customer
    /// </summary>
    bool isShowTut;
    IEnumerator ShowTut() {
        if (!isShowTut) {
            IsBlockClose = true;
            for (int i = 0; i < researchTabs.Count; i++) {
                researchTabs[i].IsBlock = true;
            }
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
            Transform slot = researchGroupMains[0].GetResearchSlotTran(ResearchType.Hamburger);
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
            // wait user press finish button......
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
