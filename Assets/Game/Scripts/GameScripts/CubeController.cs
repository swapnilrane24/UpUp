using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

namespace MadFireOn
{
    public class CubeController : MonoBehaviour
    {
        public static CubeController instance;

        #region Public Variables
        public SpriteRenderer spriteImg; //ref to the sprite which shows the color
        public LayerMask wallLayers, tileDetector;//ref to the layermaks
        public GameObject deathEffect, trailHolder;//ref to deathEffect and trailHolder gameobject
        public Transform[] rayPos; //ref to the rays which check for wall detection
        //public Transform tileRayDetect;//ref to the ray which check for detecting tile below it 
        [Header("Player speed variables")]
        public float speed = 2f;//speed of the player
        public float maxSpeed = 8f;//max speed player can have
        //the milestone which tells when to increase speed
        public float speedIncreaseMilestone = 5;
        //this is used to set new milestone when we reach one
        [HideInInspector]
        public float speedMileStoneCount = 5;
        //amount by which speed is increase when we hit milestone
        public float speedMultiplier = 1.5f;
        public Transform defaultParent;
        #endregion

        #region Hidden Public Variables
        //[HideInInspector]
        public int colorInd; //ref to the player color 
        [HideInInspector]
        public bool[] collided; //bools which changes when the left and right collide
        [HideInInspector]
        public Vector3 direction; //ref to the direction of player moving
        [HideInInspector]
        public bool deactivateCollision = false;
        #endregion

        #region Private Variables
        private Rigidbody2D myBody; //ref to rigidbody attached to the player
        private int hitByWall = 0; //varaible which count how many time the player hits the wall
        private bool hitLeft = false; //this is just to make hitbywall increase by one
        #endregion

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            //at start we want the player to move right
            direction = Vector3.right;
            myBody = GetComponent<Rigidbody2D>();
            //initially we need some milestone 
            speedMileStoneCount = speedIncreaseMilestone;
        }

        void Update()
        {
            //if game is not over and game is started
            if (!GameManager.instance.gameOver && GameManager.instance.gameStarted)
            {
                //detect the tiles and check for milestone
                CheckMileStone();
            }
            //if game is over
            else if (GameManager.instance.gameOver)
            {
                //deactivate its physics
                myBody.isKinematic = true;
                //shake the camera
                CameraShake.instance.ShakeCamera(0.1f, 1f);
                //changes the deatheffect position
                deathEffect.transform.position = transform.position;
                //set it active
                deathEffect.SetActive(true);
                //deactiavte the gameobject 
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //if game is not over and game is started
            if (!GameManager.instance.gameOver && GameManager.instance.gameStarted)
            {
                //make the player move
                CubeMovement();
                DetectCollision(); // keep detectig collision
            }

        }

        void CubeMovement()
        {
            //provide the velocity in the direction
            myBody.velocity = direction * speed;
        }

        //methode which assign color to the player
        public void AssignColor(Color32 color, int colorIndx)
        {
            colorInd = colorIndx;// save the ind of color
            spriteImg.color = color;//changes the sprite color which indicates the color
        }

        //methode which detect wall collistion
        void DetectCollision()
        {
            //create the rays for all the ray position
            for (int i = 0; i < rayPos.Length; i++)
            {
                Vector2 rayDirection; //set teh direction
                if (i == 0)
                {
                    rayDirection = new Vector2(-1, 0);
                }
                else
                {
                    rayDirection = new Vector2(1, 0);
                }
                //create the raycast
                RaycastHit2D hit = Physics2D.Raycast(rayPos[i].position, rayDirection, 0.08f , wallLayers);

                //here we check if all the rays of the piece are detecting objects
                if (hit.collider != null)
                {
                    //check which wall its has collided
                    if (hit.collider.tag == "LeftWall")
                    {
                        //cahnges the direction
                        direction = Vector3.right;

                        if (!hitLeft)
                        {//increase the hit count
                            hitByWall++;
                        }
                        hitLeft = true;
                        //select the tile colors
                        TileHolder.instance.SelectColor();
                        //assign the selected color to the tiles
                        TileHolder.instance.AssignColorToTiles();
                    }
                    else if (hit.collider.tag == "RightWall")
                    {
                        direction = Vector3.left;

                        if (hitLeft)
                        {
                            hitByWall++;
                        }
                        hitLeft = false;

                        TileHolder.instance.SelectColor();
                        TileHolder.instance.AssignColorToTiles();
                    }
                    else if (hit.collider.tag == "Tiles")
                    {
                        collided[i] = true;
                    }
                }
            }
            //chekc it again **********************************
            for (int i = 0; i < collided.Length; i++)
            {
                if (collided[i] == true)
                {
                    //game over is true
                    GameManager.instance.gameOver = true;
                    GameManager.instance.gameStarted = false;
                    return;
                }

            }
        }
        

        public void MoveUp(float timeTake)
        {
            Vector3 tempPos = transform.position;
            tempPos.y += 0.5f;
            transform.DOMoveY(tempPos.y, timeTake);
        }

        void CheckMileStone()
        {
            //if the hitByWall gets grater than milestone then speed is increased
            if (hitByWall > speedMileStoneCount)
            {
                //when the speeed is increased we increase the milestones and set new ones
                speedMileStoneCount += speedIncreaseMilestone;
                speedIncreaseMilestone += speedIncreaseMilestone;
                //speed is increase my multiplying with the specific number whihc you can change from inspector
                speed *= speedMultiplier;

                //if our speed goes above max speed limit then we set the speed to max speed
                if (speed >= maxSpeed)
                {
                    speed = maxSpeed;
                }
            }
        }

    }//Class

}//namespace