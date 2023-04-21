using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum GroupResearchName {
    Food,
    Drink,
    Extra
}
public enum ResearchName {
    //Normal
    HotDogs,
    FrenchFries,
    Hamburger,
    ChefSalad,
    FishAndChips,
    RoastBeef,
    BeefWellington,
    //Mexico
    Tacos,
    Tortas,
    Enchiladas,
    Tamales,
    ChiliRellenos,
    Pozole,
    Tostadas,
    //Japan
    Sushi,
    Ramen,
    Tempura,
    Tonkatsu,
    Takoyaki,
    Soba,
    ShabuShabu,
    //Build
    BigtableRoom,
    BarRoom,
    CoffeeRoom,
    VipRoom,
    ConveyorHotPotRoom,
    //Coffee
    Coffee,
    Tea,
    HotChocolate,
    Smoothie,
    Milkshake,
    Lemonade,
    FruitJuice,
    //Drink
    Martini,
    Mojito,
    Cosmopolitan,
    WhiskeySour,
    MoscowMule,
    GinAndTonic,
    Daiquiri
}

public enum ResearchType {
    DefaultFood,
    MexicoFood,
    JapanFood,
    DrinkBar,
    DrinkCoffee
}
public enum ResearchDependWorld {
    None,
    World2,
    World3,
}


[System.Serializable]
public class Research {
    public ResearchName researchName;
    public GameObject objFood;
    public Sprite icon;
    public string strName;
    public string strDes;
    public int desLocalizeID;
    public int profit;
    public float time;
    public int priceUpgrade;
    public int timeBlock;
    public int point;
    public ResearchType researchType;
    public ResearchDependWorld researchDependWorld = ResearchDependWorld.None;
    public double CalculateProfit(int level) {
        double newProfit = profit;
        for (int i = 2; i <= level; i++) {
            newProfit += System.Math.Round(newProfit * 0.1f, 2);
        }
        return System.Math.Round(newProfit, 2);
    }
    public double CalculateIncreaseNextProfit(double profit) {
        return System.Math.Round(profit * 0.2f, 2);
    }
    public int CalculatePoint(int level) {
        int newPoint = point/ 2;
        for (int i = 2; i <= level+1; i++) {
            newPoint += point / 20;
        }
        return newPoint;
    }
    public int CalculateIncreaseNextPoint() {
        return point / 20;
    }
    public int CalulateReseachPrice(int level) {
        if(level==0)  return priceUpgrade/2;
        return priceUpgrade / 20;
    }
}

[System.Serializable]
public class GroupResearchData {
    public GroupResearchName researchGroupName;
    public List<Research> researches;
    public Research GetResearch(ResearchName researchName) {
        for (int i = 0; i < researches.Count; i++) {
            if (researches[i].researchName == researchName)
                return researches[i];
        }
        return null;
    }
    public Research GetResearchByID(int id) {
        for (int i = 0; i < researches.Count; i++) {
            if ((int)researches[i].researchName == id)
                return researches[i];
        }
        return null;
    }
}


[CreateAssetMenu(fileName = "ResearchData", menuName = "ScriptableObjects/ResearchData")]
public class ResearchData : ScriptableObject {
    public List<Sprite> sprConfig;
    public List<GameObject> objFoods;
    public List<GroupResearchData> groupResearches;
    public List<Research> avaiables_food_default = new List<Research>();
    public List<Research> avaiables_food_japan = new List<Research>();
    public List<Research> avaiables_food_mexico = new List<Research>();
    public Research GetResearch(ResearchName researchName) {
        for (int i = 0; i < groupResearches.Count; i++) {
            Research research = groupResearches[i].GetResearch(researchName);
            if (research != null)
                return research;
        }
        return null;
    }
    public Research GetResearchByID(int id) {
        for (int i = 0; i < groupResearches.Count; i++) {
            Research research = groupResearches[i].GetResearchByID(id);
            if (research != null)
                return research;
        }
        return null;
    }
    public List<Research> GetGroupResearch(GroupResearchName name) {
        foreach (var gr in groupResearches) {
            if (gr.researchGroupName == name) return gr.researches;
        }
        return null;
    }

