using UnityEngine;
using System.Collections.Generic;
using System.Reflection;


public abstract class ScriptableBuff : ScriptableObject
{
    public bool IsPermanent;

    //Time duration of the buff in seconds.
    public float Duration;

    //Duration is increased each time the buff is applied.

    public bool IsDurationStacked;

    //Effect value is increased each time the buff is applied.
    public bool IsEffectStacked;

    public string BuffName;

    public Texture Icon;

    public string Description;

    public List<string> Arguments = new List<string>();

    public abstract TimedBuff InitializeBuff(GameObject obj);

    public string GetFormattedDescription()
    {
        string formattedDescription = Description;
        foreach (var arg in Arguments)
        {
            FieldInfo field = GetType().GetField(arg);
            if (field != null && formattedDescription.Contains($"{{{arg}}}"))
            {
                object value = field.GetValue(this);
                formattedDescription = formattedDescription.Replace($"{{{arg}}}", value.ToString());
            }
        }
        return formattedDescription;
    }
}