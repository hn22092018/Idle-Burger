using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDespawnEff : MonoBehaviour {
    public float time;
    float deltaTime;
    private void OnEnable() {
        deltaTime = time;
    }

    // Update is called once per frame
    void Update() {
        if (deltaTime > 0) {
            deltaTime -= Time.deltaTime;
            if (deltaTime <= 0) {
                Off();
            }
        }
    }
    void Off() {
        PoolManager.Pools["GameEntity"].Despawn(transform);
    }
}
