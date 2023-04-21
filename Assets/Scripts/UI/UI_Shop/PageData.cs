using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Page Data", menuName = "ScriptableObjects/NewShopPageData")]
public class PageData : ScriptableObject
{
    public List<OfferData> offerDatas;
    public TabName shopName;
    public OfferData GetOfferData(string _offerID)
    {
        for (int i = 0; i < offerDatas.Count; i++)
        {
            if (offerDatas[i].offerID.ToString() == _offerID)
            {
                return offerDatas[i];
            }
        }
        return null;
    }
}
