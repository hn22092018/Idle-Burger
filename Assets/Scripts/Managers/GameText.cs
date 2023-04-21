using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Localizes {
    public int ID;
    public string Text;
}
[System.Serializable]
public class GameText:ScriptableObject {
    public List<Localizes> ens = new List<Localizes>();
    public List<Localizes> frs = new List<Localizes>();
    public List<Localizes> des = new List<Localizes>();
    public List<Localizes> its = new List<Localizes>();
    public List<Localizes> ess = new List<Localizes>();
    public List<Localizes> pts = new List<Localizes>();
    //public List<Localizes> rus = new List<Localizes>();
    //public List<Localizes> inds = new List<Localizes>();
    public List<Localizes> cns = new List<Localizes>();
    public List<Localizes> kos = new List<Localizes>();
    public List<Localizes> jps = new List<Localizes>();
    public List<Localizes> vns = new List<Localizes>();
    public string GetTextByID(int id) {
        LANGUAGE_ID languageID = ProfileManager.Instance._LanguageManager._CurrentLanguageID;
        switch (languageID) {
            //case LANGUAGE_ID.EN:
            //    foreach (var data in ens) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            //case LANGUAGE_ID.FR:
            //    foreach (var data in frs) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            //case LANGUAGE_ID.DE:
            //    foreach (var data in des) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            //case LANGUAGE_ID.IT:
            //    foreach (var data in its) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            //case LANGUAGE_ID.ES:
            //    foreach (var data in ess) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            //case LANGUAGE_ID.PT:
            //    foreach (var data in pts) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            ////case LANGUAGE_ID.RU:
            ////    foreach (var data in rus) {
            ////        if (data.ID == id) return data.Text;
            ////    }
            ////    break;
            ////case LANGUAGE_ID.IND:
            ////    foreach (var data in inds) {
            ////        if (data.ID == id) return data.Text;
            ////    }
            ////    break;
            //case LANGUAGE_ID.CN:
            //    foreach (var data in cns) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            //case LANGUAGE_ID.KO:
            //    foreach (var data in kos) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            //case LANGUAGE_ID.JP:
            //    foreach (var data in jps) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            //case LANGUAGE_ID.VN:
            //    foreach (var data in vns) {
            //        if (data.ID == id) return data.Text;
            //    }
            //    break;
            default:
                foreach (var data in ens) {
                    if (data.ID == id) return data.Text;
                }
                break;
        }
        return "";
       
    }
    public string RoomIDToString(RoomID roomID) {
        switch (roomID) {
            case RoomID.Table1:
                return GetTextByID(89) + " 1";
            case RoomID.Table2:
                return GetTextByID(89) + " 2";
            case RoomID.Table3:
                return GetTextByID(89) + " 3";
            case RoomID.Table4:
                return GetTextByID(89) + " 4";
            case RoomID.Table5:
                return GetTextByID(89) + " 5";
            case RoomID.Table6:
                return GetTextByID(89) + " 6";
            case RoomID.BigTable1:
                return GetTextByID(90) + " 1";
            case RoomID.BigTable2:
                return GetTextByID(90) + " 2";
            case RoomID.BigTable3:
                return GetTextByID(90) + " 3";
            case RoomID.BigTable4:
                return GetTextByID(90) + " 4";
            case RoomID.BigTable5:
                return GetTextByID(90) + " 5";
            case RoomID.BigTable6:
                return GetTextByID(90) + " 6";
            case RoomID.BigTable7:
                return GetTextByID(90) + " 7";
            case RoomID.BigTable8:
                return GetTextByID(90) + " 8";
            case RoomID.BigTable9:
                return GetTextByID(90) + " 9";
            case RoomID.BigTable10:
                return GetTextByID(90) + " 10";
            case RoomID.BigTable11:
                return GetTextByID(90) + " 11";
            case RoomID.BigTable12:
                return GetTextByID(90) + " 12";
            case RoomID.BigTable13:
                return GetTextByID(90) + " 13";
            case RoomID.BigTable14:
                return GetTextByID(90) + " 14";
            case RoomID.Manager:
                return GetTextByID(91) + "";
            case RoomID.Lobby:
                return GetTextByID(92) + "";
            case RoomID.Kitchen:
                return GetTextByID(93) + "";
            case RoomID.Clean:
                return GetTextByID(94) + "";
            case RoomID.Restroom:
                return GetTextByID(95) + "";
            case RoomID.Restroom2:
                return GetTextByID(95) + " 2";
            case RoomID.DeliverRoom:
                return GetTextByID(97) + "";
            case RoomID.Power:
                return GetTextByID(393) ;
           
        }
        return "";
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
                case RestroomModelType.Restroom_HandsDryer:
                    return GetTextByID(379);
            }
        }
        if (e is CleanModelType) {
            switch (e) {
                case CleanModelType.Clean_Vacuum:
                    return GetTextByID(360);
                case CleanModelType.Clean_Trolley:
                    return GetTextByID(361);
                case CleanModelType.Clean_Broom:
                    return GetTextByID(362);
                case CleanModelType.Clean_ProductClean:
                    return GetTextByID(363);
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
