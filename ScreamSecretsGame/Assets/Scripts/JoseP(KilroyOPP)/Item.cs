using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [SerializeField] string ItemName;
    [SerializeField] string description;
    [SerializeField] Image icon;
}
