using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace MadFireOn
{
    //ref to type of mission and amount of points to complete it
    [System.Serializable]
    public class MissionInfo
    {
        public enum MissionType { Score,GamePlayed/*,CombosAchieved */}

        public MissionType missionType;
        public int number;
    }

    public class Missions : MonoBehaviour
    {
        public static Missions instance;

        private int currentMissionInd; //ref to current mission number
        [HideInInspector]
        public bool missionCompleted = false; //track the mission status
        private MissionInfo currentMission;

        #region Current Mission Data
        private string missionDescription;
        private string type;
        private int number;
        #endregion

        public MissionInfo[] missionInfo; //array of missions

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        void Awake()
        {
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

        // Use this for initialization
        void Start()
        {
            InitializeMission();
        }
        //on every loading of scene
        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {//chekc if mission number is within total mission
            if (currentMissionInd < missionInfo.Length)
            {
                if (missionCompleted)//if completed
                {   //change the mission number
                    GameManager.instance.currentMission++;
                    GameManager.instance.level++;//increase the level
                    GameManager.instance.Save();//save it 
                    InitializeMission();
                }
                else
                {
                    InitializeMission();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!missionCompleted)
            {
                MissionTacker();
            }
        }


        public void MissionTacker()
        {
            //check the mission type
            if (type == "Score")
            {
                //check if the condition is satisfied
                if (GameManager.instance.currentScore == number)
                {
                    //if yes then set the mission to true
                    missionCompleted = true;
                    Debug.Log("Score Mission Completed");
                }

            }
            else if (type == "GamePlayed")
            {
                if (GameManager.instance.gamesPlayed == number)
                {
                    missionCompleted = true;
                    Debug.Log("GamePlay Mission Completed");
                }
            }
            //else if (type == "CombosAchieved")
            //{
            //    if (GameManager.instance.combos == number)
            //    {
            //        missionCompleted = true;
            //        Debug.Log("CombosAchieved Mission Completed");
            //    }
            //}

        }

        void InitializeMission()
        {
            MissionGui.instance.missionDescriptionImage.fillAmount = 0;
            //get the mission number 
            currentMissionInd = GameManager.instance.currentMission;
            //then get the mission from the list at that number
            currentMission = missionInfo[currentMissionInd];
            //store the mission type
            type = currentMission.missionType.ToString();
            //store the number in that mission
            number = currentMission.number;

            //check if the mission type is GamePlayed
            if (currentMission.missionType == MissionInfo.MissionType.GamePlayed)
            {
                //then keep increasing the game played with every game started
                GameManager.instance.gamesPlayed++;
            }
            else
            {
                GameManager.instance.gamesPlayed = 0;
            }

            //check if the type is score
            if (type == "Score")
            {
                //create the description for the mission
                missionDescription = "Score " + number + " in a Game";
            }
            else if (type == "GamePlayed")
            {
                missionDescription = "Play " + number + " Game";
            }
            //else if (type == "CombosAchieved")
            //{
            //    missionDescription = "Achieve Combo X" + number + " in a Game";
            //}

            if (!missionCompleted)
            {
                MissionGui.instance.AssignValues("Level " + GameManager.instance.level,
                                                    "Score Multiplier X" + GameManager.instance.level,
                                                    missionDescription);
            }
            else
            {
                StartCoroutine(MissionGui.instance.AssignValuesAnim("Level " + GameManager.instance.level,
                                                    "Score Multiplier X" + GameManager.instance.level,
                                                    missionDescription));
                missionCompleted = false;
            }
        }


    }//class
}//namespace