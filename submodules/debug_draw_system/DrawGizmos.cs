namespace BP.DebugGizmos
{
   using System;
   using Godot;
   using System.Collections.Generic;
   
   public sealed partial class DrawGizmos : Node
   {
      private static DebugGizmosLayers _allLayers = DebugGizmosLayers.Everythings;
      
      private static ushort _targetPoolSize;
      private static ushort _startPoolSize;
      private static bool _doDepthTest;
      
      private static ulong _time;
      
      public static DebugCanvasDrawer _canvasDrawer;
      public static DebugMeshDrawer _meshDrawer;

      public static DebugGizmosLayers AllLayers => _allLayers;
      public static ushort TargetPoolSize => _targetPoolSize;
      public static ushort StartPoolSize => _startPoolSize;
      public static ulong Time => _time;

      public static void SetDepthTest(bool doDepthTest)
      {
         if(_doDepthTest == doDepthTest) return;

         _doDepthTest = doDepthTest;
         _meshDrawer.SetDepthTest(doDepthTest);
      }
      
      public override void _Ready()
      {
         Name = "DebugGizmos";

         _targetPoolSize = ProjectSettings.GetSetting(DrawGizmosPlugin.GeneralProperties["target_pool_size"].Path).AsUInt16();
         _startPoolSize = ProjectSettings.GetSetting(DrawGizmosPlugin.GeneralProperties["start_pool_size"].Path).AsUInt16();

         _canvasDrawer = new (this);
         _meshDrawer = new (this);
      }
      public override void _PhysicsProcess(double delta)
      {
         _time = Godot.Time.GetTicksMsec();
         _canvasDrawer.Update();
         _meshDrawer.Update();
      }
      public override void _ExitTree()
      {
         _canvasDrawer?.Dispose();
         _meshDrawer?.Dispose();
      }
      
      #region Drawing Functions

      public static void Box(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Box(position, rotation, size, duration, color, layer);
      }
      public static void Arrow(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Arrow(position, rotation, size, duration, color, layer);
      }
      public static void Circle(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Circle(position, rotation, size, duration, color, layer);
      }
      public static void Plane(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Plane(position, rotation, size, duration, color, layer);
      }
      public static void Sphere(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Sphere(position, rotation, size, duration, color, layer);
      }
      public static void Capsule(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Capsule(position, rotation, size, duration, color, layer);
      }
      public static void Cylinder(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Cylinder(position, rotation, size, duration, color, layer);
      }
      public static void Point(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Point(position, rotation, size, duration, color, layer);
      }
      public static void SolidBox(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.SolidBox(position, rotation, size, duration, color, layer);
      }
      public static void Arrows(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.Arrows(position, rotation, size, duration, layer);
      }
      public static void SolidCapsule(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.SolidCapsule(position, rotation, size, duration, color, layer);
      }
      public static void SolidCylinder(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.SolidCylinder(position, rotation, size, duration, color, layer);
      }
      public static void SolidSphere(Vector3 position, Quaternion rotation, Vector3 size, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _meshDrawer?.SolidSphere(position, rotation, size, duration, color, layer);
      }
      public static void Text(object text, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _canvasDrawer?.Text(text.ToString(), duration, color, layer);
      }
      public static void Text2D(object text, Vector2 position, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _canvasDrawer?.Text2D(text.ToString(), duration, position, color, layer);
      }
      public static void Text3D(object text, Vector3 position, float duration = 0f, Color? color = null, DebugGizmosLayers layer = DebugGizmosLayers.Everythings)
      {
         _canvasDrawer?.Text3D(text.ToString(), duration, position, color, layer);
      }
      
      #endregion
   }
   
   [Flags]
   public enum DebugGizmosLayers : byte
   {
      None = 0,
      Layer1 = 1 << 1,
      Layer2 = 1 << 2,
      Layer3 = 1 << 3,
      Layer4 = 1 << 4,
      Layer5 = 1 << 5,
      Layer6 = 1 << 6,
      Layer7 = 1 << 7,
      Everythings = Layer7 | Layer6 | Layer5 | Layer4 | Layer3 | Layer2 | Layer1
   }

   public sealed class DebugCanvasDrawer
   {
      public readonly GizmosInstancesPool<GizmosTextInstance> GizmosTextPool;
      public readonly List<GizmosTextInstance> GizmosTextList;
      
      public readonly GizmosInstancesPool<GizmosText2DInstance> GizmosText2dPool;
      public readonly List<GizmosText2DInstance> GizmosText2dList;
      
      public readonly GizmosInstancesPool<GizmosText3DInstance> GizmosText3dPool;
      public readonly List<GizmosText3DInstance> GizmosText3dList;

      private Camera3D _camera;
      private readonly CanvasLayer _canvasLayer;
      private readonly Node2D _canvas2d;
      private readonly Node2D _canvas3d;
      
      private readonly Font _textFont;
      private readonly int _fontSize = 12;
      
      public DebugCanvasDrawer(Node parent)
      {
         GizmosTextPool = new();
         GizmosTextList = new();
         GizmosText2dPool = new();
         GizmosText2dList = new();
         GizmosText3dPool = new();
         GizmosText3dList = new();
         
         _canvasLayer = new CanvasLayer();
         _canvasLayer.Name = "CanvasLayer";
         _canvasLayer.Layer = 100;
         parent.AddChild(_canvasLayer);
         _canvasLayer.Owner = parent;
         
         _canvas2d = new();
         _canvas2d.Name = "Canvas2D";
         _canvas2d.ZIndex = 101;
         _canvas2d.Draw += DrawText2D;
         _canvasLayer.AddChild(_canvas2d);
         
         _canvas3d = new();
         _canvas3d.Name = "Canvas3D";
         _canvas3d.ZIndex = 102;
         _canvas3d.Draw += DrawText3D;
         _canvasLayer.AddChild(_canvas3d);

         _camera = _canvas3d.GetViewport().GetCamera3D();
         
         Label l = new Label();
         _textFont = l.GetThemeDefaultFont(); 
         l.Free();
      }
      public void Text(string text, float duration, Color? color, DebugGizmosLayers layer)
      {
         GizmosTextInstance instance = GizmosTextPool.Retrieve();
         if (instance != null)
         {
            instance.Color = color ?? Colors.White;
            instance.DebugLayer = layer;
            instance.SetDuration(duration);
            instance.Text = text;
            GizmosTextList.Add(instance);
            _canvas2d.QueueRedraw();
         }
      }
      public void Text2D(string text, float duration, Vector2 position, Color? color, DebugGizmosLayers layer)
      {
         GizmosText2DInstance instance = GizmosText2dPool.Retrieve();
         if (instance != null)
         {
            instance.Color = color ?? Colors.White;
            instance.DebugLayer = layer;
            instance.SetDuration(duration);
            instance.Text = text;
            instance.Position = position;
            GizmosText2dList.Add(instance);
            _canvas2d.QueueRedraw();
         }
      }
      public void Text3D(string text, float duration, Vector3 position, Color? color, DebugGizmosLayers layer)
      {
         if (_camera == null)
         {
            _camera = _canvas3d.GetViewport().GetCamera3D();
            return;
         }
         
         GizmosText3DInstance instance = GizmosText3dPool.Retrieve();
         if (instance != null)
         {
            instance.Color = color ?? Colors.White;
            instance.DebugLayer = layer;
            instance.SetDuration(duration);
            instance.Text = text;
            instance.Position = position;
            GizmosText3dList.Add(instance);
            _canvas3d.QueueRedraw();
         }
      }
      public void Update()
      {
          for (int i = 0; i < GizmosTextList.Count ; i++)
          {
             GizmosTextInstance instance = GizmosTextList[i];
             if (instance.CantDraw())
             {
                GizmosTextPool.Return(instance);
                GizmosTextList.Remove(instance);
                _canvas2d.QueueRedraw();
             }
          }
         
          for (int i = 0; i < GizmosText2dList.Count ; i++)
          {
             GizmosText2DInstance instance = GizmosText2dList[i];
             if (instance.CantDraw())
             {
                GizmosText2dPool.Return(instance);
                GizmosText2dList.Remove(instance);
                _canvas2d.QueueRedraw();
             }
          }
          
          for (int i = 0; i < GizmosText3dList.Count ; i++)
          {
             GizmosText3DInstance instance = GizmosText3dList[i];
             if (instance.CantDraw())
             {
                GizmosText3dPool.Return(instance);
                GizmosText3dList.Remove(instance);
                _canvas3d.QueueRedraw();
             }
          }
      }
      private void DrawText2D()
      {
         Vector2 position = new Vector2(10, _fontSize * 1.5f);

         foreach (GizmosTextInstance instance in GizmosTextList)
         {
            if (instance.CantDraw())
            {
               continue;
            }
            _canvas2d.DrawString(_textFont, position, instance.Text, HorizontalAlignment.Left, -1, _fontSize, instance.Color);
            position.Y += _fontSize * 1.5f;
            instance.BeenDrawn = true;
         }
         
         foreach (GizmosText2DInstance instance in GizmosText2dList)
         {
            if (instance.CantDraw())
            {
               continue;
            }
            
            _canvas2d.DrawString(_textFont, instance.Position, instance.Text, HorizontalAlignment.Left, -1, _fontSize, instance.Color);
            
            instance.BeenDrawn = true;
         }
      }
      private void DrawText3D()
      {
         Camera3D camera = _canvas3d.GetViewport().GetCamera3D();

         foreach (GizmosText3DInstance instance in GizmosText3dList)
         {
            if (instance.CantDraw())
            {
               continue;
            }
            Vector2 offset = _textFont.GetStringSize(instance.Text, HorizontalAlignment.Left, -1f, _fontSize) * 0.5f;
            Vector2 pos = camera.UnprojectPosition(instance.Position) - offset;
            _canvas3d.DrawString(_textFont, pos, instance.Text, HorizontalAlignment.Left, -1, _fontSize, instance.Color);
            
            instance.BeenDrawn = true;
         }
      }
      public void Dispose()
      {
         GizmosTextPool?.Clear();
         GizmosTextList?.Clear();
         GizmosText2dPool?.Clear();
         GizmosText2dList?.Clear();
         GizmosText3dPool?.Clear();
         GizmosText3dList?.Clear();
         _canvas2d?.Free();
         _canvas3d?.Free();
         _textFont?.Free();
      }
   }

   public sealed class DebugMeshDrawer
   {
      public readonly GizmosInstancesPool<GizmosMeshInstance> GizmosMeshPool;
      private readonly GizmosShapeCollection[] _gizmosShapeCollections;

      private readonly GizmosShapeCollection _gizmosBoxCollection;
      private readonly GizmosShapeCollection _gizmosArrowCollection;
      private readonly GizmosShapeCollection _gizmosCircleCollection;
      private readonly GizmosShapeCollection _gizmosPlaneCollection;
      private readonly GizmosShapeCollection _gizmosSphereCollection;
      private readonly GizmosShapeCollection _gizmosCapsuleCollection;
      private readonly GizmosShapeCollection _gizmosCylinderCollection;
      private readonly GizmosShapeCollection _gizmosPointCollection;
      
      private readonly GizmosShapeCollection _gizmosSolidBoxCollection;
      private readonly GizmosShapeCollection _gizmosArrowsCollection;
      private readonly GizmosShapeCollection _gizmosSolidCapsuleCollection;
      private readonly GizmosShapeCollection _gizmosSolidCylinderCollection;
      private readonly GizmosShapeCollection _gizmosSolidSphereCollection;
      
      public DebugMeshDrawer(Node parent)
      {
         GizmosMeshPool = new();
         
         GizmosShapeCollection.OnReturn = (instance) => GizmosMeshPool.Return(instance);

         _gizmosBoxCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["box_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false));
         _gizmosArrowCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["arrow_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false));
         _gizmosCircleCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["circle_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false, true));
         _gizmosPlaneCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["plane_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false, true));
         _gizmosSphereCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["sphere_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false));
         _gizmosCapsuleCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["capsule_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false));
         _gizmosCylinderCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["cylinder_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false));
         _gizmosPointCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Lines, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["point_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false));
         _gizmosSolidBoxCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Triangles, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["solid_box_mesh_res"].Path).ToString(), CreateStandardMaterial3D());
         _gizmosArrowsCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Triangles, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["arrows_mesh_res"].Path).ToString(), CreateStandardMaterial3D(false));
         _gizmosSolidCapsuleCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Triangles, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["solid_capsule_mesh_res"].Path).ToString(), CreateStandardMaterial3D());
         _gizmosSolidCylinderCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Triangles, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["solid_cylinder_mesh_res"].Path).ToString(), CreateStandardMaterial3D());
         _gizmosSolidSphereCollection = CreateGizmosShapeCollection(parent, Mesh.PrimitiveType.Triangles, ProjectSettings.GetSetting(DrawGizmosPlugin.MeshesProperties["solid_sphere_mesh_res"].Path).ToString(), CreateStandardMaterial3D());

         _gizmosShapeCollections = new[] 
         { 
            _gizmosBoxCollection, _gizmosArrowCollection, _gizmosCircleCollection, _gizmosPlaneCollection, _gizmosSphereCollection, _gizmosCapsuleCollection, _gizmosCylinderCollection, _gizmosPointCollection,
            _gizmosSolidBoxCollection, _gizmosArrowsCollection, _gizmosSolidCapsuleCollection, _gizmosSolidCylinderCollection, _gizmosSolidSphereCollection
         };
      }
      
      public void SetDepthTest(bool depthTest)
      {
         foreach (var shape in _gizmosShapeCollections)
         {
            ((StandardMaterial3D)shape.MultiMeshInstance.MaterialOverride).NoDepthTest = !depthTest;
         }
      }
      private GizmosShapeCollection CreateGizmosShapeCollection(Node parent, Mesh.PrimitiveType primitiveType, string path, StandardMaterial3D material)
      {
         return new GizmosShapeCollection(parent, new GizmosShape(primitiveType, path), material);
      }
      private StandardMaterial3D CreateStandardMaterial3D(bool blend = true, bool billboard = false, bool noDepthTest = true)
      {
         return new()
         {
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded,
            BlendMode = blend ? BaseMaterial3D.BlendModeEnum.Add : BaseMaterial3D.BlendModeEnum.Mix,
            VertexColorUseAsAlbedo = true,
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
            BillboardMode = billboard ? BaseMaterial3D.BillboardModeEnum.Enabled : BaseMaterial3D.BillboardModeEnum.Disabled,
            BillboardKeepScale = billboard,
            NoDepthTest = noDepthTest,
            CullMode = BaseMaterial3D.CullModeEnum.Back
         };
      }
      public GizmosMeshInstance GetGizmosShape(Transform3D transform, float duration, Color? color, DebugGizmosLayers layer)
      {
         GizmosMeshInstance instance = GizmosMeshPool.Retrieve();
         if (instance != null)
         {
            instance.Color = color ?? Colors.White;
            instance.DebugLayer = layer;
            instance.SetDuration(duration);
            instance.Transform = transform;
            return instance;
         }

         return null;
      }
      public void Box(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosBoxCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Arrow(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosArrowCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Circle(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosCircleCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Plane(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosPlaneCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Sphere(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosSphereCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Capsule(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosCapsuleCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Cylinder(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosCylinderCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Point(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosPointCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void SolidBox(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosSolidBoxCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Arrows(Vector3 position, Quaternion rotation, Vector3 size, float duration, DebugGizmosLayers layer)
      {
         _gizmosArrowsCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, null, layer));
      }
      public void SolidCapsule(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosSolidCapsuleCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void SolidCylinder(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosSolidCylinderCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void SolidSphere(Vector3 position, Quaternion rotation, Vector3 size, float duration, Color? color, DebugGizmosLayers layer)
      {
         _gizmosSolidSphereCollection.Add(GetGizmosShape(new Transform3D(new Basis(rotation), position).ScaledLocal(size), duration, color, layer));
      }
      public void Update()
      {
         foreach (GizmosShapeCollection gizmosShapeCollection in _gizmosShapeCollections)
         {
            gizmosShapeCollection.Update();
         }
      }
      public void Dispose()
      {
         GizmosMeshPool?.Clear();
         foreach (GizmosShapeCollection gizmosShapeCollection in _gizmosShapeCollections)
         {
            gizmosShapeCollection.Clear();
         }
      }
   }

   public sealed class GizmosShapeCollection
   {
      private readonly HashSet<GizmosMeshInstance> _gizmosMeshHashSet;
      public readonly MultiMeshInstance3D MultiMeshInstance;

      public static Action<GizmosMeshInstance> OnReturn;
      
      public GizmosShapeCollection(Node parent, GizmosShape shape, StandardMaterial3D standardMaterial)
      {
         _gizmosMeshHashSet = new();
         
         MultiMeshInstance = new MultiMeshInstance3D
         {
            Name = shape.LoadedMesh.ResourceName,
            CastShadow = GeometryInstance3D.ShadowCastingSetting.Off,
            GIMode = GeometryInstance3D.GIModeEnum.Disabled,
            IgnoreOcclusionCulling = true,
         
            MaterialOverride = standardMaterial,
            Multimesh = new MultiMesh
            {
               TransformFormat = MultiMesh.TransformFormatEnum.Transform3D,
               UseColors = true,
               Mesh = shape.GizmosMesh
            }
         };
         
         parent.AddChild(MultiMeshInstance);
         MultiMeshInstance.Owner = parent;
      }
      
      public void Update()
      {
         if (MultiMeshInstance.Multimesh.InstanceCount != _gizmosMeshHashSet.Count)
         {
            MultiMeshInstance.Multimesh.InstanceCount = _gizmosMeshHashSet.Count;
         }
         
         ushort i = 0;
         foreach (GizmosMeshInstance instance in _gizmosMeshHashSet)
         {
            MultiMeshInstance.Multimesh.SetInstanceTransform(i, instance.Transform);
            MultiMeshInstance.Multimesh.SetInstanceColor(i, instance.Color);
            instance.BeenDrawn = true;
            i++;
         }
         
         foreach (GizmosMeshInstance instance in _gizmosMeshHashSet)
         {
            if (instance.CantDraw())
            {
               OnReturn?.Invoke(instance);
               _gizmosMeshHashSet.Remove(instance);
            }
         }
      }
      public void Add(GizmosMeshInstance instance)
      {
         if(instance == null) return;
         _gizmosMeshHashSet.Add(instance);
      }
      public void Clear()
      {
         _gizmosMeshHashSet?.Clear();
         MultiMeshInstance?.Free();
      }
   }
   
   public class GizmosShape
   {
      public readonly ArrayMesh GizmosMesh;
      public readonly Mesh LoadedMesh;

      public GizmosShape(Mesh.PrimitiveType primitiveType, string path)
      {
         GizmosMesh = new ArrayMesh();
         LoadedMesh = ResourceLoader.Load<Mesh>(path);
         GizmosMesh.AddSurfaceFromArrays(primitiveType, LoadedMesh.SurfaceGetArrays(0));
      }
   }
   
   public sealed class GizmosInstancesPool<T> where T : GizmosInstance, new()
   {
      private readonly Queue<T> _pool;
      private ushort _availableInstances;
      private ushort _currentNumberOfInstances;
      
      public readonly ushort MaxSize;
      public ushort AvailableInstances => _availableInstances;
      public ushort CurrentNumberOfInstances => _currentNumberOfInstances;

      public GizmosInstancesPool()
      {
         _pool = new Queue<T>();
         MaxSize = DrawGizmos.TargetPoolSize;
         Expand(DrawGizmos.StartPoolSize);
      }
      public bool Expand(ushort target)
      {
         target = (ushort)Math.Min(target, MaxSize - _currentNumberOfInstances);
         if (target < 1) return false;
            
         for (ushort i = 0; i < target; i++)
         {
            _pool.Enqueue(new T());
         }

         _availableInstances += target;
         _currentNumberOfInstances += target;
            
         return true;
      }
      public T Retrieve()
      {
         if (_availableInstances < 1 && !Expand(1))
         {
            GD.PushWarning($"{GetType()} has no instances available");
            return default;
         }

         _availableInstances--;
         return _pool.Dequeue();
      }
      public void Return(T instance)
      {
         instance.Drop();
         _pool.Enqueue(instance);
         _availableInstances++;
      }

      public void Clear() => _pool.Clear();
   }

   public class GizmosInstance
   {
      public Color Color;
      public DebugGizmosLayers DebugLayer;
      public ulong ExpirationTime;
      public bool BeenDrawn;

      public virtual void Drop()
      {
         Color = default;
         DebugLayer = default;
         ExpirationTime = default;
         BeenDrawn = default;
      }
      public bool CantDraw() => ((DrawGizmos.AllLayers & DebugLayer) == 0 || DrawGizmos.Time > ExpirationTime && BeenDrawn);
      public void SetDuration(float duration)
      {
         ExpirationTime = DrawGizmos.Time + (ulong)(duration * 1000.0f);
         BeenDrawn = false;
      }
   }

   public class GizmosTextInstance : GizmosInstance
   {
      public string Text;
      public override void Drop()
      {
         base.Drop();
         Text = default;
      }
   }

   public class GizmosText2DInstance : GizmosTextInstance
   {
      public Vector2 Position;
      public override void Drop()
      {
         base.Drop();
         Position = default;
      }
   }

   public class GizmosText3DInstance : GizmosTextInstance
   {
      public Vector3 Position;
      public override void Drop()
      {
         base.Drop();
         Position = default;
      }
   }

   public class GizmosMeshInstance : GizmosInstance
   {
      public Transform3D Transform;

      public override void Drop()
      {
         base.Drop();
         Transform = default;
      }
   }

   public class GizmosLineInstance : GizmosInstance
   {
      public Vector3[] Points;

      public override void Drop()
      {
         base.Drop();
         Points = default;
      }
   }
}