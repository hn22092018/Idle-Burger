using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LANGUAGE_ID { EN = 0, FR, DE, IT, ES, PT, CN, JP, KO, VN, IND }
public class LanguageManager {
    public List<LanguageInfo> _LanguageInfoList = new List<LanguageInfo>();
    public LANGUAGE_ID _CurrentLanguageID = LANGUAGE_ID.EN;
    public void OnLoadLangugage() {
        LoadLanguageInfo();
        if (!PlayerPrefs.HasKey("LanguageID")) {
            GetSystemLangugage();
        } else {
            string lg = PlayerPrefs.GetString("LanguageID");
            _CurrentLanguageID = (LANGUAGE_ID)Enum.Parse(typeof(LANGUAGE_ID), lg);
        }
    }
    public void SaveLanguage(LANGUAGE_ID id) {
        _CurrentLanguageID = id;
        PlayerPrefs.SetString("LanguageID", _CurrentLanguageID.ToString());
    }
    void GetSystemLangugage() {
        _CurrentLanguageID = LANGUAGE_ID.EN;
        foreach (var lan in _LanguageInfoList) {
            if (lan.systemLanguage == Application.systemLanguage) {
                _CurrentLanguageID = lan.languageID;
            }
        }
        SaveLanguage(_CurrentLanguageID);
    }

    void LoadLanguageInfo() {
        _LanguageInfoList = new List<LanguageInfo>();
        LanguageInfo eng = new LanguageInfo(LANGUAGE_ID.EN, "English", "EN", SystemLanguage.English);
        _LanguageInfoList.Add(eng);
        LanguageInfo french = new LanguageInfo(LANGUAGE_ID.FR, "Français", "FR", SystemLanguage.French);
        _LanguageInfoList.Add(french);
        LanguageInfo german = new LanguageInfo(LANGUAGE_ID.DE, "Deutsche", "DE", SystemLanguage.German);
        _LanguageInfoList.Add(german);
        LanguageInfo italian = new LanguageInfo(LANGUAGE_ID.IT, "Italiano", "IT", SystemLanguage.Italian);
        _LanguageInfoList.Add(italian);
        LanguageInfo spanish = new LanguageInfo(LANGUAGE_ID.ES, "Español", "ES", SystemLanguage.Spanish);
        _LanguageInfoList.Add(spanish);
        LanguageInfo portuguese = new LanguageInfo(LANGUAGE_ID.PT, "Português", "PT", SystemLanguage.Portuguese);
        _LanguageInfoList.Add(portuguese);
        //LanguageInfo russian = new LanguageInfo(LANGUAGE_ID.RU, "русский", "RU", SystemLanguage.Russian);
        //_LanguageInfoList.Add(russian);
        //LanguageInfo indonesian = new LanguageInfo(LANGUAGE_ID.IND, "Bahasa Indonesia", "IND", SystemLanguage.Indonesian);
        //_LanguageInfoList.Add(indonesian);
        LanguageInfo chineseS = new LanguageInfo(LANGUAGE_ID.CN, "中国简体", "CN", SystemLanguage.Chinese);
        _LanguageInfoList.Add(chineseS);
        LanguageInfo korean = new LanguageInfo(LANGUAGE_ID.KO, "한국어", "KO", SystemLanguage.Korean);
        _LanguageInfoList.Add(korean);
        LanguageInfo japanese = new LanguageInfo(LANGUAGE_ID.JP, "日本語", "JP", SystemLanguage.Japanese);
        _LanguageInfoList.Add(japanese);
        LanguageInfo vietnamese = new LanguageInfo(LANGUAGE_ID.VN, "Tiếng Việt", "VN", SystemLanguage.Vietnamese);
        _LanguageInfoList.Add(vietnamese);
    }
}
public class LanguageInfo {
    public string name;
    public string fileName;
    public LANGUAGE_ID languageID;
    public SystemLanguage systemLanguage;
    public LanguageInfo(LANGUAGE_ID lid, string lname, string lfileName, SystemLanguage sl) {
        languageID = lid;
        name = lname;
        fileName = lfileName;
        systemLanguage = sl;
    }
}