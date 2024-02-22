using UnityEngine;
using System;
using System.Collections.Generic;
public enum ItemType
{
    Ammo,
    Medkit,
    Equipment
}

public abstract class InventoryItem : ScriptableObject
{
    [Header("Item Settings")]
    public string itemName; 
    public Sprite icon; 
    public ItemType itemType; 

    [Header("Stack Settings")]
    [Range(0,100)]
    public int stackSize = 1;
    [Range(0,100)]
    public float weight = 0f;

    public bool CanStack()
    {
        return stackSize > 1;
    }
}


