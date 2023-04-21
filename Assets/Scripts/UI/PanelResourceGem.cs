using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelResourceGem : MonoBehaviour
{
    public static PanelResourceGem instance;
    public Text txtGem;
    [SerializeField] Button goToGemOffers;
    private void Awake() {
        instance = this;
    }
    void Start() {
        UpdateGem();
        EventManager.AddListener(EventName.UpdateGem.ToString(), UpdateGem);
        GetComponent<Button>().onClick.AddListener(() => {
            UIManager.instance.ShowPanelShop();
        });
        goToGemOffers.onClick.AddListener(GotoGemOffer);
    }
    void UpdateGem() {
        txtGem.text = GameManager.instance.GetGem().ToString();
    }
    void GotoGemOffer() {
        UIManager.instance.GotoShopPage(TabName.Gems);
    }
}
