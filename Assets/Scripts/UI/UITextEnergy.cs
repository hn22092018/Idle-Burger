using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextEnergy : MonoBehaviour {
    [SerializeField] Button btnGoRoom;
    [SerializeField] Text txtEnergy;
    int currentEnergy, maxEnergy;
    // Update is called once per frame
    void Start() {
        btnGoRoom.onClick.AddListener(() => GameManager.instance.GoToPowerRoom());
        UpdateEnergy();
        EventManager.AddListener(EventName.UpdateEnergy.ToString(), UpdateEnergy);
    }
    void UpdateEnergy() {
        maxEnergy = GameManager.instance.GetPowerRoomEnergy();
        currentEnergy = maxEnergy - GameManager.instance.GetTotalEnergyUsed();
        if (currentEnergy < 0) currentEnergy = 0;
        txtEnergy.text = currentEnergy + "<color=yellow>/" + maxEnergy + "</color>";
    }
}
