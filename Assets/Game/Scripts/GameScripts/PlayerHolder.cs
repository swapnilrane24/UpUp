using UnityEngine;
using System.Collections;

namespace MadFireOn
{

    public class PlayerHolder : MonoBehaviour
    {

        public static PlayerHolder instance;
        [SerializeField]
        private GameObject[] shapes;//ref to all the shapes present in the scene
        [SerializeField]
        private GameObject[] trails;//ref to all the trails present in the scene
        [HideInInspector]
        public GameObject activeShape;//ref to the active shape present in the scene
        public GameObject trailHolder;//ref to the trailholder

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {//select the shape and trail
            SelectedShape();
            SelectedTrail();
        }

        // Update is called once per frame
        void Update()
        {

        }
        //select the shape
        public void SelectedShape()
        {//loop through each shape
            for (int i = 0; i < shapes.Length; i++)
            {//check its index with the selectedSkin
                if (i != GameManager.instance.selectedSkin)
                {//if not equal then deactivate the shape
                    shapes[i].SetActive(false);
                }
                else
                {//if equal then activate the shape and save it to activeshape variable
                    shapes[i].SetActive(true);
                    activeShape = shapes[i];
                }
            }
        }
        //select the trail
        public void SelectedTrail()
        {//loop through each trail
            for (int i = 0; i < shapes.Length; i++)
            {//check its index with the selectedTrail
                if (i != GameManager.instance.selectedTrail)
                {//if not equal then deactivate the trail
                    trails[i].SetActive(false);
                    //change its parent to the trail holder
                    trails[i].transform.parent = trailHolder.transform;
                }
                else
                {//else we 1st check if that trail is unlocked
                    if (GameManager.instance.trailUnlocked[i] == true)
                    {
                        //if unlocked then we set ot active
                        trails[i].SetActive(true);
                        //change its parent
                        trails[i].transform.parent = activeShape.GetComponent<CubeController>().trailHolder.transform;
                        //change its position
                        trails[i].transform.position = activeShape.GetComponent<CubeController>().trailHolder.transform.position;
                    }
                }
            }
        }


    }//class
}//namespace