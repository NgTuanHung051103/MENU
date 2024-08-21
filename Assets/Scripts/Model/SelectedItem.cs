using UnityEngine;

public class SelectedItemRecord
{
    public SelectedItemRecord(string? name, GameObject? newObject)
    {
        Name = name;
        NewObject = newObject;
    }

    public string? Name { get; set; }
    public GameObject? NewObject { get; set; }
    public GameObject? ChildNewObject { get; set; }
}