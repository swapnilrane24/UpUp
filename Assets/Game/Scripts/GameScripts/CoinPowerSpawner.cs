using UnityEngine;
using System.Collections;

namespace MadFireOn
{

    public class CoinPowerSpawner : MonoBehaviour
    {
        public static CoinPowerSpawner instance;
        //time interval between each spawn
        public float spawnTime = 1.3f;
        public int coinPoints = 1;//count of points a coin will give
        [Header("Percent by which the speed is reduced")]
        [Range(1,100)]
        public float speedSlowDown = 60f;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        // Use this for initialization
        void Start()
        {
            StartCoroutine(WaitForNextSpawn());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator WaitForNextSpawn()
        {
            //wait for the time interval
            yield return new WaitForSeconds(spawnTime);
            int j = Random.Range(0, 4); //get the random number
            if (j == 1) //spawn the item when the number is 1
            {
                //check if the game is not over and game has started
                if (!GameManager.instance.gameOver && GameManager.instance.gameStarted)
                {
                    SelectItem();
                }
            }

            StartCoroutine(WaitForNextSpawn());
        }

        void SelectItem()
        {
            //get the random number
            int i = Random.Range(0, 2);
            //get the random position
            Vector2 temp = new Vector2(Random.Range(-2.5f, 2.5f), transform.position.y);
            //if o the spawn the coin
            if (i == 0)
            {
                //call the methode from pooling script to activate the coin prefab
                GameObject newCoin = ObjectPooling.instance.GetCoins();
                //assign the value
                newCoin.GetComponent<CoinAndPower>().coinPoint = coinPoints;
                //change its position
                newCoin.transform.position = temp;
                //set it active
                newCoin.SetActive(true);
            }
            else if (i == 1)
            {
                //call the methode from pooling script to activate the slowdown prefab
                GameObject newSlowDown = ObjectPooling.instance.GetSlowDown();
                //change its position
                newSlowDown.transform.position = temp;
                //set it active
                newSlowDown.SetActive(true);
            }

        }

    }//class
}//namespace