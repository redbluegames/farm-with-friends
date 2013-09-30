using UnityEngine;
using System.Collections;

public class InputDevices
{
    public static InputDevice KEYBOARD = new InputDevice ("PC");
    public static InputDevice XBOX = new InputDevice ("XBox");

    public enum ControllerTypes
    {
        Keyboard,
        XBox
    }

    public InputDevice[] inputDevices = {new InputDevice("PC"), new InputDevice("XBox")};

    public class InputDevice
    {
        string deviceName;

        public string DeviceName {
            get {
                return deviceName;
            }
        }

        public InputDevice (string name)
        {
            deviceName = name;
        }
    }
}
