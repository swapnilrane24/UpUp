using UnityEngine;
using System.Collections;

namespace MadFireOn
{
    public class TileDetectCude : MonoBehaviour
    {
        #region Public Variables
        public TileController parentTile;   //ref to parent tile
        [SerializeField]
        private TileController[] tileList;  //ref to all tiles in scene
        public Color[] colors;              //ref to colors which are used for floating text  
        [HideInInspector]
        public int parentCode;              //ref to the parent number
        #endregion

        #region Private Variables
        private LerpColor[] lerpColorObjectList; //ref to all the object which has lerpcolor script
        private CubeController cubePlayer;       //ref to the player
        private TileHolder holder;               //ref to the tileHolder
        private string parentName;               //ref to parent tile name
        private int parentNumber, comboCount;    //total combo done in a row
        Vector3 direction;                       //ref to direction of player
        float speedRatio;                        //player speed ratio
        #endregion

        void Awake()
        {
            parentName = parentTile.name;          //get the tile name
            parentNumber = int.Parse(parentName);  //convert it into number
            lerpColorObjectList = FindObjectsOfType<LerpColor>(); //get objects with lerpcolor script
            holder = FindObjectOfType<TileHolder>(); //get the tileHolder
        }

        // Use this for initialization
        void Start()
        {//get ref to the player
            cubePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<CubeController>();
        }

        // Update is called once per frame
        void Update()
        {
            parentCode = parentTile.colorInd;    //keep track of parent color index
            direction = cubePlayer.direction;    //keep track of player direction
            speedRatio = (2 / cubePlayer.speed); //keep track of parent speed ratio

        }

        void OnTriggerEnter2D(Collider2D other)
        {//check if the triggering object is player
            if (other.CompareTag("Player"))
            {//get the ref to the object
                CubeController cube = other.GetComponent<CubeController>();
                //check the color ind of player and parent tile
                if (parentTile.colorInd == cube.colorInd)
                {//if same

                    //here we increase score , change cube color , move all tiles to equal level
                    GameManager.instance.currentScore += (GameManager.instance.level);
                    ChangeCubePlayerColor();
                    //make all the objects with lerpcolor script blink
                    for (int i = 0; i < lerpColorObjectList.Length; i++)
                    {
                        lerpColorObjectList[i].blink = true;
                    }
                    //move all the other tiles up
                    for (int i = 0; i < tileList.Length; i++)
                    {
                        if (tileList[i].name != parentName)
                        {
                            if (tileList[i].transform.position.y < parentTile.transform.position.y)
                            {
                                tileList[i].MoveUp(0.3f * speedRatio);
                            }
                        }
                    }
                }
                else //if not same then game is over
                {
                    //game over
                    GameManager.instance.gameOver = true;
                    GameManager.instance.gameStarted = false;
                }
            }

            gameObject.SetActive(false);
        }

        // method which changes the player color
        void ChangeCubePlayerColor()
        {//get the tile name and convert it into number
            int i = int.Parse(parentTile.name);
            //check the direction
            if (direction == Vector3.right)
            {//if moving right then we check if the tile number is below 3
                if (i < 3)
                {//if yes then we add 1 to it
                    i = i + 1;
                    //then we assign the color of next tile to the player
                    cubePlayer.AssignColor(tileList[i].spriteColor, tileList[i].colorInd);
                }//if the tile is 3 which is last from the roght side
                else if (i == 3)
                {//then we randomly get the color
                    int r = Random.Range(0, TileHolder.instance.colors.Length);
                    cubePlayer.AssignColor(TileHolder.instance.colors[r], r);
                }
            }
            else if (direction == Vector3.left)
            {//if moving left then we check if the tile number is below 3
                if (i > 0)
                {//if yes then we minus 1 from it
                    i = i - 1;
                    //then we assign the color of next tile to the player
                    cubePlayer.AssignColor(tileList[i].spriteColor, tileList[i].colorInd);
                }
                else if (i == 0)
                {
                    int r = Random.Range(0, TileHolder.instance.colors.Length);
                    cubePlayer.AssignColor(TileHolder.instance.colors[r], r);
                }
            }
            //add color ind ref of the player to holder
            //this ensure that atlest one tile will have color matching to the player
            holder.cubeColorInd = cubePlayer.colorInd;
        }

    }//class
}//namespace