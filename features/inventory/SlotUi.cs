using Godot;

namespace IslebyIsle.features.inventory;

public partial class SlotUi : Panel
{
	[Export] public TextureRect IconTexture;
	[Export] public Label CountLabel;
	[Export] public TextureRect BackgroundTexture;
	[Export] public Texture2D ActiveTexture;
	[Export] public Texture2D InactiveTexture;
	public InventorySlot Slot { get; set; }
	private bool _active = false;

	public void UpdateDisplay(InventorySlot slot)
	{
		Slot = slot;
		if (slot.IsEmpty)
		{
			IconTexture.Hide();
			IconTexture.Texture = null;
			CountLabel.Text = string.Empty;
			CountLabel.Hide();
		}
		else
		{
			IconTexture.Texture = slot.Item?.Icon;
			IconTexture.Show();
			CountLabel.Text = $"{slot.Count}";
			if (slot.Count > 1)
			{
				CountLabel.Show();
			}
			else
			{
				CountLabel.Hide();
			}
			
		}

		BackgroundTexture.Texture = !_active ? InactiveTexture : ActiveTexture;
	}
	
	public void SetActive(bool active)
	{
		_active = active;
		BackgroundTexture.Texture = !_active ? InactiveTexture : ActiveTexture;

	}
}
