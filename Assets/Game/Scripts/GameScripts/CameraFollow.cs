using UnityEngine;
using System.Collections;
using DG.Tweening;

namespace MadFireOn
{
    public class CameraFollow : MonoBehaviour
    {

        private Transform cubePlayerPos;
        private float distance; //default distance between cube and camera

        // Use this for initialization
        void Start()
        {
            //get the ref to the player in the scene
            cubePlayerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            //get the y difference between camera and player
            distance = transform.position.y - cubePlayerPos.position.y;
        }

        // Update is called once per frame
        void Update()
        {
            //checks is player is not active and game over is false
            if (!cubePlayerPos.gameObject.activeInHierarchy && !GameManager.instance.gameOver)
            {
                //then we try to find the new player 
                cubePlayerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            }

            MoveCamera();
        }

        //methode which make camera follow player on y axis
        void MoveCamera()
        {
            Vector3 tempPos = transform.position;
            tempPos.y = cubePlayerPos.position.y + distance;
            transform.DOMoveY(tempPos.y, 0.6f);
        }


    }
}//namespace