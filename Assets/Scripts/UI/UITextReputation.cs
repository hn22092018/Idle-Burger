using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextReputation : MonoBehaviour
{
    [SerializeField] Text txtTicket;
    // Update is called once per frame
    private void Update() {
        txtTicket.text = ProfileManager.PlayerData.researchManager.researchValue.ToString();
    }
}
