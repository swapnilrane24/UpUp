using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MadFireOn
{
    public class CombatTextManager : MonoBehaviour
    {

        public static CombatTextManager instance;

        public float speed, fadeTime;
        public Vector3 direction;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void CreateText(Vector3 position, string textValue, Color32 colorVal, bool crit)
        {
            GameObject text = ObjectPooling.instance.GetText();
            text.transform.position = position;
            text.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            text.GetComponent<Text>().text = textValue;
            text.GetComponent<Text>().color = new Color(colorVal.r, colorVal.g, colorVal.b, 255f);
            text.SetActive(true);
            text.GetComponent<CombatText>().Initialize(speed, direction, fadeTime, crit);
        }

    }//class
}//namespace