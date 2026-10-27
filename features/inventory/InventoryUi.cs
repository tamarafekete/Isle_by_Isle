using Godot;
using IslebyIsle.features.interaction;


namespace IslebyIsle.features.inventory;

public partial class InventoryUi : Control
{
	public Inventory TargetInventory;
	[Export] public GridContainer GridContainer;
	[Export] public GridContainer HotBarContainer;
	[Export] public PanelContainer InventoryPanel;
	[Export] public PackedScene SlotScene;
	[Export] public ItemData HardcodedItem;
	private int _active = 0;

	public void Initialize(Inventory inventory)
	{
		TargetInventory = inventory;
		if (HardcodedItem != null)
		{
			TargetInventory.AddItem(HardcodedItem, 1);
		}
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

	public void SetActive(int i)
	{
		if (i < 0 || i >= HotBarContainer.GetChildCount()) return;
		HotBarContainer.GetChild<SlotUi>(_active).SetActive(false);
		_active = i;
		HotBarContainer.GetChild<SlotUi>(_active).SetActive(true);
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

		HotBarContainer.GetChild<SlotUi>(_active).SetActive(true);
	}

	public ToolData GetActiveTool()
	{
		var activeItem = HotBarContainer.GetChild<SlotUi>(_active);
		if (activeItem.Slot.Item is ToolData activeTool)
		{
			return activeTool;
		}
		return new ToolData();
	}
}
