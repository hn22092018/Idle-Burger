using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeTravelSlot : UIShopSlot {
    [SerializeField] GameObject objHaveTicket;
    [SerializeField] Text txtCash;
    [SerializeField] Text txtTicket;
    [SerializeField] Image imgButtonGem;
    [SerializeField] Image imgEnoughGem;
    [SerializeField] Sprite sprEnough, sprNotenough;
    [SerializeField] GameObject objTicketCount;
    [SerializeField] Text txtTicketCount;
    int reward;
    int price;
    public int currentTicket;
    OfferData currentOfferData;
    public override void InitData(OfferData offerData) {
        base.InitData(offerData);
        currentOfferData = offerData;
        price = (int)offerData.price;
        switch (currentOfferData.offerID) {
            case OfferID.TimeTrave_30M:
                titleText.text = "30M";
                break;
            case OfferID.TimeTrave_60M:
                titleText.text = "1H";
                break;
            case OfferID.TimeTrave_2H:
                titleText.text = "2H";
                break;
        }
    }
    private void OnEnable() {
        if (currentOfferData == null) return;
        switch (currentOfferData.offerID) {
            case OfferID.TimeTrave_30M:
                titleText.text = "30M";
                reward = GameManager.instance.GetCashProfit(30).ToIntValue();
                break;
            case OfferID.TimeTrave_60M:
                titleText.text = "1H";
                reward = GameManager.instance.GetCashProfit(60).ToIntValue();
                break;
            case OfferID.TimeTrave_2H:
                titleText.text = "2H";
                reward = GameManager.instance.GetCashProfit(120).ToIntValue();
                break;
        }
    }
    void GetTicket() {
        switch (currentOfferData.offerID) {
            case OfferID.TimeTrave_30M:
                titleText.text = "30M";
                currentTicket = ProfileManager.PlayerData.ResourceSave.GetTimeSkipTicket_1H();
                if (reward == 0 || IsUpdateReward()) reward = GameManager.instance.GetCashProfit(30).ToIntValue();
                break;
            case OfferID.TimeTrave_60M:
                titleText.text = "1H";
                currentTicket = ProfileManager.PlayerData.ResourceSave.GetTimeSkipTicket_4H();
                if (reward == 0 || IsUpdateReward()) reward = GameManager.instance.GetCashProfit(60).ToIntValue();
                break;
            case OfferID.TimeTrave_2H:
                titleText.text = "2H";
                currentTicket = ProfileManager.PlayerData.ResourceSave.GetTimeSkipTicket_24H();
                if (reward == 0 || IsUpdateReward()) reward = GameManager.instance.GetCashProfit( 120).ToIntValue();
                break;
        }
        objHaveTicket.gameObject.SetActive(currentTicket > 0);
        objTicketCount.SetActive(currentTicket > 0);
        txtTicketCount.text = currentTicket.ToString();
        txtCash.text = "+" + new BigNumber(reward).IntToString();
    }
    public override void OnBuy() {
        base.OnBuy();
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        if (currentTicket > 0) {
            switch (currentOfferData.offerID) {
                case OfferID.TimeTrave_30M:
                    ProfileManager.PlayerData.ResourceSave.ConsumeTimeSkipTicket_1H(1);
                    ProfileManager.PlayerData.AddCash(reward);
                    break;
                case OfferID.TimeTrave_60M:
                    ProfileManager.PlayerData.ResourceSave.ConsumeTimeSkipTicket_4H(1);
                    ProfileManager.PlayerData.AddCash(reward);
                    break;
                case OfferID.TimeTrave_2H:
                    ProfileManager.PlayerData.ResourceSave.ConsumeTimeSkipTicket_24H(1);
                    ProfileManager.PlayerData.AddCash(reward);
                    break;
            }
            ShowReward();
        } else {
            if (GameManager.instance.IsEnoughGem(price)) {
                ProfileManager.Instance.playerData.ConsumeGem(price);
                ProfileManager.PlayerData.AddCash(reward);
                ShowReward();
                ABIAnalyticsManager.Instance.TrackEventGem(GemAction.Spend_TimeTravel, price);
            }
        }
    }

    void ShowReward() {
        List<ItemReward> rewards = new List<ItemReward>();
        ItemReward newReward = new ItemReward();
        newReward.type = ItemType.Cash;
        newReward.amount = reward;
        newReward.spr = ProfileManager.Instance.dataConfig.shopConfig.GetSpriteByName("icon money");
        rewards.Add(newReward);
        UIManager.instance.ShowUIPanelReward(rewards);
    }

    float timeUpdateReward;
    private void Update() {
        timeUpdateReward += Time.deltaTime;
        imgButtonGem.sprite = GameManager.instance.IsEnoughGem(price) ? sprEnough : sprNotenough;
        GetTicket();
    }
    bool IsUpdateReward() {
        if (timeUpdateReward >= 60) {
            timeUpdateReward = 0;
            return true;
        }
        return false;
    }
}
