using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputExtender
{

  public static class MouseExtender
  {
    public const int LeftButton = 0;

    public const int RightButton = 1;

    public const int MiddleButton = 2;

    public const float DoubleClickTime = 0.2f;

    private static Dictionary<int, float> _prevClickTimes = new Dictionary<int, float>();

    public static bool IsSingleClick(int mouseBtn = LeftButton)
    {
      return NumberOfClicks(mouseBtn) == 1;
    }

    public static bool IsDoubleClick(int mouseBtn = LeftButton)
    {
      return NumberOfClicks(mouseBtn) == 2;
    }

    public static int NumberOfClicks(int mouseBtn)
    {
      if (Input.GetMouseButtonDown(mouseBtn))
      {
        var now = Time.unscaledTime;

        if (!_prevClickTimes.ContainsKey(mouseBtn))
        {
           _prevClickTimes[mouseBtn] = now;
           return 1;
        }

        var clicks = PressedRecently(mouseBtn) ? 2 : 1;
        _prevClickTimes[mouseBtn] = now;
        Debug.Log(clicks + " click(s)");
        return clicks;
      }
      return 0;
    }

    private static bool PressedRecently(int mouseBtn)
    {
      return (Time.unscaledTime - _prevClickTimes[mouseBtn]) <= DoubleClickTime;
    }

  }

}
