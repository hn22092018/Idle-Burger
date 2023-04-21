using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundID {
    CHEST_OPEN, QUEST_CLAIM, BUTTON_CLICK, UPGRADE,
    CARD_COMMON, CARD_RARE, CARD_EPIC, CARD_LEGEND,
    POPUP_SHOW, CASH_COLLECT, TAB_SWITCH, IAP
}
public class SoundManager : MonoBehaviour {
    public static SoundManager instance;
    public AudioSource _SourceBG;
    public AudioSource _SourceSubBG;
    public AudioSource _SourceEffect;
    public AudioClip sub_music_kitchen, sub_music_table, sub_music_bar, sub_music_coffee, sub_music_power;
    public AudioClip sub_music_sleep;
    public AudioClip s_chest_open, s_quest_claim, s_button_click, s_upgrade, s_popup_show, s_cash_collect, s_tab_switch, s_iap_success;
    public AudioClip s_card_common, s_card_rare, s_card_epic, s_card_legend;
    public AudioClip[] s_BGs;
    public AudioClip s_BGs_Christmas;
    private void Awake() {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start() {
        if (ProfileManager.Instance.IsMusicOn()) PlayMusic();
        else PauseMusic();
        if (ProfileManager.Instance.IsSoundOn()) PlaySound();
        else PauseSound();
    }
    public void PauseMusic() {
        _SourceBG.volume = 0;
        _SourceSubBG.volume = 0;
    }
    public void PlayMusic() {
        int world = ProfileManager.PlayerData.GetSelectedWorld();
        if (world == -1) {
            _SourceBG.clip = s_BGs_Christmas;
        } else {
            if (world <= 1) world = 1;
            _SourceBG.clip = s_BGs[world - 1];
        }
        if (ProfileManager.Instance.IsMusicOn()) {
            if (!_SourceBG.isPlaying) _SourceBG.Play();
             _SourceBG.volume = 1;
            _SourceSubBG.volume = 1;
        }
       
    }
    public void PlaySound() {
        _SourceEffect.volume = 1;
    }
    public void PauseSound() {
        _SourceEffect.volume = 0;
    }
    public void PlaySubMusic(RoomID roomID) {
        PlayMusic();
        switch (roomID) {
            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
            case RoomID.BigTable1:
            case RoomID.BigTable2:
            case RoomID.BigTable3:
            case RoomID.BigTable4:
            case RoomID.BigTable5:
            case RoomID.BigTable6:
            case RoomID.BigTable7:
            case RoomID.BigTable8:
            case RoomID.BigTable9:
            case RoomID.BigTable10:
            case RoomID.BigTable11:
            case RoomID.BigTable12:
            case RoomID.BigTable13:
            case RoomID.BigTable14:
                _SourceSubBG.clip = sub_music_table;
                break;
            case RoomID.Kitchen:
                _SourceSubBG.clip = sub_music_kitchen;
                break;
            case RoomID.Power:
                _SourceSubBG.clip = sub_music_power;
                break;
            case RoomID.DeliverRoom:
                _SourceSubBG.clip = sub_music_coffee;
                break;
        }
        if (ProfileManager.Instance.IsMusicOn()) {
            _SourceSubBG.volume = 1;
            _SourceSubBG.Play();
        }
    }
    public void StopSubMusic() {
        PlayMusic();
        _SourceSubBG.volume = 0;
        _SourceSubBG.Stop();
    }
    public void PlaySubMusicSleep() {
        _SourceSubBG.clip = sub_music_sleep;
        _SourceSubBG.Play();
    }
    public void PlaySoundEffect(SoundID id) {
        switch (id) {
            case SoundID.CHEST_OPEN:
                _SourceEffect.PlayOneShot(s_chest_open);
                break;
            case SoundID.QUEST_CLAIM:
                _SourceEffect.PlayOneShot(s_quest_claim);
                break;
            case SoundID.BUTTON_CLICK:
                _SourceEffect.PlayOneShot(s_button_click);
                break;
            case SoundID.UPGRADE:
                _SourceEffect.PlayOneShot(s_upgrade);
                break;
            case SoundID.CARD_COMMON:
                _SourceEffect.PlayOneShot(s_card_common);
                break;
            case SoundID.CARD_RARE:
                _SourceEffect.PlayOneShot(s_card_rare);
                break;
            case SoundID.CARD_EPIC:
                _SourceEffect.PlayOneShot(s_card_epic);
                break;
            case SoundID.CARD_LEGEND:
                _SourceEffect.PlayOneShot(s_card_legend);
                break;
            case SoundID.POPUP_SHOW:
                _SourceEffect.PlayOneShot(s_popup_show);
                break;
            case SoundID.CASH_COLLECT:
                _SourceEffect.PlayOneShot(s_cash_collect);
                break;
            case SoundID.TAB_SWITCH:
                _SourceEffect.PlayOneShot(s_tab_switch);
                break;
            case SoundID.IAP:
                _SourceEffect.PlayOneShot(s_iap_success);
                break;
        }
    }
}
