using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchWrap : MonoBehaviour
{
    public ResearchType researchDepend;
    [SerializeField] Text txtTitleName;
    [SerializeField] Transform slotParent;
    [SerializeField] GameObject objWarning;
    [SerializeField] Text txtWarning;
    private void Update()
    {
        switch (researchDepend)
        {
            case ResearchType.MexicoFood:
                if (ProfileManager.Instance.playerData.unlockedWorld < 3)
                {
                    objWarning.SetActive(true);
                    txtWarning.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(449);
                }
                else objWarning.SetActive(false);
                break;
            case ResearchType.JapanFood:
                if (ProfileManager.Instance.playerData.unlockedWorld < 2)
                {
                    objWarning.SetActive(true);
                    txtWarning.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(448);
                }
                else objWarning.SetActive(false);   
                break;
          
            case ResearchType.DrinkCoffee:
                if (ProfileManager.Instance.playerData.unlockedWorld < 2 && !GameManager.instance.IsUnlockDeliverRoom()) {
                    objWarning.SetActive(true);
                    txtWarning.text = ProfileManager.Instance.dataConfig.GameText.GetTextByID(475);
                } else objWarning.SetActive(false);
                break;
            default:
                objWarning.SetActive(false);
                break;
        }
        txtTitleName.text = ResearchDependTitleToString();
    }
    public ResearchSlot InitSlot(Research research, ResearchSlot researchSlot) {
        ResearchSlot newtechSlot = Instantiate(researchSlot, slotParent);
        newtechSlot.InitDataResearch(research.researchName);
        return newtechSlot;
    }
    string ResearchDependTitleToString() {
        return researchDepend switch {
            ResearchType.DefaultFood => ProfileManager.Instance.dataConfig.GameText.GetTextByID(445),
            ResearchType.MexicoFood => ProfileManager.Instance.dataConfig.GameText.GetTextByID(447),
            ResearchType.JapanFood => ProfileManager.Instance.dataConfig.GameText.GetTextByID(446),
            ResearchType.DrinkBar => ProfileManager.Instance.dataConfig.GameText.GetTextByID(451),
            ResearchType.DrinkCoffee => ProfileManager.Instance.dataConfig.GameText.GetTextByID(452),
            _ => throw new System.NotImplementedException(),
        };
    }
}
