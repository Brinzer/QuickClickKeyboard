using System;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VirtualKeyboardTest
{

    //REF:
    //http://pinvoke.net/default.aspx/user32.keybd_event
    //https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    //https://stackoverflow.com/questions/30205450/using-sendkeys-when-windows-locks-gets-access-denied
    //https://social.msdn.microsoft.com/Forums/vstudio/en-US/2b89d89d-ae7b-4652-aca6-976a1513a655/get-a-pid-of-a-process-before-the-process-can-do-anything?forum=csharpgeneral
    //https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-createprocessa?redirectedfrom=MSDN
    //https://social.msdn.microsoft.com/Forums/sqlserver/en-US/b0065f3f-849e-4fe8-b358-487eb685d200/acess-denied-in-c-execption?forum=csharplanguage
    //https://www.c-sharpcorner.com/article/working-with-win32-api-in-net/
    //https://www.c-sharpcorner.com/UploadFile/40e97e/send-keys-to-application-programmatically-using-C-Sharp/
    //https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.mainwindowhandle?view=netframework-4.8
    //https://stackoverflow.com/questions/1953582/how-to-i-get-the-window-handle-by-giving-the-process-name-that-is-running
    //https://docs.microsoft.com/en-us/dotnet/api/system.windows.input.keyboard?view=netframework-4.8
    //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys.send?view=netframework-4.8
    //http://code.fitness/post/2017/09/how-to-activate-window-of-foreign-process.html
    //https://www.codeguru.com/vb/gen/vb_system/keyboard/article.php/c14629/SendKeys.htm
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]

        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        [DllImport("user32.dll")]

        [return: MarshalAs(UnmanagedType.Bool)]

        static extern bool SetForegroundWindow(IntPtr hWnd);

        private void button1_Click(object sender, EventArgs e)
        {
            //Trying to get the handle of a open notepad for testing.
            // Thsi needs mroe investigation, as it's not working
            // We are able to get the handle, but not able to set it as current process.
            var process = Process.GetProcessesByName("Notepad");
            var handle = process.First().Handle;
            SetForegroundWindow(handle); // This is not working

            //Virtual Keys Reference:
            //https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
            var capsLock = 0x14;
            const int KEYEVENTF_KEYUP = 0x0002;

            //Call the Key event and set Caps Lock folloowed by Key up event.
            // Bingo.! This is working.
            keybd_event((byte)capsLock, 0, 0, IntPtr.Zero);
            keybd_event((byte)capsLock, 0, KEYEVENTF_KEYUP, IntPtr.Zero);


            var A_Key = 0x41;
            // Calling the "A" Key event. This is unfortunately not working
            // as I'm not able to get a current process to type
            // If you debug, this is typing "A" in the current window here.. lol
            // So working, but we need to set the current window to another application to test
            keybd_event((byte)A_Key, 0, 0, handle);
            keybd_event((byte)A_Key, 0, KEYEVENTF_KEYUP, handle);


            // This is another way of sending Keys. This is partially working and not consistent.
            // Caps lock is turning off immediately, so dont know why
            
            //SendKeys.SendWait("{CAPSLOCK}");

            //SendKeys.Send("{CAPSLOCK}");


            //var handle = process.FirstOrDefault().Handle;

            //SendKeys.SendWait("{}");

            //System.Diagnostics.Process.
            //SendKeys.Send("A");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
