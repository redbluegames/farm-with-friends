using UnityEngine;
using System.Collections;

public class RBInput {

    const string PREFIX = "_P";

    public static bool GetButtonDownForPlayer(string buttonName, int playerIndex)
    {
        return Input.GetButtonDown (ConcatPlayerIndex (buttonName, playerIndex));
    }

    public static float GetAxisRawForPlayer (string axisName, int playerIndex)
    {
        return Input.GetAxisRaw (ConcatPlayerIndex(axisName, playerIndex));
    }

    public static float GetAxisForPlayer (string axisName, int playerIndex)
    {
        return Input.GetAxis (ConcatPlayerIndex (axisName, playerIndex));
    }

    static string ConcatPlayerIndex (string buttonName, int playerIndex)
    {
        return buttonName + PREFIX + playerIndex.ToString ();
    }
}
