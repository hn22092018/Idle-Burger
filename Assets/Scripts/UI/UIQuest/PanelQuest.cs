using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PanelQuest : UIPanel {
    public static PanelQuest instance;
    [SerializeField] Button btnClose;
    [SerializeField] GameObject questPrefab;
    [SerializeField] Transform questContainer;
    [SerializeField] Transform noneOfQuest;
    public List<UIQuestItem> uiQuestItems;

    public override void Awake() {
        panelType = UIPanelType.PanelQuest;
        base.Awake();
        instance = this;
        btnClose.onClick.AddListener(() => {
            SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
            OnClose();
        });
        LoadAllQuest();
    }

    void OnEnable() {
        // Check if any of current quests have been done yet
        for (int i = 0; i < uiQuestItems.Count; i++) {
            if (uiQuestItems[i] != null) {
                uiQuestItems[i].LoadQuestState();
                uiQuestItems[i].transform.localScale = Vector3.zero;
                uiQuestItems[i].transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.Linear).SetDelay(0.2f + 0.1f * i);
            }
        }
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.ClaimQuest) {
            StartCoroutine(ICheckShowTut());
        }
    }
    IEnumerator ICheckShowTut() {
        bool IsHasQuestClaimTut = false;
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < uiQuestItems.Count; i++) {
            if (uiQuestItems[i].IsQuestClaimable()) {
                IsHasQuestClaimTut = true;
                Tutorials.instance.OnShow(uiQuestItems[i].GetClaimBtnTranform());
                break;
            }
        }
        if (!IsHasQuestClaimTut) {
            Tutorials.instance.OnCloseTutorial();
            Tutorials.instance.FinishTutorial();
        }
    }
    public void LoadAllQuest() {
        // Load all current quest
        // Run when start game
        for (int i = 0; i < 3; i++) {
            Quest newQuest = ProfileManager.PlayerData.GetQuestManager().GetNextQuests();
            if (newQuest != null) {
                GameObject quest = Instantiate(questPrefab, questContainer);
                quest.SetActive(true);
                UIQuestItem ui = quest.GetComponent<UIQuestItem>();
                ui.LoadQuestInfo(newQuest);
                uiQuestItems.Add(ui);
            }
        }
        CheckNonOfQuest();
    }

    public void ReplaceQuestUI(UIQuestItem questUI) {
        if (Tutorials.instance.IsShow && Tutorials.instance.GetTutorialStep() == TutorialStepID.ClaimQuest) {
            Tutorials.instance.OnCloseTutorial();
            Tutorials.instance.FinishTutorial();
            string text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(17);
            Tutorials.instance.ShowIntro(new List<string> { text });

        }
        questUI.transform.localScale = Vector3.zero;
        Quest newQuest = ProfileManager.PlayerData.GetQuestManager().GetNextQuests();
        if (newQuest != null) {
            questUI.LoadQuestInfo(newQuest);
            questUI.transform.SetAsLastSibling();
            questUI.transform.DOScale(Vector2.one, 0.2f);
        } else {
            Destroy(questUI.gameObject);
        }
        CheckNonOfQuest();
    }

    void CheckNonOfQuest() {
        bool IsHasQuest = false;
        foreach (var quest in uiQuestItems) {
            if (quest != null && quest.gameObject.activeInHierarchy) {
                IsHasQuest = true;
                break;
            }
        }
        noneOfQuest.gameObject.SetActive(!IsHasQuest);
    }
    public void OnClose() {
        UIManager.instance.ClosePanelQuest();
    }
}
