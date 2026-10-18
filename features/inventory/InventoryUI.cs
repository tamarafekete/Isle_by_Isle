using Godot;


namespace IslebyIsle.features.inventory;

public partial class InventoryUI : Control
{
    public Inventory TargetInventory;
    [Export] public GridContainer GridContainer;
    [Export] public GridContainer HotBarContainer;
    [Export] public PackedScene SlotScene;

    public void Initialize(Inventory inventory)
    {
        TargetInventory = inventory;
        TargetInventory.InventoryChanged += RefreshAll;
        RefreshAll();
    }

    public override void _Ready()
    {
        for (int i = 0; i < InventoryConstants.HotBarSize;i++)
        {
            var slotInstance = SlotScene.Instantiate<SlotUi>();
            HotBarContainer.AddChild(slotInstance);
        }

        for (int i = 0; i < InventoryConstants.BackPackSize; i++)
        {
            var slotInstance = SlotScene.Instantiate<SlotUi>();
            GridContainer.AddChild(slotInstance);
        }
    }

    private void RefreshAll()
    {
        for (int i = 0; i < InventoryConstants.HotBarSize; i++)
        {
           HotBarContainer.GetChild<SlotUi>(i).UpdateDisplay(TargetInventory.Slots[i]);
        }

        for (int i = 0; i < InventoryConstants.BackPackSize; i++)
        {
            GridContainer.GetChild<SlotUi>(i).UpdateDisplay(TargetInventory.Slots[i+InventoryConstants.HotBarSize]);
        }
    }
}