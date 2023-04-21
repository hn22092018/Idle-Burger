using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDK;

public class WareHouseWatchVideoPanel : MonoBehaviour {
    [SerializeField] Button btnWatch;
    [SerializeField] Button btnTicket;
    [SerializeField] Button btnGem;
    [SerializeField] Button btnClose;
    [SerializeField] Text txtGemPrice;
    int GemPrice = 20;
    public void Start() {
        btnTicket.onClick.AddListener(SkipByTicket);
        btnWatch.onClick.AddListener(WatchVideo);
        btnGem.onClick.AddListener(SkipByGem);
        btnClose.onClick.AddListener(Close);
        txtGemPrice.text = GemPrice.ToString();
    }
    void WatchVideo() {
        if (!ProfileManager.PlayerData.ResourceSave.activeRemoveAds) {
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.WareHouse_Energy.ToString(), AddEnergy);
            ABIAnalyticsManager.Instance.TrackEventWareHouse(WareHouseAction.WatchAds_BuyEnergy);
        } else AddEnergy();
    }
    void SkipByTicket() {
        AddEnergy();
        ProfileManager.PlayerData.ResourceSave.ConsumeADTicket();
        ABIAnalyticsManager.Instance.TrackEventWareHouse(WareHouseAction.Ticket_BuyEnergy);
    }
    void SkipByGem() {
        AddEnergy();
        ProfileManager.PlayerData.ConsumeGem(GemPrice);
        ABIAnalyticsManager.Instance.TrackEventWareHouse(WareHouseAction.UseGem_BuyEnergy);
        ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Spend_WareHouse_MoreEnergy, GemPrice);
    }
    void AddEnergy() {
        ProfileManager.PlayerData.wareHouseManager.ChangeCurrentEnergy(10, true);
        Close();
    }
    void Update() {
        bool checkTicket = ProfileManager.PlayerData.ResourceSave.adTicket > 0;
        btnWatch.gameObject.SetActive(!checkTicket);
        btnTicket.gameObject.SetActive(checkTicket);
        btnGem.interactable = GameManager.instance.IsEnoughGem(GemPrice);
    }
    void Close() {
        gameObject.SetActive(false);
    }
}
