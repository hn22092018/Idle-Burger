using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelRewardItem : UIPanel {
    [SerializeField] Button btnClose;
    [SerializeField] Transform rootItems;
    [SerializeField] UIItemReward prefab;
    [SerializeField] GridLayoutGroup gridLayout;
    List<UIItemReward> uIItemRewards = new List<UIItemReward>();
    Vector2 gridCellSize;

    public override void Awake() {
        panelType = UIPanelType.PanelRewardItem;
        base.Awake();
        btnClose.onClick.AddListener(OnClose);
        gridCellSize = gridLayout.cellSize;
    }
    public void ShowItem(List<ItemReward> items, bool hasSkin = false) {
        transform.SetAsLastSibling();
        SetCollum(items.Count);
        SetCellSize(items.Count);
        btnClose.gameObject.SetActive(false);
        uIItemRewards.Clear();
        while (rootItems.childCount < items.Count) {
            Instantiate(prefab.transform, rootItems);
        }
        for (int i = 0; i < rootItems.childCount; i++) {
            rootItems.GetChild(i).gameObject.SetActive(true);
            rootItems.GetChild(i).transform.localScale = Vector3.zero;
            if (i < items.Count) {
                UIItemReward ui = rootItems.GetChild(i).GetComponent<UIItemReward>();
                ui.Setup(items[i]);
                uIItemRewards.Add(ui);
            } else {
                rootItems.GetChild(i).gameObject.SetActive(false);
            }
        }
        StartCoroutine(IShowItem());

    }
    IEnumerator IShowItem() {
        for (int i=0;i< uIItemRewards.Count;i++) {
            uIItemRewards[i].transform.localScale = Vector3.zero;
            uIItemRewards[i].gameObject.SetActive(true);
            uIItemRewards[i].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1f);
        btnClose.gameObject.SetActive(true);
    }
    void OnClose() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.CloseUIPanelReward();
    }

    void SetCollum(int amount)
    {
        if (amount <= 2) gridLayout.constraintCount = 2;
        else if (amount <= 3) gridLayout.constraintCount = 3;
        else if (amount <= 4) gridLayout.constraintCount = 2;
        else gridLayout.constraintCount = 3;
    }
    void SetCellSize(int amount)
    {
        if(amount < 5)
        {
            gridLayout.cellSize = gridCellSize;
        }
        else
        {
            gridLayout.cellSize = gridCellSize * 0.85f;
        }
    }
}
