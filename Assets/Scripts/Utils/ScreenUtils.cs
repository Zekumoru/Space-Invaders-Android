
using UnityEngine;

public static class ScreenUtils
{
    #region Properties
    
    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }
    public static float ScreenRight { get; private set; }
    public static float ScreenLeft { get; private set; }
    public static float ScreenTop { get; private set; }
    public static float ScreenBottom { get; private set; }

    #endregion

    #region Public methods

    public static void Initialize()
    {
        Camera camera = Camera.main;
        Vector3 bottomLeft = camera.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
        ScreenLeft = bottomLeft.x;
        ScreenBottom = bottomLeft.y;
        ScreenRight = topRight.x;
        ScreenTop = topRight.y;
        ScreenWidth = ScreenRight - ScreenLeft;
        ScreenHeight = ScreenBottom - ScreenTop;
    }

    #endregion
}
