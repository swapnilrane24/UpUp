using UnityEngine;
using System.Collections;

namespace MadFireOn
{

    public class CameraShake : MonoBehaviour
    {

        public static CameraShake instance;
        private float shakeTimer;        //amount of time shake is going to last
        private float shakeAmount;  //intensity of the shake

        private Vector3 defaultPos;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
            defaultPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

            if (shakeTimer >= 0)
            {
                Vector2 shakePos = Random.insideUnitCircle * shakeAmount;

                transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y + shakePos.y, transform.position.z);

                shakeTimer -= Time.deltaTime;
            }
            else if (shakeTimer <= 0 && GameManager.instance.gameOver)
            {
                Vector3 temp = new Vector3(defaultPos.x, transform.position.y, defaultPos.z);

                transform.position = Vector3.Lerp(transform.position, temp, 0.5f);
            }
        }

        public void ShakeCamera(float shakePwr, float shakeDur)
        {
            shakeTimer = shakeDur;
            shakeAmount = shakePwr;
        }

    }//class
}//namespace