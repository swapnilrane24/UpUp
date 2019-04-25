using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MadFireOn
{
    public class TileHolder : MonoBehaviour
    {

        public static TileHolder instance;

        public GameObject[] tiles; //ref to all the tile objects
        public Color32[] colors;   //ref to colors
        private int a, b, c, d; //this to select random colors
        [HideInInspector]
        public int cubeColorInd;//this is to keep track color index which is on cube
        [HideInInspector]
        public bool shapeChanged = false; //to keep track when the player shape changes
        GameObject shape; //ref to the player

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {//if the array of colors has elements
            if (colors.Length > 0)
            {//we select the colors from the array
                SelectColor();
                SelectStartColor();    //this is just to select the color when the game start
                AssignColorToTiles();  //then we assign the selected color to the tiles
                shape = PlayerHolder.instance.activeShape; //get the shape which is active in scene
                //then assign the color to the shape
                shape.GetComponent<CubeController>().AssignColor(colors[cubeColorInd], cubeColorInd);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (shapeChanged)
            {//when the shape change , like whne player unlock new shape or select another
                //get the shape which is active in scene
                shape = PlayerHolder.instance.activeShape;
                //then assign the color to the shape
                shape.GetComponent<CubeController>().AssignColor(colors[cubeColorInd], cubeColorInd);
                shapeChanged = false;
            }
        }
        //only called at the start methode
        public void SelectStartColor()
        {
            //here we make one of the variable equal to cubecolorInd
            int j = Random.Range(0, 4);
            if (j == 0)
            {
                cubeColorInd = a;
            }
            else if (j == 1)
            {
                cubeColorInd = b;
            }
            else if (j == 2)
            {
                cubeColorInd = c;
            }
            else if (j == 3)
            {
                cubeColorInd = d;
            }
        }
        //methode which decide which color to get from color array
        public void SelectColor()
        {
            //after assigning cubecolorindex to one of the variable
            //we assign index to remaining variables
            //select the number for intex 0
            a = Random.Range(0, colors.Length);
            while (a == cubeColorInd)
            {
                a = Random.Range(0, colors.Length);
            }

            b = Random.Range(0, colors.Length);
            while (b == a || b == cubeColorInd) //keep selecting till the number is not equal to intex 0
            {
                b = Random.Range(0, colors.Length);
            }

            c = Random.Range(0, colors.Length);
            while (c == a || c == b || c == cubeColorInd) //it make sure that all the index are different
            {
                c = Random.Range(0, colors.Length);
            }

            d = Random.Range(0, colors.Length);
            while (d == a || d == b || d == c || d == cubeColorInd)
            {
                d = Random.Range(0, colors.Length);
            }

            //here we make one of the variable equal to cubecolorInd
            int j = Random.Range(0, 4);
            if (j == 0)
            {
                a = cubeColorInd;
            }
            else if (j == 1)
            {
                b = cubeColorInd;
            }
            else if (j == 2)
            {
                c = cubeColorInd;
            }
            else if (j == 3)
            {
                d = cubeColorInd;
            }

        }
        //methode which assign the color to the tiles
        public void AssignColorToTiles()
        {
            for (int i = 0; i < tiles.Length; i++)
            {//get the sprite rendere of the tile
                SpriteRenderer square = tiles[i].transform.FindChild("square").GetComponent<SpriteRenderer>();
                //get the tilecontroller component of the tile
                TileController tileController = tiles[i].GetComponent<TileController>();
                if (i == 0) //this is for element zero tile
                {
                    square.color = colors[a];                 //set the color
                    tileController.spriteColor = colors[a];   //set the variable of script
                    tileController.colorInd = a;              //set script index variable
                }
                else if (i == 1) //this is for element 1 tile
                {
                    square.color = colors[b];
                    tileController.spriteColor = colors[b];
                    tileController.colorInd = b;
                }
                else if (i == 2) //this is for element 2 tile
                {
                    square.color = colors[c];
                    tileController.spriteColor = colors[c];
                    tileController.colorInd = c;
                }
                else if (i == 3) //this is for element 3 tile
                {
                    square.color = colors[d];
                    tileController.spriteColor = colors[d];
                    tileController.colorInd = d;
                }
            }
        }

    }
}//namespace