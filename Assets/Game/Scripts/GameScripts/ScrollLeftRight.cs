using UnityEngine;
using System.Collections;

namespace MadFireOn
{
    public class ScrollLeftRight : MonoBehaviour
    {
        //speed with which the object move
        public float speed = 0.1f;
        private Vector3 direction = Vector3.zero;//direction of movement
        CubeController cube;

        // Use this for initialization
        void Start()
        {//ref to the playe in the scene
            cube = GameObject.FindGameObjectWithTag("Player").GetComponent<CubeController>();
        }

        // Update is called once per frame
        void Update()
        {
            //checks is player is not active and game over is false
            if (!cube.gameObject.activeInHierarchy && !GameManager.instance.gameOver)
            {
                cube = GameObject.FindGameObjectWithTag("Player").GetComponent<CubeController>();
            }

                if (cube.direction == Vector3.right)
            {
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.right;
            }
            //if game is not over and game is started
            if (!GameManager.instance.gameOver && GameManager.instance.gameStarted)
                transform.Translate(direction * speed * Time.deltaTime);//move

        }

    }
}//namespace