using System.Collections.Generic;
using Godot;

namespace IslebyIsle.features.inventory;

public class Inventory
{
    public List<InventorySlot> Slots { get; set; }
    
    public Inventory(int size)
    {
        Slots = new List<InventorySlot>(size);
        for (int i = 0; i < size; i++)
        {
            Slots.Add(new InventorySlot());
        }
    }

    public int AddItem(ItemData item, int count)
    {
        int remaining = count;
        InventorySlot slot;
        while (remaining > 0 && (slot = FindSameItem(item)) != null)
        {
            remaining = slot.AddItem(item, remaining);
        }

        while (remaining > 0 && (slot = FindFirstEmptySlot()) != null)
        {
            remaining = slot.AddItem(item, remaining);
        }
        return remaining;
    }

    private InventorySlot FindSameItem(ItemData item)
    {
        foreach (InventorySlot slot in Slots)
        {
            if (slot.Item == item && slot.Count < slot.Item?.MaxStackSize)
            {
                return slot;
            }
        }
        return null;
    }

    private InventorySlot FindFirstEmptySlot()
    {
        foreach (InventorySlot slot in Slots)
        {
            if (slot.Item == null)
            {
                return slot;
            }
        }
        return null;
    }
    
    public void PrintInventory()
    {
        foreach (InventorySlot slot in Slots)
        {
            GD.Print("Item: " + slot.Item + " Count: " + slot.Count);
        }
    }

}