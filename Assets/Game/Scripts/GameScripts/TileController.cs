using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace MadFireOn
{
    public class TileController : MonoBehaviour
    {

        //[HideInInspector]
        public int colorInd; //ref to the color ind , this the index of color in tile holder color list
        [HideInInspector]
        //this is ref to tile sprite color which is used to assign color to player
        public Color32 spriteColor;
        public AudioClip[] sfx; //0 for tileTape , 1 for bang and 2 for point

        private GameObject cubeDetector; //ref to gameobject which detects the player
        private BoxCollider2D triggerBox; //ref to triggered box collider component

        float speedRatio; //speed ratio of the player basic speed an current speed

        private CubeController playerObj;
        private int tileIndex;
        private AudioSource music;

        // Use this for initialization
        void Start()
        {
            music = GetComponent<AudioSource>();
            tileIndex = int.Parse(gameObject.name);
            //getting the gameobject
            cubeDetector = transform.FindChild("cubeDetector").gameObject;
            cubeDetector.SetActive(false); //deactivating it

            //it will assign the 1st box collide attached to the gameobject
            //so the 1st boxcollide must be the trigger one
            triggerBox = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            speedRatio = (2 / CubeController.instance.speed);//calculating the ratio
        }
        //when the mouse is clicked
        void OnMouseDown()
        {
            //checks if the game over is false and game is started
            if (!GameManager.instance.gameOver && GameManager.instance.gameStarted)
            {//disable the trigger box collider

                if (playerObj == null)
                {
                    music.PlayOneShot(sfx[0]);
                    MoveUp(0.2f * speedRatio);        //move the tile up  
                }
                else if (playerObj != null)
                {
                    cubeDetector.SetActive(true);
                    playerObj.transform.parent = transform;
                    playerObj.MoveUp(0.2f * speedRatio);
                    MoveUp(0.2f * speedRatio);        //move the tile up  

                    if (playerObj.colorInd == colorInd)
                    {
                        music.PlayOneShot(sfx[2]);
                        CombatTextManager.instance.CreateText(playerObj.gameObject.transform.position,
                            "SCORE!!", Color.green, false);
                    }
                    else
                    {
                        music.PlayOneShot(sfx[1]);
                        CombatTextManager.instance.CreateText(playerObj.gameObject.transform.position,
                            "BANG!!", Color.red, false);
                    }

                }

            }
        }
        //methode which move the tile up
        public void MoveUp(float timeTake)
        {//get ref to current pos
            Vector3 tempPos = transform.position;
            tempPos.y += 0.5f;//modify the y axis
            transform.DOMove(tempPos, timeTake);//and move the tile to new position

            if (playerObj != null)
            {
                playerObj.transform.parent = playerObj.defaultParent;
            }
        }

        //it detect the player
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {   //assign the playerObj value
                playerObj = other.GetComponent<CubeController>();
            }
        }

        //it detect the player
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {//remove the playerObj value
                playerObj = null;
            }
        }

    }
}//namespace