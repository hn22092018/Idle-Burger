using SDK;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour {
    public static Tutorials instance;
    public UIFocusBlocker blocker;
    public bool IsShow;
    public bool IsBlockInput;
    [SerializeField] GameObject characterTutGr;
    [SerializeField] RectTransform rectMask;
    [SerializeField] RectTransform rectText;
    [SerializeField] Text txtTutorial;
    private int characterIndex = 0;
    private float timePerCharacter;
    float timer;
    private string textToWrite;
    List<TutorialStep> steps = new List<TutorialStep>();
    TutorialStep currentTut;
    public bool IsRunStory, IsReadyShowTutBuildTable, IsReadyShowIntroKitchen, IsFinishIntroKitchen, IsShowIntro;
    string s1 = "At last, our very own restaurant can be launched!. I can't wait. Let's open it and make delecious foods for everyone!";
    string s2 = "Oh, look! Our first customer!";
    string s3 = "Here our cooks will creat all the essential ingreadients needed for our foods.";
    string s4 = "They will continue to cook as soon as they have new customers";
    string s5 = "Oh! Look, we also completed a mission while doing so!";
    string s6 = "Missions are a good way to earn a juicy money boost for our restaurant! Complete as many as you can.";
    float delayTime = 0;
    private void Awake() {
        instance = this;
        blocker.Off();
        characterTutGr.SetActive(false);
        timePerCharacter = 0.06f;
        ProfileManager.Instance.dataConfig.tutorialConfig.Load();
        steps = ProfileManager.Instance.dataConfig.tutorialConfig.GetTutorialSteps();

    }

    private void Start() {
        if (!GameManager.instance.IsUnlockSmallTable(0) && ProfileManager.PlayerData.selectedWorld == 1 && !IsPassedStory()) {
            ProfileManager.Instance.dataConfig.tutorialConfig.Load();
            steps = ProfileManager.Instance.dataConfig.tutorialConfig.GetTutorialStepsDefault();
            StartCoroutine(IRunStory());
            Debug.Log("Install version " + Application.version);
            ABIFirebaseManager.Instance.SetUserProperty("Install_Version", Application.version);
        }

    }
    private void Update() {
        if (delayTime > 0) {
            delayTime -= Time.unscaledDeltaTime;
            if (Input.GetMouseButton(0) && delayTime > 0.5f) delayTime = 0.5f;
        }
        if (IsRunStory) return;
        if (UIManager.instance.IsHideUI) return;
        if (UIManager.instance.isHasPopupOnScene) return;
        foreach (var step in steps) {
            if (step.IsEnoughConditionShow() && !IsShow) {
                IsShow = true;
                currentTut = step;
                StartCoroutine(IShowTut());
                break;
            }
        }
    }
    IEnumerator IRunStory() {
        UIManager.instance.HideOrShowUI(false);
        IsRunStory = true;
        IsShow = true;
        IsBlockInput = true;
        CameraMove.instance.Story1();
        yield return new WaitForSeconds(1f);
        string sInfo = "";
        {
            Debug.Log("Start Story 1");
            // Story 1. Giới thiệu vị khách đầu tiên, khách đi tới quầy lễ tân để đặt bàn và đợi story 2
            characterTutGr.SetActive(true);
            characterTutGr.GetComponent<Animator>().SetTrigger("IsShow");
            sInfo = s1;
            sInfo = ProfileManager.Instance.dataConfig.GameText.GetTextByID(423);
            AddTextWriter(sInfo);
            while (characterIndex < textToWrite.Length) {
                ShowText();
                yield return new WaitForEndOfFrame();
            }
            delayTime = 2;
            while (delayTime > 0) {
                yield return new WaitForEndOfFrame();
            }
            characterTutGr.SetActive(false);
            yield return new WaitForSeconds(1f);
            characterTutGr.SetActive(true);
            characterTutGr.GetComponent<Animator>().SetTrigger("IsShow2");
            sInfo = s2;
            sInfo = ProfileManager.Instance.dataConfig.GameText.GetTextByID(424);
            AddTextWriter(sInfo);
            while (characterIndex < textToWrite.Length) {
                ShowText();
                yield return new WaitForEndOfFrame();
            }
            delayTime = 2;
            while (delayTime > 0) {
                yield return new WaitForEndOfFrame();
            }
            characterTutGr.SetActive(false);
            GameManager.instance.SpawnCustomer_Story();
            Debug.Log("Finish Story 1");
            // story 1 kết thúc khi khách hàng đến nói chuyện với lễ tân.
        }
        {
            // story 2: Sau khi khách đi tới quầy tiếp tân. Hiển thị tutorial  build table
            // Trigger in CustomerOrderTableState
            Debug.Log("Start Story 2");
            Debug.Log("-----Story 2----Waiting Customer To Lobby....");
            while (!IsReadyShowTutBuildTable) {
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("-----Story 2----Show Tut BuildTable ");
            // Get Tutorial Build Table in config
            currentTut = GetTutorial(TutorialStepID.BuildTable, 1);
            for (int i = 0; i < currentTut.textLocalizeIds.Count; i++) {

                characterTutGr.SetActive(true);
                characterTutGr.GetComponent<Animator>().SetTrigger("IsShow" + (Random.Range(0, 2) == 0 ? "" : 2));
                sInfo = ProfileManager.Instance.dataConfig.GameText.GetTextByID(currentTut.textLocalizeIds[i]);
                AddTextWriter(sInfo);
                while (characterIndex < textToWrite.Length) {
                    ShowText();
                    yield return new WaitForEndOfFrame();
                }
                delayTime = 2;
                while (delayTime > 0) {
                    yield return new WaitForEndOfFrame();
                }
                characterTutGr.SetActive(false);
                delayTime = 1;
                while (delayTime > 0) {
                    yield return new WaitForEndOfFrame();
                }
            }
            CameraMove.instance.StopFollowStory();
            CameraMove.instance.ChangePosition(currentTut.cameraPos, () => {
                ChangeBlockerPos(currentTut.blockerCenterPos);
            });
            characterTutGr.SetActive(false);
            Debug.Log("Finish Story 2");
           
            // kết thúc story 2 khi người chơi đã mua thành công table 1. 
            // CustomerOrderTableState continue task order.
        }
        // Wait... to customer start state CustomerUseFoodState and sit in table.
        {
            while (!IsReadyShowIntroKitchen) {
                // wait tut 
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("Start Story 4");
            Debug.Log("-----Story 4----Intro Kitchen");
            // Story 4
            // Intro kitchen, action cooking.
            IsBlockInput = true;
            yield return new WaitForSeconds(1f);
            Transform t = GameManager.instance.KitchenRoom.staffSetting.listStaffTrans[0];
            CameraMove.instance.SetTargetFollow(t);
            delayTime = 2;
            while (delayTime > 0) {
                yield return new WaitForEndOfFrame();
            }
            characterTutGr.SetActive(true);
            characterTutGr.GetComponent<Animator>().SetTrigger("IsShow");
            sInfo = s3;
            sInfo = ProfileManager.Instance.dataConfig.GameText.GetTextByID(425);
            AddTextWriter(sInfo);
            while (characterIndex < textToWrite.Length) {
                ShowText();
                yield return new WaitForEndOfFrame();
            }
            delayTime = 2;
            while (delayTime > 0) {
                yield return new WaitForEndOfFrame();
            }
            characterTutGr.SetActive(false);

            yield return new WaitForSeconds(1f);
            characterTutGr.SetActive(true);
            characterTutGr.GetComponent<Animator>().SetTrigger("IsShow2");
            sInfo = s4;
            sInfo = ProfileManager.Instance.dataConfig.GameText.GetTextByID(426);
            AddTextWriter(sInfo);
            while (characterIndex < textToWrite.Length) {
                ShowText();
                yield return new WaitForEndOfFrame();
            }
            delayTime = 2;
            while (delayTime > 0) {
                yield return new WaitForEndOfFrame();
            }
            characterTutGr.SetActive(false);
            IsFinishIntroKitchen = true;
            Debug.Log("Finish Story 4");
            IsBlockInput = false;
        }
        {
            while (IsRunStory) {
                yield return new WaitForSeconds(1);
            }
            PassedStory();
        }
    }
    TutorialStep GetTutorial(TutorialStepID tutorialStepID, int level) {
        foreach (var step in steps) {
            if (step.stepID == tutorialStepID && step.levelRequire == level) return step;
        }
        return null;
    }
    void PassedStory() {
        CameraMove.instance.StopFollow();
        IsBlockInput = false;
        IsShow = false;
        UIManager.instance.HideOrShowUI(true);
        PlayerPrefs.SetInt("IsPassStory", 1);
    }
    bool IsPassedStory() {
        return PlayerPrefs.GetInt("IsPassStory") == 1;
    }

    IEnumerator IShowTut() {
        IsShow = true;
        IsBlockInput = true;
        UIManager.instance.CloseAllPopup();
        txtTutorial.color = Color.white;
        txtTutorial.alignment = TextAnchor.UpperLeft;
        string sInfo = "";
        for (int i = 0; i < currentTut.textLocalizeIds.Count; i++) {
            if (i == 0) {
                characterTutGr.SetActive(true);
                characterTutGr.GetComponent<Animator>().SetTrigger("IsShow");
            }
            sInfo = ProfileManager.Instance.dataConfig.GameText.GetTextByID(currentTut.textLocalizeIds[i]);
            if (currentTut.stepID == TutorialStepID.SelectNewWorld) {
                sInfo = string.Format(sInfo, ProfileManager.Instance.dataConfig.worldDataAsset.GetDataByLevel(2).starNeededToUnlock);
            }
            AddTextWriter(sInfo);
            while (characterIndex < textToWrite.Length) {
                ShowText();
                yield return new WaitForEndOfFrame();
            }
            delayTime = 2;
            while (delayTime > 0) {
                yield return new WaitForEndOfFrame();
            }
        }
        characterTutGr.SetActive(false);
        if (currentTut.IsTutNonUI) {
            CameraMove.instance.ChangePosition(currentTut.cameraPos, () => {
                ChangeBlockerPos(currentTut.blockerCenterPos);
            });
        } else {
            if (currentTut.stepID == TutorialStepID.ClaimQuest)
                OnShow(UIManager.instance.btnMission.transform);
            else if (currentTut.stepID == TutorialStepID.ActiveMarketingCampaign)
                OnShow(UIManager.instance.btnMarketingCampaign.transform);
            else if (currentTut.stepID == TutorialStepID.AdBoost)
                OnShow(UIManager.instance.btnAdBoost.transform);
            else if (currentTut.stepID == TutorialStepID.SelectNewWorld)
                OnShow(UIManager.instance.btnStatistic.transform);
            else if (currentTut.stepID == TutorialStepID.Research)
                OnShow(UIManager.instance.btnTech.transform);
        }
        UIManager.instance.CloseAllPopup();
    }

    public void ShowIntro(List<string> texts) {
        StartCoroutine(IShowIntro(texts));
    }
    IEnumerator IShowIntro(List<string> texts) {
        IsShowIntro = true;
        IsBlockInput = true;
        for (int i = 0; i < texts.Count; i++) {
            if (i == 0) {
                characterTutGr.SetActive(true);
                characterTutGr.GetComponent<Animator>().SetTrigger("IsShow" + (Random.Range(0, 2) == 0 ? "" : 2));
            }
            AddTextWriter(texts[i]);
            while (characterIndex < textToWrite.Length) {
                ShowText();
                yield return new WaitForEndOfFrame();
            }
            delayTime = 2;
            while (delayTime > 0) {
                yield return new WaitForEndOfFrame();
            }
        }
        characterTutGr.SetActive(false);
        IsBlockInput = false;
        IsShowIntro = false;
    }
    public void ShowTutWakeupStaff() {
        currentTut = steps.Where(x => x.stepID == TutorialStepID.WakeupStaff).ToList()[0];
        StartCoroutine(IShowTut());
    }

    void ShowText() {
        if (Input.GetMouseButton(0)) {
            characterIndex += textToWrite.Length;
            txtTutorial.text = textToWrite;
        }
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer += timePerCharacter;
            characterIndex++;
            if (characterIndex >= textToWrite.Length) {
                characterIndex = textToWrite.Length;
            };
            float preferredSizeText = LayoutUtility.GetPreferredSize(rectText, (int)RectTransform.Axis.Vertical);
            if (preferredSizeText >= rectMask.rect.height) {
                rectText.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredSizeText);
            }
            txtTutorial.text = textToWrite.Substring(0, characterIndex);
        }
    }
    void AddTextWriter(string text) {
        timer = 0;
        characterIndex = 0;
        textToWrite = text;
        txtTutorial.text = "";
        rectText.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectMask.rect.height);
    }
    public void ChangeBlockerPos(Vector2 pos, float delayTime = 0.3f, int size = 300) {
        StartCoroutine(IDelayShow(pos, delayTime, size));
    }

    public void UpdateBlockerPos(Vector2 pos, int size = 300) {
        blocker.OnChangePos(pos, size);
    }


    IEnumerator IDelayShow(Vector2 pos, float delayTime, int size = 300) {
        IsBlockInput = true;
        blocker.Off();
        blocker.OnChangePos(pos, size);
        yield return new WaitForSeconds(delayTime);
        blocker.OnShow();
        IsBlockInput = false;
    }

    public void FinishTutorial() {
        if (currentTut != null) {
            currentTut.FinishTut();
            steps.Remove(currentTut);
        }
    }
    public void OnCloseTutorial() {
        if (!IsRunStory) IsShow = false;
        blocker.Off();
    }
    public void OffBlocker() {
        blocker.Off();
    }
    public void OnShow(Transform target, int size = 250) {
        blocker.OnShow();
        blocker.SetPivot(target, size);
        IsBlockInput = false;
    }
    public TutorialStepID GetTutorialStep() {
        return currentTut != null ? currentTut.stepID : TutorialStepID.None;
    }
    public bool IsActiveOnScene() {
        return IsShow || blocker.isActiveAndEnabled;
    }

}
