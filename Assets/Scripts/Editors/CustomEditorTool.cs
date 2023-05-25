using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class CustomEditorTool : EditorWindow {
    [MenuItem("Tool/Custom/ClearData")]
    public static void ClearData() {
        Debug.Log("ClearData");
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tool/Custom/Localizes")]
    public static void CreatTextLocalizes() {
        string dataPath = "Assets/AssetDatas/Localizes.asset";
        GameText asset = AssetDatabase.LoadAssetAtPath<GameText>(dataPath);
        if (asset == null) {
            Debug.Log("Creat New Data Localizes");
            asset = CreateInstance<GameText>();
            FillDataLocalize(asset);

            AssetDatabase.CreateAsset(asset, dataPath);
            AssetDatabase.SaveAssets();
        } else {
            Debug.Log("Overide Data Localizes");
            FillDataLocalize(asset);
        }
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

    }
    static void FillDataLocalize(GameText asset) {
        string enPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - EN.csv";
        FillDataLocalizeByCountry(enPath, asset.ens);
        //string cnPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - CN.csv";
        //FillDataLocalizeByCountry(cnPath, asset.cns);
        //string dePath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - DE.csv";
        //FillDataLocalizeByCountry(dePath, asset.des);
        //string esPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - ES.csv";
        //FillDataLocalizeByCountry(esPath, asset.ess);
        //string frPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - FR.csv";
        //FillDataLocalizeByCountry(frPath, asset.frs);
        ////string inPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - IN.csv";
        ////FillDataLocalizeByCountry(inPath, asset.inds);
        //string itPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - IT.csv";
        //FillDataLocalizeByCountry(itPath, asset.its);
        //string jpPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - JP.csv";
        //FillDataLocalizeByCountry(jpPath, asset.jps);
        //string koPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - KO.csv";
        //FillDataLocalizeByCountry(koPath, asset.kos);
        //string ptPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - PT.csv";
        //FillDataLocalizeByCountry(ptPath, asset.pts);
        //string vnPath = "Assets/AssetDatas/csv/Localizes/Abi Idle Restaurent Localize - VN.csv";
        //FillDataLocalizeByCountry(vnPath, asset.vns);
#if UNITY_EDITOR
        EditorUtility.SetDirty(asset);
#endif
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static void FillDataLocalizeByCountry(string path, List<Localizes> language) {
        TextAsset txtdata = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        List<Dictionary<string, object>> dataCN = CSVReader.Read2Col(txtdata);
        language.Clear();
        for (var i = 0; i < dataCN.Count; i++) {
            string strID = dataCN[i]["ID"].ToString();
            int id = int.Parse(strID);
            string value = dataCN[i]["Text"].ToString();
            Localizes data = new Localizes() { ID = id, Text = value };
            language.Add(data);
        }
    }
    [MenuItem("Tool/Custom/Creat Card Data")]
    public static void CreatCardData() {
        string cardPath = "Assets/AssetDatas/Card/CardDataConfig.asset";
        CardDataConfig asset = AssetDatabase.LoadAssetAtPath<CardDataConfig>(cardPath);
        if (asset == null) {
            Debug.Log("Creat New Data CardDataConfig");
            asset = ScriptableObject.CreateInstance<CardDataConfig>();
            FillDataCard(asset);
            AssetDatabase.CreateAsset(asset, cardPath);
            AssetDatabase.SaveAssets();
        } else {
            Debug.Log("Overide Data CardDataConfig");
            FillDataCard(asset);
        }
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    static void FillDataCard(CardDataConfig asset) {
        string cardTextPath = "Assets/AssetDatas/csv/Idle Burger - Card.csv";
        string cardSpritePath = "Assets/Sprites/card/card_game/";
        TextAsset cardData = AssetDatabase.LoadAssetAtPath<TextAsset>(cardTextPath);
        List<Dictionary<string, object>> data = CSVReader.Read(cardData);

        var cardList = new List<CardNormalConfig>();
        for (var i = 0; i < 15; i++) {
            string strID = data[i]["ID"].ToString();
            if (string.IsNullOrEmpty(strID)) continue;
            int id = int.Parse(data[i]["ID"].ToString());
            string name = data[i]["Card Name"].ToString();
            int nameLocalizeID = int.Parse(data[i]["Name Localize ID"].ToString());
            int desLocalizeID = int.Parse(data[i]["Des Localize ID"].ToString());
            string des = data[i]["Description"].ToString();
            string rarity = data[i]["Rarity"].ToString();
            string sprOnName = data[i]["SpriteOn"].ToString();
            string sprOffName = data[i]["SpriteOff"].ToString();
            Sprite sprOn = AssetDatabase.LoadAssetAtPath<Sprite>(cardSpritePath + sprOnName + ".png");
            Sprite sprOff = AssetDatabase.LoadAssetAtPath<Sprite>(cardSpritePath + sprOffName + ".png");
            CardNormalConfig card = new CardNormalConfig();
            card.ID = id;
            card.name = name;
            card.NameLocalizeID = nameLocalizeID;
            card.description = des;
            card.DesLocalizeID = desLocalizeID;
            card.cardRarity = (Rarity)Enum.Parse(typeof(Rarity), rarity);
            card.cardValues = new List<float>();
            for (int k = 0; k < 5; k++) {
                card.cardValues.Add(float.Parse(data[i]["Level " + (k + 1)].ToString()));
            }
            card.cardAmountLevel = new List<int>() { 1, 6, 12, 18, 24 };
            card.sprOn = sprOn;
            card.sprOff = sprOff;
            cardList.Add(card);
        }
        asset.cardList = cardList;
#if UNITY_EDITOR
        EditorUtility.SetDirty(asset);
#endif
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    [MenuItem("Tool/Custom/Creat Quest Data_W1")]
    public static void CreatQuestData_W1() {
        RoomLoadID = 1;
        string questPath = "Assets/AssetDatas/QuestDataConfig.asset";
        QuestData asset = AssetDatabase.LoadAssetAtPath<QuestData>(questPath);
        if (asset == null) {
            Debug.Log("Creat New Data QuestData Config");
            asset = ScriptableObject.CreateInstance<QuestData>();
            FillDataQuest(asset);
            AssetDatabase.CreateAsset(asset, questPath);
            AssetDatabase.SaveAssets();
        } else {
            Debug.Log("Overide Data QuestData Config W1");
            FillDataQuest(asset);
        }
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    [MenuItem("Tool/Custom/Creat Quest Data_W2")]

    public static void CreatQuestData_W2() {
        RoomLoadID = 2;
        string questPath = "Assets/AssetDatas/QuestDataConfig.asset";
        QuestData asset = AssetDatabase.LoadAssetAtPath<QuestData>(questPath);
        if (asset == null) {
            Debug.Log("Creat New Data QuestData Config W2");
            asset = ScriptableObject.CreateInstance<QuestData>();
            FillDataQuest(asset);
            AssetDatabase.CreateAsset(asset, questPath);
            AssetDatabase.SaveAssets();
        } else {
            Debug.Log("Overide Data QuestData Config W2");
            FillDataQuest(asset);
        }
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    [MenuItem("Tool/Custom/Creat Quest Data_W3")]
    public static void CreatQuestData_W3() {
        RoomLoadID = 3;
        string questPath = "Assets/AssetDatas/QuestDataConfig.asset";
        QuestData asset = AssetDatabase.LoadAssetAtPath<QuestData>(questPath);
        if (asset == null) {
            Debug.Log("Creat New Data QuestData Config W3");
            asset = ScriptableObject.CreateInstance<QuestData>();
            FillDataQuest(asset);
            AssetDatabase.CreateAsset(asset, questPath);
            AssetDatabase.SaveAssets();
        } else {
            Debug.Log("Overide Data QuestData Config W3");
            FillDataQuest(asset);
        }
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    static void FillDataQuest(QuestData asset) {
        string questTextPath = "Assets/AssetDatas/csv/Idle Burger - Quest_W" + RoomLoadID + ".csv";
        TextAsset questData = AssetDatabase.LoadAssetAtPath<TextAsset>(questTextPath);
        List<Dictionary<string, object>> data = CSVReader.Read(questData);
        List<Quest> listConfig = new List<Quest>();

        listConfig = new List<Quest>();
        for (int i = listConfig.Count; i < data.Count; i++) {
            listConfig.Add(new Quest());
        }
        for (var i = 0; i < data.Count; i++) {
            int id = int.Parse(data[i]["QuestID"].ToString());
            string typeStr = data[i]["Type"].ToString();
            string roomTarget = data[i]["RoomTarget"].ToString();
            int priority = int.Parse(data[i]["Priority"].ToString());
            int level = int.Parse(data[i]["Require"].ToString());
            string strgem = data[i]["Gem Reward"].ToString();
            int gemReward = string.IsNullOrEmpty(strgem) ? 0 : int.Parse(strgem);
            string strcash = data[i]["Cash Reward"].ToString();
            int cashReward = string.IsNullOrEmpty(strcash) ? 0 : int.Parse(strcash);
            Quest quest = new Quest();
            quest.questID = id;
            quest.type = (QuestType)Enum.Parse(typeof(QuestType), typeStr);
            quest.roomTarget = (RoomID)Enum.Parse(typeof(RoomID), roomTarget);
            quest.priority = priority;
            quest.level = level;
            quest.reward = new ItemReward();

            if (cashReward > 0) {
                quest.reward.type = ItemType.Cash;
                quest.reward.amount = cashReward;
            } else if (gemReward > 0) {
                quest.reward.type = ItemType.Gem;
                quest.reward.amount = gemReward;
            }
            listConfig[i] = quest;
        }

        // Sort Quest list by Priority
        listConfig.Sort((x, y) => x.priority.CompareTo(y.priority));
        if (RoomLoadID == 1) asset.questList_W1 = listConfig;
        else if (RoomLoadID == 2) asset.questList_W2 = listConfig;
        else if (RoomLoadID == 3) asset.questList_W3 = listConfig;
#if UNITY_EDITOR
        EditorUtility.SetDirty(asset);
#endif
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    [MenuItem("Tool/Custom/Creat Research")]
    public static void CreatResearch() {
        string path = "Assets/AssetDatas/ResearchData.asset";
        ResearchData asset = AssetDatabase.LoadAssetAtPath<ResearchData>(path);
        if (asset == null) {
            Debug.Log("Creat New ResearchData");
            asset = ScriptableObject.CreateInstance<ResearchData>();
            FillResearchData(asset);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
        } else {
            Debug.Log("Overide Data QuestData Config W3");
            FillResearchData(asset);
        }
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
    static void FillResearchData(ResearchData asset) {
        string questTextPath = "Assets/AssetDatas/csv/Idle Burger - ResearchMenu" + ".csv";
        string spritePath = "Assets/3D Art Lv2_3/Foods and Drinks/icons/";
        string modelPath = "Assets/3D Art Lv2_3/Foods and Drinks/Models/";
        TextAsset questData = AssetDatabase.LoadAssetAtPath<TextAsset>(questTextPath);
        List<Dictionary<string, object>> data = CSVReader.Read(questData);
        List<Research> listConfig = new List<Research>();
        for (int i = listConfig.Count; i < data.Count; i++) {
            listConfig.Add(new Research());
        }
        for (var i = 0; i < data.Count; i++) {
            string strName = data[i]["Name"].ToString();
            string strResearchType = data[i]["ResearchType"].ToString();
            string strModelName = data[i]["3D Name"].ToString();
            int profit = int.Parse(data[i]["Profit"].ToString());
            float time = float.Parse(data[i]["Food Time"].ToString());
            int upgradePrice = int.Parse(data[i]["Upgrade Price"].ToString());
            int blockTime = int.Parse(data[i]["Block Time"].ToString());
            Research research = new Research() {
                foodName = strName,
                researchType = (ResearchType)Enum.Parse(typeof(ResearchType), strResearchType),
                objFood = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath + strModelName + ".fbx"),
                foodIcon = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath + strModelName + ".png"),
                foodProfit = profit,
                makeTime = time,
                priceUpgrade = upgradePrice,
                foodBlockTime = blockTime * 60
            };
            listConfig[i] = research;
        }
        asset.foodResearchs = listConfig;
#if UNITY_EDITOR
        EditorUtility.SetDirty(asset);
#endif
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    static int RoomLoadID;
    [MenuItem("Tool/Custom/LoadRoomCost_W1")]
    static void LoadRoomCost() {
        RoomLoadID = 1;
        string path = "Assets/AssetDatas/csv/Idle Burger - W1.csv";
        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        List<Dictionary<string, object>> data = CSVReader.Read(textAsset);
        string roomName = "";
        for (var i = 0; i < data.Count; i++) {
            string name = data[i]["RoomID"].ToString();
            if (!string.IsNullOrEmpty(name)) {
                roomName = name;
            }
            SetRoomData(roomName, data[i]);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    [MenuItem("Tool/Custom/LoadRoomCost_W2")]
    static void LoadRoomCost2() {
        RoomLoadID = 2;
        string path = "Assets/AssetDatas/csv/Idle Burger - W2.csv";
        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        List<Dictionary<string, object>> data = CSVReader.Read(textAsset);
        string roomName = "";
        for (var i = 0; i < data.Count; i++) {
            string name = data[i]["RoomID"].ToString();
            if (!string.IsNullOrEmpty(name)) {
                roomName = name;
            }
            SetRoomData(roomName, data[i]);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    [MenuItem("Tool/Custom/LoadRoomCost_W3")]
    static void LoadRoomCost3() {
        RoomLoadID = 3;
        string path = "Assets/AssetDatas/csv/Idle Burger - W3.csv";
        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        List<Dictionary<string, object>> data = CSVReader.Read(textAsset);
        string roomName = "";
        for (var i = 0; i < data.Count; i++) {
            string name = data[i]["RoomID"].ToString();
            if (!string.IsNullOrEmpty(name)) {
                roomName = name;
            }
            SetRoomData(roomName, data[i]);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private static void SetRoomData(string roomID, Dictionary<string, object> data) {
        string sRoomID = data["RoomID"].ToString();
        string sModelType = data["ModelType"].ToString();
        string sBuildPrice = data["BuildPrice"].ToString();
        string sBuildStar = data["BuildStar"].ToString();
        string sBuildTime = data["BuildTime"].ToString();
        string sBuildEnergy = data["BuildEnergy"].ToString();
        string sBaseProfit = data["BaseProfit"].ToString();
        string sProfitIncreaseRate = data["ProfitIncreaseRate"].ToString();
        string sBaseCost = data["BaseCost"].ToString();
        string sCostIncreaseRate = data["CostIncreaseRate"].ToString();
        string sLevelMax = data["LevelMax"].ToString();
        string sTimeService = data["TimeService"].ToString();

        #region Build Room Data
        int buildPrice = string.IsNullOrEmpty(sBuildPrice) ? 0 : int.Parse(sBuildPrice);
        int buildStar = string.IsNullOrEmpty(sBuildStar) ? 0 : int.Parse(sBuildStar);
        int buildTime = string.IsNullOrEmpty(sBuildTime) ? 0 : int.Parse(sBuildTime);
        int buildEnergy = string.IsNullOrEmpty(sBuildEnergy) ? 0 : int.Parse(sBuildEnergy);
        #endregion

        #region Upgrade Model in Room Data
        int levelMax = string.IsNullOrEmpty(sLevelMax) ? 0 : int.Parse(sLevelMax);
        int baseCost = string.IsNullOrEmpty(sBaseCost) ? 0 : int.Parse(sBaseCost);
        float costIncreaseRate = string.IsNullOrEmpty(sCostIncreaseRate) ? 0f : float.Parse(sCostIncreaseRate);
        float baseProfit = string.IsNullOrEmpty(sBaseProfit) ? 0f : float.Parse(sBaseProfit);
        float profitIncreaseRate = string.IsNullOrEmpty(sProfitIncreaseRate) ? 0f : float.Parse(sProfitIncreaseRate);
        float timeService = string.IsNullOrEmpty(sTimeService) ? 0f : float.Parse(sTimeService);

        List<BigNumber> upgradePrices = new List<BigNumber>();
        BigNumber price = new BigNumber(baseCost);
        for (int k = 0; k <= levelMax; k++) {
            upgradePrices.Add(price);
            price += price * (costIncreaseRate / 100f);
        }
        #endregion

        #region Enery 
        string sBaseEnergy = data["BaseEnergy"].ToString();
        string sEnergyInCreaseRate = data["EnergyIncreaseRate"].ToString();
        int baseEnergy = string.IsNullOrEmpty(sBaseEnergy) ? 0 : int.Parse(sBaseEnergy);
        int energyIncreaseRate = string.IsNullOrEmpty(sEnergyInCreaseRate) ? 0 : int.Parse(sEnergyInCreaseRate);
        float energyEarn = baseEnergy;
        List<int> energyEarns = new List<int>();
        energyEarns.Add(0);
        for (int k = 1; k <= levelMax; k++) {
            energyEarn += energyEarn * energyIncreaseRate / 100f;
            energyEarns.Add((int)energyEarn);
        }
        #endregion

        List<float> reduceTimes = new List<float>();
        reduceTimes.Add(0);

        RoomID currentRoomID = (RoomID)Enum.Parse(typeof(RoomID), roomID);
        if (!string.IsNullOrEmpty(sRoomID)) FillBuildDataRoom(currentRoomID, buildPrice, buildStar, buildTime, buildEnergy);
        switch (currentRoomID) {
            case RoomID.Lobby:
                LobbyModelType lobbyModelType = (LobbyModelType)Enum.Parse(typeof(LobbyModelType), sModelType);
                LobbyRoomDataAsset lobby = GetAllRoomDataAssets<LobbyRoomDataAsset>()[0];
                if (timeService > 0) lobby.timeService = timeService;
                if (baseProfit > 0) lobby.profitBase = baseProfit;
                for (int i = 0; i < lobby.datas.Count; i++) {
                    BaseModelData<LobbyModelType> modelData = lobby.datas[i];
                    if (modelData.type == lobbyModelType) {
                        FillDataRoom(modelData, profitIncreaseRate, upgradePrices, reduceTimes, levelMax);
                    }
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(lobby);
#endif
                break;
            case RoomID.Table1:
            case RoomID.Table2:
            case RoomID.Table3:
            case RoomID.Table4:
            case RoomID.Table5:
            case RoomID.Table6:
                SmallTableModelType smallTableModelType = (SmallTableModelType)Enum.Parse(typeof(SmallTableModelType), sModelType);
                SmallTableRoomDataAsset[] smallTableRoomDataAssets = GetAllRoomDataAssets<SmallTableRoomDataAsset>();
                for (int s = 0; s < smallTableRoomDataAssets.Length; s++) {
                    SmallTableRoomDataAsset smallTableRoomDataAsset = smallTableRoomDataAssets[s];
                    if (smallTableRoomDataAsset.roomID == currentRoomID) {
                        if (timeService > 0) smallTableRoomDataAsset.timeService = timeService;
                        if (baseProfit > 0) smallTableRoomDataAsset.profitBase = baseProfit;
                        for (int i = 0; i < smallTableRoomDataAsset.datas.Count; i++) {
                            BaseModelData<SmallTableModelType> modelData = smallTableRoomDataAsset.datas[i];
                            if (modelData.type == smallTableModelType) {
                                FillDataRoom(modelData, profitIncreaseRate, upgradePrices, reduceTimes, levelMax);
                            }
                        }
#if UNITY_EDITOR
                        EditorUtility.SetDirty(smallTableRoomDataAsset);
#endif
                        break;
                    }
                }
                break;
            case RoomID.BigTable1:
            case RoomID.BigTable2:
            case RoomID.BigTable3:
            case RoomID.BigTable4:
            case RoomID.BigTable5:
            case RoomID.BigTable6:
            case RoomID.BigTable7:
            case RoomID.BigTable8:
            case RoomID.BigTable9:
            case RoomID.BigTable10:
            case RoomID.BigTable11:
            case RoomID.BigTable12:
            case RoomID.BigTable13:
            case RoomID.BigTable14:
                BigTableModelType bigTableModelType = (BigTableModelType)Enum.Parse(typeof(BigTableModelType), sModelType);
                BigTableRoomDataAsset[] bigTableRoomDataAssets = GetAllRoomDataAssets<BigTableRoomDataAsset>();
                for (int s = 0; s < bigTableRoomDataAssets.Length; s++) {
                    BigTableRoomDataAsset bigTableRoomDataAsset = bigTableRoomDataAssets[s];
                    if (bigTableRoomDataAsset.roomID == currentRoomID) {
                        if (timeService > 0) bigTableRoomDataAsset.timeService = timeService;
                        if (baseProfit > 0) bigTableRoomDataAsset.profitBase = baseProfit;
                        for (int i = 0; i < bigTableRoomDataAsset.datas.Count; i++) {
                            BaseModelData<BigTableModelType> modelData = bigTableRoomDataAsset.datas[i];
                            if (modelData.type == bigTableModelType) {
                                FillDataRoom(modelData, profitIncreaseRate, upgradePrices, reduceTimes, levelMax);
                            }
                        }
#if UNITY_EDITOR
                        EditorUtility.SetDirty(bigTableRoomDataAsset);
#endif
                        break;
                    }
                }
                break;
            case RoomID.Kitchen:
                KitchenModelType kitchenModelType = (KitchenModelType)Enum.Parse(typeof(KitchenModelType), sModelType);
                KitchenRoomDataAsset kitchenRoomDataAsset = GetAllRoomDataAssets<KitchenRoomDataAsset>()[0];
                if (timeService > 0) kitchenRoomDataAsset.timeService = timeService;
                if (baseProfit > 0) kitchenRoomDataAsset.profitBase = baseProfit;
                for (int i = 0; i < kitchenRoomDataAsset.datas.Count; i++) {
                    BaseModelData<KitchenModelType> modelData = kitchenRoomDataAsset.datas[i];
                    if (modelData.type == kitchenModelType) {
                        FillDataRoom(modelData, profitIncreaseRate, upgradePrices, reduceTimes, levelMax);
                    }
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(kitchenRoomDataAsset);
#endif
                break;
            case RoomID.Restroom:
            case RoomID.Restroom2:
                RestroomModelType restroomModelType = (RestroomModelType)Enum.Parse(typeof(RestroomModelType), sModelType);
                RestroomDataAsset rroomDataAsset = GetAllRoomDataAssets<RestroomDataAsset>()[0];
                if (timeService > 0) rroomDataAsset.timeService = timeService;
                if (baseProfit > 0) rroomDataAsset.profitBase = baseProfit;
                for (int i = 0; i < rroomDataAsset.datas.Count; i++) {
                    BaseModelData<RestroomModelType> modelData = rroomDataAsset.datas[i];
                    if (modelData.type == restroomModelType) {
                        FillDataRoom(modelData, profitIncreaseRate, upgradePrices, reduceTimes, levelMax);
                    }
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(rroomDataAsset);
#endif
                break;

            case RoomID.DeliverRoom:
                DeliverModelType coffeeModelType = (DeliverModelType)Enum.Parse(typeof(DeliverModelType), sModelType);
                DeliverRoomDataAsset roomDataAsset = GetAllRoomDataAssets<DeliverRoomDataAsset>()[0];
                if (timeService > 0) roomDataAsset.timeService = timeService;
                if (baseProfit > 0) roomDataAsset.profitBase = baseProfit;
                for (int i = 0; i < roomDataAsset.datas.Count; i++) {
                    BaseModelData<DeliverModelType> modelData = roomDataAsset.datas[i];
                    if (modelData.type == coffeeModelType) {
                        FillDataRoom(modelData, profitIncreaseRate, upgradePrices, reduceTimes, levelMax);
                    }
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(roomDataAsset);
#endif
                break;

            case RoomID.Power:
                PowerModelType powerModelType = (PowerModelType)Enum.Parse(typeof(PowerModelType), sModelType);
                PowerRoomDataAsset powerRoomDataAsset = GetAllRoomDataAssets<PowerRoomDataAsset>()[0];
                if (timeService > 0) powerRoomDataAsset.timeService = timeService;
                if (baseProfit > 0) powerRoomDataAsset.profitBase = baseProfit;
                for (int i = 0; i < powerRoomDataAsset.datas.Count; i++) {
                    BaseModelData<PowerModelType> modelData = powerRoomDataAsset.datas[i];
                    if (modelData.type == powerModelType) {
                        FillDataRoom(modelData, profitIncreaseRate, upgradePrices, reduceTimes, levelMax);
                        modelData.energyEarns = energyEarns;
                    }
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(powerRoomDataAsset);
#endif
                break;
        }

    }
    static void FillBuildDataRoom(RoomID roomID, int price, int star, int time, int energy) {
        BuildData build = GetAllBuildRoomDataAssets<BuildData>()[0];
        BuildDataSetting data = build.GetData(roomID);
        if (data != null) {
            data.energyRequire = energy;
            data.cashRequire = price;
            data.starRequire = star;
            data.timeRequire = time;
            Debug.Log("Overide: " + data.ToString());
        } else {
            data = new BuildDataSetting() {
                buildTarget = roomID,
                energyRequire = energy,
                cashRequire = price,
                starRequire = star,
                timeRequire = time
            };
            Debug.Log("Add New: " + data.ToString());
            build.datas.Add(data);
        }
#if UNITY_EDITOR
        EditorUtility.SetDirty(build);
#endif
    }
    static void FillDataRoom<T>(BaseModelData<T> model, float profitIncreaseRate, List<BigNumber> upgradePrices, List<float> reduceTimes, int levelMax) {
        model.profitIncreaseRate = profitIncreaseRate;
        model.upgradePrices = upgradePrices;
        model.reduceTimes = reduceTimes;
        model.levelMax = levelMax;
        ModelDataAsset modelConfigs = GetAllRoomDataAssets<ModelDataAsset>()[0];
        if (modelConfigs.FindModelsByType(model.type) != null)
            model.models3D = modelConfigs.FindModelsByType(model.type).ToArray();

    }

    static private T[] GetAllRoomDataAssets<T>() where T : ScriptableObject {
        string foderPath = "Assets/AssetDatas/Rooms/W" + RoomLoadID;
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { foderPath });
        T[] dataAssets = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            dataAssets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return dataAssets;
    }
    static private T[] GetAllBuildRoomDataAssets<T>() where T : ScriptableObject {
        string foderPath = "Assets/AssetDatas/Build/W" + RoomLoadID;
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { foderPath });
        T[] dataAssets = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++) {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            dataAssets[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return dataAssets;
    }
}
#endif