using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionText : MonoBehaviour {
    public Text m_TextVersion;
    private void Start() {
        m_TextVersion.text = Application.version;
    }
}