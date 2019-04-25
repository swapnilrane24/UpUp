using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MadFireOn
{
    public class MissionGui : MonoBehaviour {

        public static MissionGui instance;

        public Text levelInfo;
        public Text scoreMultiplierInfo;
        public Text missionDescription;
        public Image missionDescriptionImage;
        public float speed = 2f;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void AssignValues(string level, string scoreMultiplier, string missionDes)
        {
            levelInfo.text = level;
            scoreMultiplierInfo.text = scoreMultiplier;
            missionDescription.text = missionDes;
        }

        public IEnumerator AssignValuesAnim(string level, string scoreMultiplier, string missionDes)
        {
            yield return new WaitForSeconds(0.5f);
            levelInfo.fontSize = 0;
            yield return new WaitForSeconds(0.2f);
            levelInfo.text = level;
            yield return new WaitForSeconds(0.2f);
            levelInfo.fontSize = 20;

            scoreMultiplierInfo.text = scoreMultiplier;

            yield return new WaitForSeconds(0.2f);
            missionDescription.fontSize = 0;
            yield return new WaitForSeconds(0.2f);
            missionDescription.text = missionDes;
            yield return new WaitForSeconds(0.2f);
            missionDescription.fontSize = 20;
        }

    }//class
}//namespace