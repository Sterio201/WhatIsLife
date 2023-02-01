using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSkinScript : MonoBehaviour
{
    [SerializeField] Skin skin;

    [SerializeField] GameObject closeSkin;
    [SerializeField] Text buttonText;
    [SerializeField] GameObject BuyPanel;
    [SerializeField] GameObject NotScorePanel;

    [SerializeField] TypeSkin typeSkin;

    public static GameObject[] chooseSkinButton = new GameObject[2];

    private void Start()
    {
        bool chooseCurentSkin = false;

        if(!PlayerPrefs.HasKey(skin.nameSkin))
        {
            closeSkin.SetActive(true);
            buttonText.text = skin.cost.ToString();
        }

        if(!PlayerPrefs.HasKey(typeSkin.ToString()))
        {
            if (skin.nameSkin == "StandartSkinPlayer" || skin.nameSkin == "StandartBlockSkin")
            {
                closeSkin.SetActive(false);
                PlayerPrefs.SetString(skin.nameSkin, "buy");
                PlayerPrefs.SetString(typeSkin.ToString(), skin.nameSkin);
                buttonText.text = "Выбрать";
                chooseCurentSkin = true;
            }
        }
        else
        {
            if(skin.nameSkin == PlayerPrefs.GetString(typeSkin.ToString()))
            {
                chooseCurentSkin = true;
            }
        }

        if (chooseCurentSkin)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);

            if(typeSkin == TypeSkin.playerSkin)
            {
                chooseSkinButton[0] = gameObject;
            }
            else if(typeSkin == TypeSkin.blockSkin)
            {
                chooseSkinButton[1] = gameObject;
            }
        }
    }

    public void ChooseSkin()
    {
        if (!PlayerPrefs.HasKey(skin.nameSkin))
        {
            if (PlayerPrefs.GetInt("PlayerScore") >= skin.cost)
            {
                BuyPanel.SetActive(true);
                BuyPanel.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(BuySkin);
            }
            else
            {
                NotScorePanel.SetActive(true);
            }
        }
        else
        {
            PlayerPrefs.SetString(typeSkin.ToString(), skin.nameSkin);

            if(typeSkin == TypeSkin.playerSkin)
            {
                chooseSkinButton[0].transform.GetChild(0).gameObject.SetActive(true);
                chooseSkinButton[0].transform.GetChild(1).gameObject.SetActive(false);

                chooseSkinButton[0] = gameObject;
            }
            else if(typeSkin == TypeSkin.blockSkin)
            {
                chooseSkinButton[1].transform.GetChild(0).gameObject.SetActive(true);
                chooseSkinButton[1].transform.GetChild(1).gameObject.SetActive(false);

                chooseSkinButton[1] = gameObject;
            }

            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void BuySkin()
    {
        int score = PlayerPrefs.GetInt("PlayerScore");
        score -= skin.cost;
        PlayerPrefs.SetInt("PlayerScore", score);

        PlayerPrefs.SetString(skin.nameSkin, "buy");
        closeSkin.SetActive(false);
        buttonText.text = "Выбрать";
        BuyPanel.transform.GetChild(0).GetChild(0).GetComponent<Button>().onClick.RemoveListener(BuySkin);
    }
}

public enum TypeSkin {playerSkin, blockSkin }