using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeItem : MonoBehaviour {
    [SerializeField] int Index;
    [SerializeField] Image ImgBg;
    [SerializeField] Sprite[] SprBgs;
    [SerializeField] Image ImgIcon;
    [SerializeField] Text TxtLevel;
    [SerializeField] GameObject ObjLevelInfo;
    [SerializeField] Image ObjLock;
    [SerializeField] int Level, MaxLevel;
    [SerializeField] Color[] colorIcons;
    [SerializeField] GameObject imgUpLevel;
    [SerializeField] Image imgBgLevel;
    [SerializeField] Color[] colorBGLevel;
    BigNumber priceUpgrade;
    public int energyRequire;
    int maxLevel;
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            PanelRoomInfo.instance.OnShowInfoItem(Index);
        });
    }
    public void Setup(int index, Sprite sprIcon, int level, int maxLevel) {
        Index = index;
        TxtLevel.text = "" + level;
        ObjLock.gameObject.SetActive(level <= 0);
        ObjLevelInfo.SetActive(level > 0 && level<maxLevel);
        ImgIcon.sprite = sprIcon;
        ImgBg.sprite = SprBgs[level > 0 ? 0 : 2];
        ImgIcon.color = colorIcons[level > 0 ? 1 : 2];
        Level = level;
        MaxLevel = maxLevel;
        imgUpLevel.SetActive(level < MaxLevel);
        imgBgLevel.color = colorBGLevel[level < MaxLevel ? 0 : 1];
        priceUpgrade = GameManager.instance.selectedRoom.GetUpgradePriceItem(index);
        int energyRequire1 = GameManager.instance.selectedRoom.GetEnergyRequireItem(index);
        int energyRequire2 = GameManager.instance.selectedRoom.GetEnergyRequirePreviorLevel(index);
        energyRequire = energyRequire1 - energyRequire2;
    }
    public void SetupNextLevel(int level) {
        Level = level;
        TxtLevel.text = "" + level;
        ObjLock.gameObject.SetActive(level <= 0);
        ObjLevelInfo.SetActive(level > 0 && level < MaxLevel);
        imgUpLevel.SetActive(level < MaxLevel);
        imgBgLevel.color = colorBGLevel[level < MaxLevel ? 0 : 1];
        priceUpgrade = GameManager.instance.selectedRoom.GetUpgradePriceItem(Index);
        int energyRequire1 = GameManager.instance.selectedRoom.GetEnergyRequireItem(Index);
        int energyRequire2 = GameManager.instance.selectedRoom.GetEnergyRequirePreviorLevel(Index);
        energyRequire = energyRequire1 - energyRequire2;
    }

    public void OnSelect(bool IsSelect) {
        ImgBg.sprite = SprBgs[IsSelect ? 1 : (Level > 0 ? 0 : 2)];
        ImgIcon.color = colorIcons[IsSelect ? 0 : (Level > 0 ? 1 : 2)];
    }
    private void Update() {

        if (Level == 0) {
            ObjLock.color = IsEnoughConditionUpgrade() ? Color.white : colorBGLevel[1];
        } else if (Level < MaxLevel) {
            imgBgLevel.color = colorBGLevel[IsEnoughConditionUpgrade() ? 0 : 1];
        } else {
            imgBgLevel.color = colorBGLevel[1];
        }

    }
    public bool IsEnoughConditionUpgrade() {
        return GameManager.instance.IsEnoughCash(priceUpgrade) && GameManager.instance.IsEnoughEnergy(energyRequire);
    }
}
