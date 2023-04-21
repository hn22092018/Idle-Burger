using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStarUpgradeProcess : MonoBehaviour {
    public static UIStarUpgradeProcess instance;
    [SerializeField] Sprite[] sprsRanking;
    [SerializeField] Image imgRanking;
    [SerializeField] Text txtRanking;
    [SerializeField] Image imgProcessCurrentMap;
    [SerializeField] GameObject notifyRank;

    int lastStar = 0;
    private void Awake() {
        instance = this;
        lastStar = ProfileManager.PlayerData.GetTotalStarEarned();
        UpdateStarUpgrade();
    }

    private void Update() {
        if (star >= 1 && ProfileManager.PlayerData.ResourceSave.countRewardRank < star) {
            notifyRank.gameObject.SetActive(true);
        } else {
            notifyRank.gameObject.SetActive(false);
        }
    }
    int star;
    float currentProcessPerStar = 0;
    public void UpdateStarUpgrade() {
        star = ProfileManager.PlayerData.GetTotalStarEarned();
        if (lastStar != star) {
            txtRanking.transform.localScale = Vector3.one;
            txtRanking.transform.DOPunchScale(new Vector3(.5f, .5f, 0), 0.4f, 1, 1).SetEase(Ease.Linear).OnComplete(() => {
                txtRanking.transform.localScale = Vector3.one;
            });
        }
        lastStar = star;
        if (star < 10) {
            imgRanking.sprite = sprsRanking[0];
        } else if (star < 30) {
            imgRanking.sprite = sprsRanking[1];
        } else {
            imgRanking.sprite = sprsRanking[2];
        }
        txtRanking.text = star.ToString();

        if (star == 0) {
            currentProcessPerStar = ProfileManager.PlayerData.GetTotalUpgradeProcess() % 40;
            imgProcessCurrentMap.fillAmount = currentProcessPerStar / 40;
        } else if (star == 1) {
            currentProcessPerStar = (ProfileManager.PlayerData.GetTotalUpgradeProcess() - 40) % 60;
            imgProcessCurrentMap.fillAmount = currentProcessPerStar / 60;
        } else {
            currentProcessPerStar = (ProfileManager.PlayerData.GetTotalUpgradeProcess() - 100) % 100;
            imgProcessCurrentMap.fillAmount = currentProcessPerStar / 100;
        }
    }

}
