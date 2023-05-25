using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {
    private AsyncOperation m_AsyncLoad;
    bool startLoad = false;
    public Transform m_SliderBar;
    public Text loadedPercent;
    float loadCounter;
    float loadTime = 3f;
    int countStart = 0;
    // Start is called before the first frame update
    private void Start() {
        int index = ProfileManager.PlayerData.GetSelectedWorld();
        StartCoroutine(OnLoadingNextScene("Level" + index));
    }
    IEnumerator OnLoadingNextScene(string sceneName) {
        m_SliderBar.transform.localScale = new Vector3(0, 1, 1);
        loadCounter = 0;
        startLoad = false;
        m_AsyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        m_AsyncLoad.allowSceneActivation = false;
        yield return new WaitForEndOfFrame();
        while (m_AsyncLoad.progress < 0.9f) {
            yield return new WaitForEndOfFrame();
        }
        startLoad = true;
        m_SliderBar.DOScaleX(1, loadTime);
        yield return new WaitForSeconds(loadTime);
        m_AsyncLoad.allowSceneActivation = true;
    }
    private void Update() {
        if (loadCounter < 100 && startLoad) {
            loadCounter += Time.deltaTime * 100f / loadTime;
        }
        loadedPercent.text = ((int)loadCounter).ToString() + "%";
    }
}
