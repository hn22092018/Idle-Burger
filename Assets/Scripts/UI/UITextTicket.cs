using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextTicket : MonoBehaviour
{
    [SerializeField] Text txtTicket;
    // Update is called once per frame
    private void Update() {
        txtTicket.text = ProfileManager.PlayerData.ResourceSave.GetADTicket().ToString();
    }

}
