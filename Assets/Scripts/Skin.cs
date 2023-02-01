using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "New Entity/NewSkin")]

public class Skin : ScriptableObject
{
    public string nameSkin;
    public Sprite sprite;
    public int cost;
}