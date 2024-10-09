using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;

// TODO: Make it work statically

public class DebugManager : StaticInstance<DebugManager>
{
    [Header("Debug Checks")]
    [SerializeField]
    SerializedDictionary<string, bool> checks = new SerializedDictionary<string, bool>() {
            {"Systems", true}, {"Input", true}, {"Manager", true}, {"Behaviours", true}, {"CharacterMechanics", true}, {"Animations", true}, {"Movement", true}
        };

    /// <summary>
    /// Static debug used to log messages into the console without the need of an Instance reference. This is the method that should be used in the other
    /// classes to debug messages. The method uses a dictionary of bools that checks if the desired type of messages will get logged or not. This
    /// is done to clean the console of debug messages and only log the desired type of information.
    /// </summary>
    /// <param name="check">The type of message that is to be logged.</param>
    /// <param name="message">The message to be logged.</param>
    public static void Log(MessageTypes check, string message)
    {
        Instance.DebugMessage(check, message);
    }

    private void DebugMessage(MessageTypes check, string message)
    {
        if (!checks.ContainsKey(check.ToString())) return;
        if (!checks[check.ToString()]) return;
        Debug.Log(message);
    }

    protected override void Awake()
    {
        base.Awake();
        if (checks.Count != Enum.GetNames(typeof(MessageTypes)).Length) GenerateDictionary();
    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void GenerateDictionary()
    {
        checks = new SerializedDictionary<string, bool>() {
                {"Systems", true},
                {"Input", true},
                {"Manager", true},
                {"Behaviours", true},
                {"CharacterMechanics", true},
                {"Animations", true},
                {"Movement", true},
                {"Interaction", true},
                {"Crafting", true}
            };
    }
}

public enum MessageTypes
{
    Systems,
    Input,
    Managers,
    Behaviours,
    CharacterMechanics,
    Animations,
    Movement,
    Interaction,
    Crafting
}

