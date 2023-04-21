using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestItem : MonoBehaviour {
    Quest currentQuest;
    [SerializeField] Text QDetail;
    [SerializeField] Text QReward;
    [SerializeField] Image RewardImg;
    [SerializeField] Sprite gemIcon;
    [SerializeField] Sprite cashIcon;
    [SerializeField] Button goBtn;
    [SerializeField] Button claimBtn;
    [SerializeField] GameObject doneMark;

    public GameObject gemReward;
    public GameObject cashReward;
    public Transform rewardContainer;

    private void Awake() {
        goBtn.onClick.AddListener(() => {
            OnDoQuest();
        });
        claimBtn.onClick.AddListener(() => {
            OnClaim();
        });
    }

    public void LoadQuestInfo(Quest toActiveQuest) {
        // Load Quest prefab
        currentQuest = toActiveQuest;
        QDetail.text = currentQuest.GetDes();
        QReward.text = currentQuest.reward.amount.ToString();
        if (currentQuest.reward.type == ItemType.Gem) {
            RewardImg.sprite = gemIcon;
        } else if (currentQuest.reward.type == ItemType.Cash) {
            RewardImg.sprite = cashIcon;
        }
        LoadQuestState();
    }
    private void OnEnable() {
        if (currentQuest != null)
            QDetail.text = currentQuest.GetDes();
    }

    public void LoadQuestState() {
        goBtn.gameObject.SetActive(false);
        claimBtn.gameObject.SetActive(false);
        doneMark.SetActive(false);
        if (ProfileManager.PlayerData.GetQuestManager().IsQuestClaimed(currentQuest)) {
            doneMark.SetActive(true);
        } else if (IsQuestClaimable()) {
            claimBtn.interactable = true;
            claimBtn.gameObject.SetActive(true);
        } else goBtn.gameObject.SetActive(true);
    }


    public void OnClaim() {
        claimBtn.interactable = false;
        SoundManager.instance.PlaySoundEffect(SoundID.QUEST_CLAIM);
        ProfileManager.PlayerData.GetQuestManager().ClaimQuest(currentQuest);
        SpawnReward();
        PanelQuest.instance.ReplaceQuestUI(this);

    }

    public void OnDoQuest() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        QuestHelperUI.instance.OnGoQuest(currentQuest);
    }
    public void SpawnReward() {
        // Spawn Gem or Cash fly to gem or card box
        switch (currentQuest.reward.type) {
            case ItemType.Gem: {
                    for (int i = 0; i < 10; i++) {
                        GameObject rewardG = Instantiate(gemReward, this.gameObject.transform.parent);
                        rewardG.transform.position = RewardImg.gameObject.transform.position;
                        rewardG.GetComponent<QuestReward>().rewardBoxRT = UIManager.instance.gemRewardPos;
                        rewardG.SetActive(true);
                        rewardG.transform.SetParent(rewardContainer);
                    }
                }
                break;
            case ItemType.Cash: {
                    for (int i = 0; i < 10; i++) {
                        GameObject rewardC = Instantiate(cashReward, this.gameObject.transform.parent);
                        rewardC.transform.position = RewardImg.gameObject.transform.position;
                        rewardC.GetComponent<QuestReward>().rewardBoxRT = UIManager.instance.cashRewardPos;
                        rewardC.SetActive(true);
                        rewardC.transform.SetParent(rewardContainer);
                    }

                }
                break;
            default:
                // code block
                break;
        }
    }
    public bool IsQuestClaimable() {
        return ProfileManager.PlayerData.GetQuestManager().IsQuestClaimable(currentQuest);
    }
    public Transform GetClaimBtnTranform() {
        return claimBtn.transform;
    }
}
