using BP.ComponentSystem;
using Godot;

public partial class PlayerStats : ComponentObject
{
    public long PlayerId { get; set; } = -1;
    public string PlayerName { get; set; } = string.Empty;

}