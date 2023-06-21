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
    Deluxe_Spicy_Crispy_Chicken,
    None
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
    public int CalculateProfit(int level) {
        return (int)(foodProfit * Mathf.Pow(1.2f, level));
    }
    public int CalculateIncreaseNextProfit(int level) {
        return (int)(foodProfit * Mathf.Pow(1.2f, level + 1)) - (int)(foodProfit * Mathf.Pow(1.2f, level));
    }
    public int GetReseachPrice(int level) {
        return (int)(priceUpgrade * Mathf.Pow(1.3f - level * 0.012f, level));
    }
    public int GetResearchTime(int level) {
        if (level == 0) return 5 * 60;
        return 5 * 60 * (level + 1);
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


