using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class WareHousePreviewRewardPanel : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Transform trsPreview;
    [SerializeField] Transform trsSpawn;
    [SerializeField] AnimationCurve animGlass;
    [SerializeField] Image glassEffect;
    [SerializeField] float speedGlass;
    [SerializeField] UIObjectMove previewMove;
    bool glassLeft;
    bool glassRight;
    float timeAnim;
    UnityAction actionDone;
    public void InitPanel(Sprite spr, WarehouseSlotReward slotReward, UnityAction actionDone) { 
        icon.sprite = spr;
        glassEffect.sprite = spr;
        trsPreview.position = trsSpawn.position;
        trsPreview.localScale = new Vector3(0, 0, 0);
        this.actionDone = actionDone;
        trsPreview.DOScale(new Vector3(1, 1, 1), .25f).OnComplete(()=> {
            Setting();
        });
        previewMove.PrepareToMove(trsSpawn, slotReward.transform);
    }
    void Setting() {
        glassLeft = true;
        glassRight = false;
        glassEffect.fillAmount = 0;
        glassEffect.fillOrigin = 0;
        timeAnim = 0;
    }
    private void Update()
    {
        if (glassLeft) { GlassLeft(); }
        if (glassRight) { GlassRight(); }
    }
    void GlassLeft() {
        if (timeAnim < animGlass.keys[animGlass.length - 1].time)
        {
            glassEffect.fillAmount = animGlass.Evaluate(timeAnim);
            timeAnim += Time.deltaTime * speedGlass;
        }
        else
        {
            glassLeft = false;
            glassRight = true;
            timeAnim = 0;
            glassEffect.fillOrigin = 1;
        }
    }
    void GlassRight() {
        if (timeAnim < animGlass.keys[animGlass.length - 1].time)
        {
            glassEffect.fillAmount = animGlass.Evaluate(1 - timeAnim);
            timeAnim += Time.deltaTime * speedGlass;
        }
        else
        {
            glassRight = false;
            previewMove.StartMove(AffterMove);
            timeAnim = 0;
        }
    }
    void AffterMove() {
        trsPreview.DOPunchScale(new Vector3(.5f, .5f, 0), .25f).OnComplete(() =>
        {
            if (gameObject.activeSelf) StartCoroutine(IE_WaitSeconds());
        });
    }
    public void ResetPanel()
    {  
        glassRight = false;
        glassLeft = false;
        timeAnim = 0;
    }
    IEnumerator IE_WaitSeconds() {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        actionDone();
    }
}
