using UnityEngine;
using System.Collections;
using System;

namespace MadFireOn
{
    public class RateTimeTracker : MonoBehaviour
    {
        public static RateTimeTracker instance;
        private ulong lastRatePanelShown;
        [Tooltip("Its time between two consecutive rate pop up in minute")]
        public int timeToWait = 15;
        bool showRatePanel;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        void Start()
        {
            //if player has clikced rate or never button we wont execute the below code
            if (GameManager.instance.rateBtnClicked)
                return;

            lastRatePanelShown = GameManager.instance.rateTime;
            IsRateReady();
        }

        void Update()
        {
            //if player has clikced rate or never button we wont execute the below code
            if (GameManager.instance.rateBtnClicked)
                return;

            if (GameManager.instance.gameOver && !GameManager.instance.gameStarted)
            {
                IsRateReady();

                if (showRatePanel)
                {
                    GuiManager.instance.ratePanel.SetActive(true);
                }
                else
                {
                    GuiManager.instance.ratePanel.SetActive(false);
                }
            }
        }

        public void TrackTime()
        {
            GameManager.instance.rateTime = (ulong)DateTime.Now.Ticks;
            lastRatePanelShown = GameManager.instance.rateTime;
            GameManager.instance.Save();
        }

        private void IsRateReady()
        {
            ulong diff = (ulong)DateTime.Now.Ticks - lastRatePanelShown;
            ulong milliSec = diff / TimeSpan.TicksPerMillisecond;
            //(1000 millisec = 1 second)
            //1st converted timeGap into seconds then converted milliseconds into seconds and subtracted
            float secondsLeft = (float)(timeToWait * 60) - milliSec / 1000;

            if (secondsLeft < 0)
            {
                showRatePanel = true;
                return;
            }
            showRatePanel = false;
        }



    }//class
}//namespace