    private void OnEnable() {

        groupResearches.Clear();
        LoadDefaultFood();
        //LoadJapanFood();
        //LoadMexicoFood();
        //LoadDrink();
        avaiables_food_default = GetGroupResearch(GroupResearchName.Food).Where(x => x.researchType == ResearchType.DefaultFood).ToList();
        //avaiables_food_japan = GetGroupResearch(GroupResearchName.Food).Where(x => x.researchType == ResearchType.JapanFood).ToList();
        //avaiables_food_mexico = GetGroupResearch(GroupResearchName.Food).Where(x => x.researchType == ResearchType.MexicoFood).ToList();
    }

    void LoadDefaultFood() {
        int groupIndex = GetIndexGroupResearchData(GroupResearchName.Food);
        if (groupIndex < 0)
            groupIndex = CreateGroupData(GroupResearchName.Food);
        Research hamburger = new Research() {
            researchName = ResearchName.Hamburger,
            objFood = GetObjFoodByName("Hamburger"),
            icon = GetSpriteByName("Hamburger"),
            strName = "Hamburger",
            strDes = " ",
            profit =  20 ,
            priceUpgrade =  100 ,
            time =  2f ,
            timeBlock =0 * 60 ,
            point = 20 ,
            researchType = ResearchType.DefaultFood,
        };

        Research frenchFries = new Research() {
            researchName = ResearchName.FrenchFries,
            objFood = GetObjFoodByName("French_fries"),
            icon = GetSpriteByName("French_fries"),
            strName = "French Fries",
            strDes = " ",
            profit =  30 ,
            priceUpgrade = 200 ,
            time = 2.3f ,
            timeBlock = 15 * 60 ,
            point = 50 ,
            researchType = ResearchType.DefaultFood,
        };

        Research hotdog = new Research() {
            researchName = ResearchName.HotDogs,
            objFood = GetObjFoodByName("Hot_dog"),
            icon = GetSpriteByName("Hot_dog"),
            strName = "Hot Dogs",
            strDes = " ",
            profit = 45 ,
            priceUpgrade = 400 ,
            time = 2.5f ,
            timeBlock =  25 * 60 ,
            point = 100 ,
            researchType = ResearchType.DefaultFood,
        };



        Research chefSalad = new Research() {
            researchName = ResearchName.ChefSalad,
            objFood = GetObjFoodByName("Chef_Salad"),
            icon = GetSpriteByName("Chef_Salad"),
            strName = "Chef Salad",
            strDes = " ",
            profit = 65 ,
            priceUpgrade =  800 ,
            time =  2.8f ,
            timeBlock =  45 * 60 ,
            point =  150 ,
            researchType = ResearchType.DefaultFood,
        };


        Research fishAndChips = new Research() {
            researchName = ResearchName.FishAndChips,
            objFood = GetObjFoodByName("Fish_and_chips"),
            icon = GetSpriteByName("Fish_and_chips"),
            strName = "Fish And Chips",
            strDes = " ",
            profit =  90 ,
            priceUpgrade = 1000 ,
            time = 3f ,
            timeBlock =  90 * 60 ,
            point = 200 ,
            researchType = ResearchType.DefaultFood,
        };


        Research roastBeef = new Research() {
            researchName = ResearchName.RoastBeef,
            objFood = GetObjFoodByName("Roast_beef"),
            icon = GetSpriteByName("Roast_beef"),
            strName = "Roast Beef",
            strDes = " ",
            profit = 120 ,
            priceUpgrade =  1500 ,
            time = 3.5f ,
            timeBlock = 120 * 60 ,
            point =  300 ,
            researchType = ResearchType.DefaultFood,
        };

        Research beefWellington = new Research() {
            researchName = ResearchName.BeefWellington,
            objFood = GetObjFoodByName("Wellington_Beef"),
            icon = GetSpriteByName("Wellington_Beef"),
            strName = "Beef Wellington",
            strDes = " ",
            profit =  150 ,
            priceUpgrade =  2000 ,
            time =  4f ,
            timeBlock =  150 * 60 ,
            point =  500 ,
            researchType = ResearchType.DefaultFood,
        };

        groupResearches[groupIndex].researches.Add(hamburger);
        groupResearches[groupIndex].researches.Add(frenchFries);
        groupResearches[groupIndex].researches.Add(hotdog);
        groupResearches[groupIndex].researches.Add(chefSalad);
        groupResearches[groupIndex].researches.Add(fishAndChips);
        groupResearches[groupIndex].researches.Add(roastBeef);
        groupResearches[groupIndex].researches.Add(beefWellington);
    }
    void LoadMexicoFood() {
        int groupIndex = GetIndexGroupResearchData(GroupResearchName.Food);
        if (groupIndex < 0)
            groupIndex = CreateGroupData(GroupResearchName.Food);
        Research tacos = new Research() {
            researchName = ResearchName.Tacos,
            objFood = GetObjFoodByName("MXC_Food_Tacos"),
            icon = GetSpriteByName("MXC_Food_Tacos"),
            strName = "Tacos",
            strDes = " ",
            profit =  30 ,
            priceUpgrade =  400 ,
            time =  2f ,
            timeBlock =  0 * 60 ,
            point =  50 ,
            researchType = ResearchType.MexicoFood,
        };

        Research tortas = new Research() {
            researchName = ResearchName.Tortas,
            objFood = GetObjFoodByName("MXC_Food_Tortas"),
            icon = GetSpriteByName("MXC_Food_Tortas"),
            strName = "Tortas",
            strDes = " ",
            profit =  40 ,
            priceUpgrade =  600 ,
            time =  2.3f ,
            timeBlock =  25 * 60 ,
            point =  250 ,
            researchType = ResearchType.MexicoFood,
        };

        Research enchiladas = new Research() {
            researchName = ResearchName.Enchiladas,
            objFood = GetObjFoodByName("MXC_Food_Enchiladas"),
            icon = GetSpriteByName("MXC_Food_Enchiladas"),
            strName = "Enchiladas",
            strDes = " ",
            profit =  60 ,
            priceUpgrade =  1000 ,
            time =  2.5f ,
            timeBlock =  45 * 60 ,
            point =  400 ,
            researchType = ResearchType.MexicoFood,
        };

        Research tamales = new Research() {
            researchName = ResearchName.Tamales,
            objFood = GetObjFoodByName("MXC_Food_Tamales"),
            icon = GetSpriteByName("MXC_Food_Tamales"),
            strName = "Tamales",
            strDes = " ",
            profit =  80 ,
            priceUpgrade =  1500 ,
            time =  2.8f ,
            timeBlock =  90 * 60 ,
            point =  600 ,
            researchType = ResearchType.MexicoFood,
        };

        Research chiliRellenos = new Research() {
            researchName = ResearchName.ChiliRellenos,
            objFood = GetObjFoodByName("MXC_Food_Chili_rellenos"),
            icon = GetSpriteByName("MXC_Food_Chili_rellenos"),
            strName = "Chili Rellenos",
            strDes = " ",
            profit =  120 ,
            priceUpgrade =  2000 ,
            time =  3f ,
            timeBlock =  120 * 60 ,
            point =  800 ,
            researchType = ResearchType.MexicoFood,
        };

        Research pozole = new Research() {
            researchName = ResearchName.Pozole,
            objFood = GetObjFoodByName("MXC_Food_Pozole"),
            icon = GetSpriteByName("MXC_Food_Pozole"),
            strName = "Pozole",
            strDes = " ",
            profit =  150 ,
            priceUpgrade =  2500 ,
            time =  3.5f ,
            timeBlock =  180 * 60 ,
            point =  1100 ,
            researchType = ResearchType.MexicoFood,
        };

        Research tostadas = new Research() {
            researchName = ResearchName.Tostadas,
            objFood = GetObjFoodByName("MXC_Food_Tostadas"),
            icon = GetSpriteByName("MXC_Food_Tostadas"),
            strName = "Tostadas",
            strDes = " ",
            profit =  200 ,
            priceUpgrade =  3000 ,
            time =  4f ,
            timeBlock =  240 * 60 ,
            point =  1500 ,
            researchType = ResearchType.MexicoFood,
        };

        groupResearches[groupIndex].researches.Add(tacos);
        groupResearches[groupIndex].researches.Add(tortas);
        groupResearches[groupIndex].researches.Add(enchiladas);
        groupResearches[groupIndex].researches.Add(tamales);
        groupResearches[groupIndex].researches.Add(chiliRellenos);
        groupResearches[groupIndex].researches.Add(pozole);
        groupResearches[groupIndex].researches.Add(tostadas);

    }
    void LoadJapanFood() {
        int groupIndex = GetIndexGroupResearchData(GroupResearchName.Food);
        if (groupIndex < 0)
            groupIndex = CreateGroupData(GroupResearchName.Food);

        Research sushi = new Research() {
            researchName = ResearchName.Sushi,
            objFood = GetObjFoodByName("JP_Food_Sushi_2"),
            icon = GetSpriteByName("JP_Food_Sushi_2"),
            strName = "Sushi",
            strDes = " ",
            profit =  25 ,
            priceUpgrade =  400 ,
            time =  2f ,
            timeBlock =  0 * 60 ,
            point =  50 ,
            researchType = ResearchType.JapanFood,
        };

        Research ramen = new Research() {
            researchName = ResearchName.Ramen,
            objFood = GetObjFoodByName("JP_Food_Ramen"),
            icon = GetSpriteByName("JP_Food_Ramen"),
            strName = "Ramen",
            strDes = " ",
            profit =  40 ,
            priceUpgrade =  500 ,
            time =  2.3f ,
            timeBlock =  15 * 60 ,
            point =  100 ,
            researchType = ResearchType.JapanFood,
        };

        Research tempura = new Research() {
            researchName = ResearchName.Tempura,
            objFood = GetObjFoodByName("Tempura"),
            icon = GetSpriteByName("Tempura"),
            strName = "Tempura",
            strDes = " ",
            profit =  55 ,
            priceUpgrade =  800 ,
            time =  2.5f ,
            timeBlock =  25 * 60 ,
            point =  200 ,
            researchType = ResearchType.JapanFood,
        };

        Research tonkatsu = new Research() {
            researchName = ResearchName.Tonkatsu,
            objFood = GetObjFoodByName("Tonkatsu"),
            icon = GetSpriteByName("Tonkatsu"),
            strName = "Tonkatsu",
            strDes = " ",
            profit =  75 ,
            priceUpgrade =  1000 ,
            time =  2.8f ,
            timeBlock =  45 * 60 ,
            point =  300 ,
            researchType = ResearchType.JapanFood,
        };

        Research takoyaki = new Research() {
            researchName = ResearchName.Takoyaki,
            objFood = GetObjFoodByName("Takoyaki"),
            icon = GetSpriteByName("Takoyaki"),
            strName = "Takoyaki",
            strDes = " ",
            profit =  100 ,
            priceUpgrade =  1500 ,
            time =  3f ,
            timeBlock =  90 * 60 ,
            point =  450 ,
            researchType = ResearchType.JapanFood,
        };

        Research soba = new Research() {
            researchName = ResearchName.Soba,
            objFood = GetObjFoodByName("Soba"),
            icon = GetSpriteByName("Soba"),
            strName = "Soba",
            strDes = " ",
            profit =  140 ,
            priceUpgrade =  2000 ,
            time =  3.5f ,
            timeBlock =  120 * 60 ,
            point =  700 ,
            researchType = ResearchType.JapanFood,
        };

        Research shabuShabu = new Research() {
            researchName = ResearchName.ShabuShabu,
            objFood = GetObjFoodByName("Shabu_shabu"),
            icon = GetSpriteByName("Shabu_shabu"),
            strName = "Shabu-Shabu",
            strDes = " ",
            profit =  160 ,
            priceUpgrade =  2500 ,
            time =  4f ,
            timeBlock =  150 * 60 ,
            point =  1000 ,
            researchType = ResearchType.JapanFood,
        };

        groupResearches[groupIndex].researches.Add(sushi);
        groupResearches[groupIndex].researches.Add(ramen);
        groupResearches[groupIndex].researches.Add(tempura);
        groupResearches[groupIndex].researches.Add(tonkatsu);
        groupResearches[groupIndex].researches.Add(takoyaki);
        groupResearches[groupIndex].researches.Add(soba);
        groupResearches[groupIndex].researches.Add(shabuShabu);
    }

