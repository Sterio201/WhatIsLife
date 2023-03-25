using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSkins : MonoBehaviour
{
    [SerializeField] List<Skin> skins;
    [SerializeField] TypeSkin typeSkin;

    void Start()
    {
        if (PlayerPrefs.HasKey(typeSkin.ToString()))
        {
            GetComponent<SpriteRenderer>().sprite = skins.Find(x => x.nameSkin == PlayerPrefs.GetString(typeSkin.ToString())).sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = skins[0].sprite;
        }

        if(gameObject.name == "BodyTest")
        {
            GetComponent<SpriteRenderer>().size = new Vector2(1, 1);
        }
    }
}