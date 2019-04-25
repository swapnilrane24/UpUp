using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MadFireOn
{

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance;

        private GameData data;

        [HideInInspector]
        public bool gameOver = false , gameStarted = false;
        [HideInInspector]
        public int currentScore, gamesPlayed, combos, lastTileTapped, gamesPlayedBeforeAds, lastScore;

        //variables which are saved on the device
        [HideInInspector]
        public bool isGameStartedFirstTime;
        [HideInInspector]
        public bool canShowAds;
        [HideInInspector]
        public bool isMusicOn;
        [HideInInspector]
        public bool fbBtnClicked, twitterBtnClicked, rateBtnClicked;
        [HideInInspector]
        public int bestScore;
        //[HideInInspector]
        public bool[] skinUnlocked, achievements, trailUnlocked;
        //[HideInInspector]
        public int selectedSkin, level, currentMission, selectedTrail;
        //[HideInInspector]
        public int coins; //to buy new skins
        [HideInInspector]
        public ulong giftTime, adsTime, rateTime; //to save the time at which the gift button was clicked

        void Awake()
        {
            MakeSingleton();
            InitializeGameVariables();
        }

        // Use this for initialization
        void Start()
        {
            
        }

        void MakeSingleton()
        {
            //this state that if the gameobject to which this script is attached , if it is present in scene then destroy the new one , and if its not present
            //then create new 
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void InitializeGameVariables()
        {
            Load();
            if (data != null)
            {
                isGameStartedFirstTime = data.getIsGameStartedFirstTime();
            }
            else
            {
                isGameStartedFirstTime = true;
            }

            if (isGameStartedFirstTime)
            {
                isGameStartedFirstTime = false;
                isMusicOn = true;
                canShowAds = true;
                bestScore = 0;
                currentMission = 0;
                coins = 10;
                level = 1;

                skinUnlocked = new bool[6];//if you want ot add more skins change the value here
                skinUnlocked[0] = true;
                trailUnlocked = new bool[6];
                for (int i = 1; i < skinUnlocked.Length; i++)
                {
                    skinUnlocked[i] = false;
                }

                for (int i = 0; i < trailUnlocked.Length; i++)
                {
                    trailUnlocked[i] = false;
                }
                selectedSkin = 0;

                achievements = new bool[5];//if you want ot add more achievements change the value here
                for (int i = 0; i < achievements.Length; i++)
                {
                    achievements[i] = false;
                }


                fbBtnClicked = twitterBtnClicked = rateBtnClicked = false;
                

                data = new GameData();

                data.setIsGameStartedFirstTime(isGameStartedFirstTime);
                data.setMusicOn(isMusicOn);
                data.setCanShowAds(canShowAds);
                data.setFbClick(fbBtnClicked);
                data.setTwitterClick(twitterBtnClicked);
                data.setRateClick(rateBtnClicked);
                data.setBestScore(bestScore);
                data.setCurrentMission(currentMission);
                data.setSkinUnlocked(skinUnlocked); //add this line 
                data.setTrailUnlocked(trailUnlocked);
                data.setSelectedTrail(selectedTrail);
                data.setCoins(coins);
                data.setSelectedSkin(selectedSkin);   
                data.setAchievementsUnlocked(achievements);
                data.setLevel(level);
                Save();

                Load();
            }
            else
            {
                isGameStartedFirstTime = data.getIsGameStartedFirstTime();
                isMusicOn = data.getMusicOn();
                canShowAds = data.getCanShowAds();
                fbBtnClicked = data.getFbClick();
                twitterBtnClicked = data.getTwitterClick();
                rateBtnClicked = data.getRateClick();
                bestScore = data.getBestScore();
                currentMission = data.getCurrentMission();
                coins = data.getCoins();
                giftTime = data.getGiftTime();
                adsTime = data.getAdsTime();
                rateTime = data.getRateTime();
                selectedSkin = data.getSelectedSkin();
                skinUnlocked = data.getSkinUnlocked();
                trailUnlocked = data.getTrailUnlocked();
                selectedTrail = data.getSelectedTrail();
                achievements = data.getAchievementsUnlocked();
                level = data.getLevel();
            }
        }


        //                              .........this function take care of all saving data like score , current player , current weapon , etc
        public void Save()
        {
            FileStream file = null;
            //whicle working with input and output we use try and catch
            try
            {
                BinaryFormatter bf = new BinaryFormatter();

                file = File.Create(Application.persistentDataPath + "/GameData.dat");

                if (data != null)
                {
                    data.setIsGameStartedFirstTime(isGameStartedFirstTime);
                    data.setMusicOn(isMusicOn);
                    data.setCanShowAds(canShowAds);
                    data.setFbClick(fbBtnClicked);
                    data.setTwitterClick(twitterBtnClicked);
                    data.setRateClick(rateBtnClicked);
                    data.setBestScore(bestScore);
                    data.setCurrentMission(currentMission);
                    data.setSkinUnlocked(skinUnlocked);
                    data.setCoins(coins);
                    data.setGiftTime(giftTime);
                    data.setAdsTime(adsTime);
                    data.setRateTime(rateTime);
                    data.setSelectedSkin(selectedSkin);
                    data.setAchievementsUnlocked(achievements);
                    data.setLevel(level);
                    data.setTrailUnlocked(trailUnlocked);
                    data.setSelectedTrail(selectedTrail);
                    bf.Serialize(file, data);
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }


        }
        //                            .............here we get data from save
        public void Load()
        {
            FileStream file = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + "/GameData.dat", FileMode.Open);
                data = (GameData)bf.Deserialize(file);

            }
            catch (Exception e)
            {
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }
        }

        //for resetting the gameManager

        public void ResetGameManager()
        {
            isGameStartedFirstTime = false;
            isMusicOn = true;
            canShowAds = true;

            bestScore = 0;
            currentMission = 0;
            coins = 10;
            level = 1;
            skinUnlocked = new bool[6];//if you want ot add more skins change the value here

            skinUnlocked[0] = true;

            trailUnlocked = new bool[6];
            for (int i = 1; i < skinUnlocked.Length; i++)
            {
                skinUnlocked[i] = false;
            }

            for (int i = 0; i < trailUnlocked.Length; i++)
            {
                trailUnlocked[i] = false;
            }

            achievements = new bool[5];//if you want ot add more achievements change the value here
            for (int i = 0; i < achievements.Length; i++)
            {
                achievements[i] = false;
            }

            fbBtnClicked = twitterBtnClicked = rateBtnClicked = false;
            

            data = new GameData();

            data.setIsGameStartedFirstTime(isGameStartedFirstTime);
            data.setMusicOn(isMusicOn);
            data.setCanShowAds(canShowAds);
            data.setFbClick(fbBtnClicked);
            data.setTwitterClick(twitterBtnClicked);
            data.setRateClick(rateBtnClicked);
            data.setBestScore(bestScore);
            data.setCurrentMission(currentMission);
            data.setSkinUnlocked(skinUnlocked);
            data.setCoins(coins);
            data.setSelectedSkin(selectedSkin);
            data.setLevel(level);
            data.setTrailUnlocked(trailUnlocked);
            data.setSelectedTrail(selectedTrail);
            data.setAchievementsUnlocked(achievements);
            Save();
            Load();

            Debug.Log("GameManager Reset");
        }


    }

    [Serializable]
    class GameData
    {
        private bool isGameStartedFirstTime;
        private bool isMusicOn;
        private bool canShowAds;
        private bool fbBtnClicked, twitterBtnClicked, rateBtnClicked;
        private int bestScore, currentMission;
        private bool[] skinUnlocked, achievements, trailUnlocked;
        private int selectedSkin, level, selectedTrail;
        private int coins; //to buy new skins
        private ulong giftTime, adsTime, rateTime; //to save the time at which the gift button was clicked

        public void setCanShowAds(bool canShowAds)
        {
            this.canShowAds = canShowAds;
        }

        public bool getCanShowAds()
        {
            return this.canShowAds;
        }

        public void setIsGameStartedFirstTime(bool isGameStartedFirstTime)
        {
            this.isGameStartedFirstTime = isGameStartedFirstTime;

        }

        public bool getIsGameStartedFirstTime()
        {
            return this.isGameStartedFirstTime;

        }
        //                                                                    ...............music
        public void setMusicOn(bool isMusicOn)
        {
            this.isMusicOn = isMusicOn;

        }

        public bool getMusicOn()
        {
            return this.isMusicOn;

        }
        //                                                                      .......music
        
        //....................................................for fb btn
        public void setFbClick(bool fbBtnClicked)
        {
            this.fbBtnClicked = fbBtnClicked;

        }

        public bool getFbClick()
        {
            return this.fbBtnClicked;

        }

        //....................................................for twitter btn
        public void setTwitterClick(bool twitterBtnClicked)
        {
            this.twitterBtnClicked = twitterBtnClicked;
        }

        public bool getTwitterClick()
        {
            return this.twitterBtnClicked;
        }

        //....................................................for rate btn
        public void setRateClick(bool rateBtnClicked)
        {
            this.rateBtnClicked = rateBtnClicked;
        }

        public bool getRateClick()
        {
            return this.rateBtnClicked;
        }

        //best score
        public void setBestScore(int bestScore)
        {
            this.bestScore = bestScore;
        }

        public int getBestScore()
        {
            return this.bestScore;
        }

        //levels
        public void setLevel(int level)
        {
            this.level = level;
        }

        public int getLevel()
        {
            return this.level;
        }

        //current mission
        public void setCurrentMission(int currentMission)
        {
            this.currentMission = currentMission;
        }

        public int getCurrentMission()
        {
            return this.currentMission;
        }

        //gift time
        public void setGiftTime(ulong giftTime)
        {
            this.giftTime = giftTime;
        }

        public ulong getGiftTime()
        {
            return this.giftTime;
        }

        //ads time
        public void setAdsTime(ulong adsTime)
        {
            this.adsTime = adsTime;
        }

        public ulong getAdsTime()
        {
            return this.adsTime;
        }

        //rate time
        public void setRateTime(ulong rateTime)
        {
            this.rateTime = rateTime;
        }

        public ulong getRateTime()
        {
            return this.rateTime;
        }

        //coins
        public void setCoins(int coins)
        {
            this.coins = coins;
        }

        public int getCoins()
        {
            return this.coins;
        }

        //skin unlocked
        public void setSkinUnlocked(bool[] skinUnlocked)
        {
            this.skinUnlocked = skinUnlocked;
        }

        public bool[] getSkinUnlocked()
        {
            return this.skinUnlocked;
        }

        //selectedSkin
        public void setSelectedSkin(int selectedSkin)
        {
            this.selectedSkin = selectedSkin;
        }

        public int getSelectedSkin()
        {
            return this.selectedSkin;
        }

        //trail unlocked
        public void setTrailUnlocked(bool[] trailUnlocked)
        {
            this.trailUnlocked = trailUnlocked;
        }

        public bool[] getTrailUnlocked()
        {
            return this.trailUnlocked;
        }

        //selectedTrail
        public void setSelectedTrail(int selectedTrail)
        {
            this.selectedTrail = selectedTrail;
        }

        public int getSelectedTrail()
        {
            return this.selectedTrail;
        }

        //achievements unlocked
        public void setAchievementsUnlocked(bool[] achievements)
        {
            this.achievements = achievements;
        }

        public bool[] getAchievementsUnlocked()
        {
            return this.achievements;
        }
    }
}//namespace MadFireOn
