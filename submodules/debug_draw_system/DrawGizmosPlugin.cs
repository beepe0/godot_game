using Godot;

public partial class DrawGizmosPlugin : EditorPlugin
{
    public override void _EnterTree()
    {
        AddAutoloadSingleton("DrawGizmos", "res://submodules/debug_draw_system/DrawGizmos.cs");
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton("DrawGizmos");
    }
}