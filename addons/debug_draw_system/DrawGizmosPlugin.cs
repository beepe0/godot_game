using System.IO;
using BP.DebugGizmos;
using BP.GameConsole;
using Godot;

public partial class DrawGizmosPlugin : EditorPlugin
{
    public static readonly System.Collections.Generic.Dictionary<string, ProjectProperty> GeneralProperties = new()
    {
        {"target_pool_size", new ProjectProperty("debug_gizmos_system/target_pool_size", 1024, Variant.Type.Int, PropertyHint.None)},
        {"start_pool_size", new ProjectProperty("debug_gizmos_system/start_pool_size", 256, Variant.Type.Int, PropertyHint.None)},
    };
    public static readonly System.Collections.Generic.Dictionary<string, ProjectProperty> MeshesProperties = new()
    {
        {"box_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/box_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Box.res", Variant.Type.String, PropertyHint.File)},
        {"arrow_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/arrow_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Arrow.res", Variant.Type.String, PropertyHint.File)},
        {"circle_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/circle_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Circle.res", Variant.Type.String, PropertyHint.File)},
        {"plane_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/plane_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Plane.res", Variant.Type.String, PropertyHint.File)},
        {"sphere_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/sphere_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Sphere.res", Variant.Type.String, PropertyHint.File)},
        {"capsule_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/capsule_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Capsule.res", Variant.Type.String, PropertyHint.File)},
        {"cylinder_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/cylinder_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Cylinder.res", Variant.Type.String, PropertyHint.File)},
        {"point_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/point_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Point.res", Variant.Type.String, PropertyHint.File)},
        {"solid_box_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/solid_box_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_SolidBox.res", Variant.Type.String, PropertyHint.File)},
        {"arrows_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/arrows_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_Arrows.res", Variant.Type.String, PropertyHint.File)},
        {"solid_capsule_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/solid_capsule_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_SolidCapsule.res", Variant.Type.String, PropertyHint.File)},
        {"solid_cylinder_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/solid_cylinder_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_SolidCylinder.res", Variant.Type.String, PropertyHint.File)},
        {"solid_sphere_mesh_res", new ProjectProperty("debug_gizmos_system/meshes/solid_sphere_mesh_res", "res://submodules/debug_draw_system/gizmos/meshes/gizmos_SolidSphere.res", Variant.Type.String, PropertyHint.File)},
    };
    
    public override void _EnterTree()
    {
        AddAutoloadSingleton("DrawGizmos", "res://submodules/debug_draw_system/DrawGizmos.cs");

        foreach (var property in GeneralProperties)
        {
            AddProjectSetting(property.Value.Path, property.Value.InitialValue, property.Value.VariantType, property.Value.PropertyHint);
        }
        foreach (var property in MeshesProperties)
        {
            AddProjectSetting(property.Value.Path, property.Value.InitialValue, property.Value.VariantType, property.Value.PropertyHint);
        }

        ProjectSettings.Save();
    }
    public override void _ExitTree()
    {
        foreach (var property in GeneralProperties)
        {
            ProjectSettings.Clear(property.Value.Path);
        }
        foreach (var property in MeshesProperties)
        {
            ProjectSettings.Clear(property.Value.Path);
        }
        
        RemoveAutoloadSingleton("DrawGizmos");
    }
    private bool AddProjectSetting(string name, Variant initialValue, Variant.Type type = Variant.Type.Int, PropertyHint hint = PropertyHint.None)
    {
        if (ProjectSettings.HasSetting(name)) return false;

        var dict = new Godot.Collections.Dictionary()
        {
            {"name", name},
            {"type", (int)type},
            {"hint", (int)hint},
        };

        ProjectSettings.SetSetting(name, initialValue);
        ProjectSettings.AddPropertyInfo(dict);
        return true;
    }
    
    public class ProjectProperty
    {
        public readonly string Path;
        public readonly Variant.Type VariantType;
        public readonly PropertyHint PropertyHint;
        
        public Variant InitialValue;

        public ProjectProperty(string path, Variant initialValue, Variant.Type variantType, PropertyHint propertyHint)
        {
            Path = path;
            InitialValue = initialValue;
            VariantType = variantType;
            PropertyHint = propertyHint;
        }
    }
}