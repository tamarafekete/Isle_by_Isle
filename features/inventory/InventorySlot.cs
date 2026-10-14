
namespace IslebyIsle.features.inventory;

public partial class InventorySlot
{
    public ItemData? Item { get; set; }
    public int Count;

    public bool IsEmpty => Item == null;
    
    public int AddItem(ItemData item, int count)
    {
        if (IsEmpty)
        {
            Item = item;
            Count = 0;
        }
        else if (item != Item)
        {
            return count;
        }
        
        Count += count;
        if (!(Count > Item?.MaxStackSize)) return 0;
        int remaining = Count -  Item.MaxStackSize;
        Count = Item.MaxStackSize;
        return remaining;

    }
}