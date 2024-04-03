using System.Diagnostics;

namespace BP.DebugGizmos
{
   using System;
   using Godot;
   using System.Collections.Generic;
   
   public sealed partial class DrawGizmos : Node
   {
      private static DebugCanvasDrawer _canvasDrawer;
      private static CanvasLayer _canvasLayer;
      
      private static DebugGizmosLayers _allLayers = DebugGizmosLayers.Everythings;
      private static ushort _targetPoolSize = 1024;
      private static ushort _startPoolSize = 256;
      private static ulong _time;

      public static DebugGizmosLayers AllLayers => _allLayers;
      public static ushort TargetPoolSize => _targetPoolSize;
      public static ushort StartPoolSize => _startPoolSize;
      public static ulong Time => _time;

      public override void _Ready()
      {
         Name = "DebugGizmos";
         
         _canvasLayer = new CanvasLayer();
         _canvasLayer.Name = "CanvasLayer";
         _canvasLayer.Layer = 100;
         AddChild(_canvasLayer);
         _canvasLayer.Owner = this;

         _canvasDrawer = new (_canvasLayer);
      }

      private ulong _timer;
      public override void _PhysicsProcess(double delta)
      {
         _time = Godot.Time.GetTicksMsec();
         _canvasDrawer.Update();
      }
      public override void _ExitTree()
      {
         _canvasDrawer?.Dispose();
      }
      
      #region Drawing Functions
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
      private readonly GizmosInstancesPool<GizmosTextInstance> _gizmosTextPool;
      private readonly List<GizmosTextInstance> _gizmosTextList;
      
      private readonly GizmosInstancesPool<GizmosText2DInstance> _gizmosText2dPool;
      private readonly List<GizmosText2DInstance> _gizmosText2dList;
      
      private readonly GizmosInstancesPool<GizmosText3DInstance> _gizmosText3dPool;
      private readonly List<GizmosText3DInstance> _gizmosText3dList;

      private readonly Node2D _canvas2d;
      private readonly Node2D _canvas3d;
      
      private readonly Font _textFont;
      private readonly int _fontSize = 12;

      public DebugCanvasDrawer(Node parent)
      {
         _gizmosTextPool = new();
         _gizmosTextList = new();
         _gizmosText2dPool = new();
         _gizmosText2dList = new();
         _gizmosText3dPool = new();
         _gizmosText3dList = new();
         
         _canvas2d = new();
         _canvas2d.Name = "Canvas2D";
         _canvas2d.ZIndex = 101;
         _canvas2d.Draw += DrawText2D;
         parent.AddChild(_canvas2d);
         
         _canvas3d = new();
         _canvas3d.Name = "Canvas3D";
         _canvas3d.ZIndex = 102;
         _canvas3d.Draw += DrawText3D;
         parent.AddChild(_canvas3d);
         
         Label l = new Label();
         _textFont = l.GetThemeDefaultFont(); 
         l.Free();
      }
      public void Text(string text, float duration, Color? color, DebugGizmosLayers layer)
      {
         GizmosTextInstance instance = _gizmosTextPool.Retrieve();
         if (instance != null)
         {
            instance.Color = color ?? Colors.White;
            instance.DebugLayer = layer;
            instance.SetDuration(duration);
            instance.Text = text;
            _gizmosTextList.Add(instance);
            _canvas2d.QueueRedraw();
         }
      }
      public void Text2D(string text, float duration, Vector2 position, Color? color, DebugGizmosLayers layer)
      {
         GizmosText2DInstance instance = _gizmosText2dPool.Retrieve();
         if (instance != null)
         {
            instance.Color = color ?? Colors.White;
            instance.DebugLayer = layer;
            instance.SetDuration(duration);
            instance.Text = text;
            instance.Position = position;
            _gizmosText2dList.Add(instance);
            _canvas2d.QueueRedraw();
         }
      }
      public void Text3D(string text, float duration, Vector3 position, Color? color, DebugGizmosLayers layer)
      {
         GizmosText3DInstance instance = _gizmosText3dPool.Retrieve();
         if (instance != null)
         {
            instance.Color = color ?? Colors.White;
            instance.DebugLayer = layer;
            instance.SetDuration(duration);
            instance.Text = text;
            instance.Position = position;
            _gizmosText3dList.Add(instance);
            _canvas3d.QueueRedraw();
         }
      }
      public void Update()
      {
          for (int i = 0; i < _gizmosTextList.Count ; i++)
          {
             GizmosTextInstance instance = _gizmosTextList[i];
             if (instance.IsExpired())
             {
                _gizmosTextPool.Return(instance);
                _gizmosTextList.Remove(instance);
                _canvas2d.QueueRedraw();
             }
          }
         
          for (int i = 0; i < _gizmosText2dList.Count ; i++)
          {
             GizmosText2DInstance instance = _gizmosText2dList[i];
             if (instance.IsExpired())
             {
                _gizmosText2dPool.Return(instance);
                _gizmosText2dList.Remove(instance);
                _canvas2d.QueueRedraw();
             }
          }
          
          for (int i = 0; i < _gizmosText3dList.Count ; i++)
          {
             GizmosText3DInstance instance = _gizmosText3dList[i];
             if (instance.IsExpired())
             {
                _gizmosText3dPool.Return(instance);
                _gizmosText3dList.Remove(instance);
                _canvas3d.QueueRedraw();
             }
          }
      }
      private void DrawText2D()
      {
         Vector2 position = new Vector2(400, _fontSize * 1.5f);

         foreach (GizmosTextInstance instance in _gizmosTextList)
         {
            if (instance.IsExpired())
            {
               continue;
            }
            _canvas2d.DrawString(_textFont, position, instance.Text, HorizontalAlignment.Left, -1, _fontSize, instance.Color);
            position.Y += _fontSize * 1.5f;
            instance.BeenDrawn = true;
         }
         
         foreach (GizmosText2DInstance instance in _gizmosText2dList)
         {
            if (instance.IsExpired())
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

         foreach (GizmosText3DInstance instance in _gizmosText3dList)
         {
            if (instance.IsExpired())
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
         _gizmosTextPool?.Clear();
         _gizmosTextList?.Clear();
         _gizmosText2dPool?.Clear();
         _gizmosText2dList?.Clear();
         _gizmosText3dPool?.Clear();
         _gizmosText3dList?.Clear();
         _canvas2d?.Dispose();
         _canvas3d?.Dispose();
         _textFont?.Dispose();
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
      public bool IsExpired() => ((DrawGizmos.AllLayers & DebugLayer) == 0 || DrawGizmos.Time > ExpirationTime && BeenDrawn);
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
}