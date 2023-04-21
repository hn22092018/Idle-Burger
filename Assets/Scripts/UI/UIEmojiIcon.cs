using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEmojiIcon : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] Image emoji_Icon;
    bool IsShow;
    float deltaTimeShow;
    float timeShow;
    private void Update() {
        transform.eulerAngles = new Vector3(40, 45, 0);
        if (IsShow) {
            deltaTimeShow += Time.deltaTime;
            if(deltaTimeShow>= timeShow) {
                gameObject.SetActive(false);
                deltaTimeShow = 0;
                IsShow = false;
            }
        }
    }
    public void ShowFunnyEmoji(float time=2.5f) {
        timeShow = time;
        IsShow = true;
        deltaTimeShow = 0;
        emoji_Icon.sprite = GameManager.instance.emoji_funny[Random.Range(0, GameManager.instance.emoji_funny.Length)];
    }
    public void ShowSadEmoji(float time = 2.5f) {
        timeShow = time;
        IsShow = true;
        deltaTimeShow = 0;
        emoji_Icon.sprite = GameManager.instance.emoji_sad[Random.Range(0, GameManager.instance.emoji_sad.Length)];
    }
    public void ShowAngryEmoji(float time = 2.5f) {
        timeShow = time;
        IsShow = true;
        deltaTimeShow = 0;
        emoji_Icon.sprite = GameManager.instance.emoji_angry[Random.Range(0, GameManager.instance.emoji_angry.Length)];
    }
}
