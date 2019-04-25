using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

namespace MadFireOn
{

    public class UnityAds : MonoBehaviour
    {

        public static UnityAds instance;

        private int i = 0;

        [HideInInspector]
        public ManageVariables vars;

        void OnEnable()
        {
            vars = Resources.Load("ManageVariablesContainer") as ManageVariables;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            i = 0;
        }

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            i = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.instance == null)
                return;

            if (GameManager.instance.gameOver == true && GameManager.instance.canShowAds == true)
            {
                //we want only one ad to be shown so we put condition that when i is 0 we show ad.
                if (i == 0)
                {
                    i++;
                    GameManager.instance.gamesPlayedBeforeAds++;

                    if (GameManager.instance.gamesPlayedBeforeAds >= vars.showInterstitialAfter)
                    {
                        GameManager.instance.gamesPlayedBeforeAds = 0;
#if AdmobDef
                        AdsManager.instance.ShowInterstitial();

#elif UNITY_ADS     //unity ads     
                        if(!vars.admobActive)
                            ShowUnityAd();
#endif
                    }
                }
            }
        }

        public void ShowAd()
        {
    #if UNITY_ADS
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
    #endif
        }

        //use this function for showing reward ads
        public void ShowRewardedAd()
        {
    #if UNITY_ADS
            if (Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("rewardedVideo", options);
            }
            else
            {
                Debug.Log("Ads not ready");
            }
    #endif
        }

#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");

    /*here we give 50 poinst as reward*/
                GameManager.instance.coins += 5;
                GameManager.instance.Save();

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");

                break;
        }
    }
#endif

    }
}//namespace
