using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class FlashModel : MonoBehaviour {
    [SerializeField] List<Renderer> renders = new List<Renderer>();
    List<Material> materials = new List<Material>();
    float intensity = 1;
    float intensityMax = 0.6f;
    bool isIncreaseEmission;
    bool isEnableFlash;
    List<Color> defaultColors = new List<Color>();
    private void Awake() {
        defaultColors.Clear();
        for (int i = 0; i < renders.Count; i++) {
            if (renders[i] != null) {
                Material[] mats = renders[i].materials;
                for (int k = 0; k < mats.Length; k++) {
                    Material mat = mats[k];
                    if (mat != null) {
                        string s = "_EmissionColor";
                        if (mat.HasProperty(s)) {
                            Color color = mat.GetColor(s);
                            defaultColors.Add(color);
                        } else {
                            defaultColors.Add(Color.white);
                        }
                        materials.Add(mat);
                    }
                }
            }
        }
    }
    private void Start() {
        TurnOffFlash();
    }

    public void TurnOnFlash() {
        for (int i = 0; i < materials.Count; i++) {
            materials[i].EnableKeyword("_EMISSION");
        }
        intensity = 0;
        isIncreaseEmission = true;
        isEnableFlash = true;
        StartCoroutine(IFlash());
    }
    IEnumerator IFlash() {
        while (isEnableFlash) {
            for (int i = 0; i < materials.Count; i++) {
                string s = "_EmissionColor";
                if (materials[i].HasColor(s)) {
                    materials[i].SetVector(s, Color.white * intensity);
                }
            }
            if (!isIncreaseEmission) {
                intensity -= Time.deltaTime;
                if (intensity <= 0) {
                    intensity = 0;
                    isIncreaseEmission = true;
                }
            } else {
                intensity += Time.deltaTime;
                if (intensity >= intensityMax) {
                    intensity = intensityMax;
                    isIncreaseEmission = false;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < materials.Count; i++) {
            string s = "_EmissionColor";
            Material mat = materials[i];
            if (mat.HasProperty(s)) {
                mat.SetVector(s, defaultColors[i]);
            }
        }
    }
    
    public void TurnOffFlash() {
        for (int i = 0; i < materials.Count; i++) {
            string s = "_EmissionColor";
            Material mat = materials[i];
            if (mat.HasProperty(s)) {
                mat.SetVector(s, defaultColors[i]);
            }
        }
        isEnableFlash = false;
        StopAllCoroutines();
    }
    [Button]
    public void FindMesh() {
        renders.Clear();
      foreach (var chil in transform.GetComponentsInChildren<Renderer>()) {
          if(chil is MeshRenderer)  renders.Add(chil);
        }
    }
}
