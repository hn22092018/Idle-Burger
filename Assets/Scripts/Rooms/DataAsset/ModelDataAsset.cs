using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ModelDataAsset", menuName = "ScriptableObjects/ModelDataAsset", order = 1)]
public class ModelDataAsset : ScriptableObject {
    public List<BigTableModelDataConfig> bigTableModelConfigs;
    public List<SmalTableModelDataConfig> smallTableModelConfigs;
    public List<LobbyModelDataConfig> lobbyModelConfigs;
    public List<ManagerModelDataConfig> managerModelConfigs;
    public List<PowerModelDataConfig> powerModelConfigs;
    public List<RestroomModelDataConfig> restroomModelConfigs;
    public List<KitchenModelDataConfig> kitchenModelConfigs;
    public List<DeliverModelDataConfig> deliverModelConfigs;

    public List<GameObject> FindModelsByType<T>(T type) {
        if (type is BigTableModelType) {
            for (int i = 0; i < bigTableModelConfigs.Count; i++) {
                if (bigTableModelConfigs[i].type.ToString() == type.ToString()) return bigTableModelConfigs[i].models;
            }
        } else if (type is SmallTableModelType) {
            for (int i = 0; i < smallTableModelConfigs.Count; i++) {
                if (smallTableModelConfigs[i].type.ToString() == type.ToString()) return smallTableModelConfigs[i].models;
            }
        } else if (type is LobbyModelType) {
            for (int i = 0; i < lobbyModelConfigs.Count; i++) {
                if (lobbyModelConfigs[i].type.ToString() == type.ToString()) return lobbyModelConfigs[i].models;
            }
        } else if (type is ManagerModelType) {
            for (int i = 0; i < managerModelConfigs.Count; i++) {
                if (managerModelConfigs[i].type.ToString() == type.ToString()) return managerModelConfigs[i].models;
            }
        } else if (type is PowerModelType) {
            for (int i = 0; i < powerModelConfigs.Count; i++) {
                if (powerModelConfigs[i].type.ToString() == type.ToString()) return powerModelConfigs[i].models;
            }
        } else if (type is RestroomModelType) {
            for (int i = 0; i < restroomModelConfigs.Count; i++) {
                if (restroomModelConfigs[i].type.ToString() == type.ToString()) return restroomModelConfigs[i].models;
            }
        } else if (type is KitchenModelType) {
            for (int i = 0; i < kitchenModelConfigs.Count; i++) {
                if (kitchenModelConfigs[i].type.ToString() == type.ToString()) return kitchenModelConfigs[i].models;
            }
        } else if (type is DeliverModelType) {
            for (int i = 0; i < deliverModelConfigs.Count; i++) {
                if (deliverModelConfigs[i].type.ToString() == type.ToString()) return deliverModelConfigs[i].models;
            }
        }
        return null;
    }


}
[System.Serializable]
public class BigTableModelDataConfig {
    public BigTableModelType type;
    public List<GameObject> models;
    public Sprite spr;
}
[System.Serializable]
public class SmalTableModelDataConfig {
    public SmallTableModelType type;
    public List<GameObject> models;
    public Sprite spr;
}
[System.Serializable]
public class LobbyModelDataConfig {
    public LobbyModelType type;
    public List<GameObject> models;
    public Sprite spr;
}
[System.Serializable]
public class ManagerModelDataConfig {
    public ManagerModelType type;
    public List<GameObject> models;
    public Sprite spr;
}

[System.Serializable]
public class PowerModelDataConfig {
    public PowerModelType type;
    public List<GameObject> models;
    public Sprite spr;
}
[System.Serializable]
public class RestroomModelDataConfig {
    public RestroomModelType type;
    public List<GameObject> models;
    public Sprite spr;
}
[System.Serializable]
public class KitchenModelDataConfig {
    public KitchenModelType type;
    public List<GameObject> models;
    public Sprite spr;
}
[System.Serializable]
public class DeliverModelDataConfig {
    public DeliverModelType type;
    public List<GameObject> models;
    public Sprite spr;
}

