using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Doughnut, Cake }
    [Header("Info")]
    public ItemType itemType;
    public string itemName;
    public int cost;
    public Sprite itemIcon;
}