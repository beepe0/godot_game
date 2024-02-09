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
        if (keys[1] == "rec")
        {
            VoiceManager.Instance.recording = !VoiceManager.Instance.recording;
        }
        else if (keys[1] == "idev")
        {
            GetInputDeviceList();
        }
        else if (keys[1] == "odev")
        {
            GetOutputDeviceList();
        }
        else if (keys[1] == "iset")
        {
            SetInputDevice(ushort.Parse(keys[2]));
        }
        else if (keys[1] == "oset")
        {
            SetOutputDevice(ushort.Parse(keys[2]));
        }
    }
    private void GetInputDeviceList()
    {
        GameConsole.Instance.Debug($"Current input device is {AudioServer.InputDevice}");

        devices = AudioServer.GetInputDeviceList();
        for (int i = 0; i < devices.Length; i++)
        {
            GameConsole.Instance.Debug($"ID: {i} - {devices[i]}");
        }
    }
    private void GetOutputDeviceList()
    {
        GameConsole.Instance.Debug($"Current output device is {AudioServer.InputDevice}");

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
