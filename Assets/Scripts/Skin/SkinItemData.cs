using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkinType {
    Default,
    GoldenSuit,
    PremiumSuit,
    Cash,
    Gem,
    Video
}
public enum DiligenceWork {
    Normal,
    Hard,
    Crazy
}
[System.Serializable]
public class SkinItem {
    [GUIColor(0, 1f, 1f, 1f)]
    public int id;
    public StaffID staffType;
    public string skinName;
    [HorizontalGroup("Group1")]
    [BoxGroup("Group1/Skin Icon")]
    [HideLabel]
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public Sprite skinIcon;
    [BoxGroup("Group1/Mesh")]
    [HideLabel]
    [PreviewField(70, ObjectFieldAlignment.Left)]
    public Mesh workMesh;
    public SkinType skinType;
    [ShowIf("IsNeedShowPrice")]
    public int skinPrice;
    public DiligenceWork diligenceWork;
    public int workEfficiency;
    public int incomeX2Chance;
    public int specialSkillChance;
    bool IsNeedShowPrice() {
        if (skinType == SkinType.Default || skinType == SkinType.GoldenSuit || skinType == SkinType.PremiumSuit)
            return false;
        return true;
    }

}



[CreateAssetMenu(fileName = "SkinItemData", menuName = "ScriptableObjects/SkinItemDataAsset", order = 1)]
public class SkinItemData : ScriptableObject {
    public Sprite[] sprConfig;
    public GameObject[] meshConfig;
    public List<SkinItem> _ReceptionSkins;
    public List<SkinItem> _ChefSkins;
    public List<SkinItem> _WaiterSkins;
    public List<SkinItem> _DeliverSkins;
    private List<SkinItem> _AllSkins = new List<SkinItem>();

    public List<SkinItem> AllSkins { get => _AllSkins; set => _AllSkins = value; }

