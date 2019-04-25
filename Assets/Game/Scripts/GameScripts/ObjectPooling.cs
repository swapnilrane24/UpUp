using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MadFireOn
{
    public class ObjectPooling : MonoBehaviour
    {
        public static ObjectPooling instance;

        public GameObject text;         //ref to text prefabs
        public GameObject coins;        //ref to coins prefabs
        public GameObject slowDown;     //ref to slowDown prefabs

        public int count = 3; //total clones of each object to be spawned
        public int textCount = 6;

        List<GameObject> SpawnCoins = new List<GameObject>();    
        List<GameObject> SpawnText = new List<GameObject>();
        List<GameObject> SpawnSlowDown = new List<GameObject>();    

        public GameObject canvas;

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
            //coins
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(coins);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                SpawnCoins.Add(obj);
            }

            //slowDown
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(slowDown);
                obj.transform.parent = gameObject.transform;
                obj.SetActive(false);
                SpawnSlowDown.Add(obj);
            }

            //text
            for (int i = 0; i < textCount; i++)
            {
                GameObject obj = Instantiate(text);
                //obj.transform.parent = gameObject.transform;
                obj.transform.SetParent(canvas.transform);
                obj.SetActive(false);
                SpawnText.Add(obj);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        //coins
        public GameObject GetCoins()
        {
            for (int i = 0; i < SpawnCoins.Count; i++)
            {
                if (!SpawnCoins[i].activeInHierarchy)
                {
                    return SpawnCoins[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(coins);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            SpawnCoins.Add(obj);
            return obj;
        }

        //slowDown
        public GameObject GetSlowDown()
        {
            for (int i = 0; i < SpawnSlowDown.Count; i++)
            {
                if (!SpawnSlowDown[i].activeInHierarchy)
                {
                    return SpawnSlowDown[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(slowDown);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            SpawnSlowDown.Add(obj);
            return obj;
        }

        //text
        public GameObject GetText()
        {
            for (int i = 0; i < SpawnText.Count; i++)
            {
                if (!SpawnText[i].activeInHierarchy)
                {
                    return SpawnText[i];
                }
            }
            GameObject obj = (GameObject)Instantiate(text);
            obj.transform.parent = gameObject.transform;
            obj.SetActive(false);
            SpawnText.Add(obj);
            return obj;
        }


    }//class
}//namespace