using Godot;
using System;

namespace IslebyIsle.features.inventory;

public partial class SlotUi : Panel
{
	[Export] public TextureRect IconTexture;
	[Export] public Label CountLabel;

	public void UpdateDisplay(InventorySlot slot)
	{
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
	}
}
