using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MadFireOn
{
    //few classes just to simplify the inspector look , contains variables
    [System.Serializable]
    public class MainMenu
    {
        public Button achievementBtn, leaderboardBtn, settingsBtn, playBtn, shopBtn;
    }

    [System.Serializable]
    public class GameOver
    {
        public Button giftBtn, adsBtn, laterBtn, shareBtn, actualShareBtn, homeBtn;
        public Text scoreText, giftTimer, adsTimer, shareScore;
    }

    [System.Serializable]
    public class Settings
    {
        public Button facebookBtn, removeAdsBtn, restoreBuyBtn, backBtn, creditsBtn, contactBtn, twitterBtn;
    }

    [System.Serializable]
    public class GamePlay
    {
        public Button soundBtn;
        [Tooltip("Drag sound button")]
        public Image soundImg;
        public Text score, bestScore, coins, bonusCoins;
        [Tooltip("Reference to sound on off sprites")]
        public Sprite[] soundSprites;
    }

    [System.Serializable]
    public class Rate
    {
        public Button laterBtn, nevenBtn, rateBtn;
    }

    [System.Serializable]
    public class Shop
    {
        public Button backBtn, adsBtn;
        public Text coins, adsTimer2;
    }

    public class GuiManager : MonoBehaviour
    {
        public static GuiManager instance; //instance to make it acessable by all scripts
        //variables to the classes
        public MainMenu mainMenu; 
        public Settings settings;
        public GameOver gameOver;
        public GamePlay gamePlay;
        public Rate rate;
        public Shop shop;
        //ref to the panels
        public GameObject menuSettingsHolder, gameOverPanel, ratePanel, shareScreen;
        public Animator menuSettingsHolderAnim; //ref to the animator
        public AudioClip[] sfx; //0 for normal and 1 for gift button

        private AudioSource music;

        [HideInInspector]
        public ManageVariables vars;

        void OnEnable()
        {
            vars = Resources.Load("ManageVariablesContainer") as ManageVariables;
        }

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            if (GameManager.instance.canShowAds)
            {
                AdsManager.instance.HideBannerAds();
            }
            music = GetComponent<AudioSource>();
            //at start we want game over and game started false
            GameManager.instance.gameOver = false;
            GameManager.instance.gameStarted = false;
            GameManager.instance.currentScore = 0; //start score need to be zero
            //ref to the text gui in the scene
            gamePlay.score.text = "" + GameManager.instance.currentScore;
            gamePlay.bestScore.text = "" + GameManager.instance.bestScore;
            gamePlay.coins.text = "" + GameManager.instance.coins;
            shop.coins.text = "" + GameManager.instance.coins;

            #region MainMenu Assign
            //all the buttom methods for main menu are assigned here
            mainMenu.playBtn.GetComponent<Button>().onClick.AddListener(() => { PlayBtn(); });
            mainMenu.settingsBtn.GetComponent<Button>().onClick.AddListener(() => { SettingsBtn(); });
            mainMenu.achievementBtn.GetComponent<Button>().onClick.AddListener(() => { AchievementBtn(); });
            mainMenu.leaderboardBtn.GetComponent<Button>().onClick.AddListener(() => { LeaderBoardBtn(); });
            mainMenu.shopBtn.GetComponent<Button>().onClick.AddListener(() => { ShopBtn(); });

            #endregion

            #region Settings Assign
            //all the buttom methods for setting menu are assigned here
            settings.backBtn.GetComponent<Button>().onClick.AddListener(() => { BackBtn(); });
            settings.removeAdsBtn.GetComponent<Button>().onClick.AddListener(() => { RemoveAdsBtn(); });
            settings.restoreBuyBtn.GetComponent<Button>().onClick.AddListener(() => { RestoreBuyBtn(); });
            settings.creditsBtn.GetComponent<Button>().onClick.AddListener(() => { CreditsBtn(); });
            settings.contactBtn.GetComponent<Button>().onClick.AddListener(() => { ContactUsBtn(); });
            settings.twitterBtn.GetComponent<Button>().onClick.AddListener(() => { TwitterBtn(); });
            settings.facebookBtn.GetComponent<Button>().onClick.AddListener(() => { FacebookBtn(); });

            #endregion

            #region Gameover Assign
            //all the buttom methods for gameover menu are assigned here
            gameOver.laterBtn.GetComponent<Button>().onClick.AddListener(() => { LaterBtn(); });
            gameOver.shareBtn.GetComponent<Button>().onClick.AddListener(() => { ShareBtn(); });
            gameOver.giftBtn.GetComponent<Button>().onClick.AddListener(() => { GiftsBtn(); });
            gameOver.adsBtn.GetComponent<Button>().onClick.AddListener(() => { AdsBtn(); });
            gameOver.actualShareBtn.GetComponent<Button>().onClick.AddListener(() => { ActualShareBtn(); });
            gameOver.homeBtn.GetComponent<Button>().onClick.AddListener(() => { HomeBtn(); });

            #endregion

            #region GamePlay Assign

            if (GameManager.instance.isMusicOn == true)
            {
                AudioListener.volume = 1;
                gamePlay.soundImg.sprite = gamePlay.soundSprites[1];
            }
            else
            {
                AudioListener.volume = 0;
                gamePlay.soundImg.sprite = gamePlay.soundSprites[0];
            }

            //all the buttom methods for gameplay menu are assigned here
            gamePlay.soundBtn.GetComponent<Button>().onClick.AddListener(() => { SoundBtn(); });

            #endregion

            #region Rate Assign
            //all the buttom methods for rate menu are assigned here
            rate.rateBtn.GetComponent<Button>().onClick.AddListener(() => { RateBtn(); });
            rate.laterBtn.GetComponent<Button>().onClick.AddListener(() => { RateLaterBtn(); });
            rate.nevenBtn.GetComponent<Button>().onClick.AddListener(() => { NeverBtn(); });

            #endregion

            #region Shop Assign
            //all the buttom methods for shop menu are assigned here
            shop.backBtn.GetComponent<Button>().onClick.AddListener(() => { ShopBackBtn(); });
            shop.adsBtn.GetComponent<Button>().onClick.AddListener(() => { AdsBtn2(); });

            #endregion

        }

        // Update is called once per frame
        void Update()
        {
            //keep updating the text 
            gamePlay.coins.text = "" + GameManager.instance.coins;
            shop.coins.text = "" + GameManager.instance.coins;
            //check if game over is false and game is started
            if (!GameManager.instance.gameOver && GameManager.instance.gameStarted)
            {
                gamePlay.score.text = "" + GameManager.instance.currentScore;
                //if the current score become greater than best score
                if (GameManager.instance.currentScore > GameManager.instance.bestScore)
                {//new best score is saved
                    GameManager.instance.bestScore = GameManager.instance.currentScore;
                    GameManager.instance.Save();
                }
                gamePlay.bestScore.text = "" + GameManager.instance.bestScore;
            }
            //check if game over is true and gameover panel is not active
            if (GameManager.instance.gameOver && !gameOverPanel.activeInHierarchy)
            {//then we start a coroutine

                StartCoroutine(WaitForSomeTime());
            }
            //checks if the fb button is clikced and the button is interactable
            if (GameManager.instance.fbBtnClicked && settings.facebookBtn.interactable == true)
            {//then we make its interactable false
                settings.facebookBtn.interactable = false;
            }
            //checks if the twitter button is clikced and the button is interactable
            if (GameManager.instance.twitterBtnClicked && settings.twitterBtn.interactable == true)
            {
                settings.twitterBtn.interactable = false;
            }

            if (GameManager.instance.canShowAds && !settings.removeAdsBtn.enabled)
            {
                settings.removeAdsBtn.enabled = true;
#if UNITY_IOS
                settings.restoreBuyBtn.enabled = false;
#endif
            }
#if UNITY_IOS
            else if (!GameManager.instance.canShowAds && !settings.restoreBuyBtn.enabled)
            {
                settings.removeAdsBtn.enabled = false;
                settings.restoreBuyBtn.enabled = true;
            }
#endif
        }

#region MainMenu
        void PlayBtn() //methode called when play button is clicked
        {
            music.PlayOneShot(sfx[0]);
            GameManager.instance.gameOver = false;
            GameManager.instance.gameStarted = true;
            menuSettingsHolder.SetActive(false);
            if (GameManager.instance.canShowAds)
            {
                AdsManager.instance.ShowBannerAds();
            }
        }

        void AchievementBtn() //methode called when Achievement button is clicked
        {
            music.PlayOneShot(sfx[0]);
            #if GooglePlayDef
            GooglePlayManager.singleton.OpenAchievements()();
            #endif
        }

        void LeaderBoardBtn() //methode called when LeaderBoard button is clicked
        {
            music.PlayOneShot(sfx[0]);
#if UNITY_ANDROID
#if GooglePlayDef
            GooglePlayManager.singleton.OpenLeaderboardsScore();
#endif
#elif UNITY_IOS
            LeaderboardiOSManager.instance.ShowLeaderboard();
#endif
        }

        void SettingsBtn() //methode called when Settings button is clicked
        {
            music.PlayOneShot(sfx[0]);
            menuSettingsHolderAnim.Play("SettingIn");
        }

        void ShopBtn() //methode called when Shop button is clicked
        {
            music.PlayOneShot(sfx[0]);
            menuSettingsHolderAnim.Play("ShopIn");
        }

#endregion

#region Settings
        void BackBtn() //methode called when play button is clicked
        {
            music.PlayOneShot(sfx[0]);
            menuSettingsHolderAnim.Play("SettingOut");
        }

        void FacebookBtn() //methode called when play button is clicked
        {//open the url
            music.PlayOneShot(sfx[0]);
            Application.OpenURL(vars.facebookBtnUrl);
            GameManager.instance.fbBtnClicked = true; //store that the button is clicked
            GameManager.instance.Save();
        }

        void TwitterBtn() //methode called when play button is clicked
        {
            music.PlayOneShot(sfx[0]);
            Application.OpenURL(vars.twitterBtnUrl);
            GameManager.instance.twitterBtnClicked = true;
            GameManager.instance.Save();
        }

        void RemoveAdsBtn() //methode called when play button is clicked
        {
            music.PlayOneShot(sfx[0]);
            //Purchaser.instance.BuyNoAds();
        }

        void RestoreBuyBtn() //methode called when play button is clicked
        {
            music.PlayOneShot(sfx[0]);
            //Purchaser.instance.RestorePurchases();
        }

        void CreditsBtn() //methode called when play button is clicked
        {
            music.PlayOneShot(sfx[0]);
            //create panel and make it active
        }

        void ContactUsBtn() //methode called when play button is clicked
        {
            music.PlayOneShot(sfx[0]);
            //create panel and make it active
        }

#endregion

#region GameOver
        void LaterBtn() //methode called when Later button is clicked
        {
            music.PlayOneShot(sfx[0]);
            gameOverPanel.SetActive(false);
            menuSettingsHolder.SetActive(true);
            //reload the scene
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }

        void GiftsBtn() //methode called when Gifts button is clicked
        {
            music.PlayOneShot(sfx[1]);
            StartCoroutine(BonusPopUp("20"));      //some visual effect of giving reward
            GameManager.instance.coins += 20;      //increase the coins
            GameManager.instance.Save();           //save it
            GiftTimeTracker.instance.TrackTime();  //start the timer
            gameOver.giftBtn.interactable = false; //make button interactable false
        }

        void AdsBtn() //methode called when Ads button is clicked
        {
            music.PlayOneShot(sfx[0]);
            //call the reward ads methode here
            AdsTimeTracker.instance.TrackTime();  //start the timer
            gameOver.adsBtn.interactable = false; //make button interactable false
            shop.adsBtn.interactable = false;     //make button interactable false
        }

        void ShareBtn() //methode called when Share button is clicked
        {
            music.PlayOneShot(sfx[0]);
            shareScreen.SetActive(true); //activate the share panel
        }

        void ActualShareBtn() //methode called when ActualShare button is clicked
        {
            music.PlayOneShot(sfx[0]);
#if UNITY_ANDROID || UNITY_IOS
            ShareScreenShot.instance.ButtonShare(); //call the methode which does sharing stuff
#endif
        }

        void HomeBtn() //methode called when Home button is clicked
        {
            music.PlayOneShot(sfx[0]);
            gameOverPanel.SetActive(false);
            menuSettingsHolder.SetActive(true);
            //reload the scene
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }

        public IEnumerator BonusPopUp(string bonus)
        {   //the coins popup when the gift button is clicked or reward ads is seen
            gamePlay.bonusCoins.text = bonus; //set the value of bonus text
            gamePlay.bonusCoins.gameObject.SetActive(true); //activate it
            yield return new WaitForSeconds(1.5f);          //wait for few seconds
            gamePlay.bonusCoins.gameObject.SetActive(false);//deactivate it again

        }

#endregion

#region GamePlay
        void SoundBtn() //methode called when Sound button is clicked
        {
            music.PlayOneShot(sfx[0]);
            if (GameManager.instance.isMusicOn == true)
            {
                GameManager.instance.isMusicOn = false;
                AudioListener.volume = 0;
                gamePlay.soundImg.sprite = gamePlay.soundSprites[0];
                GameManager.instance.Save();
            }
            else
            {
                GameManager.instance.isMusicOn = true;
                AudioListener.volume = 1;
                gamePlay.soundImg.sprite = gamePlay.soundSprites[1];
                GameManager.instance.Save();
            }
        }
#endregion

#region Rate

        void RateBtn() //methode called when Rate button is clicked
        {
            music.PlayOneShot(sfx[0]);
            //open the respective urls
#if UNITY_ANDROID
            Application.OpenURL(vars.rateButtonUrl);
#elif UNITY_IOS
            Application.OpenURL(vars.rateButtonUrl);
#endif

            ratePanel.SetActive(false);
            GameManager.instance.rateBtnClicked = true;
            GameManager.instance.Save();
        }

        void RateLaterBtn() //methode called when RateLater button is clicked
        {
            music.PlayOneShot(sfx[0]);
            ratePanel.SetActive(false);
            RateTimeTracker.instance.TrackTime();
        }

        void NeverBtn() //methode called when Never button is clicked
        {
            music.PlayOneShot(sfx[0]);
            ratePanel.SetActive(false);
            GameManager.instance.rateBtnClicked = true;
            GameManager.instance.Save();
        }

#endregion

#region Shop

        void ShopBackBtn() //methode called when ShopBack button is clicked
        {
            music.PlayOneShot(sfx[0]);
            menuSettingsHolderAnim.Play("ShopOut");
        }

        void AdsBtn2() //methode called when AdsBtn2 button is clicked
        {
            music.PlayOneShot(sfx[0]);
            //use any one of them
            //admob ads
            //AdsManager.instance.ShowRewardBasedVideo();
            //unity ads
            //UnityAds.instance.ShowRewardedAd();

            AdsTimeTracker.instance.TrackTime();
            shop.adsBtn.interactable = false;
            gameOver.adsBtn.interactable = false;
        }

#endregion


        IEnumerator WaitForSomeTime()
        {
            yield return new WaitForSeconds(1f);
            gameOver.scoreText.text = "" + GameManager.instance.currentScore;
            gameOver.shareScore.text = "" + GameManager.instance.currentScore;
            //gameOver.hiScoreText.gameObject.SetActive(true);
            GameManager.instance.lastScore = GameManager.instance.currentScore;
            gameOverPanel.SetActive(true);
        }

    }//class
}//namespace