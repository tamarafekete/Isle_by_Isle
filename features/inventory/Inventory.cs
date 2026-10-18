using System.Collections.Generic;
using Godot;

namespace IslebyIsle.features.inventory;

public partial class Inventory : GodotObject
{
    public List<InventorySlot> Slots { get; set; }
    [Signal] public delegate void InventoryChangedEventHandler();
    
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

        EmitSignal(SignalName.InventoryChanged);
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
            GD.Print("Item: " + slot.Item?.Name + " Count: " + slot.Count);
        }
    }

}