using System.Collections.Generic;
using UnityEngine;

public class DialogueDatabase : MonoBehaviour
{
    public Dictionary<string, string> lines = new Dictionary<string, string>()
    {
        { "S1_Boss_01", "1, 2… Can you hear me? Great." },
        { "S1_Boss_02", "Welcome to your first task, agent. Don't worry. It's a simple one." },
        { "S1_Boss_03", "Wondering how to tell who’s infected? You can’t. So eliminate them all." },
        { "S1_Boss_04", "Now, don’t lose more time and start. I’m keeping track of you through the camera in your visor." }
    };
}