using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Order {
    public int id;
    public Sprite sprOrderStaff;
    public int bugerCollected;
    public int bugerRequire;
    public BigNumber cashProfit;
    public int bCoinProfit;
    public bool isReject;
    public bool isFreeAccept;
    public float GetProcess() {
        return (float)bugerCollected / (float)bugerRequire;
    }
    public bool IsDone() {
        return bugerCollected >= bugerRequire;
    }
    public int GetAmountNeedToCompleteOrder() {
        return bugerRequire - bugerCollected;
    }
    public void OnCollect(int amount = 1) {
        bugerCollected += amount;
    }
    public void OnComplete() {
        bugerCollected = bugerRequire;
    }
}
[System.Serializable]
public class OrderBookManager {
    public List<Order> activeOrders;
    public List<Order> currentOffers;
    public float dtTimeToNewOffer;
    int timeToNewOffer = 120;
    public void Update() {
        if (dtTimeToNewOffer > 0) dtTimeToNewOffer -= Time.deltaTime;
    }

    public List<Order> CheckCreatOffers(List<Sprite> sprChars) {
        if (currentOffers.Count > 0) return null;
        List<Sprite> sprCharacters = new List<Sprite>(sprChars);
        Sprite spr1 = sprCharacters[UnityEngine.Random.Range(0, sprCharacters.Count)];
        sprCharacters.Remove(spr1);
        Sprite spr2 = sprCharacters[UnityEngine.Random.Range(0, sprCharacters.Count)];
        sprCharacters.Remove(spr2);
        Sprite spr3 = sprCharacters[UnityEngine.Random.Range(0, sprCharacters.Count)];
        currentOffers = new List<Order>();
        Order offer1 = new Order {
            id = GetNextID(),
            bugerRequire = 30,
            cashProfit = GameManager.instance.GetCashProfit(3),
            bCoinProfit = 5,
            sprOrderStaff = spr1,
            isFreeAccept = true
        };
        Order offer2 = new Order {
            id = offer1.id + 1,
            bugerRequire = 40,
            cashProfit = GameManager.instance.GetCashProfit(4),
            bCoinProfit = 7,
            sprOrderStaff = spr2,
            isFreeAccept = true
        };
        Order offer3 = new Order {
            id = offer2.id + 1,
            bugerRequire = 40,
            cashProfit = GameManager.instance.GetCashProfit(10),
            bCoinProfit = 16,
            sprOrderStaff = spr3,
            isFreeAccept = false
        };
        currentOffers.Add(offer1);
        currentOffers.Add(offer2);
        currentOffers.Add(offer3);
        ProfileManager.PlayerData.OrderSave.IsMarkChangeData();
        return currentOffers;
    }
    /// <summary>
    /// Reject Offer.
    /// if all offers is rejected, clear all & wait to new offer
    /// </summary>
    /// <param name="order"></param>
    public void OnRejectOffer(Order order) {
        order.isReject = true;
        int countreject = currentOffers.Count(x => x.isReject);
        if (countreject >= 3) {
            currentOffers.Clear();
            OnStartTimeToWaitNewOffer();
        }
        ProfileManager.PlayerData.OrderSave.SaveData();

    }
    /// <summary>
    /// Accept Offer , add it to active order. Clear all offer, reset wait time
    /// </summary>
    /// <param name="order"></param>
    public void OnAcceptOffer(Order order, int valueRate = 1) {
        order.bCoinProfit *= valueRate;
        order.cashProfit *= valueRate;
        activeOrders.Add(order);
        OnStartTimeToWaitNewOffer();
        currentOffers.Clear();
        ProfileManager.PlayerData.OrderSave.SaveData();
    }
    public int GetNextID() {
        int last = 0;
        foreach (Order saver in activeOrders) {
            if (saver.id > last) last = saver.id;
        }
        return last + 1;
    }


    /// <summary>
    /// OnCollect
    /// </summary>
    /// <param name="ID"></param>
    public void OnCollect(int amount = 1) {
        int sum = amount;
        for (int i = 0; i < activeOrders.Count; i++) {
            // if amount collect >= require, continue subtract require until amount collect =0
            if (!activeOrders[i].IsDone() && sum <= activeOrders[i].GetAmountNeedToCompleteOrder()) {
                activeOrders[i].OnCollect(sum);
                ProfileManager.PlayerData.OrderSave.IsMarkChangeData();
                break;
            } else if (!activeOrders[i].IsDone()) {
                sum -= activeOrders[i].GetAmountNeedToCompleteOrder();
                activeOrders[i].OnComplete();
                ProfileManager.PlayerData.OrderSave.IsMarkChangeData();
            }
        }
    }
    /// <summary>
    /// OnForceDoneOrder
    /// </summary>
    /// <param name="ID"></param>
    public void OnForceDoneOrder(Order order) {
        order.OnCollect(order.GetAmountNeedToCompleteOrder());
        ProfileManager.PlayerData.OrderSave.IsMarkChangeData();

    }
    /// <summary>
    /// Claim
    /// </summary>
    /// <param name="ID"></param>
    public void OnClaimOrder(Order order) {
        for (int i = activeOrders.Count - 1; i >= 0; i--) {
            if (activeOrders[i].id == order.id) {
                OnClaimRewardOrder(order);
                activeOrders.RemoveAt(i);
                OnStartTimeToWaitNewOffer();
                ProfileManager.PlayerData.OrderSave.IsMarkChangeData();
                break;
            }
        }
    }
    void OnClaimRewardOrder(Order order) {
        ProfileManager.PlayerData.AddCash(order.cashProfit);
        ProfileManager.PlayerData.AddBCoin(order.bCoinProfit);
    }
    public void OnCancleOrder(Order order) {
        for (int i = activeOrders.Count - 1; i >= 0; i--) {
            if (activeOrders[i].id == order.id) {
                activeOrders.RemoveAt(i);
                OnStartTimeToWaitNewOffer();
                ProfileManager.PlayerData.OrderSave.IsMarkChangeData();
                break;
            }
        }
    }
    public int GetTimeToNewOffer() {
        return (int)dtTimeToNewOffer;
    }
    public void SkipWaitToNewOffer() {
        dtTimeToNewOffer = 0;
    }
    void OnStartTimeToWaitNewOffer() {
        dtTimeToNewOffer = timeToNewOffer;
    }
    public bool IsMaxOrder() {
        return activeOrders.Count >= ProfileManager.PlayerData.OrderSave.maxOrder;
    }
    public int GetTotalBurgetNeedInOrder() {
        int total = 0;
        for (int i = 0; i < activeOrders.Count; i++) {
            total += activeOrders[i].GetAmountNeedToCompleteOrder();
        }
        return total;
    }
    public bool IsNotify() {
        return activeOrders.Count(x => x.IsDone()) > 0;
    }
}
