using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private string[] lines;

    public string[] GetLines()
    {
        return lines;
    }
}