using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PanelResearchDetail : MonoBehaviour {
    [SerializeField] CanvasGroup detailWrap;
    [Header("Button")]
    public Button btnCloseDetail;
    public Button btnImprove;
    [SerializeField] Button btnReduce;
    public Button btnFinish;
    [Header("Info")]
    [SerializeField] Text txtNameDetail;
    [SerializeField] List<Image> imgStars;
    [SerializeField] Text txtProfit;
    [SerializeField] Text txtTimeMake;
    [SerializeField] Text txtTimeDetail;
    [SerializeField] Text txtPrice;
    [SerializeField] Text txtGemSkipTime;
    [Header("Process")]
    [SerializeField] Image processUpgrade;
    [SerializeField] Image imgIconDetail;
    [Header("GameObject")]
    [SerializeField] GameObject objComplete;
    [SerializeField] GameObject objHaveTicket;
    [SerializeField] GameObject objNoTicket;
    [SerializeField] GameObject objTimeWrap;
    [SerializeField] GameObject objRequireMoreSlot;

    ResearchManager researchManager;
    ResearchType researchDetailName;
    Research researchData;
    public int levelResearchDetail;
    int researchPrice;
    int gemsSkipTime;
    bool showDetail;
    int _MaxLevelResearch = 10;
    private void Awake() {
        researchManager = ProfileManager.Instance.playerData.researchManager;
        btnCloseDetail.onClick.AddListener(OnCloseDetail);
        btnImprove.onClick.AddListener(OnImprove);
        btnReduce.onClick.AddListener(OnReduce);
        btnFinish.onClick.AddListener(OnFinish);
    }
    public void ShowDetail(ResearchType researchName) {
        UIManager.instance.dotweenManager.DoFade(detailWrap, 1);
        UIManager.instance.dotweenManager.PunchScale(detailWrap.transform, new Vector3(.2f, .2f, 0));
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        showDetail = true;
        researchDetailName = researchName;
        researchData = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName);
        imgIconDetail.sprite = researchData.foodIcon;
        txtNameDetail.text = researchData.foodName;
        OnLoadDetail(researchDetailName);
    }

    bool updateTime;
    void OnLoadDetail(ResearchType researchName) {
        levelResearchDetail = researchManager.GetLevelByName(researchName);
        for (int i = 0; i < imgStars.Count; i++) {
            if (i < levelResearchDetail / 2) {
                imgStars[i].fillAmount = 1;
            } else if (i < levelResearchDetail / 2 + 1 && levelResearchDetail % 2 == 1) {
                imgStars[i].fillAmount = 0.5f;
            } else imgStars[i].fillAmount = 0f;
        }
        researchPrice = researchData.GetReseachPrice(levelResearchDetail);
        ChangeTextDes();
        if (levelResearchDetail >= _MaxLevelResearch) {
            DetailResearchReachLevelMax();
        } else if (researchManager.IsResearching(researchDetailName)) {
            DetailOnResearching();
        } else {
            DetailNormal();
        }
    }
    private void FixedUpdate() {

        if (showDetail) {
            OnLoadDetail(researchDetailName);
            if (updateTime) {
                float timeCoolDown = researchManager.GetTimeEndResearch(researchDetailName);
                txtTimeDetail.text = TimeUtil.TimeToString(timeCoolDown);
                processUpgrade.fillAmount = 1 - timeCoolDown / researchData.GetResearchTime(levelResearchDetail);
                gemsSkipTime = (int)(timeCoolDown / 30);
            }
            if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.Research) gemsSkipTime = 0;
            txtGemSkipTime.text = gemsSkipTime.ToString();
            btnFinish.interactable = GameManager.instance.IsEnoughGem(gemsSkipTime);
            btnImprove.interactable =
                GameManager.instance.IsEnoughResearchValue(researchPrice)
                && ProfileManager.PlayerData.researchManager.IsAvaiableSlotToResearch();

        }

    }
    void OnCloseDetail() {
        SoundManager.instance.PlaySoundEffect(SoundID.TAB_SWITCH);
        UIManager.instance.dotweenManager.DoFade(detailWrap, 0, () => {
            gameObject.SetActive(false);
        });
        UIManager.instance.dotweenManager.PunchScale(detailWrap.transform, new Vector3(-.2f, -.2f, 0));
        showDetail = false;
    }

    void OnImprove() {
        SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        researchManager.ConsumeResearchValue(researchPrice);
        researchManager.Research(ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchDetailName));
        ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.Research, researchDetailName.ToString() + "_" + levelResearchDetail);
    }
    void OnReduce() {
        SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        if (ProfileManager.Instance.playerData.ResourceSave.adTicket > 0) {
            ProfileManager.Instance.playerData.ResourceSave.ConsumeADTicket();
            researchManager.SkipByTicket(researchDetailName);
        } else {
            researchManager.SkipByWatchVideo(researchDetailName);
            ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.WatchAds_SkipTime, researchDetailName.ToString() + "_" + levelResearchDetail);
        }
    }
    void OnFinish() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.Research) gemsSkipTime = 0;
        SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        researchManager.SkipNow(researchDetailName);
        ProfileManager.Instance.playerData.ConsumeGem(gemsSkipTime);
        ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.UseGem_SkipTime, researchDetailName.ToString() + "_" + levelResearchDetail);
        if (gemsSkipTime > 0) ABIAnalyticsManager.Instance.TrackEventResearchSkipGem(gemsSkipTime);
    }


    void ChangeTextDes() {
        if (researchData == null)
            return;
        if (levelResearchDetail >= _MaxLevelResearch) {
            double profit = researchData.CalculateProfit(levelResearchDetail);
            txtProfit.text = "<color=#06FF04>" + profit + "</color>";
            txtTimeMake.text = researchData.makeTime + "s";
        } else if (levelResearchDetail > 0) {
            double profit = researchData.CalculateProfit(levelResearchDetail);
            txtProfit.text = "<color=#06FF04>" + profit + "</color>" + " <color=#FFE800>(+" + researchData.CalculateIncreaseNextProfit(levelResearchDetail) + ")</color>";
            txtTimeMake.text = researchData.makeTime + "s";

        } else {
            txtProfit.text = "0 <color=#06FF04>(+" + researchData.CalculateProfit(1) + ")</color>";
            txtTimeMake.text = "0 <color=#FFE800>(+" + researchData.makeTime + "s)</color>";
        }

    }
    void DetailNormal() {
        objTimeWrap.SetActive(true);
        updateTime = false;
        txtTimeDetail.text = TimeUtil.TimeToString(researchData.GetResearchTime(levelResearchDetail));
        processUpgrade.fillAmount = 0;
        txtPrice.text = researchData.GetReseachPrice(levelResearchDetail).ToString();
        objComplete.SetActive(false);
        btnImprove.gameObject.SetActive(ProfileManager.PlayerData.researchManager.IsAvaiableSlotToResearch());
        btnReduce.gameObject.SetActive(false);
        btnFinish.gameObject.SetActive(false);
        objRequireMoreSlot.gameObject.SetActive(!ProfileManager.PlayerData.researchManager.IsAvaiableSlotToResearch());
    }
    void DetailOnResearching() {
        objTimeWrap.SetActive(true);
        updateTime = true;
        txtTimeDetail.text = TimeUtil.TimeToString(ProfileManager.PlayerData.researchManager.GetTimeEndResearch(researchDetailName));
        objComplete.SetActive(false);
        btnImprove.gameObject.SetActive(false);
        btnReduce.gameObject.SetActive(true);
        btnFinish.gameObject.SetActive(true);
        objHaveTicket.SetActive(ProfileManager.Instance.playerData.ResourceSave.adTicket > 0);
        objNoTicket.SetActive(!(ProfileManager.Instance.playerData.ResourceSave.adTicket > 0));
        objRequireMoreSlot.gameObject.SetActive(false);
    }

    void DetailResearchReachLevelMax() {
        objTimeWrap.SetActive(false);
        updateTime = false;
        objComplete.SetActive(true);
        btnImprove.gameObject.SetActive(false);
        btnReduce.gameObject.SetActive(false);
        btnFinish.gameObject.SetActive(false);
        objRequireMoreSlot.gameObject.SetActive(false);
    }

    public ResearchType GetCurrentResearchNameOnDetail() { return researchDetailName; }

}