    void LoadDrink() {
        int groupIndex = GetIndexGroupResearchData(GroupResearchName.Drink);
        if (groupIndex < 0)
            groupIndex = CreateGroupData(GroupResearchName.Drink);

        Research martini = new Research() {
            researchName = ResearchName.Martini,
            objFood = GetObjFoodByName("Martini"),
            icon = GetSpriteByName("Martini"),
            strName = "Martini",
            strDes = " ",
            profit =  40 ,
            priceUpgrade =  300 ,
            time =  2f ,
            timeBlock =  0 * 60 ,
            point =  50 ,
            researchType = ResearchType.DrinkBar,
        };

        Research mojito = new Research() {
            researchName = ResearchName.Mojito,
            objFood = GetObjFoodByName("Mojito"),
            icon = GetSpriteByName("Mojito"),
            strName = "Mojito",
            strDes = " ",
            profit =  50 ,
            priceUpgrade =  500 ,
            time =  2.5f ,
            timeBlock =  25 * 60 ,
            point =  150 ,
            researchType = ResearchType.DrinkBar,
        };

        Research cosmopolitan = new Research() {
            researchName = ResearchName.Cosmopolitan,
            objFood = GetObjFoodByName("Cosmopolitan"),
            icon = GetSpriteByName("Cosmopolitan"),
            strName = "Cosmopolitan",
            strDes = " ",
            profit =  65 ,
            priceUpgrade =  800 ,
            time =  2.8f ,
            timeBlock =  45 * 60 ,
            point =  250 ,
            researchType = ResearchType.DrinkBar,
        };

        Research whiskeySour = new Research() {
            researchName = ResearchName.WhiskeySour,
            objFood = GetObjFoodByName("Whiskey_sour"),
            icon = GetSpriteByName("Whiskey_sour"),
            strName = "Whiskey Sour",
            strDes = " ",
            profit =  90 ,
            priceUpgrade =  1200 ,
            time =  3f ,
            timeBlock =  90 * 60 ,
            point =  450 ,
            researchType = ResearchType.DrinkBar,
            researchDependWorld = ResearchDependWorld.World2
        };

        Research moscowMule = new Research() {
            researchName = ResearchName.MoscowMule,
            objFood = GetObjFoodByName("Moscow_Mule"),
            icon = GetSpriteByName("Moscow_Mule"),
            strName = "Moscow Mule",
            strDes = " ",
            profit =  120 ,
            priceUpgrade =  1600 ,
            time =  3.3f ,
            timeBlock =  120 * 60 ,
            point =  600 ,
            researchType = ResearchType.DrinkBar,
            researchDependWorld = ResearchDependWorld.World2
        };

        Research ginAndTonic = new Research() {
            researchName = ResearchName.GinAndTonic,
            objFood = GetObjFoodByName("Gin_and_tonic"),
            icon = GetSpriteByName("Gin_and_tonic"),
            strName = "Gin And Tonic",
            strDes = " ",
            profit =  150 ,
            priceUpgrade =  2000 ,
            time =  3.6f ,
            timeBlock =  180 * 60 ,
            point =  800 ,
            researchType = ResearchType.DrinkBar,
            researchDependWorld = ResearchDependWorld.World3
        };

        Research daiquiri = new Research() {
            researchName = ResearchName.Daiquiri,
            objFood = GetObjFoodByName("Daiquiri"),
            icon = GetSpriteByName("Daiquiri"),
            strName = "Daiquiri",
            strDes = " ",
            profit =  180 ,
            priceUpgrade =  2200 ,
            time =  4f ,
            timeBlock =  240 * 60 ,
            point =  1200 ,
            researchType = ResearchType.DrinkBar,
            researchDependWorld = ResearchDependWorld.World3
        };

        //Coffee
        Research coffee = new Research() {
            researchName = ResearchName.Coffee,
            objFood = GetObjFoodByName("Coffee"),
            icon = GetSpriteByName("Coffee"),
            strName = "Coffee",
            strDes = " ",
            profit =  50 ,
            priceUpgrade =  400 ,
            time =  2.5f ,
            timeBlock =  0 * 60 ,
            point =  50 ,
            researchType = ResearchType.DrinkCoffee,
        };

        Research tea = new Research() {
            researchName = ResearchName.Tea,
            objFood = GetObjFoodByName("Tea"),
            icon = GetSpriteByName("Tea"),
            strName = "Tea",
            strDes = " ",
            profit =  65 ,
            priceUpgrade =  600 ,
            time =  2.8f ,
            timeBlock =  25 * 60 ,
            point =  250 ,
            researchType = ResearchType.DrinkCoffee,
        };

        Research hotChocolate = new Research() {
            researchName = ResearchName.HotChocolate,
            objFood = GetObjFoodByName("Hot Chocolate"),
            icon = GetSpriteByName("Hot-Chocolate"),
            strName = "Hot Chocolate",
            strDes = " ",
            profit =  80 ,
            priceUpgrade =  1000 ,
            time =  3.3f ,
            timeBlock =  45 * 60 ,
            point =  350 ,
            researchType = ResearchType.DrinkCoffee,
        };

        Research smoothie = new Research() {
            researchName = ResearchName.Smoothie,
            objFood = GetObjFoodByName("Smoothie"),
            icon = GetSpriteByName("Smoothie"),
            strName = "Smoothie",
            strDes = " ",
            profit =  110 ,
            priceUpgrade =  1500 ,
            time =  3.5f ,
            timeBlock =  90 * 60 ,
            point =  500 ,
            researchType = ResearchType.DrinkCoffee,
            researchDependWorld = ResearchDependWorld.World2
        };

        Research milkshake = new Research() {
            researchName = ResearchName.Milkshake,
            objFood = GetObjFoodByName("Milkshake"),
            icon = GetSpriteByName("Milkshake"),
            strName = "Milk Shake",
            strDes = " ",
            profit =  140 ,
            priceUpgrade =  2000 ,
            time =  3.8f ,
            timeBlock =  120 * 60 ,
            point =  800 ,
            researchType = ResearchType.DrinkCoffee,
            researchDependWorld = ResearchDependWorld.World2
        };

        Research lemonade = new Research() {
            researchName = ResearchName.Lemonade,
            objFood = GetObjFoodByName("Lemonade"),
            icon = GetSpriteByName("Lemonade"),
            strName = "Lemonade",
            strDes = " ",
            profit =  180 ,
            priceUpgrade =  2200 ,
            time =  4.2f ,
            timeBlock =  180 * 60 ,
            point =  1200 ,
            researchType = ResearchType.DrinkCoffee,
            researchDependWorld = ResearchDependWorld.World3
        };

        Research fruitJuice = new Research() {
            researchName = ResearchName.FruitJuice,
            objFood = GetObjFoodByName("Fruit_juice"),
            icon = GetSpriteByName("Fruit_juice"),
            strName = "Fruit Juice",
            strDes = " ",
            profit =  200 ,
            priceUpgrade =  2500 ,
            time =  4.5f ,
            timeBlock =  240 * 60 ,
            point =  1500 ,
            researchType = ResearchType.DrinkCoffee,
            researchDependWorld = ResearchDependWorld.World3
        };

        groupResearches[groupIndex].researches.Add(martini);
        groupResearches[groupIndex].researches.Add(mojito);
        groupResearches[groupIndex].researches.Add(cosmopolitan);
        groupResearches[groupIndex].researches.Add(whiskeySour);
        groupResearches[groupIndex].researches.Add(moscowMule);
        groupResearches[groupIndex].researches.Add(ginAndTonic);
        groupResearches[groupIndex].researches.Add(daiquiri);

        groupResearches[groupIndex].researches.Add(coffee);
        groupResearches[groupIndex].researches.Add(tea);
        groupResearches[groupIndex].researches.Add(hotChocolate);
        groupResearches[groupIndex].researches.Add(smoothie);
        groupResearches[groupIndex].researches.Add(milkshake);
        groupResearches[groupIndex].researches.Add(lemonade);
        groupResearches[groupIndex].researches.Add(fruitJuice);
    }

    int GetIndexGroupResearchData(GroupResearchName name) {
        for (int i = 0; i < groupResearches.Count; i++) {
            if (groupResearches[i].researchGroupName == name)
                return i;
        }
        return -1;
    }
    int CreateGroupData(GroupResearchName groupName) {
        GroupResearchData groupFoodResearchData = new GroupResearchData();
        groupFoodResearchData.researches = new List<Research>();
        groupFoodResearchData.researchGroupName = groupName;
        groupResearches.Add(groupFoodResearchData);
        return groupResearches.Count - 1;
    }
    public Sprite GetSpriteByName(string name) {
        foreach (Sprite spr in sprConfig) {
            if (spr.name == name) return spr;
        }
        return null;
    }
    public GameObject GetObjFoodByName(string name) {
        foreach (GameObject obj in objFoods) {
            if (obj.name == name) return obj;
        }
        return null;
    }
}


