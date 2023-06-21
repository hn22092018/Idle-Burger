using SDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelBuyResearchSlot : UIEffect {
    [SerializeField] Button btnBuySlot, btnClose;
    EResearchSlotIndex currentSlotUnlock;
    int gemPriceUnlock = 50;
    private void Awake() {
        btnBuySlot.onClick.AddListener(OnUnlockNewSlot);
        btnClose.onClick.AddListener(OnClose);
    }
  
    public void Init(EResearchSlotIndex index) {
        currentSlotUnlock = index;
        switch (currentSlotUnlock) {
            case EResearchSlotIndex.Slot2:
                gemPriceUnlock = 50;
                break;
            case EResearchSlotIndex.Slot3:
                gemPriceUnlock = 70;
                break;
            case EResearchSlotIndex.Slot4:
                gemPriceUnlock = 100;
                break;
        }

        btnBuySlot.interactable = GameManager.instance.IsEnoughGem(gemPriceUnlock);
    }
    void OnUnlockNewSlot() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        ProfileManager.PlayerData.researchManager.UnlockResearchSlot(currentSlotUnlock);
        ProfileManager.PlayerData.ConsumeGem(gemPriceUnlock);
        ScaleEffectButton(btnBuySlot, () => {
            gameObject.SetActive(false);
        });
        ABIAnalyticsManager.Instance.TrackEventResearch(ResearchAction.UseGem_BuySlot, gemPriceUnlock.ToString());

    }
    void OnClose() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        gameObject.SetActive(false);
    }
}
