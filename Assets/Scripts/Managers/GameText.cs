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
            QuestType.Upgrade_Lobby_Desk => "desk",
            QuestType.Upgrade_Lobby_Computer => "computer",
            QuestType.Upgrade_Lobby_Case => "case",
            QuestType.Upgrade_Lobby_Decor => "decor",
            QuestType.Upgrade_SmallTable_Table => "table",
            QuestType.Upgrade_SmallTable_Chair => "chair",
            QuestType.Upgrade_SmallTable_Plates => "plates",
            QuestType.Upgrade_SmallTable_Decor => "decor",
            QuestType.Upgrade_BigTable_Table => "table",
            QuestType.Upgrade_BigTable_Chair => "chair",
            QuestType.Upgrade_BigTable_Plates => "plates",
            QuestType.Upgrade_BigTable_Decor => "decor",
            QuestType.Upgrade_Kitchen_StoveOven => "stove oven",
            QuestType.Upgrade_Power_SmallGenerator => "small generator",
            QuestType.Upgrade_Power_BigGenerator => "big generator",
            QuestType.Upgrade_Restroom_MaleStall => "male stalls",
            QuestType.Upgrade_Restroom_FemaleStall => "female stall",
            QuestType.Upgrade_Restroom_MaleSink => "male sink",
            QuestType.Upgrade_Restroom_FemaleSink => "female sink",
            QuestType.Upgrade_Deliver_StaffTable => "staff table",
            QuestType.Upgrade_Deliver_BakeryGlass => "bakery glass",
            QuestType.Upgrade_Deliver_Fridge => "fridge",
            QuestType.Upgrade_Deliver_Decor => "decor",
            _ => "",
        };
    }
    public string RoomIDToString(RoomID roomID) {
        return roomID switch {
            RoomID.Lobby => "Lobby Room",
            RoomID.Table1 => "Table 1",
            RoomID.Table2 => "Table 2",
            RoomID.Table3 => "Table 3",
            RoomID.Table4 => "Table 4",
            RoomID.Table5 => "Table 5",
            RoomID.Table6 => "Table 6",
            RoomID.BigTable1 => "Big Table 1",
            RoomID.BigTable2 => "Big Table 2",
            RoomID.BigTable3 => "Big Table 3",
            RoomID.BigTable4 => "Big Table 4",
            RoomID.BigTable5 => "Big Table 5",
            RoomID.BigTable6 => "Big Table 6",
            RoomID.BigTable7 => "Big Table 7",
            RoomID.BigTable8 => "Big Table 8",
            RoomID.BigTable9 => "Big Table 9",
            RoomID.BigTable10 => "Big Table 10",
            RoomID.BigTable11 => "Big Table 11",
            RoomID.BigTable12 => "Big Table 12",
            RoomID.BigTable13 => "Big Table 13",
            RoomID.BigTable14 => "Big Table 14",
            RoomID.Kitchen => "Kitchen Room",
            RoomID.Restroom => "Restroom",
            RoomID.Restroom2 => "Restroom 2",
            RoomID.Power => "Power Room",
            RoomID.DeliverRoom => "Deliver Room",
            _ => "",
        };
    }
    public string ItemNameToString<T>(T e) {

        if (e is PowerModelType) {
            switch (e) {
                case PowerModelType.Power_BigGenerator:
                    return GetTextByID(374);
                case PowerModelType.Power_MediumGenerator:
                    return GetTextByID(375);
                case PowerModelType.Power_SmallGenerator:
                    return GetTextByID(376);
            }
        }
        if (e is ManagerModelType) {
            switch (e) {
                case ManagerModelType.Manager_Desk:
                    return GetTextByID(358);
                case ManagerModelType.Manager_Table:
                    return GetTextByID(346);
                case ManagerModelType.Manager_Fridge:
                    return GetTextByID(364);
                case ManagerModelType.Manager_Bookshelf:
                    return GetTextByID(373);
            }
        }
        if (e is BigTableModelType) {
            switch (e) {
                case BigTableModelType.BigTable_Table:
                    return GetTextByID(346);
                case BigTableModelType.BigTable_Chair:
                    return GetTextByID(347);
                case BigTableModelType.BigTable_Plate:
                    return GetTextByID(348);
                case BigTableModelType.BigTable_Decor:
                    return GetTextByID(351);
            }
        }
        if (e is SmallTableModelType) {
            switch (e) {
                case SmallTableModelType.SmallTable_Table:
                    return GetTextByID(346);
                case SmallTableModelType.SmallTable_Chair:
                    return GetTextByID(347);
                case SmallTableModelType.SmallTable_Plate:
                    return GetTextByID(348);
                case SmallTableModelType.SmallTable_Decor:
                    return GetTextByID(351);
            }
        }
        if (e is DeliverModelType) {
            switch (e) {
                case DeliverModelType.Deliver_StaffTable:
                    return GetTextByID(346);
                case DeliverModelType.Deliver_Fridge:
                    return GetTextByID(364);
                case DeliverModelType.Deliver_BakeryGlass:
                    return GetTextByID(383);
                case DeliverModelType.Deliver_Decor:
                    return GetTextByID(351);
            }
        }
        if (e is KitchenModelType) {
            switch (e) {
                case KitchenModelType.Kitchen_Fridge:
                    return GetTextByID(364);
                case KitchenModelType.Kitchen_Cupboards:
                    return GetTextByID(365);
                case KitchenModelType.Kitchen_ChoppingStation:
                    return GetTextByID(366);
                case KitchenModelType.Kitchen_Stove_Oven:
                    return GetTextByID(367);
                case KitchenModelType.Kitchen_Sink:
                    return GetTextByID(368);
                case KitchenModelType.Kitchen_Food:
                    return GetTextByID(349);
                case KitchenModelType.Kitchen_Decor:
                    return GetTextByID(351);
            }
        }
        if (e is RestroomModelType) {
            switch (e) {
                case RestroomModelType.Restroom_SinkMale:
                    return GetTextByID(368);
                case RestroomModelType.Restroom_SinkFemale:
                    return GetTextByID(368);
                case RestroomModelType.Restroom_StallFemale:
                    return GetTextByID(377);
                case RestroomModelType.Restroom_StallMale:
                    return GetTextByID(378);
            }
        }

        if (e is LobbyModelType) {
            switch (e) {
                case LobbyModelType.Lobby_Desk:
                    return GetTextByID(358);
                case LobbyModelType.Lobby_Computer:
                    return GetTextByID(369);
                case LobbyModelType.Lobby_Case:
                    return GetTextByID(386);
                case LobbyModelType.Lobby_Decor:
                    return GetTextByID(351);
            }
        }
        return "";
    }

}
