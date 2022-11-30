using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mycom.Target.Unity.Ads;
using System;

public class MyTargetManager : MonoBehaviour
{
    [SerializeField] uint ANDROID_SLOT_ID;
    private InterstitialAd _interstitialAd;

    private void Start()
    {
        InitAd();

        if (PlayerPrefs.HasKey("target"))
        {
            CounterMyTarget.count = PlayerPrefs.GetInt("target");
        }
        else
        {
            CounterMyTarget.count = 0;
        }
    }

    InterstitialAd CreateInterstitial()
    {
        UInt32 slotId = ANDROID_SLOT_ID;
#if UNITY_ANDROID
        slotId = ANDROID_SLOT_ID;
#elif UNITY_IOS
    slotId = IOS_SLOT_ID;
#endif

        // ��������� ������ �������
        // InterstitialAd.IsDebugMode = true;
        // ������� ��������� InterstitialAd
        return new InterstitialAd(slotId);
    }

    void InitAd()
    {
        // ������� ��������� InterstitialAd
        _interstitialAd = CreateInterstitial();

        // ��������� �������� ������
        _interstitialAd.Load();
    }

    public void Show()
    {
        if(CounterMyTarget.count >= 2)
        {
            _interstitialAd.Show();
            CounterMyTarget.count = 0;
        }
    }
}