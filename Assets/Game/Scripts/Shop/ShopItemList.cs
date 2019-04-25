using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MadFireOn
{

    [System.Serializable]
    public class Shapes
    {
        public Sprite skinSprite;
        public int skinIndex;
        public int skinCost;
        [HideInInspector]
        public bool unLock;
        public Button.ButtonClickedEvent thingToDo;
    }

    [System.Serializable]
    public class Trails
    {
        public Sprite skinSprite;
        public int skinIndex;
        public int skinCost;
        [HideInInspector]
        public bool unLock;
        public Button.ButtonClickedEvent thingToDo;
    }

    public class ShopItemList : MonoBehaviour
    {
        public static ShopItemList instance;

        public Shapes[] skins;                    //skins array
        public Trails[] trails;                    //trails array
        public GameObject refButton;              //ref to sample button prefab
        public Transform shapeScrollPanel, trailScrollPanel; //ref to scroll panel where all the buttons will be Instantiated
        private AudioSource sound;

        void Awake()
        {
            MakeInstance();
        }

        void MakeInstance()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
            sound = GetComponent<AudioSource>();

            ShapeTracking();
            TrailTracking();

        }

        // Update is called once per frame
        void Update()
        {

        }

        void ShapeTracking()
        {
            //we check how many skins are unlocked before loading all the buttons
            for (int i = 0; i < GameManager.instance.skinUnlocked.Length; i++)
            {
                skins[i].unLock = GameManager.instance.skinUnlocked[i];
            }

            //handle the requirement for each shapes
            foreach (Shapes i in skins)
            {

                GameObject btn = Instantiate(refButton);//here we get ref to the instanciated button

                SampleButton samBtn = btn.GetComponent<SampleButton>();//ref to the script of button

                samBtn.shapeButton = true;
                samBtn.skinIndex = i.skinIndex;     //here we set index to keep track which skin is selected
                samBtn.cost = i.skinCost;      //here we set the cost of skin
                samBtn.shapeSkin.sprite = i.skinSprite;       //here we set the skin image of button
                //if skin is unlocked we do following
                if (i.unLock == true)
                {
                    //deactivate the lock image
                    samBtn.lockImg.SetActive(false);
                }
                //if not we do following
                else
                {
                    //activate the lock image
                    samBtn.lockImg.SetActive(true);
                }

                if (i.skinIndex == GameManager.instance.selectedSkin)
                {
                    samBtn.unselectImg.SetActive(false);
                }
                else
                {
                    samBtn.unselectImg.SetActive(true);
                }

                samBtn.button.onClick = i.thingToDo;
                samBtn.button.interactable = true;
                btn.transform.SetParent(shapeScrollPanel);
                //we set the scale of button fo by default they dot become too large or too small
                btn.transform.localScale = new Vector3(1f, 1f, 1f);

            }
        }

        void TrailTracking()
        {
            //we check how many trails are unlocked before loading all the buttons
            for (int i = 0; i < GameManager.instance.trailUnlocked.Length; i++)
            {
                trails[i].unLock = GameManager.instance.trailUnlocked[i];
            }

            //handle the requirement for each shapes
            foreach (Trails i in trails)
            {

                GameObject btn = Instantiate(refButton);//here we get ref to the instanciated button

                SampleButton samBtn = btn.GetComponent<SampleButton>();//ref to the script of button

                samBtn.shapeButton = false;
                samBtn.skinIndex = i.skinIndex;     //here we set index to keep track which trail is selected
                samBtn.cost = i.skinCost;      //here we set the cost of trail
                samBtn.shapeSkin.sprite = i.skinSprite;       //here we set the trail image of button
                //if trail is unlocked we do following
                if (i.unLock == true)
                {
                    //deactivate the lock image
                    samBtn.lockImg.SetActive(false);
                }
                //if not we do following
                else
                {
                    //activate the lock image
                    samBtn.lockImg.SetActive(true);
                }

                if (i.skinIndex == GameManager.instance.selectedTrail)
                {
                    samBtn.unselectImg.SetActive(false);
                }
                else
                {
                    samBtn.unselectImg.SetActive(true);
                }

                samBtn.button.onClick = i.thingToDo;
                samBtn.button.interactable = true;
                btn.transform.SetParent(trailScrollPanel);
                //we set the scale of button fo by default they dot become too large or too small
                btn.transform.localScale = new Vector3(1f, 1f, 1f);

            }
        }

    }//class
}//namespace