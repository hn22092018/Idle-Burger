using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class ResearchSlot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] Image imgIcon;
    [SerializeField] Image imgTextLevelBG;
    [SerializeField] Image imgBG;
    [SerializeField] Image imgProcess;
    [SerializeField] Text txtName;
    [SerializeField] Button btnTechSlot;
    [SerializeField] Sprite sprUpgradeDone, sprNormal, sprDisable;
    [SerializeField] Color c_UpgradeDone, c_Normal, c_Disable;
    [SerializeField] GameObject objCanUpgrade;
    [SerializeField] GameObject objUpgradeDone;
    [SerializeField] GameObject objOnUpgrade;
    [SerializeField] GameObject objDisable;
    [SerializeField] GameObject objProcess;
    [SerializeField] GameObject objOnselect;
    [SerializeField] Vector3 vectorScaleDown;
    [SerializeField] Vector3 vectorScaleUp;
    [SerializeField] float duration;
    [SerializeField] Transform onUpgradeAnim;
    [SerializeField] Transform canUpgradeAnim;
    public ResearchName researchName;
    int level;
    float timeRemain;
    float timeTotal;
    private void Awake()
    {
        btnTechSlot.onClick.AddListener(Select);
        timeReAnim = Random.Range(2f, 4f);
    }
    void Update() {
        if (timeAnim > timeReAnim)
        {
            onUpgradeAnim.DOPunchScale(vectorPunch, scaleDuration, scaleVibaration);
            canUpgradeAnim.DOPunchScale(vectorPunch, scaleDuration, scaleVibaration);
            timeAnim = 0;
        }
        else { timeAnim += Time.deltaTime; }

        if (objProcess.activeSelf)
        {
            timeRemain = ProfileManager.PlayerData.researchManager.GetTimeCoolDown(researchName);
            timeTotal = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName).timeBlock;
            imgProcess.fillAmount = 1f - timeRemain / timeTotal;
        }
    }
    public Vector3 vectorPunch;
    public float scaleDuration;
    public int scaleVibaration;
    float timeAnim;
    float timeReAnim;
    public void InitDataResearch(ResearchName researchName) {
        Research research = ProfileManager.Instance.dataConfig.researchDataConfig.GetResearch(researchName);
        imgIcon.sprite = research.icon;
        this.researchName = researchName;
        ChangeNameResearch();
    }
    void Select() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.dotweenManager.ScaleObj(transform, vectorScaleUp, duration, () => { 
            PanelResearch.instance.ShowDetail(researchName); 
        });
    }
    public void ChangeNameResearch() {
        level = ProfileManager.Instance.playerData.researchManager.GetLevelByName(researchName);
        txtName.text = "Lv." + level.ToString();
    }
    #region mode
    public void NormalMode() {
        objCanUpgrade.SetActive(false);
        objUpgradeDone.SetActive(false);
        objOnUpgrade.SetActive(false);
        objDisable.SetActive(false);
        objProcess.SetActive(false);
        imgBG.sprite = sprNormal;
        imgTextLevelBG.color = c_Normal;
    }
    public void CanUpgradeMode() {
        objCanUpgrade.SetActive(true);
        objUpgradeDone.SetActive(false);
        objOnUpgrade.SetActive(false); 
        objDisable.SetActive(false);
        objProcess.SetActive(false);
        imgBG.sprite = sprNormal;
        imgTextLevelBG.color = c_Normal;
        imgIcon.color = Color.white;
    }
    public void UpgradeMode() {
        objCanUpgrade.SetActive(false);
        objUpgradeDone.SetActive(false);
        objOnUpgrade.SetActive(true);
        objDisable.SetActive(false);
        objProcess.SetActive(true);
        imgBG.sprite = sprNormal;
        imgTextLevelBG.color = c_Normal;
        imgIcon.color = Color.white;
    }
    public void UpgradeDoneMode()
    {
        objCanUpgrade.SetActive(false);
        objUpgradeDone.SetActive(true);
        objOnUpgrade.SetActive(false);
        objDisable.SetActive(false);
        objProcess.SetActive(false);
        imgBG.sprite = sprUpgradeDone;
        imgTextLevelBG.color = c_UpgradeDone;
        imgIcon.color = Color.white;
    }
    public void LevelZeroMode()
    {
        objCanUpgrade.SetActive(false);
        objUpgradeDone.SetActive(false);
        objOnUpgrade.SetActive(false);
        objDisable.SetActive(false);
        objProcess.SetActive(false);
        imgBG.sprite = sprDisable;
        imgTextLevelBG.color = c_Disable;
        imgIcon.color = c_Disable;
    }
    public void Lock() {
        objCanUpgrade.SetActive(false);
        objUpgradeDone.SetActive(false);
        objOnUpgrade.SetActive(false);
        objDisable.SetActive(true);
        objProcess.SetActive(false);
        imgBG.sprite = sprDisable;
        imgTextLevelBG.color = c_Disable;
        imgIcon.color = c_Disable;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(vectorScaleUp, duration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(vectorScaleDown, duration);
    }
    public void OnSelect() { objOnselect.SetActive(true); }
    public void OnDeSelect() { objOnselect.SetActive(false); }
    #endregion
}
