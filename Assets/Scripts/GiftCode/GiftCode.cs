using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class GiftCode {
    private const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public string code;
    public List<ItemPackage> rewards = new List<ItemPackage>();
    public GiftCode() {
        code = GenerateRandomString();
    }
    public string GenerateRandomString() {
        string s = "";
        int charAmount = 10; //set those to the minimum and maximum length of your string
        for (int i = 0; i < charAmount; i++) {
            s += glyphs[Random.Range(0, glyphs.Length)];
        }
        return s;
    }
}
[Serializable]
public class ItemPackage {
    public ItemType type;
    public int amount;
}
