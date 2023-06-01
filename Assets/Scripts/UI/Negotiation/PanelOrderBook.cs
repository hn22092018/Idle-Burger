using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PanelOrderBook : UIPanel {
    public static PanelOrderBook instance;
    public List<Sprite> sprCharacters;
    [SerializeField] Transform containerTrans;
    [SerializeField] GameObject uiInDeiveryPrefab;
    [SerializeField] List<UIOrderBookInDeivery> uiInDeiveryList;
    [SerializeField] UIOrderBookOffer uiOrderBookOffer;
    [SerializeField] UIOrderBookWaiting uiOrderBookWaiting;
    [SerializeField] Button btnClose;
    public override void Awake() {
        instance = this;
        panelType = UIPanelType.PanelOrderBook;
        base.Awake();
        btnClose.onClick.AddListener(OnClose);
    }
    void OnEnable() {
        ReloadUIInDelivery();
        ReloadUIOffer();
    }
    public void RemoveUIInDeivery(UIOrderBookInDeivery ui) {
        uiInDeiveryList.Remove(ui);
    }
    public void ReloadUIInDelivery() {
        uiInDeiveryPrefab.gameObject.SetActive(false);
        List<Order> activesOrders = ProfileManager.PlayerData.GetOrderBookManager().activeOrders;
        int countSpawn = activesOrders.Count - uiInDeiveryList.Count;
        for (int i = 0; i < countSpawn; i++) {
            Transform t = Instantiate(uiInDeiveryPrefab.transform, containerTrans);
            uiInDeiveryList.Add(t.GetComponent<UIOrderBookInDeivery>());
        }
        for (int i = 0; i < uiInDeiveryList.Count; i++) {
            if (i < activesOrders.Count) {
                uiInDeiveryList[i].gameObject.SetActive(true);
                uiInDeiveryList[i].Setup(activesOrders[i]);
            } else {
                uiInDeiveryList[i].gameObject.SetActive(false);
            }
        }
    }
    public void ReloadUIOffer() {
        if (ProfileManager.PlayerData.GetOrderBookManager().GetTimeToNewOffer() > 0 || ProfileManager.PlayerData.GetOrderBookManager().IsMaxOrder()) {
            uiOrderBookWaiting.gameObject.SetActive(true);
            uiOrderBookOffer.gameObject.SetActive(false);
            uiOrderBookWaiting.transform.SetAsLastSibling();
        } else {
            uiOrderBookWaiting.gameObject.SetActive(false);
            uiOrderBookOffer.gameObject.SetActive(true);
            uiOrderBookOffer.InitOffer();
            uiOrderBookOffer.transform.SetAsLastSibling();

        }

    }
    public void OnClose() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ClosePanelOrderBook();
    }
    public Sprite GetSpriteByName(string name) {
        Sprite spr = sprCharacters.Where(x => x.name == name).FirstOrDefault();
        return spr != null ? spr : sprCharacters[0];
    }
}
