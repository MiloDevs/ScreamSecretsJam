using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [SerializeField] public string ItemName;
    [SerializeField] public Sprite sprite;
}
