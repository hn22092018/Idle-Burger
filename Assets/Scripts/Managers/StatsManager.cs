using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour {
    public static StatsManager instance;
    [SerializeField] float satisfactionPoint = 50;
    [SerializeField] float famousPoint = 50;
    [SerializeField] float cleanPoint = 50;
    [SerializeField] float deliciousPoint = 50;
    float deltaTimefamousPoint;
    private void Awake() {
        instance = this;
        satisfactionPoint = 50f;
        famousPoint = 50f;
        cleanPoint = 50f;
        deliciousPoint = 50f;
    }
    private void Update() {
        deltaTimefamousPoint += Time.deltaTime;
        if (deltaTimefamousPoint >= 10f) {
            deltaTimefamousPoint = 0;
            famousPoint -= 1;
        }
    }
    public void IncreaseSatisfactionPoint(Rarity customerRarity, float timewait) {
        float customerRate = 0f;
        switch (customerRarity) {
            case Rarity.Common:
                customerRate = 0f;
                break;
            case Rarity.Rare:
                customerRate = 2f;
                break;
            case Rarity.Epic:
                customerRate = 3f;
                break;
            case Rarity.Legendary:
                customerRate = 5f;
                break;
        }
        if (timewait <= 5f) satisfactionPoint += 2f + customerRate * 0.2f;
        else if (timewait <= 10f) satisfactionPoint += 1f + customerRate * 0.2f;
        else if (timewait <= 15f) satisfactionPoint += 0f;
        else if (timewait <= 20f) satisfactionPoint -= 1f - customerRate * 0.2f;
        else satisfactionPoint -= 2f - customerRate * 0.2f;
        satisfactionPoint = Mathf.Clamp(satisfactionPoint, 0, 100);
    }

    public void IncreaseFamousPoint(Rarity customerRarity) {
        switch (customerRarity) {
            case Rarity.Common:
                famousPoint += 1;
                break;
            case Rarity.Rare:
                famousPoint += 2;
                break;
            case Rarity.Epic:
                famousPoint += 3;
                break;
            case Rarity.Legendary:
                famousPoint += 4;
                break;
        }
        famousPoint = Mathf.Clamp(famousPoint, 0, 100);
    }
    public void DecreaseFamousPoint(Rarity customerRarity) {
        switch (customerRarity) {
            case Rarity.Common:
                famousPoint -= 0.2f;
                break;
            case Rarity.Rare:
                famousPoint -= 0.5f;
                break;
            case Rarity.Epic:
                famousPoint -= 1f;
                break;
            case Rarity.Legendary:
                famousPoint -= 1.5f;
                break;
        }
        famousPoint = Mathf.Clamp(famousPoint, 0, 100);
    }
    public void IncreaseCleanPoint() {
        cleanPoint++;
        cleanPoint = Mathf.Clamp(cleanPoint, 0, 100);
    }
    public void DecreaseCleanPoint() {
        cleanPoint -= 0.5f;
        cleanPoint = Mathf.Clamp(cleanPoint, 0, 100);
    }
    public float GetStatsRate() {
        float total = satisfactionPoint + famousPoint + cleanPoint + deliciousPoint;
        float rate = total / 4;
        if (rate <= 25f) return 0.75f;
        else if (rate < 50f) return 0.85f;
        else if (rate < 70f) return 1f;
        else if (rate < 90f) return 1.2f;
        return 1.5f;
    }
}
