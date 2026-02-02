using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector3 initialValue;
    public Vector3 defaultValue;
    
    public void OnAfterDeserialize()
    {
        initialValue = defaultValue;
    }

    public void OnBeforeSerialize()
    {
        
    }
}
