using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldInfoItem : MonoBehaviour {
    [SerializeField] Image worldBG;
    [SerializeField] Text resNameTxt;
    [SerializeField] Text resDesTxt;
    [SerializeField] Image resIcon;
    [SerializeField] List<GameObject> unlockedStar;
    [SerializeField] GameObject unlockedStarField;
    [SerializeField] GameObject requireStar;
    [SerializeField] Text requireStarTxt;
    [SerializeField] Text requireTxt;
    [SerializeField] GameObject objCash;
    [SerializeField] Text txtCash;

    [SerializeField] GameObject travelBtn;
    [SerializeField] GameObject alreadyHereAlert;
    [SerializeField] GameObject ableToOpenBtn;
    [SerializeField] GameObject unAbleToOpenBtn;
    [SerializeField] GameObject eventTime;
    [SerializeField] Text txteventTime;
    public int selectedWorld;
    [SerializeField] int starToUnlock;
    [SerializeField] bool unlocked;
    WorldBaseData currentWorldBaseData;
    [SerializeField] List<Sprite> bg;

    string str1 = "Event Closed";
    string str2 = "Incoming In ";
    string str3 = "Christmas Event Avaiable";
    private void Awake() {
        str1 = ProfileManager.Instance.dataConfig.GameText.GetTextByID(412);
        str2 = ProfileManager.Instance.dataConfig.GameText.GetTextByID(407);
        str3 = ProfileManager.Instance.dataConfig.GameText.GetTextByID(406);
        travelBtn.GetComponent<Button>().onClick.AddListener(OnTravel);
        ableToOpenBtn.GetComponent<Button>().onClick.AddListener(OnOpenWorld);
    }

    public void Setup(WorldBaseData info) {
        currentWorldBaseData = info;
        selectedWorld = info.selectedWorld;
        starToUnlock = info.starNeededToUnlock;
        resNameTxt.text = info.GetName();
        resDesTxt.text = info.GetDes();
    }
    private void OnEnable() {
        if (currentWorldBaseData != null) {
            resNameTxt.text = currentWorldBaseData.GetName();
            resDesTxt.text = currentWorldBaseData.GetDes();
        }
    }
    private void Update() {
        txtCash.text = new BigNumber(ProfileManager.Instance.playerData.GetCashByWorld(selectedWorld)).IntToString();
        //if (currentWorldBaseData.selectedWorld == -1) UpdateStateChristmas();
        //else {
            eventTime.gameObject.SetActive(false);
            CheckProcessStatus();
        //}
    }
    //void UpdateStateChristmas() {
    //    requireStar.gameObject.SetActive(false);
    //    if (ProfileManager.PlayerData.christmasEventManager.IsHasEvent()) {
    //        if (selectedWorld == ProfileManager.Instance.playerData.selectedWorld) {
    //            travelBtn.SetActive(false);
    //            alreadyHereAlert.SetActive(false);
    //            ableToOpenBtn.SetActive(false);
    //            unAbleToOpenBtn.SetActive(false);
    //            eventTime.SetActive(true);
    //            txteventTime.text = str3 + ": " + ProfileManager.PlayerData.christmasEventManager.RemainTimeToString();
    //        } else {
    //            travelBtn.SetActive(true);
    //            alreadyHereAlert.SetActive(false);
    //            ableToOpenBtn.SetActive(false);
    //            unAbleToOpenBtn.SetActive(false);
    //            eventTime.SetActive(false);
    //        }
    //    } else if (ProfileManager.PlayerData.christmasEventManager.IsInCommingEvent()) {
    //        eventTime.SetActive(true);
    //        travelBtn.SetActive(false);
    //        alreadyHereAlert.SetActive(false);
    //        ableToOpenBtn.SetActive(false);
    //        unAbleToOpenBtn.SetActive(false);
    //        txteventTime.text = str2 + ": " + ProfileManager.PlayerData.christmasEventManager.RemainIncomingTimeToString();
    //    } else {
    //        this.gameObject.SetActive(false);
    //    }
    //}


    void CheckProcessStatus() {
        unlocked = ProfileManager.Instance.playerData.IsWolrdUnlocked(selectedWorld);
        resIcon.sprite = currentWorldBaseData.restaurantIcon;
        if (unlocked) {
            worldBG.sprite = bg[0];
            objCash.SetActive(true);
            txtCash.text = new BigNumber(ProfileManager.Instance.playerData.GetCashByWorld(selectedWorld)).IntToString();
            requireStar.SetActive(false);
            unlockedStarField.SetActive(false);
            if (selectedWorld == ProfileManager.Instance.playerData.selectedWorld) {
                travelBtn.SetActive(false);
                alreadyHereAlert.SetActive(true);
                ableToOpenBtn.SetActive(false);
                unAbleToOpenBtn.SetActive(false);
            } else {
                travelBtn.SetActive(true);
                alreadyHereAlert.SetActive(false);
                ableToOpenBtn.SetActive(false);
                unAbleToOpenBtn.SetActive(false);
            }
            resNameTxt.color = new Color(15f / 255f, 33f / 255f, 57f / 255f, 1f);
            resDesTxt.color = resNameTxt.color;

        } else {
            worldBG.sprite = bg[1];
            objCash.SetActive(false);
            unlockedStarField.SetActive(false);
            requireStar.SetActive(true);
            requireStarTxt.text = starToUnlock.ToString();
            int currentTotalStar = ProfileManager.Instance.playerData.GetTotalStarEarned();
            if (currentTotalStar >= starToUnlock) {
                travelBtn.SetActive(false);
                alreadyHereAlert.SetActive(false);
                ableToOpenBtn.SetActive(true);
                unAbleToOpenBtn.SetActive(false);
            } else {
                travelBtn.SetActive(false);
                alreadyHereAlert.SetActive(false);
                ableToOpenBtn.SetActive(false);
                unAbleToOpenBtn.SetActive(true);
            }
            resNameTxt.color = new Color(1f, 1f, 1f, 1f);
            resDesTxt.color = resNameTxt.color;
            requireStarTxt.color = resNameTxt.color;
            requireTxt.color = resNameTxt.color;
        }
    }

    void OnTravel() {
        travelBtn.gameObject.SetActive(false);
        GameManager.instance.LoadScene(selectedWorld);
    }
    void OnOpenWorld() {
        ableToOpenBtn.gameObject.SetActive(false);
        unlocked = true;
        ProfileManager.Instance.playerData.UnlockWorld(selectedWorld);
        OnTravel();
    }
}
