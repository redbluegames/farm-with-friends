using UnityEngine;
using System.Collections;

public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public int maxCount;
    public int price;
    public int sellPrice;
    public bool isEquippable;
    public GameObject plantPrefab;
}
