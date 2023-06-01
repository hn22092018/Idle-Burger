using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDeliverCarOrderIcon : MonoBehaviour {
    public List<GameObject> objOrders;
    public List<Image> imgIcons;
    public List<Text> txtValues;
    // Start is called before the first frame update
    private void Update() {
        transform.eulerAngles = new Vector3(40, 45, 0);
    }
    public void ShowOrder(List<DeliverCarOrder> cusOrders) {
        for (int i = 0; i < objOrders.Count; i++) {
            objOrders[i].SetActive(false);
        }
        for (int i = 0; i < cusOrders.Count; i++) {
            objOrders[i].SetActive(true);
            imgIcons[i].sprite = cusOrders[i].foodOrder.foodIcon;
            txtValues[i].text = cusOrders[i].amount + "";
        }

    }
    public void ShowMenu() {
        UIManager.instance.ShowPanelTech();
    }
}
