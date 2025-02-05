using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public enum ItemType { Doughnut, Cake, Money }
    [Header("Info")]
    public ItemType itemType;
    public string itemName;
    public int cost;
    public Sprite itemIcon;
}