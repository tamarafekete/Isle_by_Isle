using Godot;

namespace IslebyIsle.features.interaction;

public interface IInteractable
{
    void  Interact(Player player);
    string? GetPrompt();
    Vector3 GlobalPosition { get; }
}