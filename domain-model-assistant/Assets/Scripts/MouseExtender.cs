using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputExtender
{
    public static class MouseExtender
    {
        private static float lastTime;
        private static float currentTime;
        private static int clicks = 0;

        public static bool isDoubleClick(int mouseBtn)
        {
            if(Input.GetMouseButtonDown(mouseBtn))
            {
                clicks += 1;
                if(clicks == 1)
                {
                    lastTime = Time.unscaledTime;
                }

                if(clicks >= 2)
                {
                    currentTime = Time.unscaledTime;
                    float difference = currentTime - lastTime;

                    clicks = 0;
                    if(difference <= 0.2f)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
    }
}