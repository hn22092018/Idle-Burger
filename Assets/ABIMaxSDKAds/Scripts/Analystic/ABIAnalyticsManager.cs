﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Analytics;

namespace SDK {
    [ScriptOrder(-98)]
    public class ABIAnalyticsManager : MonoBehaviour {
        private static ABIAnalyticsManager instance;
        public static ABIAnalyticsManager Instance { get { return instance; } }

        void Awake() {
            if (instance) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        #region Ads Tracking

        #region Revenue
        public const string key_ad_rewarded_revenue = "ad_rewarded_revenue";
        public const string key_ad_rewarded_count = "ad_rewarded_count";
        public const string key_ad_inters_revenue = "ad_inters_revenue";
        public const string key_ad_inters_count = "ad_inters_count";

        public const string ad_inters_show_count = "ad_inters_show_count_";
        public const string ad_rewarded_show_count = "ad_rewarded_show_count_";

        public void TrackAdImpression(ImpressionData impressionData) {
            double revenue = impressionData.ad_revenue;
            var impressionParameters = new[] {
                new Parameter("ad_platform", impressionData.ad_platform),
                new Parameter("ad_source", impressionData.ad_source),
                new Parameter("ad_unit_name", impressionData.ad_unit_name),
                new Parameter("ad_format", impressionData.ad_format),
                new Parameter("value", revenue),
                new Parameter("currency", "USD"), // All AppLovin revenue is sent in USD
            };
            FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
            FirebaseAnalytics.LogEvent("ad_impression_abi", impressionParameters);

            EventManager.AddEventNextFrame(() => {
                TrackLocalAdImpression(impressionData.ad_format, impressionData);
            });

        }
        public void TrackLocalAdImpression(string adFormat, ImpressionData impressionData) {
            switch (adFormat) {
                case "REWARDED": {
                        int totalWatched = PlayerPrefs.GetInt(key_ad_rewarded_count, 0);
                        totalWatched++;
                        PlayerPrefs.SetInt(key_ad_rewarded_count, totalWatched);

                        bool isTracking = totalWatched % 5 == 0;
                        if (!isTracking) return;

                        string eventName = ad_rewarded_show_count + totalWatched;

                        float totalRevenue = PlayerPrefs.GetFloat(key_ad_rewarded_revenue, 0);
                        totalRevenue += (float)impressionData.ad_revenue;
                        PlayerPrefs.SetFloat(key_ad_rewarded_revenue, totalRevenue);

                        Parameter[] parameters = new Parameter[] {
                            new Parameter("revenue_inters", totalRevenue)
                        };
                        ABIFirebaseManager.Instance.LogFirebaseEvent(eventName, parameters);
                        ABIAppsflyerManager.Instance.TrackRewarded_ShowCount(totalWatched);
                    }
                    break;
                case "INTER": {
                        int totalWatched = PlayerPrefs.GetInt(key_ad_inters_count, 0);
                        totalWatched++;
                        PlayerPrefs.SetInt(key_ad_inters_count, totalWatched);

                        bool isTracking = totalWatched % 5 == 0;
                        if (!isTracking) return;

                        string eventName = ad_inters_show_count + totalWatched;

                        float totalRevenue = PlayerPrefs.GetFloat(key_ad_inters_revenue, 0);
                        totalRevenue += (float)impressionData.ad_revenue;
                        PlayerPrefs.SetFloat(key_ad_inters_revenue, totalRevenue);

                        Parameter[] parameters = new Parameter[] {
                            new Parameter("revenue_reward", totalRevenue)
                        };
                        ABIFirebaseManager.Instance.LogFirebaseEvent(eventName, parameters);
                        ABIAppsflyerManager.Instance.TrackInterstitial_ShowCount(totalWatched, totalRevenue);
                    }
                    break;
            }
        }
        #endregion

        #region Rewarded Ads
        public const string ads_reward_complete = "ads_reward_complete";
        public const string ads_reward_click = "ads_reward_click";
        public const string ads_reward_show = "ads_reward_show";
        public const string ads_reward_fail = "ads_reward_fail";
        public const string ads_reward_loadsuccess = "ads_reward_loadsuccess";

        public void TrackAdsReward_ClickOnButton() {
            ABIFirebaseManager.Instance.LogFirebaseEvent(ads_reward_click);
            ABIAppsflyerManager.TrackRewarded_ClickShowButton();
        }
        public void TrackAdsReward_StartShow() {
            ABIFirebaseManager.Instance.LogFirebaseEvent(ads_reward_show);
            ABIAppsflyerManager.TrackRewarded_Displayed();
        }
        public void TrackAdsReward_ShowFail() {
            ABIFirebaseManager.Instance.LogFirebaseEvent(ads_reward_fail);
        }
        public void TrackAdsReward_ShowCompleted(string placement) {
            Parameter[] parameters = new Parameter[] {
                new Parameter("placement", placement)
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent(ads_reward_complete, parameters);
        }
        public void TrackAdsReward_LoadSuccess() {
            ABIFirebaseManager.Instance.LogFirebaseEvent(ads_reward_loadsuccess);
            ABIAppsflyerManager.TrackRewarded_LoadedSuccess();
        }
        #endregion

        #region Interstitial Ads
        public const string ad_inter_fail = "ad_inter_fail";
        public const string ad_inter_load = "ad_inter_load";
        public const string ad_inter_show = "ad_inter_show";
        public const string ad_inter_click = "ad_inter_click";

        public void TrackAdsInterstitial_LoadedSuccess() {
            ABIFirebaseManager.Instance.LogFirebaseEvent(ad_inter_load);
            ABIAppsflyerManager.TrackInterstitial_LoadedSuccess();
        }
        public void TrackAdsInterstitial_ShowSuccess() {
            ABIFirebaseManager.Instance.LogFirebaseEvent(ad_inter_show);
            ABIAppsflyerManager.TrackInterstitial_Displayed();
        }
        public void TrackAdsInterstitial_ShowFail() {
            ABIFirebaseManager.Instance.LogFirebaseEvent(ad_inter_fail);
        }
        public void TrackAdsInterstitial_ClickOnButton() {
            ABIFirebaseManager.Instance.LogFirebaseEvent(ad_inter_click);
            ABIAppsflyerManager.TrackInterstitial_ClickShowButton();
        }
        #endregion

        #endregion

        public void TrackUnlockRoom(RoomID roomID, int world) {
            string s = world + "_" + roomID.ToString();
            string day = ProfileManager.PlayerData.GetCustomTimeManager().DayRealPlayed + "";
            Parameter[] parameters = new Parameter[] {
                new Parameter("roomID",s),
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent("OpenRoom", parameters);
        }

        public void TrackSessionStart(int id) {
            string eventName = "session_start_" + id;
            ABIAppsflyerManager.SendEvent(eventName, null);
            ABIFirebaseManager.Instance.LogFirebaseEvent(eventName);
        }
        public void TrackEventMapComplete(int map) {

            string eventName = "map" + map + "_completed";
            ABIAppsflyerManager.SendEvent(eventName, null);
            ABIFirebaseManager.Instance.LogFirebaseEvent(eventName);
        }

        public void TrackEventWareHouse(WareHouseAction action) {
            Parameter[] parameters = new Parameter[] {
                new Parameter("action",action.ToString())
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent("WareHouse", parameters);
        }
        public void TrackEventResearch(ResearchAction action, string sub) {
            Parameter[] parameters = new Parameter[] {
                new Parameter("action",action.ToString()+"_"+name.ToString())
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent("Research", parameters);
        }
        public void TrackEventResearch(ResearchAction action) {
            Parameter[] parameters = new Parameter[] {
                new Parameter("action",action.ToString())
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent("Research", parameters);
        }
        public void TrackEventGem(GemAction action, string sub_action = "") {
            Parameter[] parameters = new Parameter[] {
                new Parameter("action",action.ToString() +sub_action)
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent("GemEvent", parameters);
        }
        public void TrackEventGem(GemAction action) {
            Parameter[] parameters = new Parameter[] {
                new Parameter("action",action.ToString() )
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent("GemEvent", parameters);
        }
        public void TrackEventResearchSkipGem(int value) {
            Parameter[] parameters = new Parameter[] {
                new Parameter("action",value.ToString() )
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent("ResearchGemEvent", parameters);
        }
        public void TrackEventGem(GemAction action, int value = 0) {
            Parameter[] parameters = new Parameter[] {
                new Parameter("action",action.ToString()+"_"+value)
        };
            ABIFirebaseManager.Instance.LogFirebaseEvent("GemEvent", parameters);
        }
        public void TrackEventStarUp(int current) {
            Parameter[] parameters = new Parameter[] {
                new Parameter("action",current.ToString() )
            };
            ABIFirebaseManager.Instance.LogFirebaseEvent("StarProcess", parameters);
        }

    }
    public enum WareHouseAction {
        CheckIn,
        Ticket_BuyEnergy,
        WatchAds_BuyEnergy,
        UseGem_BuyEnergy,
        Open1Box,
        Open10Box,
        UseGem_SkipTime
    }
    public enum ResearchAction {
        CheckIn,
        WatchAds_Repulation,
        UseGem_Repulation,
        BuyIAP_Repulation,
        Research,
        Upgrade,
        WatchAds_SkipTime,
        UseGem_SkipTime,
        UseGem_BuySlot
    }
    public enum GemAction {
        Earn_IAP,
        Earn_Ads,
        Spend_X3_Reputation,
        Spend_X3_Cash,
        Spend_SkipResearch,
        Spend_WareHouse_MoreEnergy,
        Spend_WareHouse_SkipBlockTime,
        Spend_TimeTravel,
        Spend_OpenBox,
        Spend_Skin,
        Spend_BuyReputation,
        Earn_WareHouse_Box,
        Earn_RoomProcces,
        Earn_StarProcess,
        Earn_Quest,
        Earn_Ads_Daily,
        GetBestOffer,
        GetNewOffer,
        SkipOffer
    }
    public class ImpressionData {
        public string ad_platform;
        public string ad_source;
        public string ad_unit_name;
        public string ad_format;
        public double ad_revenue;
        public string ad_currency;
    }
}
