// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Reflection;
using Ea;
using GoogleMobileAds.Api;
using UnityEngine;

namespace GoogleMobileAds.Common
{
    internal class DummyClient : IBannerClient, IInterstitialClient, IRewardBasedVideoAdClient,
            IAdLoaderClient, INativeExpressAdClient
    {
        public DummyClient()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        // Disable warnings for unused dummy ad events.
		EaAdvertisement _adSetting;
		public EaAdvertisement adSetting{get{ return _adSetting ?? (_adSetting = Resources.Load<EaAdvertisement> (typeof(EaAdvertisement).Name));}}
        #pragma warning disable 67
        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        public event EventHandler<EventArgs> OnAdOpening;

        public event EventHandler<EventArgs> OnAdStarted;

        public event EventHandler<EventArgs> OnAdClosed;

        public event EventHandler<Reward> OnAdRewarded;

        public event EventHandler<EventArgs> OnAdLeavingApplication;

        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

        #pragma warning restore 67

        public string UserId
        {
            get
            {
                if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
                return "UserId";
            }

            set
            {
                if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            }
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, int positionX, int positionY)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void LoadAd(AdRequest request)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowBannerView()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void HideBannerView()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyBannerView()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateInterstitialAd(string adUnitId)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public bool IsLoaded()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return true;
        }

        public void ShowInterstitial()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyInterstitial()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateRewardBasedVideoAd()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetUserId(string userId)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void LoadAd(AdRequest request, string adUnitId)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyRewardBasedVideoAd()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowRewardBasedVideoAd()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateAdLoader(AdLoader.Builder builder)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void Load(AdRequest request)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, int positionX, int positionY)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetAdSize(AdSize adSize)
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowNativeExpressAdView()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void HideNativeExpressAdView()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyNativeExpressAdView()
        {
            if(adSetting.googleDebug) Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }
    }
}
