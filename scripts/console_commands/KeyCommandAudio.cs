using BP.GameConsole.Attribute;
using BP.GameConsole.Behaviour;
using BP.GameConsole;
using Godot;

[Command("audio")]
public partial class KeyCommandAudio : Command
{
    string[] devices = null;
    public override void Execute(string[] keys)
    {
        if (keys[1].Equals("record"))
        {
            VoiceManager.Instance.recording = !VoiceManager.Instance.recording;
        }
        if (keys[1].Equals("listen"))
        {
            VoiceManager.Instance.listen = !VoiceManager.Instance.listen;
        }
        else if (keys[1].Equals("input-device"))
        {
            GetInputDeviceList();
        }
        else if (keys[1].Equals("output-device"))
        {
            GetOutputDeviceList();
        }
        else if (keys[1].Equals("set-input-device"))
        {
            SetInputDevice(ushort.Parse(keys[2]));
        }
        else if (keys[1].Equals("set-output-device"))
        {
            SetOutputDevice(ushort.Parse(keys[2]));
        }
    }
    private void GetInputDeviceList()
    {
        GameConsole.Instance.DebugCallDeferrd($"Current input device is {AudioServer.InputDevice}");

        devices = AudioServer.GetInputDeviceList();
        for (int i = 0; i < devices.Length; i++)
        {
            GameConsole.Instance.Debug($"ID: {i} - {devices[i]}");
        }
    }
    private void GetOutputDeviceList()
    {
        GameConsole.Instance.DebugCallDeferrd($"Current output device is {AudioServer.InputDevice}");

        devices = AudioServer.GetOutputDeviceList();
        for (int i = 0; i < devices.Length; i++)
        {
            GameConsole.Instance.Debug($"ID: {i} - {devices[i]}");
        }
    }
    private void SetInputDevice(ushort index)
    {
        AudioServer.InputDevice = devices[index];
    }
    private void SetOutputDevice(ushort index)
    {
        AudioServer.OutputDevice = devices[index];
    }
}
