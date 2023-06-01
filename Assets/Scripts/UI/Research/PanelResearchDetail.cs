using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelResearchDetail : MonoBehaviour {
    [SerializeField] CanvasGroup detailWrap;
    [Header("Button")]
    public Button btnCloseDetail;
    public Button btnImprove;
    public Button btnUpgrade;
    [SerializeField] Button btnReduce;
    public Button btnFinish;
    [Header("Text")]
    [SerializeField] Text txtNameDetail;
    [SerializeField] Text txtLevel;
    [SerializeField] Text txtProfit;
    [SerializeField] Text txtTimeMake;
    [SerializeField] Text txtTimeDetail;
    [SerializeField] Text txtPrice;
    [SerializeField] Text txtGemSkipTime;
    [SerializeField] Text txtUpgradePrice;
    [Header("Image")]
    [SerializeField] Image processUpgrade;
    [SerializeField] Image imgIconDetail;
    [SerializeField] Image imgBGDetail;
    [Header("GameObject")]
    [SerializeField] GameObject objComplete;
    [SerializeField] GameObject objNotice;
    [SerializeField] GameObject objHaveTicket;
    [SerializeField] GameObject objNoTicket;
    [SerializeField] GameObject objTimeWrap;
    [Header("RectTransform")]
    [SerializeField] RectTransform priceRect;
    [SerializeField] RectTransform contentRect;
    [SerializeField] RectTransform centerRect;
    [SerializeField] RectTransform gemRect;
    [Header("Sprite")]
    [SerializeField] Sprite sprNormal;
    [SerializeField] Sprite sprDisable;
    [SerializeField] Sprite sprUpgradeDone;

    ResearchManager researchManager;
    ResearchType researchDetailName;
    Research researchData;
    int levelResearchDetail;
    int gemsSkipTime;
    bool showDetail;
    int _MaxLevelResearch = 10;
    private void Awake() {
        researchManager = ProfileManager.Instance.playerData.researchManager;
        btnCloseDetail.onClick.AddListener(CloseDetail);
        btnImprove.onClick.AddListener(Improve);
        btnUpgrade.onClick.AddListener(OnUpgrade);
        btnReduce.onClick.AddListener(Reduce);
        btnFinish.onClick.AddListener(Finish);
    }

    private void FixedUpdate() {

        if (showDetail) {
            OnUpdateDetail(researchDetailName);
            if (updateTime) {
                float timeCoolDown = researchManager.GetTimeCoolDown(researchDetailName);
                txtTimeDetail.text = TimeUtil.TimeToString(timeCoolDown);
                processUpgrade.fillAmount = 1 - timeCoolDown / researchData.foodBlockTime;
                gemsSkipTime = (int)(timeCoolDown / 10);
                LayoutRebuilder.ForceRebuildLayoutImmediate(gemRect);
            }
            if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.Research) gemsSkipTime = 0;
            txtGemSkipTime.text = gemsSkipTime.ToString();
            btnFinish.interactable = GameManager.instance.IsEnoughGem(gemsSkipTime);
            btnUpgrade.interactable =
               GameManager.instance.IsEnoughResearchValue(researchData.CalulateReseachPrice(levelResearchDetail));
            btnImprove.interactable =
                GameManager.instance.IsEnoughResearchValue(researchData.CalulateReseachPrice(levelResearchDetail))
                && !ProfileManager.PlayerData.researchManager.IsMaxResearcherWorking();
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(centerRect);
            LayoutRebuilder.ForceRebuildLayoutImmediate(priceRect);
        }

    }
    void CloseDetail() {
        SoundManager.instance.PlaySoundEffect(SoundID.TAB_SWITCH);
        UIManager.instance.dotweenManager.DoFade(detailWrap, 0, () => {
            gameObject.SetActive(false);
        });
        UIManager.instance.dotweenManager.PunchScale(detailWrap.transform, new Vector3(-.2f, -.2f, 0));
        showDetail = false;
    }
    public void ShowDetail(ResearchType researchName) {
        gameObject.SetActive(true);
        UIManager.instance.dotweenManager.DoFade(detailWrap, 1);
        UIManager.instance.dotweenManager.PunchScale(detailWrap.transform, new Vector3(.2f, .2f, 0));
        SoundManager.instance.PlaySoundEffect(SoundID.POPUP_SHOW);
        showDetail = true;
        researchDetailName = researchName;
        researchData = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName);
        imgIconDetail.sprite = researchData.foodIcon;
        txtNameDetail.text = researchData.foodName;

    }
    void Improve() {
        SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        researchManager.Research(ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchDetailName));
        ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.Research, researchDetailName);
    }
    void Reduce() {
        SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        if (ProfileManager.Instance.playerData.ResourceSave.adTicket > 0) {
            ProfileManager.Instance.playerData.ResourceSave.ConsumeADTicket();
            researchManager.SkipByTicket(researchDetailName);
        } else {
            researchManager.SkipByWatchVideo(researchDetailName);
            ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.WatchAds_SkipTime, researchDetailName);
        }
    }
    void Finish() {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.Research) gemsSkipTime = 0;
        SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        researchManager.SkipNow(researchDetailName);
        ProfileManager.Instance.playerData.ConsumeGem(gemsSkipTime);
        ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.UseGem_SkipTime, researchDetailName);
        if (gemsSkipTime > 0) ABIAnalyticsManager.Instance.TrackEventResearchSkipGem(gemsSkipTime);
    }
    void OnUpgrade() {
        SoundManager.instance.PlaySoundEffect(SoundID.UPGRADE);
        researchManager.UpgradeResearch(researchDetailName);
        ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.Upgrade, researchDetailName);
    }
    bool updateTime;
    void OnUpdateDetail(ResearchType researchName) {
        levelResearchDetail = ProfileManager.PlayerData.researchManager.GetLevelByName(researchName);
        ChangeTextDes();
        if (ProfileManager.PlayerData.researchManager.IsUnlockResearch(researchName)) {
            if (levelResearchDetail >= _MaxLevelResearch) {
                txtLevel.text = "" + levelResearchDetail;
                DetailUpgradeDone();
            } else if (levelResearchDetail >= 1) {
                txtLevel.text = levelResearchDetail + "/10";
                DetailOnUpgrade();
            } else if (researchManager.CheckCurrentResearch(researchDetailName)) {
                txtLevel.text = levelResearchDetail + "/10";
                DetailOnResearch();
            } else {
                txtLevel.text = levelResearchDetail + "/10";
                DetailNormal();
            }
        } else {
            OnLock();
        }
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
            txtProfit.text = "<color=#06FF04>" + profit + "</color>" + " <color=#FFE800>(+" + researchData.CalculateIncreaseNextProfit(profit) + ")</color>";
            txtTimeMake.text = researchData.makeTime + "s";

        } else {
            txtProfit.text = "0 <color=#06FF04>(+" + researchData.CalculateProfit(1) + ")</color>";
            txtTimeMake.text = "0 <color=#FFE800>(+" + researchData.makeTime + "s)</color>";
        }

    }
    void DetailNormal() {
        objTimeWrap.SetActive(true);
        updateTime = false;
        txtTimeDetail.text = TimeUtil.TimeToString(researchData.foodBlockTime);
        imgBGDetail.sprite = sprNormal;
        processUpgrade.fillAmount = 0;
        txtPrice.text = researchData.CalulateReseachPrice(levelResearchDetail).ToString();
        //objNotice.SetActive(false);
        objComplete.SetActive(false);
        btnImprove.gameObject.SetActive(true);
        btnReduce.gameObject.SetActive(false);
        btnFinish.gameObject.SetActive(false);
        btnUpgrade.gameObject.SetActive(false);
    }
    void DetailOnResearch() {
        objTimeWrap.SetActive(true);
        updateTime = true;
        txtTimeDetail.text = TimeUtil.TimeToString(ProfileManager.PlayerData.researchManager.GetTimeCoolDown(researchDetailName));
        imgBGDetail.sprite = sprNormal;
        objComplete.SetActive(false);
        //objNotice.SetActive(false);
        btnImprove.gameObject.SetActive(false);
        btnUpgrade.gameObject.SetActive(false);
        btnReduce.gameObject.SetActive(true);
        btnFinish.gameObject.SetActive(true);
        objHaveTicket.SetActive(ProfileManager.Instance.playerData.ResourceSave.adTicket > 0);
        objNoTicket.SetActive(!(ProfileManager.Instance.playerData.ResourceSave.adTicket > 0));
    }
    void DetailOnUpgrade() {
        objTimeWrap.SetActive(false);
        objComplete.SetActive(false);
        //objNotice.SetActive(false);
        btnImprove.gameObject.SetActive(false);
        btnReduce.gameObject.SetActive(false);
        btnFinish.gameObject.SetActive(false);
        btnUpgrade.gameObject.SetActive(true);
        imgBGDetail.sprite = sprUpgradeDone;
        txtUpgradePrice.text = researchData.CalulateReseachPrice(levelResearchDetail).ToString();
    }
    void DetailUpgradeDone() {
        objTimeWrap.SetActive(false);
        updateTime = false;
        imgBGDetail.sprite = sprUpgradeDone;
        objComplete.SetActive(true);
        //objNotice.SetActive(false);
        btnImprove.gameObject.SetActive(false);
        btnReduce.gameObject.SetActive(false);
        btnFinish.gameObject.SetActive(false);
        btnUpgrade.gameObject.SetActive(false);
    }
    void OnLock() {
        objTimeWrap.SetActive(true);
        updateTime = false;
        txtTimeDetail.text = TimeUtil.TimeToString(researchData.foodBlockTime);
        imgBGDetail.sprite = sprNormal;
        processUpgrade.fillAmount = 0;
        txtPrice.text = researchData.CalulateReseachPrice(levelResearchDetail).ToString();
        txtLevel.text = "0/10";
        //objNotice.SetActive(true);
        objComplete.SetActive(false);
        btnImprove.gameObject.SetActive(false);
        btnImprove.interactable = false;
        btnReduce.gameObject.SetActive(false);
        btnFinish.gameObject.SetActive(false);
        btnUpgrade.gameObject.SetActive(false);
        //if (researchData.researchDependWorld == ResearchDependWorld.World2) {
        //    txtNotice.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(448);
        //} else if (researchData.researchDependWorld == ResearchDependWorld.World3) {
        //    txtNotice.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(449);
        //}
    }
    public ResearchType GetCurrentResearchNameOnDetail() { return researchDetailName; }

}