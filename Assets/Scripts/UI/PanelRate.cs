using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelRate : UIPanel
{
    [SerializeField] Button closeBtn;
    public override void Awake() {
        panelType = UIPanelType.PanelRate;
        closeBtn.onClick.AddListener(OnClose);
        base.Awake();
    }
    public void OnRate() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        GameManager.instance.SaveRateGame();
#if UNITY_ANDROID
        //Application.OpenURL("market://details?id=" + Application.identifier);
        AppReviewManager.Instance.ShowReview();
#elif UNITY_IOS
 UnityEngine.iOS.Device.RequestStoreReview();
#endif
        UIManager.instance.ClosePanelRate();
    }
 
    public void OnClose() {
        SoundManager.instance.PlaySoundEffect(SoundID.BUTTON_CLICK);
        UIManager.instance.ClosePanelRate();
    }
    private void OnEnable() {
#if UNITY_ANDROID
        AppReviewManager.Instance.StartRequestReviewInfo();
#endif
    }
}
