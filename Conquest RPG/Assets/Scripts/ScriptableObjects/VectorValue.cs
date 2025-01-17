using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 initialValue;

    [HideInInspector]
    public Vector2 RuntimeValue;
    public void OnAfterDeserialize()
    {
        RuntimeValue = initialValue;
    }
    public void OnBeforeSerialize() { }



}

