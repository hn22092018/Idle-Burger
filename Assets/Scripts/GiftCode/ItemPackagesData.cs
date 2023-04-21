using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemFromGift
{
    public List<ItemPackage> itemPackages;
}

[CreateAssetMenu(fileName = "GiftDatas", menuName = "ScriptableObjects/NewGiftData")]
public class ItemPackagesData : ScriptableObject
{
    public List<ItemFromGift> gifts;

    private void OnEnable()
    {
        gifts.Clear();//
        ItemPackage item1 = new ItemPackage();
        item1.type = ItemType.Gem;
        item1.amount = 200;
        ItemPackage item2 = new ItemPackage();
        item2.type = ItemType.AdvancedChest;
        item2.amount = 1;
        ItemPackage item3 = new ItemPackage();
        item3.type = ItemType.Gem;
        item3.amount = 300;
        ItemPackage item4 = new ItemPackage();
        item4.type = ItemType.AdvancedChest;
        item4.amount = 1;
        ItemPackage item5 = new ItemPackage();
        item5.type = ItemType.NormalChest;
        item5.amount = 1;
        ItemPackage item6 = new ItemPackage();
        item6.type = ItemType.ADTicket;
        item6.amount = 10;
        ItemPackage item7 = new ItemPackage();
        item7.type = ItemType.TimeSkip_1H;
        item7.amount = 5;



        ItemFromGift gift1 = new ItemFromGift();
        gift1.itemPackages = new List<ItemPackage>();
        gift1.itemPackages.Add(item1);
        gift1.itemPackages.Add(item2);

        ItemFromGift gift2 = new ItemFromGift();
        gift2.itemPackages = new List<ItemPackage>();
        gift2.itemPackages.Add(item3);
        gift2.itemPackages.Add(item7);

        ItemFromGift gift3 = new ItemFromGift();
        gift3.itemPackages = new List<ItemPackage>();
        gift3.itemPackages.Add(item4);
        gift3.itemPackages.Add(item7);

        ItemFromGift gift4 = new ItemFromGift();
        gift4.itemPackages = new List<ItemPackage>();
        gift4.itemPackages.Add(item1);
        gift4.itemPackages.Add(item5);

        ItemFromGift gift5 = new ItemFromGift();
        gift5.itemPackages = new List<ItemPackage>();
        gift5.itemPackages.Add(item1);
        gift5.itemPackages.Add(item6);

        //gifts.Add(gift1);
        //gifts.Add(gift2);
        //gifts.Add(gift3);
        //gifts.Add(gift4);
        //gifts.Add(gift5);
    }

}

