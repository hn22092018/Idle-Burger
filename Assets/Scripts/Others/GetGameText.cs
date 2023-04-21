using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetGameText : MonoBehaviour {
    public int TextID;
    public bool isUpper;
    Text txt;
    TextMeshPro txtPro;
    public bool IsRunUpdate;
    private void Awake() {
        txt = GetComponent<Text>();
        txtPro = GetComponent<TextMeshPro>();
    }
    private void OnEnable() {
        LoadText();
    }
    void LoadText() {
        string s = ProfileManager.Instance.dataConfig.GameText.GetTextByID(TextID);
        if (isUpper) s = s.ToUpper();
        if (txt != null) txt.text = s;
        if (txtPro != null) txtPro.text = s;

    }
    private void Update() {
        if (IsRunUpdate) {
            LoadText();
        }
    }
}
