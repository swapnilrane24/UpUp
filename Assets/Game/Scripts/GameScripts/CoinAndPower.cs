using UnityEngine;
using System.Collections;

namespace MadFireOn
{
    [System.Serializable]
    public enum Type
    {
        coins,slowDown
    }

    public class CoinAndPower : MonoBehaviour
    {
        //ref for the type
        public Type type;
        [HideInInspector]
        public int coinPoint; //ref to the amount of coins the coin prefab will give to the player

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
        //methode which decide the behavior of object depending on type
        void TypeSelector()
        {
            switch (type)
            {
                //if coin type
                case Type.coins:
                    // increase the coins 
                    GameManager.instance.coins += coinPoint;
                    GameManager.instance.Save(); //then save it to device
                    //then we create a floating text
                    CombatTextManager.instance.CreateText(transform.position, "+" + coinPoint, Color.yellow, true);
                    break;

                //if slowdown type
                case Type.slowDown:
                    //decrease the speed of player
                    CubeController.instance.speed *= (CoinPowerSpawner.instance.speedSlowDown / 100);
                    //then we create a floating text
                    CombatTextManager.instance.CreateText(transform.position, "Slow Down", Color.red, true);
                    break;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //if the prefab collide woth player
            if (other.CompareTag("Player"))
            {
                TypeSelector();
                gameObject.SetActive(false);
            }
            //if the prefab collide woth tiles
            else if (other.CompareTag("Tiles"))
            {
                gameObject.SetActive(false);
            }
        }



    }//class
}//namespace