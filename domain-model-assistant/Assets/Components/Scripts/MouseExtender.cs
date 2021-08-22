using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputExtender
{

  public static class MouseExtender
  {
    public const int LEFT_BUTTON = 0;
    
    private static float lastTime;

    private static int clicks = 0;

    public static bool IsSingleClick(int mouseBtn = LEFT_BUTTON)
    {
      return NumberOfClicks(mouseBtn) == 1;
    }

    public static bool IsDoubleClick(int mouseBtn = LEFT_BUTTON)
    {
      return NumberOfClicks(mouseBtn) == 2;
    }

    public static int NumberOfClicks(int mouseBtn)
    {
      if (Input.GetMouseButtonDown(mouseBtn))
      {
        clicks += 1;

        var now = Time.unscaledTime;
        var diff = now - lastTime;
        //lastTime = now;

        if (clicks % 2 == 1)
        {
          lastTime = now;
        }

        if (diff <= 0.2f)
        {
          if (clicks >= 2)
          {
            Debug.Log("Double click");
            clicks = 0;
            return 2;
          }
        }
        else if (diff > 0.2f)
        {
          if (clicks == 1)
          {
            lastTime = Time.unscaledTime;
            Debug.Log("Single click");
            clicks = 0;
            return 1;
          }
        }
        else
        {
          return clicks;
        }
      }
      return 0;
    }

  }

}
