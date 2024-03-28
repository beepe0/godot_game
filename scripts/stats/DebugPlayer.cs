using BP.GameConsole;
using Godot;
using System;
using System.Collections.Generic;

public partial class DebugPlayer : Control
{
    public static DebugPlayer Instance { get; private set; }
    [Export] private Timer _timer;
    [ExportGroup("UI")]
    [Export] private RichTextLabel _richTextLable;

    private bool _isFlush;
    private List<string> _consoleContent = new();
    public override void _EnterTree()
    {
        Instance = this;
    }
    public void UpdateContent()
    {
        _isFlush = true;
        string content = string.Empty;
        foreach (string s in _consoleContent) 
        {
            content += s + "\n";
        }
        _richTextLable.Clear();
        _consoleContent.Clear();
        _richTextLable.AppendText(content);
    }
    public void Debug(string text)
    {
        if (!_isFlush) return;
        _consoleContent.Add($"[color=#ffffff]{text}[/color]");
    }
    public void Flush() => _isFlush = false;
    public void FlushCallDeferred() => CallDeferred(MethodName.Flush);
}