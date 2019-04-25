using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MadFireOn
{
    public class SampleButton : MonoBehaviour
    {

        public static SampleButton instance;

        #region Button Components
        public GameObject lockImg, unselectImg;
        public Image shapeSkin;   //need to be assigned by createScrollList
        public Button button;
        #endregion

        [HideInInspector]
        public bool shapeButton = true;
        [HideInInspector]
        public int cost;     //need to be assigned by createScrollList
        [HideInInspector]        //need to be assigned by createScrollList
        public int skinIndex;    //this the index which is respective to the "skinUnlocked" bool array in GameManager
        private AudioSource sound;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            //sound = GetComponent<AudioSource>();
            button.GetComponent<Button>().onClick.AddListener(() => { ButtonPressed(); });

        }

        // Update is called once per frame
        void Update()
        {
            if (shapeButton)
            {
                if (GameManager.instance.selectedSkin != skinIndex)
                {
                    unselectImg.SetActive(true);
                }
            }
            else
            {
                if (GameManager.instance.selectedTrail != skinIndex)
                {
                    unselectImg.SetActive(true);
                }
            }
        }

        //methode called when we press the button
        public void ButtonPressed()
        {

            if (shapeButton)
            {
                ShapeFunction();
                PlayerHolder.instance.SelectedShape();
                PlayerHolder.instance.SelectedTrail();
                TileHolder.instance.shapeChanged = true;
            }
            else
            {
                TrailFunction();
                PlayerHolder.instance.SelectedTrail();
            }

        }

        void ShapeFunction()
        {
            //here we check if the skin is unlocked
            if (GameManager.instance.skinUnlocked[skinIndex] == true)
            {
                //if yes we select the skin
                //then we will deavtivate the image which make button look as un selected
                unselectImg.SetActive(false);
                GameManager.instance.selectedSkin = skinIndex;
                GameManager.instance.Save();
                //sound.Play();
            }
            else
            {//if no we check for the cost and total points player has
                if (GameManager.instance.coins >= cost)
                {//if coins are more or equal to the required coins
                    //the cost amount is deducted from the total coins
                    GameManager.instance.coins -= cost;
                    //the respective skin is unlocked
                    GameManager.instance.skinUnlocked[skinIndex] = true;
                    //the respective skin is selected
                    GameManager.instance.selectedSkin = skinIndex;
                    //all the dala is then stored in the device
                    GameManager.instance.Save();
                    lockImg.SetActive(false);
                    //then we will deavtivate the image which make button look as un selected
                    unselectImg.SetActive(false);
                    //sound.Play();
                }
            }
        }

        void TrailFunction()
        {
            //here we check if the trail is unlocked
            if (GameManager.instance.trailUnlocked[skinIndex] == true)
            {
                //if yes we select the trail
                //then we will deavtivate the image which make button look as un selected
                unselectImg.SetActive(false);
                GameManager.instance.selectedTrail = skinIndex;
                GameManager.instance.Save();
                //sound.Play();
            }
            else
            {//if no we check for the cost and total points player has
                if (GameManager.instance.coins >= cost)
                {//if coins are more or equal to the required coins
                    //the cost amount is deducted from the total coins
                    GameManager.instance.coins -= cost;
                    //the respective skin is unlocked
                    GameManager.instance.trailUnlocked[skinIndex] = true;
                    //the respective skin is selected
                    GameManager.instance.selectedTrail = skinIndex;
                    //all the dala is then stored in the device
                    GameManager.instance.Save();
                    lockImg.SetActive(false);
                    //then we will deavtivate the image which make button look as un selected
                    unselectImg.SetActive(false);
                    //sound.Play();
                }
            }
        }


    }//class
}//namespace