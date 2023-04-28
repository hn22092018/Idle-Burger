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
    public int reputationDropChance;
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
    public List<SkinItem> _ManagerSkins;
    public List<SkinItem> _ReceptionSkins;
    public List<SkinItem> _ChefSkins;
    public List<SkinItem> _WaiterSkins;
    public List<SkinItem> _CleanerSkins;
    public List<SkinItem> _BartenderSkins;
    public List<SkinItem> _BaristaSkins;
    private List<SkinItem> _AllSkins = new List<SkinItem>();

    public List<SkinItem> AllSkins { get => _AllSkins; set => _AllSkins = value; }

    private void OnEnable() {
        AllSkins.Clear();
        //LoadManagerSkin();
        LoadReceptionSkin();
        LoadChefSkin();
        LoadWaiterSkin();
        //LoadCleanerSkin();
        LoadDeliverSkin();
        //LoadBaristaSkin();
    }

    
    private void LoadBaristaSkin() {
        _BaristaSkins.Clear();

        SkinItem skin0 = new SkinItem() {
            id = 100,
            staffType = StaffID.Deliver,
            skinName = "Donaldson",
            skinIcon = GetSpriteByName("Barista_0"),
            workMesh = GetMeshByName("Barista_0"),
            skinType = SkinType.Default,
            diligenceWork = DiligenceWork.Normal,
            workEfficiency = 0,
            specialSkillChance = 0

        };
        SkinItem skin1 = new SkinItem() {
            id = 21,
            staffType = StaffID.Deliver,
            skinName = "Convanan",
            skinIcon = GetSpriteByName("Barista_1"),
            workMesh = GetMeshByName("Barista_1"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Normal,
            skinPrice = 1,
            workEfficiency = 10,
            incomeX2Chance=10,
            specialSkillChance = 10,
            reputationDropChance=10
        };

        SkinItem skin2 = new SkinItem() {
            id = 25,
            staffType = StaffID.Deliver,
            skinName = "Miku",
            skinIcon = GetSpriteByName("Barista_2"),
            workMesh = GetMeshByName("Barista_2"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 3,
            workEfficiency = 20,
            incomeX2Chance = 20,
            specialSkillChance = 15,
            reputationDropChance = 20
        };

        SkinItem skin3 = new SkinItem() {
            id = 31,
            staffType = StaffID.Deliver,
            skinName = "Saburo",
            skinIcon = GetSpriteByName("Barista_3"),
            workMesh = GetMeshByName("Barista_3"),
            skinType = SkinType.Video,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 3,
            workEfficiency = 25,
            incomeX2Chance = 25,
            specialSkillChance =20,
            reputationDropChance = 20
        };

        SkinItem skin4 = new SkinItem() {
            id = 40,
            staffType = StaffID.Deliver,
            skinName = "Shen",
            skinIcon = GetSpriteByName("Barista_4"),
            workMesh = GetMeshByName("Barista_4"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 300,
            workEfficiency = 30,
            incomeX2Chance = 30,
            specialSkillChance = 30,
            reputationDropChance = 30
        };

        SkinItem skin5 = new SkinItem() {
            id = 46,
            staffType = StaffID.Deliver,
            skinName = "Bruno",
            skinIcon = GetSpriteByName("Barista_5"),
            workMesh = GetMeshByName("Barista_5"),
            skinType = SkinType.Gem,
            diligenceWork = DiligenceWork.Hard,
            skinPrice = 400,
            workEfficiency = 40,
            incomeX2Chance = 40,
            specialSkillChance = 35,
            reputationDropChance = 40
        };

        SkinItem skin7 = new SkinItem() {
            id = 13,
            staffType = StaffID.Deliver,
            skinName = "Michal",
            skinIcon = GetSpriteByName("Barista_VIP"),
            workMesh = GetMeshByName("Coffee_Staff_VIP"),
            skinType = SkinType.PremiumSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 80,
            incomeX2Chance = 80,
            specialSkillChance = 50,
            reputationDropChance = 50
        };
        SkinItem skin8 = new SkinItem() {
            id = 6,
            staffType = StaffID.Deliver,
            skinName = "Simonis",
            skinIcon = GetSpriteByName("Barista_SVIP"),
            workMesh = GetMeshByName("Coffee_Staff_SVIP"),
            skinType = SkinType.GoldenSuit,
            diligenceWork = DiligenceWork.Crazy,
            workEfficiency = 100,
            incomeX2Chance = 100,
            specialSkillChance = 60,
            reputationDropChance = 60
        };
        _BaristaSkins.Add(skin0);
        _BaristaSkins.Add(skin1);
        _BaristaSkins.Add(skin2);
        _BaristaSkins.Add(skin3);
        _BaristaSkins.Add(skin4);
        _BaristaSkins.Add(skin5);
        _BaristaSkins.Add(skin7);
        _BaristaSkins.Add(skin8);
        AllSkins.AddRange(_BaristaSkins);
    }

    private void LoadDeliverSkin() {
        _BartenderSkins.Clear();

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
            reputationDropChance = 10
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
            reputationDropChance = 20
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
            reputationDropChance = 30
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
            reputationDropChance = 40
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
            reputationDropChance = 40
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
            reputationDropChance = 50
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
            reputationDropChance = 60
        };
        _BartenderSkins.Add(skin0);
        _BartenderSkins.Add(skin1);
        _BartenderSkins.Add(skin2);
        _BartenderSkins.Add(skin3);
        _BartenderSkins.Add(skin4);
        _BartenderSkins.Add(skin5);
        _BartenderSkins.Add(skin6);
        _BartenderSkins.Add(skin7);
        AllSkins.AddRange(_BartenderSkins);
    }
    //private void LoadCleanerSkin() {
    //    _CleanerSkins.Clear();
    //    SkinItem skin0 = new SkinItem() {
    //        id = 102,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Robert",
    //        skinIcon = GetSpriteByName("Cleaner_0"),
    //        workMesh = GetMeshByName("Cleaner_0"),
    //        skinType = SkinType.Default,
    //        diligenceWork = DiligenceWork.Normal,
    //        workEfficiency = 0,
    //        specialSkillChance = 0
    //    };

    //    SkinItem skin1 = new SkinItem() {
    //        id = 30,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Ryoichi",
    //        skinIcon = GetSpriteByName("Cleaner_1"),
    //        workMesh = GetMeshByName("Cleaner_1"),
    //        skinType = SkinType.Video,
    //        diligenceWork = DiligenceWork.Normal,
    //        skinPrice = 1,
    //        workEfficiency = 10,
    //        incomeX2Chance = 10,
    //        specialSkillChance = 15,
    //        reputationDropChance=10

    //    };

    //    SkinItem skin2 = new SkinItem() {
    //        id = 45,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Angelica",
    //        skinIcon = GetSpriteByName("Cleaner_2"),
    //        workMesh = GetMeshByName("Cleaner_2"),
    //        skinType = SkinType.Video,
    //        diligenceWork = DiligenceWork.Hard,
    //        skinPrice = 3,
    //        workEfficiency = 20,
    //        incomeX2Chance = 20,
    //        specialSkillChance = 20,
    //        reputationDropChance = 15
    //    };
    //    SkinItem skin3 = new SkinItem() {
    //        id = 114,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Irenka",
    //        skinIcon = GetSpriteByName("Cleaner_3"),
    //        workMesh = GetMeshByName("Cleaner_3"),
    //        skinType = SkinType.Video,
    //        diligenceWork = DiligenceWork.Hard,
    //        skinPrice = 3,
    //        workEfficiency = 30,
    //        incomeX2Chance = 20,
    //        specialSkillChance = 30,
    //        reputationDropChance = 20
    //    };
    //    SkinItem skin4 = new SkinItem() {
    //        id = 106,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Daniels",
    //        skinIcon = GetSpriteByName("Cleaner_4"),
    //        workMesh = GetMeshByName("Cleaner_4"),
    //        skinType = SkinType.Video,
    //        diligenceWork = DiligenceWork.Hard,
    //        skinPrice = 5,
    //        workEfficiency = 30,
    //        incomeX2Chance = 40,
    //        specialSkillChance = 30,
    //        reputationDropChance = 30
    //    };
    //    SkinItem skin5 = new SkinItem() {
    //        id = 112,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Thomas",
    //        skinIcon = GetSpriteByName("Cleaner_5"),
    //        workMesh = GetMeshByName("Cleaner_5"),
    //        skinType = SkinType.Gem,
    //        diligenceWork = DiligenceWork.Hard,
    //        skinPrice = 100,
    //        workEfficiency = 30,
    //        incomeX2Chance = 30,
    //        specialSkillChance = 20,
    //        reputationDropChance = 20
    //    };
    //    SkinItem skin6 = new SkinItem() {
    //        id = 113,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Hornik",
    //        skinIcon = GetSpriteByName("Cleaner_6"),
    //        workMesh = GetMeshByName("Cleaner_6"),
    //        skinType = SkinType.Gem,
    //        diligenceWork = DiligenceWork.Hard,
    //        skinPrice = 300,
    //        workEfficiency = 40,
    //        incomeX2Chance = 40,
    //        specialSkillChance = 30,
    //        reputationDropChance = 30
    //    };
    //    SkinItem skin7 = new SkinItem() {
    //        id = 19,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Henderson",
    //        skinIcon = GetSpriteByName("Cleaner_7"),
    //        workMesh = GetMeshByName("Cleaner_7"),
    //        skinType = SkinType.Gem,
    //        diligenceWork = DiligenceWork.Crazy,
    //        skinPrice = 400,
    //        workEfficiency = 45,
    //        incomeX2Chance = 60,
    //        specialSkillChance = 40,
    //        reputationDropChance = 30
    //    };

    //    SkinItem skin8 = new SkinItem() {
    //        id = 11,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Jacob",
    //        skinIcon = GetSpriteByName("Cleaning Staff VIP"),
    //        workMesh = GetMeshByName("Cleaning Staff VIP"),
    //        skinType = SkinType.PremiumSuit,
    //        diligenceWork = DiligenceWork.Crazy,
    //        workEfficiency = 60,
    //        incomeX2Chance = 80,
    //        specialSkillChance = 40,
    //        reputationDropChance = 40
    //    };
    //    SkinItem skin9 = new SkinItem() {
    //        id = 4,
    //        staffType = StaffID.Cleaner,
    //        skinName = "Harland",
    //        skinIcon = GetSpriteByName("Cleaning Staff SVIP"),
    //        workMesh = GetMeshByName("Cleaning Staff SVIP"),
    //        skinType = SkinType.GoldenSuit,
    //        diligenceWork = DiligenceWork.Crazy,
    //        workEfficiency = 60,
    //        incomeX2Chance = 100,
    //        specialSkillChance = 50,
    //        reputationDropChance = 50
    //    };
    //    _CleanerSkins.Add(skin0);
    //    _CleanerSkins.Add(skin1);
    //    _CleanerSkins.Add(skin2);
    //    _CleanerSkins.Add(skin3);
    //    _CleanerSkins.Add(skin4);
    //    _CleanerSkins.Add(skin5);
    //    _CleanerSkins.Add(skin6);
    //    _CleanerSkins.Add(skin7);
    //    _CleanerSkins.Add(skin8);
    //    _CleanerSkins.Add(skin9);
    //    AllSkins.AddRange(_CleanerSkins);
    //}
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
            reputationDropChance=10
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
            reputationDropChance = 20
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
            reputationDropChance = 20
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
            reputationDropChance = 30
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
            reputationDropChance = 30
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
            reputationDropChance = 30
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
            reputationDropChance = 40
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
            reputationDropChance = 40
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
            reputationDropChance = 50
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
            reputationDropChance = 60
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
            reputationDropChance=10
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
            reputationDropChance = 20

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
            reputationDropChance = 20
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
            reputationDropChance = 30
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
             reputationDropChance = 30
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
            reputationDropChance = 40
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
            reputationDropChance = 40
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
            reputationDropChance = 40
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
            reputationDropChance = 50
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
            reputationDropChance = 60
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
            reputationDropChance=10
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
            reputationDropChance = 20
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
            reputationDropChance = 30
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
            reputationDropChance = 40
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
            reputationDropChance = 40
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
            reputationDropChance = 50

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
            reputationDropChance = 60
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
    //private void LoadManagerSkin() {
    //    _ManagerSkins.Clear();
    //    SkinItem skin1 = new SkinItem() {
    //        id = 0,
    //        charType = StaffID.Manager,
    //        skinName = "Golden Manager",
    //        skinIcon = GetSpriteByName("Manager SVIP"),
    //        workMesh = GetMeshByName("Manager SVIP"),
    //        skinType = SkinType.GoldenSuit,
    //        workEfficiency = 20,
    //        specialSkillChance = 15
    //    };

    //    SkinItem skin2 = new SkinItem() {
    //        id = 7,
    //        charType = StaffID.Manager,
    //        skinName = "Premium Manager",
    //        skinIcon = GetSpriteByName("Manager VIP"),
    //        workMesh = GetMeshByName("Manager VIP"),
    //        skinType = SkinType.PremiumSuit
    //    };

    //    SkinItem skin3 = new SkinItem() {
    //        id = 23,
    //        charType = StaffID.Manager,
    //        skinName = "Kiryujin",
    //        skinIcon = GetSpriteByName("Manager_1"),
    //        workMesh = GetMeshByName("JP_Manager")
    //    };

    //    SkinItem skin4 = new SkinItem() {
    //        id = 34,
    //        charType = StaffID.Manager,
    //        skinName = "Zhong",
    //        skinIcon = GetSpriteByName("Manager_2"),
    //        workMesh = GetMeshByName("Chinese_Manager")
    //    };

    //    SkinItem skin5 = new SkinItem() {
    //        id = 41,
    //        charType = StaffID.Manager,
    //        skinName = "Armando",
    //        skinIcon = GetSpriteByName("Manager_3"),
    //        workMesh = GetMeshByName("MXC_Manager_skin1")
    //    };

    //    SkinItem skin6 = new SkinItem() {
    //        id = 15,
    //        charType = StaffID.Manager,
    //        skinName = "Mr Howdy",
    //        skinIcon = GetSpriteByName("Manager_4"),
    //        workMesh = GetMeshByName("MXC_Manager_skin2_Cowboy")
    //    };

    //    SkinItem skin7 = new SkinItem() {
    //        id = 14,
    //        charType = StaffID.Manager,
    //        skinName = "Santana",
    //        skinIcon = GetSpriteByName("Skin_Christmas_Manager"),
    //        workMesh = GetMeshByName("MXC_Manager_skin3_Santa_Clau")
    //    };
    //    _ManagerSkins.Add(skin1);
    //    _ManagerSkins.Add(skin2);
    //    _ManagerSkins.Add(skin3);
    //    _ManagerSkins.Add(skin4);
    //    _ManagerSkins.Add(skin5);
    //    _ManagerSkins.Add(skin6);
    //    _ManagerSkins.Add(skin7);
    //    AllSkins.AddRange(_ManagerSkins);
    //}

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




