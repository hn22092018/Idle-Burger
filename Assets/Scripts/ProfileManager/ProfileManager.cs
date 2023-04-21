using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : Singleton<ProfileManager> {
    public PlayerData playerData = new PlayerData();
    public LanguageManager _LanguageManager= new LanguageManager();
    public DataConfig dataConfig;
    public static PlayerData PlayerData { get => Instance.playerData; }
    public const string key_Music = "MusicOn";
    public const string key_Sound = "SoundOn";
    protected override void Awake() {
        base.Awake();
        playerData.LoadData();
        _LanguageManager.OnLoadLangugage();
        Application.targetFrameRate = 60;
    }
    private void Update() {
        playerData.Update();
    }
    public void ChangeMusicState(bool isOn) {
        PlayerPrefs.SetInt(key_Music, isOn ? 0 : 1);
        Debug.Log("ChangeMusicState_" + isOn);
    }
    public bool IsMusicOn() {
        return PlayerPrefs.GetInt(key_Music) == 0;
    }
    public void ChangeSoundState(bool isOn) {
        PlayerPrefs.SetInt(key_Sound, isOn ? 0 : 1);
    }
    public bool IsSoundOn() {
        return PlayerPrefs.GetInt(key_Sound) == 0;
    }
    public void ClearData() {
        PlayerPrefs.DeleteAll();
        playerData = new PlayerData();
        playerData.SaveData();
        Application.Quit();
    }
}
