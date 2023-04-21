using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public List<UIShopPagePosY> shopPagePosY;
    private TabName shopNameTarget = TabName.None;
    bool tabScroll;
    Vector2 targetPos;
    bool canScroll = false;
    float spacecingLayout;
    float spacingTop;
    [SerializeField] GameObject blockScroll;
    [SerializeField] AnimationCurve moveCurve;
    float timeMove = 0f;
    Vector2 currentPosition;
    private void Awake()
    {
        scrollRect.onValueChanged.AddListener(OnChangeValueScroll);
        spacecingLayout = scrollRect.content.GetComponent<VerticalLayoutGroup>().spacing;
        spacingTop = scrollRect.content.GetComponent<VerticalLayoutGroup>().padding.top;
    }
    private void Update()
    {
        /// <summary>
        /// check scrollRect has change ? resetting position of shop page
        /// check index of target != -1 ? change target position;
        /// </summary>
        if (scrollRect.content.hasChanged)
        {
            canScroll = true;
            UIShopManager.instance.ReBuildUI();
        }
        /// <summary>
        /// check target position if position Y>=0 ==> scroll content to target.
        /// </summary>
        if (timeMove <= moveCurve.keys[moveCurve.keys.Length - 1].time && targetPos.y >= 0)
        {
            tabScroll = true;
            blockScroll.gameObject.SetActive(true);
            //scrollRect.content.anchoredPosition = Vector2.SmoothDamp(scrollRect.content.anchoredPosition, targetPos, ref refVector, scrollTime);
            scrollRect.content.anchoredPosition = Vector2.Lerp(currentPosition, targetPos, moveCurve.Evaluate(timeMove));
            timeMove += Time.deltaTime;
        }
        else if(targetPos.y >= 0)
        {
            scrollRect.content.anchoredPosition = targetPos;
            tabScroll = false;
            targetPos = new Vector2(0, -1);
            blockScroll.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Change target of scroll view
    /// </summary>
    /// <param name="index"></param>
    public void ChangeTarget(TabName shopName)
    {
        if (shopPagePosY.Count == 0)
            return;
        int indexTarget = GetIShopPagePosY(shopName);
        shopNameTarget = shopName;
        float y = -shopPagePosY[indexTarget].shopPageRect.anchoredPosition.y - spacecingLayout - 100;
        float max = -shopPagePosY[shopPagePosY.Count - 1].shopPageRect.anchoredPosition.y - shopPagePosY[shopPagePosY.Count - 1].shopPageRect.GetComponent<UIShopPage>().GetPageHeight() - spacecingLayout;
        if (indexTarget == 0) y = 0;
        else if (y >= max) y = max;
        targetPos = new Vector2(0, y);
        if (scrollRect.content.anchoredPosition.y < 0)
            currentPosition = Vector2.zero;
        else
            currentPosition = scrollRect.content.anchoredPosition;
        timeMove = 0f;
        Debug.Log("Current = " + currentPosition + "----------Target = " + targetPos);
    }
    /// <summary>
    /// when scroll content change value check content position if it > shop page position ==> activate tab button.
    /// </summary>
    /// <param name="pos"></param>
    void OnChangeValueScroll(Vector2 pos)
    {
        if (tabScroll)
            return;
        for (int i = 0; i < shopPagePosY.Count; i++)
        {
            UIShopTabButton tab = UIShopManager.instance.tabGroup.GetTabButton(shopPagePosY[i].shopPageName);
            float scrollRectY = scrollRect.content.anchoredPosition.y;
            if (i == shopPagePosY.Count - 1)
            {
                float max = -shopPagePosY[shopPagePosY.Count - 1].shopPageRect.anchoredPosition.y - shopPagePosY[shopPagePosY.Count - 1].shopPageRect.GetComponent<UIShopPage>().GetPageHeight() - spacecingLayout;
                if (scrollRectY >= max)
                    UIShopManager.instance.tabGroup.OnSelected(tab, false);
                else UIShopManager.instance.tabGroup.OnDeselected();

            }
            else
            {
                if (scrollRectY < -shopPagePosY[i].shopPageRect.anchoredPosition.y + shopPagePosY[i].shopPageRect.GetComponent<UIShopPage>().GetPageHeight() * 0.5f && scrollRectY >= -shopPagePosY[i].shopPageRect.anchoredPosition.y - spacecingLayout - spacingTop)
                {
                    UIShopManager.instance.tabGroup.OnSelected(tab, false);
                }
                else UIShopManager.instance.tabGroup.OnDeselected();
            }

        }
    }
    public int GetIndexSelected(TabName tabName) { return GetIShopPagePosY(tabName); }
    public void ResetScroll()
    {
        targetPos = Vector2.zero;
        scrollRect.content.anchoredPosition = targetPos;
        tabScroll = false;
    }
    public int GetIShopPagePosY(TabName tabName)
    {
        for (int i = 0; i < shopPagePosY.Count; i++)
        {
            if (tabName == shopPagePosY[i].shopPageName)
            {
                return i;
            }
        }
        return 0;
    }
    public bool CheckCanScroll()
    {
        return canScroll;
    }
}