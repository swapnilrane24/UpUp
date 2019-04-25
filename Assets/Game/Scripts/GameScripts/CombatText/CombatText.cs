using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MadFireOn
{

    public class CombatText : MonoBehaviour
    {
        private float speed;
        private Vector3 direction;
        private float fadeTime;
        public AnimationClip critAnim;
        private bool crit;

        void Update()
        {
            //used to move the text
            if (!crit)
            {
                float translation = speed * Time.deltaTime;

                transform.Translate(direction * translation);
            }
        }

        public void Initialize(float speed, Vector3 direction, float fadeTime, bool crit)
        {
            this.speed = speed;
            this.direction = direction;
            this.fadeTime = fadeTime;
            this.crit = crit;

            if (crit)
            {
                GetComponent<Animator>().SetTrigger("Critical");
                StartCoroutine(Critical());
            }
            else
            {
                StartCoroutine(FadeOut());
            }
        }

        IEnumerator Critical()
        {
            yield return new WaitForSeconds(critAnim.length);
            crit = false;
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeOut()
        {
            float startAlpha = GetComponent<Text>().color.a;

            float rate = 1.0f / fadeTime;
            float progress = 0.0f;

            while (progress < 1.0f)
            {
                Color temp = GetComponent<Text>().color;
                GetComponent<Text>().color = new Color(temp.r, temp.g, temp.b, Mathf.Lerp(startAlpha, 0, progress));
                progress += rate * Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);

            //Destroy(gameObject);
        }


    }//class
}//namespace