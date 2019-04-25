using UnityEngine;
using System.Collections;

namespace MadFireOn
{
    public class LerpColor : MonoBehaviour
    {
        //ref to the sprite component
        private SpriteRenderer spriteImg;
        private Color32 defaultColor; // variable to store the default color
        [HideInInspector]
        public bool blink = false; //bool which make it blink

        // Use this for initialization
        void Start()
        {//get the component
            spriteImg = GetComponent<SpriteRenderer>();
            defaultColor = spriteImg.color;//save the color value
        }

        // Update is called once per frame
        void Update()
        {
            if (blink)// check if blink is true
            {
                blink = false;
                StartCoroutine(BlinkColor());//star a coroutine
            }
        }


        public IEnumerator BlinkColor()
        {
            spriteImg.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            spriteImg.color = defaultColor;
        }

    }
}//namespace