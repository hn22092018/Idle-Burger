using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFoodOrderIcon : MonoBehaviour
{
    [SerializeField] Image icon_img;
    public void UpdateAngle() {
        transform.eulerAngles = new Vector3(40, 45, 0);
    }
    public void ShowIcon(Sprite spr) {
        icon_img.sprite = spr;
    }
    public void ShowMenu() {
        UIManager.instance.ShowPanelTech();
    }
  
}
