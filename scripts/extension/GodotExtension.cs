using Godot;

public static class GodotExtension
{
    public static T CreateOnStage<T>(this PackedScene scene, Node parent) where T : Node
    {
        T obj = scene.Instantiate<T>();
        parent.AddChild(obj);
        return obj;
    }
    public static T CreateOnStage<T>(this PackedScene scene, Node parent, Vector3 position) where T : Node3D
    {
        T obj = scene.Instantiate<T>();
        obj.Position = position;
        parent.AddChild(obj);
        return obj;
    }
    public static T CreateOnStageCallDeferred<T>(this PackedScene scene, Node parent) where T : Node
    {
        T obj = scene.Instantiate<T>();
        parent.CallDeferred(Node.MethodName.AddChild, obj);
        return obj;
    }
    public static T CreateOnStageCallDeferred<T>(this PackedScene scene, Node parent, Vector3 position) where T : Node3D
    {
        T obj = scene.Instantiate<T>();
        obj.Position = position;
        parent.CallDeferred(Node.MethodName.AddChild, obj);
        return obj;
    }
}