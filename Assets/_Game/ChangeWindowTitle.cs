using System.Runtime.InteropServices;
using UnityEngine;

public static class ChangeWindowTitle
{
#if PLATFORM_STANDALONE
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern bool SetWindowText(System.IntPtr hwnd, string title);

    [DllImport("user32.dll")]
    private static extern System.IntPtr FindWindow(string className, string windowName);

    public static void SetTitle(string newTitle)
    {
        System.IntPtr hwnd = FindWindow(null, Application.productName);

        if (hwnd != System.IntPtr.Zero)
        {
            SetWindowText(hwnd, newTitle);
            Debug.Log($"Название окна изменено на: {newTitle}");
        }
    }
#endif
}
