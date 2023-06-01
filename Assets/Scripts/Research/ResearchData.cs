using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ResearchType {
    Hamburger,
    CheeseBurger,
    Double_Hamburger,
    Double_CheeseBurger,
    McDouble,
    Quarter_Pounder_Cheese,
    Double_Quarter_Pounder_Cheese,
    Bacon_Quarter_Pounder,
    BigMac,
    Filet_O_Fish,
    McChicken,
    Crispy_Chicken_Sandwich,
    Spicy_Crispy_Chicken,
    Deluxe_Crispy_Chicken,
    Deluxe_Spicy_Crispy_Chicken
}


[System.Serializable]
public class Research {
    public ResearchType researchType;
    public GameObject objFood;
    public Sprite foodIcon;
    public string foodName;
    public int foodProfit;
    public float makeTime;
    public int priceUpgrade;
    public int foodBlockTime;
    public double CalculateProfit(int level) {
        double newProfit = foodProfit;
        for (int i = 2; i <= level; i++) {
            newProfit += System.Math.Round(newProfit * 0.1f, 2);
        }
        return System.Math.Round(newProfit, 2);
    }
    public double CalculateIncreaseNextProfit(double profit) {
        return System.Math.Round(profit * 0.2f, 2);
    }
    public int CalulateReseachPrice(int level) {
        if (level == 0) return priceUpgrade / 2;
        return priceUpgrade / 20;
    }
}


[CreateAssetMenu(fileName = "ResearchData", menuName = "ScriptableObjects/ResearchData")]
public class ResearchData : ScriptableObject {
    public List<Research> foodResearchs;
    public Research GetResearch(ResearchType researchName) {
        for (int i = 0; i < foodResearchs.Count; i++) {
            if (foodResearchs[i].researchType.Equals(researchName))
                return foodResearchs[i];
        }
        return null;
    }
    public Research GetResearchByID(int type) {
        for (int i = 0; i < foodResearchs.Count; i++) {
            if ((int)foodResearchs[i].researchType == type)
                return foodResearchs[i];
        }
        return null;
    }

}