    private void OnEnable() {
        AllSkins.Clear();
        //LoadReceptionSkin();
        //LoadChefSkin();
        //LoadWaiterSkin();
        //LoadDeliverSkin();
    }

  
    private void LoadDeliverSkin() {
        _DeliverSkins.Clear();

        SkinItem skin0 = new SkinItem() {
            id = 101,
            staffType = StaffID.Deliver,
            skinName = "Vinicius",
            skinIcon = GetSpriteByName("Bartender_0"),
            workMesh = GetMeshByName("Bartender_0"),
            skinType = SkinType.Default,
            diligenceWork = DiligenceWork.Normal,
            workEfficiency = 0,
            specialSkillChance = 0
        };


        SkinItem skin1 = new SkinItem() {
            id = 20,
            staffType = StaffID.Deliver,
            skinName = "Naruhodo",
            skinIcon = GetSpriteByName("Bartender_1"),
            workMesh = GetMeshByName("Bartender_1"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Normal,
            skinPrice = 1,
            workEfficiency = 10,
            incomeX2Chance = 10,
            specialSkillChance = 10,
        };

        SkinItem skin2 = new SkinItem() {
            id = 28,
            staffType = StaffID.Deliver,
            skinName = "Risako",
            skinIcon = GetSpriteByName("Bartender_2"),
            workMesh = GetMeshByName("Bartender_2"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 3,
            workEfficiency = 15,
            incomeX2Chance = 20,
            specialSkillChance = 20,
        };

        SkinItem skin3 = new SkinItem() {
            id = 29,
            staffType = StaffID.Deliver,
            skinName = "Dantenda",
            skinIcon = GetSpriteByName("Bartender_3"),
            workMesh = GetMeshByName("Bartender_3"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 3,
            workEfficiency = 20,
            incomeX2Chance = 30,
            specialSkillChance = 25,
        };

        SkinItem skin4 = new SkinItem() {
            id = 39,
            staffType = StaffID.Deliver,
            skinName = "Xiao",
            skinIcon = GetSpriteByName("Bartender_4"),
            workMesh = GetMeshByName("Bartender_4"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 300,
            workEfficiency = 40,
            incomeX2Chance = 40,
            specialSkillChance = 30,
        };

        SkinItem skin5 = new SkinItem() {
            id = 44,
            staffType = StaffID.Deliver,
            skinName = "Mateo",
            skinIcon = GetSpriteByName("Bartender_5"),
            workMesh = GetMeshByName("Bartender_5"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 400,
            workEfficiency = 50,
            incomeX2Chance = 50,
            specialSkillChance = 40,
        };

        SkinItem skin6 = new SkinItem() {
            id = 12,
            staffType = StaffID.Deliver,
            skinName = "Oliver",
            skinIcon = GetSpriteByName("Bar Staff VIP"),
            workMesh = GetMeshByName("Bar Staff VIP"),
            skinType = SkinType.PremiumSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 80,
            incomeX2Chance = 80,
            specialSkillChance = 50,
        };
        SkinItem skin7 = new SkinItem() {
            id = 5,
            staffType = StaffID.Deliver,
            skinName = "Hornick",
            skinIcon = GetSpriteByName("Bar Staff SVIP"),
            workMesh = GetMeshByName("Bar Staff SVIP"),
            skinType = SkinType.GoldenSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 100,
            incomeX2Chance = 100,
            specialSkillChance = 60,
        };
        _DeliverSkins.Add(skin0);
        _DeliverSkins.Add(skin1);
        _DeliverSkins.Add(skin2);
        _DeliverSkins.Add(skin3);
        _DeliverSkins.Add(skin4);
        _DeliverSkins.Add(skin5);
        _DeliverSkins.Add(skin6);
        _DeliverSkins.Add(skin7);
        AllSkins.AddRange(_DeliverSkins);
    }
    private void LoadWaiterSkin() {
        _WaiterSkins.Clear();

        SkinItem skin0 = new SkinItem() {
            id = 103,
            staffType = StaffID.Waiter,
            skinName = "Garnier",
            skinIcon = GetSpriteByName("Waiter_0"),
            workMesh = GetMeshByName("Waiter_0"),
            skinType = SkinType.Default,
            diligenceWork = DiligenceWork.Normal,
            workEfficiency = 0,
            specialSkillChance = 0
        };

        SkinItem skin1 = new SkinItem() {
            id = 32,
            staffType = StaffID.Waiter,
            skinName = "Sakae",
            skinIcon = GetSpriteByName("Waiter_1"),
            workMesh = GetMeshByName("Waiter_1"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Normal,
            skinPrice = 1,
            workEfficiency = 10,
            incomeX2Chance = 10,
            specialSkillChance = 10,
        };

        SkinItem skin2 = new SkinItem() {
            id = 27,
            staffType = StaffID.Waiter,
            skinName = "Daichi",
            skinIcon = GetSpriteByName("Waiter_2"),
            workMesh = GetMeshByName("Waiter_2"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 3,
            workEfficiency = 20,
            incomeX2Chance = 20,
            specialSkillChance = 20,
        };

        SkinItem skin3 = new SkinItem() {
            id = 43,
            staffType = StaffID.Waiter,
            skinName = "Eduardo",
            skinIcon = GetSpriteByName("Waiter_3"),
            workMesh = GetMeshByName("Waiter_3"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 3,
            workEfficiency = 20,
            incomeX2Chance = 20,
            specialSkillChance = 20,
        };

        SkinItem skin4 = new SkinItem() {
            id = 50,
            staffType = StaffID.Waiter,
            skinName = "Maruno",
            skinIcon = GetSpriteByName("Waiter_4"),
            workMesh = GetMeshByName("Waiter_4"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Crazy,
            skinPrice = 100,
            workEfficiency = 20,
            incomeX2Chance = 30,
            specialSkillChance = 20,
        };
        SkinItem skin5 = new SkinItem() {
            id = 110,
            staffType = StaffID.Waiter,
            skinName = "Sakae Akat",
            skinIcon = GetSpriteByName("Waiter_5"),
            workMesh = GetMeshByName("Waiter_5"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 300,
            workEfficiency = 30,
            incomeX2Chance = 30,
            specialSkillChance = 35,
        };
        SkinItem skin6 = new SkinItem() {
            id = 18,
            staffType = StaffID.Waiter,
            skinName = "Elodie",
            skinIcon = GetSpriteByName("Waiter_6"),
            workMesh = GetMeshByName("Waiter_6"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 300,
            workEfficiency = 40,
            incomeX2Chance = 40,
            specialSkillChance = 30,
        };

        SkinItem skin7 = new SkinItem() {
            id = 51,
            staffType = StaffID.Waiter,
            skinName = "Claire",
            skinIcon = GetSpriteByName("Waiter_7"),
            workMesh = GetMeshByName("Waiter_7"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Crazy,
            skinPrice = 400,
            workEfficiency = 40,
            incomeX2Chance = 45,
            specialSkillChance = 35,
        };
        SkinItem skin8 = new SkinItem() {
            id = 111,
            staffType = StaffID.Waiter,
            skinName = "Bambina",
            skinIcon = GetSpriteByName("Waiter_8"),
            workMesh = GetMeshByName("Waiter_8"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Crazy,
            skinPrice = 400,
            workEfficiency = 40,
            incomeX2Chance = 50,
            specialSkillChance = 40,
        };
        SkinItem skin9 = new SkinItem() {
            id = 10,
            staffType = StaffID.Waiter,
            skinName = "Mick",
            skinIcon = GetSpriteByName("Waiter VIP"),
            workMesh = GetMeshByName("Waiter VIP"),
            skinType = SkinType.PremiumSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 50,
            incomeX2Chance = 80,
            specialSkillChance = 50,
        };

        SkinItem skin10 = new SkinItem() {
            id = 3,
            staffType = StaffID.Waiter,
            skinName = "Jonny",
            skinIcon = GetSpriteByName("Waiter SVIP"),
            workMesh = GetMeshByName("Waiter SVIP"),
            skinType = SkinType.GoldenSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 60,
            incomeX2Chance = 100,
            specialSkillChance = 60,
        };

        _WaiterSkins.Add(skin0);
        _WaiterSkins.Add(skin1);
        _WaiterSkins.Add(skin2);
        _WaiterSkins.Add(skin3);
        _WaiterSkins.Add(skin4);
        _WaiterSkins.Add(skin5);
        _WaiterSkins.Add(skin6);
        _WaiterSkins.Add(skin7);
        _WaiterSkins.Add(skin8);
        _WaiterSkins.Add(skin9);
        _WaiterSkins.Add(skin10);
        AllSkins.AddRange(_WaiterSkins);
    }
    private void LoadChefSkin() {
        _ChefSkins.Clear();

        SkinItem skin0 = new SkinItem() {
            id =104,
            staffType = StaffID.Chef,
            skinName = "Dayton",
            skinIcon = GetSpriteByName("Chef_0"),
            workMesh = GetMeshByName("Chef_0"),
            skinType = SkinType.Default,
            diligenceWork = DiligenceWork.Normal,
            workEfficiency = 0,
            specialSkillChance = 0
        };
        SkinItem skin1 = new SkinItem() {
            id = 17,
            staffType = StaffID.Chef,
            skinName = "Crispy Castus",
            skinIcon = GetSpriteByName("Chef_1"),
            workMesh = GetMeshByName("Chef_1"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Normal,
            skinPrice = 1,
            workEfficiency = 10,
            incomeX2Chance = 10,
            specialSkillChance = 10,
        };

        SkinItem skin2 = new SkinItem() {
            id = 24,
            staffType = StaffID.Chef,
            skinName = "Akihiko",
            skinIcon = GetSpriteByName("Chef_2"),
            workMesh = GetMeshByName("Chef_2"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 3,
            workEfficiency = 20,
            incomeX2Chance = 20,
            specialSkillChance = 25,

        };

        SkinItem skin3 = new SkinItem() {
            id = 38,
            staffType = StaffID.Chef,
            skinName = "Han",
            skinIcon = GetSpriteByName("Chef_3"),
            workMesh = GetMeshByName("Chef_3"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Crazy,
            skinPrice = 3,
            workEfficiency = 20,
            incomeX2Chance = 20,
            specialSkillChance = 30,
        };

        SkinItem skin4 = new SkinItem() {
            id = 42,
            staffType = StaffID.Chef,
            skinName = "Arturo",
            skinIcon = GetSpriteByName("Chef_4"),
            workMesh = GetMeshByName("Chef_4"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Normal,
            skinPrice = 5,
            workEfficiency = 30,
            incomeX2Chance = 30,
            specialSkillChance = 20,
        };
        SkinItem skin5 = new SkinItem() {
            id = 107,
            staffType = StaffID.Chef,
            skinName = "Keitha",
            skinIcon = GetSpriteByName("Chef_5"),
            workMesh = GetMeshByName("Chef_5"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 100,
            workEfficiency = 20,
            incomeX2Chance = 30,
            specialSkillChance = 30,
        };
        SkinItem skin6 = new SkinItem() {
            id = 108,
            staffType = StaffID.Chef,
            skinName = "Kevin",
            skinIcon = GetSpriteByName("Chef_6"),
            workMesh = GetMeshByName("Chef_6"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 300,
            workEfficiency = 30,
            incomeX2Chance = 40,
            specialSkillChance = 30,
        };
        SkinItem skin7 = new SkinItem() {
            id = 109,
            staffType = StaffID.Chef,
            skinName = "Dewayne",
            skinIcon = GetSpriteByName("Chef_7"),
            workMesh = GetMeshByName("Chef_7"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 300,
            workEfficiency = 30,
            incomeX2Chance = 40,
            specialSkillChance = 40,
        };
        SkinItem skin8 = new SkinItem() {
            id = 52,
            staffType = StaffID.Chef,
            skinName = "Panacorn",
            skinIcon = GetSpriteByName("Chef_8"),
            workMesh = GetMeshByName("Chef_8"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Crazy,
            skinPrice = 500,
            workEfficiency = 60,
            incomeX2Chance = 60,
            specialSkillChance = 50,
        };
       
        SkinItem skin9 = new SkinItem() {
            id = 9,
            staffType = StaffID.Chef,
            skinName = "Kendrick",
            skinIcon = GetSpriteByName("Chef_VIP"),
            workMesh = GetMeshByName("Chef_VIP"),
            skinType = SkinType.PremiumSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 80,
            incomeX2Chance = 80,
            specialSkillChance = 50,
        };
        SkinItem skin10 = new SkinItem() {
            id = 2,
            staffType = StaffID.Chef,
            skinName = "Brynlee",
            skinIcon = GetSpriteByName("Chef_SVIP"),
            workMesh = GetMeshByName("Chef_SVIP"),
            skinType = SkinType.GoldenSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 100,
            incomeX2Chance = 100,
            specialSkillChance = 60,
        };
        _ChefSkins.Add(skin0);
        _ChefSkins.Add(skin1);
        _ChefSkins.Add(skin2);
        _ChefSkins.Add(skin3);
        _ChefSkins.Add(skin4);
        _ChefSkins.Add(skin5);
        _ChefSkins.Add(skin6);
        _ChefSkins.Add(skin7);
        _ChefSkins.Add(skin8);
        _ChefSkins.Add(skin9);
        _ChefSkins.Add(skin10);
        AllSkins.AddRange(_ChefSkins);
    }
    private void LoadReceptionSkin() {
        _ReceptionSkins.Clear();

        SkinItem skin0 = new SkinItem() {
            id =105,
            staffType = StaffID.Receptionist,
            skinName = "Alexander",
            skinIcon = GetSpriteByName("Reception_0"),
            workMesh = GetMeshByName("Reception_0"),
            skinType = SkinType.Default,
            diligenceWork = DiligenceWork.Normal,
            workEfficiency = 0,
            specialSkillChance = 0

        };

        SkinItem skin1 = new SkinItem() {
            id = 16,
            staffType = StaffID.Receptionist,
            skinName = "Ericson",
            skinIcon = GetSpriteByName("Reception_1"),
            workMesh = GetMeshByName("Reception_1"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Normal,
            skinPrice = 1,
            workEfficiency = 10,
            incomeX2Chance = 10,
            specialSkillChance = 10,
        };

        SkinItem skin2 = new SkinItem() {
            id = 26,
            staffType = StaffID.Receptionist,
            skinName = "Eiko",
            skinIcon = GetSpriteByName("Reception_2"),
            workMesh = GetMeshByName("Reception_2"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 3,
            workEfficiency = 30,
            incomeX2Chance = 30,
            specialSkillChance = 20,
        };


        SkinItem skin3 = new SkinItem() {
            id = 36,
            staffType = StaffID.Receptionist,
            skinName = "Juan",
            skinIcon = GetSpriteByName("Reception_3"),
            workMesh = GetMeshByName("Reception_3"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 100,
            workEfficiency = 30,
            incomeX2Chance = 30,
            specialSkillChance = 30,
        };
        SkinItem skin4 = new SkinItem() {
            id = 37,
            staffType = StaffID.Receptionist,
            skinName = "Alesha",
            skinIcon = GetSpriteByName("Reception_4"),
            workMesh = GetMeshByName("Reception_4"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 300,
            workEfficiency = 40,
            incomeX2Chance = 40,
            specialSkillChance = 40,
        };

        SkinItem skin5 = new SkinItem() {
            id = 48,
            staffType = StaffID.Receptionist,
            skinName = "Champtoshi",
            skinIcon = GetSpriteByName("Reception_5"),
            workMesh = GetMeshByName("Reception_5"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 400,
            workEfficiency = 50,
            incomeX2Chance = 50,
            specialSkillChance = 50,
        };
        SkinItem skin6 = new SkinItem() {
            id = 8,
            staffType = StaffID.Receptionist,
            skinName = "Jones",
            skinIcon = GetSpriteByName("Receptionist VIP"),
            workMesh = GetMeshByName("Receptionist VIP"),
            skinType = SkinType.PremiumSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 80,
            incomeX2Chance = 80,
            specialSkillChance = 50,

        };
        SkinItem skin7 = new SkinItem() {
            id = 1,
            staffType = StaffID.Receptionist,
            skinName = "Robbie",
            skinIcon = GetSpriteByName("Receptionist SVIP"),
            workMesh = GetMeshByName("Receptionist SVIP"),
            skinType = SkinType.GoldenSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 100,
            incomeX2Chance = 100,
            specialSkillChance = 60,
        };
        _ReceptionSkins.Add(skin0);
        _ReceptionSkins.Add(skin1);
        _ReceptionSkins.Add(skin2);
        _ReceptionSkins.Add(skin3);
        _ReceptionSkins.Add(skin4);
        _ReceptionSkins.Add(skin5);
        _ReceptionSkins.Add(skin6);
        _ReceptionSkins.Add(skin7);
        AllSkins.AddRange(_ReceptionSkins);

    }

    Sprite GetSpriteByName(string name) {
        foreach (Sprite spr in sprConfig) {
            if (spr.name == name) return spr;
        }
        return null;
    }

    Mesh GetMeshByName(string name) {
        foreach (GameObject obj in meshConfig) {
            if (obj.name == name) {
                for (int i = 0; i < obj.transform.childCount; i++) {
                    if (obj.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null) {
                        return obj.transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                    }
                }
            }
        }
        return null;
    }
#if UNITY_EDITOR
    [Button]
    void FindAllSprConfig() {
        string foderPath = "Assets/ScreenShot/SkinIcons";
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(Sprite).Name, new[] { foderPath });
        Sprite[] dataAssets = new Sprite[guids.Length];
        for (int i = 0; i < guids.Length; i++) {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            dataAssets[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
        sprConfig = dataAssets;
    }
    [Button]
    void FindAllMeshConfig() {
        string foderPath = "Assets/3D Art Lv2_3/Character";
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(GameObject).Name, new[] { foderPath });
        GameObject[] dataAssets = new GameObject[guids.Length];
        for (int i = 0; i < guids.Length; i++) {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            dataAssets[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Debug.Log(dataAssets[i].name);
        }
        meshConfig = dataAssets;
    }
#endif
}




