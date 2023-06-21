using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SkinDataSave {
    /// <summary>
    /// skin ID
    /// </summary>
    public int id;
    /// <summary>
    /// index Staff 
    /// </summary>
    public int indexStaffUsing;
    /// <summary>
    /// flag is Selected
    /// </summary>
    public bool isUsing;
    /// <summary>
    /// index world 
    /// </summary>
    public int world;
    public bool IsSkillUsing() {
        int selectWorld = ProfileManager.PlayerData.selectedWorld;
        return indexStaffUsing >= 0 && isUsing && world == selectWorld;
    }
}
[System.Serializable]
public class SkinDataSaveVideo {
    /// <summary>
    /// skin ID
    /// </summary>
    public int id;
    /// <summary>
    ///  ADS Video Need To Unlock
    /// </summary>
    public int adsCount;
}
[System.Serializable]
public class SkinManager {
    /// <summary>
    /// list save all skin unlocked
    /// </summary>
    public List<SkinDataSave> skinDataSaves;
    /// <summary>
    /// list save all skins is processing unlock. 
    ///Only with skins need ads condition to unlock 
    /// </summary>
    public List<SkinDataSaveVideo> skinDataSavesVideo;
    public void LoadData() {
        Debug.Log("SkinManager LoadData");
        string jsonData = GetJsonData();
        SkinManager dataSave = JsonUtility.FromJson<SkinManager>(jsonData);
        if (dataSave != null) {
            skinDataSaves = dataSave.skinDataSaves;
            skinDataSavesVideo = dataSave.skinDataSavesVideo;
        } else {
            skinDataSaves = new List<SkinDataSave>();
            skinDataSavesVideo = new List<SkinDataSaveVideo>();
        }
    }
    string GetJsonData() {
        return PlayerPrefs.GetString("SkinManagerSave");
    }
    bool IsChangeData;
    public void SaveData() {
        if (!IsChangeData) return;
        IsChangeData = false;
        PlayerPrefs.SetString("SkinManagerSave", JsonUtility.ToJson(this).ToString());
    }
    #region ADS SKIN
    /// <summary>
    ///  Watched Ads Func Callback To Unlock Skin
    /// </summary>
    /// <param name="skinID"></param>
    public void WatchedAdsToGetSkin(int skinID) {
        IsChangeData = true;
        if (!IsContainSkinVideo(skinID)) {
            if (IsEnoughAdsToUnlockSkin(skinID, 1)) {
                //unlock skin
                AddSkin(skinID);
            } else {
                // save to list
                skinDataSavesVideo.Add(new SkinDataSaveVideo() {
                    id = skinID,
                    adsCount = 1
                });
            }
        } else {

            for (int i = 0; i < skinDataSavesVideo.Count; i++) {
                // find skin save with id== skinID
                if (skinDataSavesVideo[i].id == skinID) {
                    // increase ads count
                    skinDataSavesVideo[i].adsCount++;
                    // check adsCount to enough require to unlock skin.
                    // if true, add skin and remove it from list saver skinDataSavesVideo
                    if (IsEnoughAdsToUnlockSkin(skinID, skinDataSavesVideo[i].adsCount)) {
                        AddSkin(skinID);
                        skinDataSavesVideo.Remove(skinDataSavesVideo[i]);
                    }
                    break;
                }
            }
        }
    }
    /// <summary>
    /// Check list save skinDataSavesVideo is contain skin by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsContainSkinVideo(int id) {
        for (int i = 0; i < skinDataSavesVideo.Count; i++) {
            if (skinDataSavesVideo[i].id == id) return true;
        }
        return false;
    }
    /// <summary>
    ///  Check Enough Ads Watched 
    /// </summary>
    /// <param name="skinID"></param>
    /// <param name="adsCount"></param>
    /// <returns></returns>
    bool IsEnoughAdsToUnlockSkin(int skinID, int adsCount) {
        if (adsCount >= GetSkinConfigByID(skinID).skinPrice) return true;
        return false;
    }

    public int GetAdsNeedToUnlock(SkinItem skin) {
        for (int i = 0; i < skinDataSavesVideo.Count; i++) {
            // find skin save with id== skinID
            if (skinDataSavesVideo[i].id == skin.id) {
                return GetSkinConfigByID(skin.id).skinPrice - skinDataSavesVideo[i].adsCount;
            }
        }
        return skin.skinPrice;
    }
    #endregion
    #region Skin
    /// <summary>
    /// Check Use Owner Skin
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsContainSkin(int id) {
        for (int i = 0; i < skinDataSaves.Count; i++) {
            if (skinDataSaves[i].id == id) return true;
        }
        return false;
    }
    public void AddSkin(int newID) {
        IsChangeData = true;
        int selectWorld = ProfileManager.PlayerData.selectedWorld;
        if (!IsContainSkin(newID)) {
            skinDataSaves.Add(new SkinDataSave() {
                id = newID,
                indexStaffUsing = -1,
                world = selectWorld
            }) ;
        } 
    }
    public void UnUseSkin(int indexStaff, StaffID staffType) {
        int selectWorld = ProfileManager.PlayerData.selectedWorld;
        for (int i = 0; i < skinDataSaves.Count; i++) {
            if (skinDataSaves[i].indexStaffUsing == indexStaff && GetStaffTypeByID(skinDataSaves[i].id) == staffType && skinDataSaves[i].world== selectWorld) {
                skinDataSaves[i].indexStaffUsing = -1;
                skinDataSaves[i].isUsing = false;
                break;
            }
        }
    }
    public void UpdateSkin(int skinID, int indexStaff) {
        IsChangeData = true;
        int selectWorld = ProfileManager.PlayerData.selectedWorld;
        if (!IsContainSkin(skinID)) {
            skinDataSaves.Add(new SkinDataSave() {
                id = skinID,
                indexStaffUsing = indexStaff,
                world = selectWorld,
                isUsing = true,
            });
        } else {
            // flag to check skin in selected world, not using in other world.
            // ex: skin A to using in world 1 => IsContainSkin =true.
            // but skin A not using in world 3 => Add new data
            bool isExactlyWorld = false;
            for (int i = 0; i < skinDataSaves.Count; i++) {
                if (skinDataSaves[i].id == skinID && skinDataSaves[i].world==selectWorld) {
                    isExactlyWorld = true;
                    skinDataSaves[i].indexStaffUsing = indexStaff;
                    skinDataSaves[i].isUsing = true;
                    break;
                }
            }
            if (!isExactlyWorld) {
                skinDataSaves.Add(new SkinDataSave() {
                    id= skinID,
                    indexStaffUsing = indexStaff,
                    isUsing=true,
                    world=selectWorld
                }); ;
            }
        }
    }
    public bool IsSkinUsed(int id) {
        for (int i = 0; i < skinDataSaves.Count; i++) {
            if (skinDataSaves[i].id == id && skinDataSaves[i].IsSkillUsing()) return true;
        }
        return false;
    }
    public int GetSkinIDInIndex(int indexStaff, StaffID staffType) {
        int world = ProfileManager.PlayerData.selectedWorld;
        for (int i = 0; i < skinDataSaves.Count; i++) {
            if (skinDataSaves[i].indexStaffUsing == indexStaff && GetStaffTypeByID(skinDataSaves[i].id) == staffType && skinDataSaves[i].isUsing && skinDataSaves[i].world==world) return skinDataSaves[i].id;
        }
        return -1;
    }
    public SkinItem GetSkinConfig(int indexStaff, StaffID staffType) {
        int skinId = GetSkinIDInIndex(indexStaff, staffType);
        return GetSkinConfigByID(skinId);
    }
    public List<SkinItem> GetSkinList() {
        return ProfileManager.Instance.dataConfig.skinDataConfig.AllSkins;
    }

    /// <summary>
    /// Get Mesh By CharaterType && Index Staff on position setting scene in config file SkinItemData
    /// </summary>
    /// <param name="charaterType"></param>
    /// <param name="indexStaff"></param>
    /// <returns></returns>
    public Mesh GetMesh(StaffID charaterType, int indexStaff) {
        int world = ProfileManager.PlayerData.selectedWorld;
        for (int i = 0; i < skinDataSaves.Count; i++) {
            if (skinDataSaves[i].indexStaffUsing == indexStaff && GetStaffTypeByID(skinDataSaves[i].id) == charaterType && skinDataSaves[i].world==world) {
                return GetSkinConfigByID(skinDataSaves[i].id).workMesh;
            }
        }
        return null;
    }


    /// <summary>
    /// Get CharaterType By Id in config file SkinItemData
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private StaffID GetStaffTypeByID(int id) {
        foreach (SkinItem skin in GetSkinList()) {
            if (skin.id == id) {
                return skin.staffType;
            }
        }
        return StaffID.Manager;
    }
    public Sprite GetSpriteIconFromSaver(StaffID StaffID, int indexStaff) {
        int world = ProfileManager.PlayerData.selectedWorld;
        for (int i = 0; i < skinDataSaves.Count; i++) {
            if (skinDataSaves[i].indexStaffUsing == indexStaff && GetStaffTypeByID(skinDataSaves[i].id) == StaffID && skinDataSaves[i].world == world) {
                return GetSkinConfigByID(skinDataSaves[i].id).skinIcon;
            }
        }
        return null;
    }
    #endregion

    public List<SkinItem> GetListSkinConfigByStaffType(StaffID type) {
        return GetSkinList().Where((x) => x.staffType == type).ToList();
    }
    public SkinItem GetSkinConfigByID(int findID) {
        List<SkinItem> list = GetSkinList();
        for (int i = 0; i < list.Count; i++) {
            if (list[i].id == findID) return list[i];
        }
        return null;
    }
}
