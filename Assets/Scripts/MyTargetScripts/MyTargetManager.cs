using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mycom.Target.Unity.Ads;
using System;

public class MyTargetManager : MonoBehaviour
{
    [SerializeField] uint ANDROID_SLOT_ID;
    private InterstitialAd _interstitialAd;
    [SerializeField] Text text;

    int count;

    private void Start()
    {
        InitAd();

        if (PlayerPrefs.HasKey("target"))
        {
            count = PlayerPrefs.GetInt("target");
        }
        else
        {
            count = 0;
        }

        StartCoroutine(Show());

        //text.text = count.ToString();
    }

    InterstitialAd CreateInterstitial()
    {
        UInt32 slotId = ANDROID_SLOT_ID;
#if UNITY_ANDROID
        slotId = ANDROID_SLOT_ID;
#elif UNITY_IOS
    slotId = IOS_SLOT_ID;
#endif

        // Включение режима отладки
        // InterstitialAd.IsDebugMode = true;
        // Создаем экземпляр InterstitialAd
        return new InterstitialAd(slotId);
    }

    void InitAd()
    {
        // Создаем экземпляр InterstitialAd
        _interstitialAd = CreateInterstitial();

        // Запускаем загрузку данных
        _interstitialAd.Load();
    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(1.2f);

        if(count >= 2)
        {
            //text.text = count.ToString() + " R";
            _interstitialAd.Show();
            count = 0;
            PlayerPrefs.SetInt("target", count);
        }
    }
}