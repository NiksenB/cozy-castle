using UnityEngine;

[CreateAssetMenu]
public class VectorValue : ScriptableObject
{
    public Vector3 value;

    // Constructor to initialize the vector value
    public VectorValue(Vector3 initialValue)
    {
        value = initialValue;
    }
}
