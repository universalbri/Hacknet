using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPHelper.System;
using static AlphaBlendControls.ABTextBox;

namespace TCPHelper
{
    internal class InternalAppHandler
    {
        private int curPID = 0;
        public struct APPInfo
        {
            public int PID;
            public string Name;
        }
        
        private List<APPInfo> _currentRunningApps = new List<APPInfo>();
        private MemoryMetrics _memoryMetrics = new MemoryMetrics();
        private Graphics _gAI = null;

        public InternalAppHandler() { }

        public ReadOnlyCollection<APPInfo> CurrentRunningApps { get {  return _currentRunningApps.AsReadOnly(); } }

        public void DrawFrame( Graphics g, AlphaBlendControls.ABTextBox txtIAInfo )
        {
            double gigDivisor = 1000000000.0;
            string memInfo = string.Format("USED RAM: {0:###0.00}gb / {1:###0.00}gb\r\n", _memoryMetrics.Used / gigDivisor, _memoryMetrics.Total / gigDivisor);
            
            //txtIAInfo.Visible = false;
            Font newFont = new Font(txtIAInfo.Font.FontFamily, 11, GraphicsUnit.Pixel);
            SolidBrush fontBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

            if (_gAI == null)
                _gAI = txtIAInfo.myPictureBox.CreateGraphics();

            //g.DrawString(memInfo, newFont, fontBrush, new Point(txtIAInfo.Left + 3 , txtIAInfo.Top + 3));
            _gAI.DrawString(memInfo, newFont, fontBrush, new Point(3, 3));

            int curY = 17;
            int msecondsLine, secondsLine, minutesLine, hoursLine;

            foreach ( APPInfo app in CurrentRunningApps)
            {
                switch (app.Name.ToLower())
                {
                    case "ips":

                        break;

                    case "clock":
                        // TODO LIST:
                        // - Fix background redraw issues with drawing of time lines where the lines don't disappear
                        //   on incrementing past 100%
                        // - Fix primary clock + font look to look like original. 
                        // - Fix colors and offsets for time lines. 
                        // - Double check all offsets. 
                        float penWidth = 1.0f; 
                        int ipenWidth = (int)penWidth / 2;
                        Pen pen = new Pen(Color.FromArgb(40, 0, 240, 240), penWidth);
                        SolidBrush blueBrush = new SolidBrush(Color.FromArgb(255, 3, 75, 101));
                        _gAI.DrawLine(pen, 0, curY, txtIAInfo.Width, curY);
                        curY += 1;
                        _gAI.FillRectangle(blueBrush, 0, curY, txtIAInfo.Width, 12);
                        curY += 12;
                        _gAI.DrawLine(pen, 0, curY, txtIAInfo.Width, curY);
                        curY += 1;
                        DateTime curDT = DateTime.Now;
                        Font clockFont = new Font(txtIAInfo.Font.FontFamily, 15, GraphicsUnit.Pixel);
                        _gAI.DrawString(curDT.ToString(), clockFont, fontBrush, new Point(3, curY + 3));
                        curY += 45;
                        msecondsLine = curY - 8;
                        secondsLine = curY - 6;
                        minutesLine = curY - 4;
                        hoursLine = curY - 2;
                        _gAI.DrawLine(pen, 0, curY, txtIAInfo.Width, curY);
                        int msWid = (int)(((float)txtIAInfo.Width) * ((float)curDT.Millisecond / 1000.0f));
                        // TODO: For less discrete clicking of second, integrate ms into second calculation. 
                        int sWid = (int)(((float)txtIAInfo.Width) * ((float)curDT.Second / 60.0f));
                        int mWid = (int)(((float)txtIAInfo.Width) * ((float)curDT.Minute / 60.0f));
                        int hWid = (int)(((float)txtIAInfo.Width) * ((float)curDT.Hour / 60.0f));
                        pen = new Pen(Color.FromArgb(255, 0, 240 / 4, 240 / 4), penWidth);
                        _gAI.DrawLine(pen, 0, msecondsLine, msWid, msecondsLine);
                        pen = new Pen(Color.FromArgb(255, 0, 240 / 3, 240 / 3), penWidth);
                        _gAI.DrawLine(pen, 0, secondsLine, sWid, secondsLine);
                        pen = new Pen(Color.FromArgb(255, 0, 240 / 2, 240 / 2), penWidth);
                        _gAI.DrawLine(pen, 0, minutesLine, mWid, minutesLine);
                        pen = new Pen(Color.FromArgb(255, 0, 240 / (3/2), 240 / (3 / 2)), penWidth);
                        _gAI.DrawLine(pen, 0, hoursLine, hWid, hoursLine);
                        //pen = new Pen(Color.FromArgb(255, 255, 255, 255), penWidth);
                        //_gAI.FillRectangle(fontBrush, 3, curY + 5, txtIAInfo.Width - 6, 32); // body
                        break;
                    default: 
                        break;
                }
            }
        }

        public void Start( string appName )
        {
            APPInfo thisApp = new APPInfo();
            thisApp.Name = appName;
            curPID += 1;
            thisApp.PID = curPID;

            _currentRunningApps.Add( thisApp );
        }

        public void End(int PID)
        {
            _currentRunningApps.RemoveAll(app => app.PID == PID);
        }

        public void End(string appName)
        {
            _currentRunningApps.RemoveAll(app => app.Name.ToLower() == appName.ToLower());
        }


    }
}
