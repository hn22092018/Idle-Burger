using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WareHouseMergePoint : MonoBehaviour, IDropHandler
{
    [SerializeField] Color mergeBGColor1;
    [SerializeField] Color mergeBGColor2;
    [SerializeField] Image imgBG;
    public bool able;
    public bool colorDark = false;
    void Awake() {
        able = true;
    }
    public void ChangeColor() {
        if (colorDark) imgBG.color = mergeBGColor2;
        else imgBG.color = mergeBGColor1;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && able)
        {
            eventData.pointerDrag.transform.localPosition = transform.localPosition;
            WareHouseSlotMaterial wareHouseSlotMaterial = eventData.pointerDrag.GetComponent<WareHouseSlotMaterial>();
            if (wareHouseSlotMaterial.lastPoint != null)
                wareHouseSlotMaterial.lastPoint.able = true;
            wareHouseSlotMaterial.ChangePoint(this);
            able = false;
        }
    }
}
