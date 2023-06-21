using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UIFoodSlot : UIEffect {
    [SerializeField] Image imgIcon;
    [SerializeField] Image imgProcess;
    [SerializeField] Text txtName;
    [SerializeField] Button btnTechSlot;
    [SerializeField] GameObject objCanResearch;
    [SerializeField] GameObject objResearchMax;
    [SerializeField] GameObject objOnResearching;
    [SerializeField] Transform onUpgradeAnim;
    [SerializeField] Transform canUpgradeAnim;
    [SerializeField] List<Image> imgStars;
    public ResearchType researchName;

    public int level;
    float timeRemain;
    float timeTotal;
    ResearchManager researchManager;
    private void Awake() {
        btnTechSlot.onClick.AddListener(OnSelect);
        timeReAnim = Random.Range(2f, 4f);
    }
    void Update() {
        if (timeAnim > timeReAnim) {
            onUpgradeAnim.DOPunchScale(vectorPunch, scaleDuration, scaleVibaration);
            canUpgradeAnim.DOPunchScale(vectorPunch, scaleDuration, scaleVibaration);
            timeAnim = 0;
        } else { timeAnim += Time.deltaTime; }

        if (imgProcess.gameObject.activeSelf) {
            timeRemain = ProfileManager.PlayerData.researchManager.GetTimeEndResearch(researchName);
            timeTotal = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName).GetResearchTime(level);
            imgProcess.fillAmount = 1f - timeRemain / timeTotal;
        }
    }
    public Vector3 vectorPunch;
    public float scaleDuration;
    public int scaleVibaration;
    float timeAnim;
    float timeReAnim;
    public void InitDataResearch(ResearchType researchName) {
        Research research = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName);
        imgIcon.sprite = research.foodIcon;
        this.researchName = researchName;
        LoadInfo();
    }
    void OnSelect() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ScaleEffectButton(btnTechSlot);
        PanelResearch.instance.ShowDetail(researchName);
    }
    public void LoadInfo() {
        researchManager = ProfileManager.Instance.playerData.researchManager;
        level = ProfileManager.Instance.playerData.researchManager.GetLevelByName(researchName);

        for (int i = 0; i < imgStars.Count; i++) {
            if (i < level / 2) {
                imgStars[i].fillAmount = 1;
            } else if (i < level / 2 + 1 && level % 2 == 1) {
                imgStars[i].fillAmount = 0.5f;
            } else imgStars[i].fillAmount = 0f;
        }
        if (researchManager.IsMaxLevel(researchName)) ResearchMaxMode();
        else if (researchManager.IsResearching(researchName)) ResearchingMode();
        else if (researchManager.IsEnoughResearchValue(researchName) && researchManager.IsAvaiableSlotToResearch()) {
            AvaiableResearchMode();
        } else NormalMode();
    }

    #region mode
    void NormalMode() {
        objCanResearch.SetActive(false);
        objResearchMax.SetActive(false);
        objOnResearching.SetActive(false);
        imgProcess.gameObject.SetActive(false);
    }

    void AvaiableResearchMode() {
        objCanResearch.SetActive(true);
        objResearchMax.SetActive(false);
        objOnResearching.SetActive(false);
        imgProcess.gameObject.SetActive(false);
    }
    void ResearchingMode() {
        objCanResearch.SetActive(false);
        objResearchMax.SetActive(false);
        objOnResearching.SetActive(true);
        imgProcess.gameObject.SetActive(true);
    }
    void ResearchMaxMode() {
        objCanResearch.SetActive(false);
        objResearchMax.SetActive(true);
        objOnResearching.SetActive(false);
        imgProcess.gameObject.SetActive(false);
    }


    #endregion
}
