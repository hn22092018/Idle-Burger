using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using Task = System.Threading.Tasks;
using System.IO;

public enum RequestStatus { WAIT = 0, FAIL = 1, SUCCESS = 2 };
public class GiftCodeManager : MonoBehaviour
{
    private static GiftCodeManager m_Instance;
    List<ItemFromGift> gifts;

    public static GiftCodeManager Instance {
        get { return m_Instance; }
    }

    private void Awake() {
        m_Instance = this;
    }
    private void Start() {
        gifts = ProfileManager.Instance.dataConfig.giftDatas.gifts;
    }
    public async void Test() {
        //GenerateGiftCodeList();
    }

    void GenerateGiftCodeList()
    {
        foreach(ItemFromGift gift in gifts)
        {
            GenerateNewGiftCodeAsync(gift.itemPackages);
        }
        
    }
    
    public async System.Threading.Tasks.Task<GiftCode> GetGiftCodeFromServer(string code) {
        UnityEngine.Debug.Log("Start get GIFT");
        GiftCode giftCode = null;
        await FirebaseDataManager.Instance.m_DatabaseReference.Child("GiftCode").Child(code).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                // Handle the error...
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                giftCode = JsonUtility.FromJson<GiftCode>(snapshot.GetRawJsonValue());
            }
        });
        return giftCode;
    }
    public System.Threading.Tasks.Task RemoveGiftCode(string code) {
        return FirebaseDataManager.Instance.m_DatabaseReference.Child("GiftCode").Child(code).RemoveValueAsync();
    }
    public async System.Threading.Tasks.Task<RequestStatus> ClaimGiftCode(string code) {
        GiftCode giftCode = await GetGiftCodeFromServer(code);
        if (giftCode == null) {
            return RequestStatus.FAIL;
        } else {
            ShowReward(giftCode);
            RemoveGiftCode(giftCode.code);
            return RequestStatus.SUCCESS;
        }
    }
    public async Task<RequestStatus> PushGiftCodeToServerAsync(GiftCode giftCode) {
        string json = JsonUtility.ToJson(giftCode);
        RequestStatus requestStatus = RequestStatus.FAIL;
        await FirebaseDataManager.Instance.m_DatabaseReference.Child("GiftCode").Child(giftCode.code).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
                Debug.LogError(task.Exception.Message);
            } else if (task.IsCompleted) {
                requestStatus = RequestStatus.SUCCESS;
            }
        });
        return requestStatus;
    }
    public async Task<GiftCode> GenerateNewGiftCodeAsync(List<ItemPackage> rewards) {
        GiftCode giftCode = new GiftCode();
        giftCode.rewards = rewards;
        RequestStatus requestStatus = await PushGiftCodeToServerAsync(giftCode);
        Debug.Log(requestStatus);
        if (requestStatus == RequestStatus.SUCCESS) {
            WriteString(giftCode.code);
            return giftCode;
        } else {
            WriteString("Can't create code");
            return null;
        }
    }

    static void WriteString(string code)
    {
        return;
        string path = "Assets/giftCodes.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(code);
        writer.Close();
    }

    void ShowReward(GiftCode gift)
    {
        List<ItemReward> listReward = new List<ItemReward>();
        foreach(ItemPackage item in gift.rewards)
        {
            ItemReward reward = ProfileManager.Instance.dataConfig.shopConfig.GetItemRewardFromGift(item);
            if(reward != null)
            {
                listReward.Add(reward);
                switch (reward.type)
                {
                    case ItemType.Gem:
                        ProfileManager.Instance.playerData.ResourceSave.AddGem(reward.amount);
                        break;
                    case ItemType.NormalChest:
                        ProfileManager.Instance.playerData.ResourceSave.AddNormalChest(reward.amount);
                        break;
                    case ItemType.AdvancedChest:
                        ProfileManager.Instance.playerData.ResourceSave.AddAdvancedChest(reward.amount);
                        break;
                    case ItemType.ADTicket:
                        ProfileManager.Instance.playerData.ResourceSave.AddADTicket(reward.amount);
                        break;
                    default:
                        break;
                }
            }
        }
        ProfileManager.Instance.playerData.SaveData();
        UIManager.instance.ShowUIPanelReward(listReward);
    }
}
