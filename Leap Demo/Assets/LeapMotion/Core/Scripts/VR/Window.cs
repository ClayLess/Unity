using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Window : MonoBehaviour {
    public Rect screenPosition;
    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    // not used rigth now
    //const uint SWP_NOMOVE = 0x2;
    //const uint SWP_NOSIZE = 1;
    //const uint SWP_NOZORDER = 0x4;
    //const uint SWP_HIDEWINDOW = 0x0080;
    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;
    private void Awake()
    {
        screenPosition.x = (int)((Screen.currentResolution.width - screenPosition.width) / 2);
        screenPosition.y = (int)((Screen.currentResolution.height - screenPosition.height) / 2);
        if (Screen.currentResolution.height <= 768)
        {
            screenPosition.y = 0;
        }
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_BORDER);//设置无框；
        bool result = SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);//exe居中显示；
    }

}
