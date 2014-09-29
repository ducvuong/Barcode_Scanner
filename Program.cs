using System;
using System.Windows.Forms;

namespace IMEIScanner
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.Run(new IMEI_Scanner());
    }
  }
}