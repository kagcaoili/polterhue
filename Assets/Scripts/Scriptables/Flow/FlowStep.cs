using System.Collections;
using UnityEngine;

/// <summary>
/// Base step for listing actions in level flow
/// E.g. fade to black, to dialogue, to prompts and tutorials
/// </summary>
[CreateAssetMenu(fileName = "FlowStep", menuName = "Scriptable Objects/FlowStep")]
public abstract class FlowStep : ScriptableObject
{
    [Header("General")]
    public string id;
    [TextArea]
    public string description; // for level designer reference
    public abstract IEnumerator Run(FlowContext ctx);
}
