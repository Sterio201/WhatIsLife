using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAllPrefs : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteAll();
    }
}