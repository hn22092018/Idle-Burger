using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontConfig : MonoBehaviour
{
    public TextType textType;
    LANGUAGE_ID languageId;
    private void Start()
    {
        languageId = ProfileManager.Instance._LanguageManager._CurrentLanguageID;
        ChangeFont();
    }

    float counter = 0;
    float recheckCoolDown = 20f;
    private void Update()
    {
        if (languageId != ProfileManager.Instance._LanguageManager._CurrentLanguageID)
        {
            languageId = ProfileManager.Instance._LanguageManager._CurrentLanguageID;
            ChangeFont();
            counter = recheckCoolDown;
        }
    }

    void ChangeFont()
    {
        if (GetComponent<Text>() != null && ProfileManager.Instance.dataConfig.fontDataAsset != null)
        {
            Text text = GetComponent<Text>();
            switch (textType)
            {
                case TextType.Text:
                    text.font = ProfileManager.Instance.dataConfig.fontDataAsset.textFont;
                    break;
                case TextType.Number:
                    text.font = ProfileManager.Instance.dataConfig.fontDataAsset.numberFont;
                    break;
                case TextType.TextTitle:
                    text.font = ProfileManager.Instance.dataConfig.fontDataAsset.textTitleFont;
                    break;
                default:
                    text.font = ProfileManager.Instance.dataConfig.fontDataAsset.textFont;
                    break;
            }
        }
    }
}
