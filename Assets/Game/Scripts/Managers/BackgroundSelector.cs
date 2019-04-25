using UnityEngine;
using System.Collections;

public class BackgroundSelector : MonoBehaviour {

    public GameObject[] backgrounds; //ref to background set gameobject

	// Use this for initialization
	void Start ()
    {//randomly select the number between 0 and total background count
        int r = Random.Range(0, backgrounds.Length);

        for (int i = 0; i < backgrounds.Length; i++)
        {//activate the selected one and deactivate others
            if (i != r)
            {
                backgrounds[i].SetActive(false);
            }
            else
            {
                backgrounds[i].SetActive(true);
            }
        }


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
