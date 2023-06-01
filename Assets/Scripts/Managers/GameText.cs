using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Localizes {
    public int ID;
    public string Text;
}
[System.Serializable]
public class GameText : ScriptableObject {
    public List<Localizes> ens = new List<Localizes>();
    public List<Localizes> frs = new List<Localizes>();
    public List<Localizes> des = new List<Localizes>();
    public List<Localizes> its = new List<Localizes>();
    public List<Localizes> ess = new List<Localizes>();
    public List<Localizes> pts = new List<Localizes>();
    public List<Localizes> cns = new List<Localizes>();
    public List<Localizes> kos = new List<Localizes>();
    public List<Localizes> jps = new List<Localizes>();
    public List<Localizes> vns = new List<Localizes>();
    public string GetTextByID(int id) {
        LANGUAGE_ID languageID = ProfileManager.Instance._LanguageManager._CurrentLanguageID;
        return languageID switch {
            LANGUAGE_ID.EN => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.FR => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.DE => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.IT => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.PT => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.CN => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.JP => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.KO => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.VN => FindTextLanaguagesByID(id, ens),
            LANGUAGE_ID.IND => FindTextLanaguagesByID(id, ens),
            _ => FindTextLanaguagesByID(id, ens),
        };
    }
    string FindTextLanaguagesByID(int id, List<Localizes> list) {
        foreach (var data in list) {
            if (data.ID == id) return data.Text;
        }
        return "";
    }
    public string QuestTypeToShortString(QuestType type) {
        return type switch {
            QuestType.Upgrade_Lobby_Desk => GetTextByID(208).ToLower(),
            QuestType.Upgrade_Lobby_Computer => GetTextByID(211).ToLower(),
            QuestType.Upgrade_Lobby_Case => GetTextByID(212).ToLower(),
            QuestType.Upgrade_Lobby_Decor => GetTextByID(207).ToLower(),
            QuestType.Upgrade_SmallTable_Table => GetTextByID(204).ToLower(),
            QuestType.Upgrade_SmallTable_Chair => GetTextByID(205).ToLower(),
            QuestType.Upgrade_SmallTable_Plates => GetTextByID(206).ToLower(),
            QuestType.Upgrade_SmallTable_Decor => GetTextByID(207).ToLower(),
            QuestType.Upgrade_BigTable_Table => GetTextByID(204).ToLower(),
            QuestType.Upgrade_BigTable_Chair => GetTextByID(205).ToLower(),
            QuestType.Upgrade_BigTable_Plates => GetTextByID(206).ToLower(),
            QuestType.Upgrade_BigTable_Decor => GetTextByID(207).ToLower(),
            QuestType.Upgrade_Kitchen_StoveOven => GetTextByID(210).ToLower(),
            QuestType.Upgrade_Power_SmallGenerator => GetTextByID(203).ToLower(),
            QuestType.Upgrade_Power_BigGenerator => GetTextByID(202).ToLower(),
            QuestType.Upgrade_Restroom_MaleStall => GetTextByID(215).ToLower(),
            QuestType.Upgrade_Restroom_FemaleStall => GetTextByID(214).ToLower(),
            QuestType.Upgrade_Restroom_MaleSink => GetTextByID(218).ToLower(),
            QuestType.Upgrade_Restroom_FemaleSink => GetTextByID(213).ToLower(),
            QuestType.Upgrade_Deliver_StaffTable => GetTextByID(204).ToLower(),
            QuestType.Upgrade_Deliver_BakeryGlass => GetTextByID(217).ToLower(),
            QuestType.Upgrade_Deliver_Fridge => GetTextByID(209).ToLower(),
            QuestType.Upgrade_Deliver_Decor => GetTextByID(207).ToLower(),
            _ => "",
        };
    }
    public string RoomIDToString(RoomID roomID) {
        return roomID switch {
            RoomID.Lobby => GetTextByID(129),
            RoomID.Table1 => GetTextByID(126) + " 1",
            RoomID.Table2 => GetTextByID(126) + " 2",
            RoomID.Table3 => GetTextByID(126) + " 3",
            RoomID.Table4 => GetTextByID(126) + " 4",
            RoomID.Table5 => GetTextByID(126) + " 5",
            RoomID.Table6 => GetTextByID(126) + " 6",
            RoomID.BigTable1 => GetTextByID(127) + " 1",
            RoomID.BigTable2 => GetTextByID(127) + " 2",
            RoomID.BigTable3 => GetTextByID(127) + " 3",
            RoomID.BigTable4 => GetTextByID(127) + " 4",
            RoomID.BigTable5 => GetTextByID(127) + " 5",
            RoomID.BigTable6 => GetTextByID(127) + " 6",
            RoomID.BigTable7 => GetTextByID(127) + " 7",
            RoomID.BigTable8 => GetTextByID(127) + " 8",
            RoomID.BigTable9 => GetTextByID(127) + " 9",
            RoomID.BigTable10 => GetTextByID(127) + " 10",
            RoomID.BigTable11 => GetTextByID(127) + " 11",
            RoomID.BigTable12 => GetTextByID(127) + " 12",
            RoomID.BigTable13 => GetTextByID(127) + " 13",
            RoomID.BigTable14 => GetTextByID(127) + " 14",
            RoomID.Kitchen => GetTextByID(130),
            RoomID.Restroom => GetTextByID(132),
            RoomID.Restroom2 => GetTextByID(132) + " 2",
            RoomID.Power => GetTextByID(133),
            RoomID.DeliverRoom => GetTextByID(131),
            _ => "",
        };
    }
    public string ItemNameToString<T>(T e) {

        if (e is PowerModelType) {
            switch (e) {
                case PowerModelType.Power_BigGenerator:
                    return GetTextByID(202);
                case PowerModelType.Power_SmallGenerator:
                    return GetTextByID(203);
            }
        }
        if (e is ManagerModelType) {
            switch (e) {
                case ManagerModelType.Manager_Desk:
                    return GetTextByID(0);
                case ManagerModelType.Manager_Table:
                    return GetTextByID(0);
                case ManagerModelType.Manager_Fridge:
                    return GetTextByID(0);
                case ManagerModelType.Manager_Bookshelf:
                    return GetTextByID(0);
            }
        }
        if (e is BigTableModelType) {
            switch (e) {
                case BigTableModelType.BigTable_Table:
                    return GetTextByID(204);
                case BigTableModelType.BigTable_Chair:
                    return GetTextByID(205);
                case BigTableModelType.BigTable_Plate:
                    return GetTextByID(206);
                case BigTableModelType.BigTable_Decor:
                    return GetTextByID(207);
            }
        }
        if (e is SmallTableModelType) {
            switch (e) {
                case SmallTableModelType.SmallTable_Table:
                    return GetTextByID(204);
                case SmallTableModelType.SmallTable_Chair:
                    return GetTextByID(205);
                case SmallTableModelType.SmallTable_Plate:
                    return GetTextByID(206);
                case SmallTableModelType.SmallTable_Decor:
                    return GetTextByID(207);
            }
        }
        if (e is DeliverModelType) {
            switch (e) {
                case DeliverModelType.Deliver_StaffTable:
                    return GetTextByID(204);
                case DeliverModelType.Deliver_Fridge:
                    return GetTextByID(209);
                case DeliverModelType.Deliver_BakeryGlass:
                    return GetTextByID(217);
                case DeliverModelType.Deliver_Decor:
                    return GetTextByID(207);
            }
        }
        if (e is KitchenModelType) {
            switch (e) {
                case KitchenModelType.Kitchen_Stove_Oven:
                    return GetTextByID(210);
                case KitchenModelType.Kitchen_Decor:
                    return GetTextByID(207);
            }
        }
        if (e is RestroomModelType) {
            switch (e) {
                case RestroomModelType.Restroom_SinkMale:
                    return GetTextByID(218);
                case RestroomModelType.Restroom_SinkFemale:
                    return GetTextByID(213);
                case RestroomModelType.Restroom_StallFemale:
                    return GetTextByID(214);
                case RestroomModelType.Restroom_StallMale:
                    return GetTextByID(215);
                case RestroomModelType.Restroom_HandsDryer:
                    return GetTextByID(216);
            }
        }

        if (e is LobbyModelType) {
            switch (e) {
                case LobbyModelType.Lobby_Desk:
                    return GetTextByID(208);
                case LobbyModelType.Lobby_Computer:
                    return GetTextByID(211);
                case LobbyModelType.Lobby_Case:
                    return GetTextByID(212);
                case LobbyModelType.Lobby_Decor:
                    return GetTextByID(207);
            }
        }
        return "";
    }

}
