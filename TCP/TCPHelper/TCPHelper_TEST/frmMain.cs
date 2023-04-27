using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using static TCPHelperLibrary.Scanner;
using static WAPHelperLibrary.WIFIAccessor;
using System.Runtime.InteropServices;
using WAPHelperLibrary;
using Windows.Devices.WiFi;
using Windows.Foundation.Metadata;
using ThisIsAHackerServiceLibrary;
using TCPHelperLibrary;
using Windows.UI.Xaml.Shapes;
using Windows.System;
using System.Drawing.Drawing2D;
using TCPHelper.System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using TCPHelper;

namespace NETHelper
{
    public partial class frmMain : Form
    {
        private const int EM_GETRECT = 0xB2;
        private int _numLinesForCommandInputWindow = 0;
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref RECT lParam);
        
        private ThisIsAHackerServiceLibrary.CommandProcessor cp = new CommandProcessor();
        private const string PROMPTCHARS = "@> ";
        private int _PromptLength = -1;

        private string _CurLocation;
        private string _HomeLocation;

        private List<string> commandHistory = new List<string>();
        private int _CHIndex = 0;

        private bool _bDrawProbed = true;
        private string _sProbeResults = "";

        private InternalCommandProcessor _icp = new InternalCommandProcessor();
        private InternalAppHandler _iah = new InternalAppHandler();
        private PictureBox _appInfo = new PictureBox();

        public frmMain()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();

            _appInfo.BackColor = Color.Transparent;
            _appInfo.Left = 0; _appInfo.Top = 0;
            _appInfo.Height = txtAppData.Height; _appInfo.Width = txtAppData.Width;

            txtAppData.Controls.Add(_appInfo);

        }


        /*private void button1_Click(object sender, EventArgs e)
        {
            TCPHelperLibrary.Scanner scanner = new TCPHelperLibrary.Scanner();
            List<TcpConnectionInformation> connections = scanner.GetActiveConnections();

            lvMain.Items.Clear();
            foreach (TcpConnectionInformation ci in connections)
            {
                ListViewItem lvi = lvMain.Items.Add(ci.LocalEndPoint.ToString());
                lvi.SubItems.Add(""); // Local Port
                lvi.SubItems.Add(ci.RemoteEndPoint.ToString());
                lvi.SubItems.Add(""); // Remote Port
                lvi.SubItems.Add(ci.State.ToString());
            }
        } // IN PROCESSS

        private void button2_Click(object sender, EventArgs e)
        {
            TCPHelperLibrary.Scanner scanner = new TCPHelperLibrary.Scanner();
            List<IPEndPoint> connections = scanner.GetActiveListeners();

            lvMain.Items.Clear();
            foreach (IPEndPoint ci in connections)
            {
                ListViewItem lvi = lvMain.Items.Add(ci.Address.ToString());
                lvi.SubItems.Add(ci.Port.ToString()); // Local Port
                lvi.SubItems.Add("");
                lvi.SubItems.Add(""); // Remote Port
                lvi.SubItems.Add("LISTENING");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TCPHelperLibrary.Scanner scanner = new TCPHelperLibrary.Scanner();
            lvMain.Items.Clear();
            List<SupportedPort> ports = scanner.GetRemotePorts(textBox1.Text);

            if (ports.Count > 0)
            {

                foreach (SupportedPort sp in ports)
                {
                    ListViewItem lvi = lvMain.Items.Add(textBox1.Text);
                    lvi.SubItems.Add(sp.Port.ToString()); // Local Port
                    lvi.SubItems.Add(sp.PortName);
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("");
                }
            }
            else
            {
                string s = string.Format("No Open Ports on Target IP {0}", textBox1.Text);
                ListViewItem lvi = lvMain.Items.Add(s);
            }
        }

        private void lvMain_DoubleClick(object sender, EventArgs e)
        {
            // Should be one and only one double clicked on
            foreach (ListViewItem lvi in lvMain.SelectedItems)
            {
                string[] sRemoteConnInfo = lvi.SubItems[2].Text.Split(':');

                textBox1.Text = sRemoteConnInfo[0];
            }

        }

        private async void button4_Click(object sender, EventArgs e)
        {
            WIFIAccessor wifi = new WIFIAccessor();
            lvMain.Items.Clear();
            var wfnws = await wifi.GetAccessPointsAsync();

            if (wfnws.Count > 0)
            {

                foreach (WiFiAvailableNetwork wan in wfnws)
                {
                    ListViewItem lvi = lvMain.Items.Add(wan.Ssid);
                    lvi.SubItems.Add(wan.NetworkKind.ToString()); // Local Port
                    lvi.SubItems.Add(wan.NetworkRssiInDecibelMilliwatts.ToString());
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add("");
                }
            }
            else
            {
                string s = string.Format("No Wireless Access Points Visible");
                ListViewItem lvi = lvMain.Items.Add(s);
            }
        } */

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Left = -1920;
            this.Top = 0;

            var rect = new RECT();
            SendMessage(this.txtBoxHistory.Handle, EM_GETRECT, IntPtr.Zero, ref rect);
            _numLinesForCommandInputWindow = ( rect.Bottom - rect.Top - this.txtBoxHistory.Margin.Bottom - this.txtBoxHistory.Margin.Top) / 
                                             ( this.txtBoxHistory.Font.Height ) - 1;
            string sInitializeData = ( new string('\n', _numLinesForCommandInputWindow ) ).Replace("\n", "\r\n");

            this.txtBoxHistory.GotFocus += txtBoxHistory_GotFocus;
            this.txtAppData.GotFocus += txtMemory_GotFocus;

            IPConfig.Initialize();
            resetCommandText();

            HideCaret(txtAppData.Handle);

            this.SetStyle( ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor |
                            ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | 
                            ControlStyles.DoubleBuffer, true);

            drawFrames();
        }

        private void drawFrames()
        {
            SolidBrush fontBrush = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
            SolidBrush fontGreenBrush = new SolidBrush(Color.FromArgb(255, 50, 255, 50));
            SolidBrush fontRedBrush = new SolidBrush(Color.FromArgb(255, 255, 100, 100));

            Graphics g = this.CreateGraphics();

            // Window Title
//            this.lblTopHeader.BackColor = Color.FromArgb(11, 57, 35);
            LinearGradientBrush linGrBrush = new LinearGradientBrush(
                  new Point(0, 3),
                  new Point(1920, 22),
                  Color.FromArgb(255, 11, 57, 35),
                  Color.FromArgb(255, 18, 79, 72));

            Pen pen = new Pen(linGrBrush);

            // Border around Netmap window
            float penWidth = 2.0f; int ipenWidth = (int)penWidth / 2;
            pen = new Pen(Color.FromArgb(40, 0, 240, 240), penWidth);
            g.DrawLine(pen, lblNetMap.Left - ipenWidth, lblNetMap.Top - ipenWidth,
                            lblNetMap.Left - ipenWidth, txtNetMap.Bottom + ipenWidth);
            g.DrawLine(pen, lblNetMap.Left - ipenWidth, lblNetMap.Top - ipenWidth,
                            lblNetMap.Right + ipenWidth, lblNetMap.Top - ipenWidth);
            g.DrawLine(pen, lblNetMap.Right + ipenWidth, txtNetMap.Bottom + ipenWidth,
                            lblNetMap.Right + ipenWidth, lblNetMap.Top - ipenWidth);
            g.DrawLine(pen, lblNetMap.Right + ipenWidth, txtNetMap.Bottom + ipenWidth,
                            lblNetMap.Left - ipenWidth, txtNetMap.Bottom + ipenWidth);

            // Border around Command Window
            penWidth = 2.0f; ipenWidth = (int)penWidth / 2;
            pen = new Pen(Color.FromArgb(40, 0, 240, 240), penWidth);
            g.DrawLine(pen, lblTerminalHeader.Left - ipenWidth, lblTerminalHeader.Top - ipenWidth,
                            lblTerminalHeader.Left - ipenWidth, txtBoxCommand.Bottom + ipenWidth);
            g.DrawLine(pen, lblTerminalHeader.Left - ipenWidth, lblTerminalHeader.Top - ipenWidth,
                            lblTerminalHeader.Right + ipenWidth, lblTerminalHeader.Top - ipenWidth);
            g.DrawLine(pen, lblTerminalHeader.Right + ipenWidth, txtBoxCommand.Bottom + ipenWidth,
                            lblTerminalHeader.Right + ipenWidth, lblTerminalHeader.Top - ipenWidth);
            g.DrawLine(pen, lblTerminalHeader.Right + ipenWidth, txtBoxCommand.Bottom + ipenWidth,
                            lblTerminalHeader.Left - ipenWidth, txtBoxCommand.Bottom + ipenWidth);

            // Border around RAM Window
            penWidth = 2.0f; ipenWidth = (int)penWidth / 2;
            pen = new Pen(Color.FromArgb(40, 0, 240, 240), penWidth);
            g.DrawLine(pen, lblRAM.Left - ipenWidth, lblRAM.Top - ipenWidth,
                            lblRAM.Left - ipenWidth, txtDisplay.Bottom + ipenWidth);
            g.DrawLine(pen, lblRAM.Left - ipenWidth, lblRAM.Top - ipenWidth,
                            lblRAM.Right + ipenWidth, lblRAM.Top - ipenWidth);
            g.DrawLine(pen, lblRAM.Right + ipenWidth, txtDisplay.Bottom + ipenWidth,
                            lblRAM.Right + ipenWidth, lblRAM.Top - ipenWidth);
            g.DrawLine(pen, lblRAM.Right + ipenWidth, txtDisplay.Bottom + ipenWidth,
                            lblRAM.Left - ipenWidth, txtDisplay.Bottom + ipenWidth);

            // Border around DISPLAY Window
            penWidth = 2.0f; ipenWidth = (int)penWidth / 2;
            pen = new Pen(Color.FromArgb(40, 0, 240, 240), penWidth);
            g.DrawLine(pen, lblDisplay.Left - ipenWidth, lblDisplay.Top - ipenWidth,
                            lblDisplay.Left - ipenWidth, txtDisplay.Bottom + ipenWidth);
            g.DrawLine(pen, lblDisplay.Left - ipenWidth, lblDisplay.Top - ipenWidth,
                            lblDisplay.Right + ipenWidth, lblDisplay.Top - ipenWidth);
            g.DrawLine(pen, lblDisplay.Right + ipenWidth, txtDisplay.Bottom + ipenWidth,
                            lblDisplay.Right + ipenWidth, lblDisplay.Top - ipenWidth);
            g.DrawLine(pen, lblDisplay.Right + ipenWidth, txtDisplay.Bottom + ipenWidth,
                            lblDisplay.Left - ipenWidth, txtDisplay.Bottom + ipenWidth);

            // Current IP Information (Upper Right hand corner)
            if( IPConfig.CurrentConnectedIP != "")
                _CurLocation = IPConfig.CurrentConnectedHostName + "@" + IPConfig.CurrentConnectedIP;
            else
                _CurLocation = IPConfig.getBaseHostName() + "@" + IPConfig.getBaseIP();
            

            Font newFont = new Font( this.lblTerminalHeader.Font.FontFamily, 11, GraphicsUnit.Pixel);
            string locFullText = "Location: " + _CurLocation;
            TextFormatFlags flags = TextFormatFlags.Right;
            Size proposedSize = new Size(int.MaxValue, int.MaxValue);

            // Gradient fill title bar as close to the text on the title bar to avoid flickering. 
            g.FillRectangle(linGrBrush, 0, 0, 1920, 25);

            Size textSize = TextRenderer.MeasureText(locFullText, newFont, proposedSize, flags);
            if( IPConfig.IsAdmin )
                g.DrawString(locFullText, newFont, fontGreenBrush, new Point(1920 - 37 - textSize.Width, 3));
            else
                g.DrawString(locFullText, newFont, fontRedBrush, new Point(1920 - 37 - textSize.Width, 3));
            _HomeLocation = "Home IP: " + IPConfig.getBaseIP();
            string cur = (new string(' ', (locFullText.Length - _HomeLocation.Length))) +
                        _HomeLocation;
            g.DrawString(cur, newFont, fontBrush, new Point(1920 - 37 - textSize.Width, 3 + textSize.Height));

            // Mail Icon. 
            penWidth = 2.0f; ipenWidth = (int)penWidth / 2;
            pen = new Pen(Color.FromArgb(180, 255, 255, 255), penWidth);
            g.DrawLine(pen, 1890, 4, 1916, 4);
            g.DrawLine(pen, 1890, 4, 1890, 17);
            g.DrawLine(pen, 1916, 4, 1916, 17);
            g.DrawLine(pen, 1899, 17, 1916, 17);
            g.DrawLine(pen, 1890, 17, 1895, 17);
            g.DrawLine(pen, 1895, 17, 1895, 22);
            g.DrawLine(pen, 1899, 17, 1895, 22);

            g.DrawLine(pen, 1895, 8, 1911, 8);
            g.DrawLine(pen, 1895, 12, 1911, 12);

            if(_bDrawProbed && picDisplay.Visible )
            {
                SolidBrush redBrush = new SolidBrush(Color.FromArgb(255, 84, 22, 22));
                SolidBrush greenBrush = new SolidBrush(Color.FromArgb(255, 50, 150, 113));
                Graphics gDisplay =  picDisplay.CreateGraphics();

                int offsetSpacingY = 45, curYOffset = 0;

                cur = "Open Ports";
                Font mainFont = new Font(this.lblTerminalHeader.Font.FontFamily, 28, GraphicsUnit.Pixel);
                gDisplay.DrawString(cur, mainFont, fontBrush, new Point(2, 5));

                cur = string.Format("{0} @{1}", IPConfig.CurrentConnectedHostName, IPConfig.CurrentConnectedIP);
                newFont = new Font(this.lblTerminalHeader.Font.FontFamily,  14, FontStyle.Regular, GraphicsUnit.Pixel);
                gDisplay.DrawString(cur, newFont, fontBrush, new Point(2, 45));

                cur = string.Format("Open Ports Required for Crack: 1", IPConfig.CurrentConnectedHostName, IPConfig.CurrentConnectedIP);
                gDisplay.DrawString(cur, newFont, greenBrush, new Point(2, 80));

                List<string> lines = _sProbeResults.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                fontBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));

                const int PORTPANELWIDTH = 420;
                foreach ( string line in lines ) 
                { 
                    if (line.Contains("Port#"))
                    {
                        int portStart = line.IndexOf("Port#:") + "Port#:".Length + 1;
                        int portEnd = line.IndexOf('-') - portStart;
                        int portNumber = System.Convert.ToInt32(line.Substring(portStart, portEnd));
                        cur = string.Format( "Port#: {0}", portNumber );
                        int oY = 120 + (curYOffset * offsetSpacingY);
                        gDisplay.FillRectangle(redBrush, 5, oY, PORTPANELWIDTH, 41);
                        gDisplay.DrawString(cur, mainFont, fontBrush, new Point(5, oY + 8));
                        float oX = gDisplay.MeasureString(cur, mainFont).Width;
                        cur = line.Substring(portEnd + portStart + 1, line.Length - portStart - portEnd - 1);
                        cur = string.Format("- {0}", expandPortDescription( cur.Trim() ));
                        gDisplay.DrawString(cur, newFont, fontBrush, new Point(5 + System.Convert.ToInt32(oX), oY + 8 ) );
                        curYOffset += 1;

                        // the lock
                        penWidth = 5.0f; ipenWidth = (int)penWidth / 2;
                        pen = new Pen(Color.FromArgb(255, 255, 255, 255), penWidth);
                        gDisplay.FillRectangle(fontBrush, PORTPANELWIDTH - 32 - 5, oY + 15, 32, 21); // body

                        gDisplay.FillRectangle(fontBrush, PORTPANELWIDTH - 31, oY + 4, 22, 4 ); // LOCK TOP
                        gDisplay.FillRectangle(fontBrush, PORTPANELWIDTH - 31, oY + 4, 6, 11); // LOCK TOP
                        gDisplay.FillRectangle(fontBrush, PORTPANELWIDTH - 15, oY + 4, 6, 11); // LOCK TOP
                    }
                }
            }
            _iah.DrawFrame(g, txtAppData);
        }

        private string expandPortDescription(string portName)
        {
            string _return = "";
            switch( portName.ToLower()) 
            {
                case "ssl":
                    _return = "HTTPS (SSL)";
                    break;
                case "ftp":
                    _return = "FTP Server";
                    break;
                case "http":
                    _return = "HTTP WebServer";
                    break;
                default:
                    _return = portName;
                    break;
            }
            return _return;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            int noControlShift = ~( (int) ( Keys.Control | Keys.Shift ) );
            Keys cur = (Keys)(noControlShift & (int)keyData);
            if (
                    (cur  == Keys.Back) || 
                    (cur == Keys.Left) || 
                    (cur == Keys.Home)
                )
            {
                if (_PromptLength >= txtBoxCommand.SelectionStart)
                    return true;
            }

            if ((cur == Keys.Up || cur == Keys.Down ) && commandHistory.Count > 0)
            {
                _CHIndex += (cur == Keys.Up)?-1:1;
                
                if (_CHIndex < 0 || (commandHistory.Count <= _CHIndex))
                {
                    System.Media.SystemSounds.Beep.Play();
                    _CHIndex += (cur == Keys.Up) ? 1 : -1; ;
                }

                resetCommandText();
                txtBoxCommand.Text += commandHistory[_CHIndex];
                this.txtBoxCommand.SelectionStart = txtBoxCommand.Text.Length;
                this.txtBoxCommand.SelectionLength = 0;
                this.txtBoxCommand.Focus();
            }

            if( cur == Keys.Tab )
            {
                string sIP = "";
                string command = txtBoxCommand.Text;
                sIP = command.Substring(0, command.LastIndexOf(PROMPTCHARS) + PROMPTCHARS.Length);
                command = command.Substring(command.LastIndexOf(PROMPTCHARS) + PROMPTCHARS.Length,
                                             command.Length - command.LastIndexOf(PROMPTCHARS) - PROMPTCHARS.Length);

                

                var exes = _icp.Commands.Where(x => x.StartsWith(command, StringComparison.OrdinalIgnoreCase));
                if( exes.Any() )
                {
                    addHistoryLines(txtBoxCommand.Text);
                    foreach (string s in exes)
                    {
                        addHistoryLines(s);
                    }
                }
            }
                return base.ProcessCmdKey(ref msg, keyData);
        }

        private async void txtBoxCommand_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter )
            {
                string sIP = "";
                string command = txtBoxCommand.Text;
                sIP = command.Substring(0, command.LastIndexOf(PROMPTCHARS) + PROMPTCHARS.Length);
                command = command.Substring( command.LastIndexOf(PROMPTCHARS) + PROMPTCHARS.Length, 
                                             command.Length - command.LastIndexOf(PROMPTCHARS) - PROMPTCHARS.Length );
                // Check for quit or exit command before sending it to the command processor. 
                if (command.Trim().ToLower().Equals("quit") || command.Trim().ToLower().Equals("exit"))
                {
                    ExitApplication();
                    return;
                }
                else if (_icp.IsInternalCommand(command) )
                {
                    commandHistory.Add(command); _CHIndex++;
                    _iah.Start(command);
                    addHistoryLines(txtBoxCommand.Text + string.Format( "\r\n{0} - ACK\r\n", command ));

                    e.Handled = true;
                }
                else
                {
                    // Do the logical aspect of command processing. 
                    var _result = await cp.ProcessCommand(command);
                    commandHistory.Add(command ); _CHIndex++;

                    // Do the UI displaying of command processing and displaying the results. 
                    uiCommandHandler(command, _result);

                    // Add history
                    if ( _result.Length > 0)
                        addHistoryLines(txtBoxCommand.Text + "\r\n" + _result);
                    else
                        addHistoryLines(txtBoxCommand.Text);

                    e.Handled = true;

                }
                txtBoxCommand.Text = "";
                resetCommandText();
            }
        }

        private void uiCommandHandler( string command, string result )
        {
            string commandBase = command.Split(' ')[0].ToLower();
            switch (commandBase)
            {
                case "cls":
                    txtBoxHistory.Text = "";
                    break;
                case "dc":
                case "disconnect":
                    webDisplay.Visible = false;
                    break;
                case "connect":
                    if (IPConfig.CurrentConnectedHostName != "" &&
                        IPConfig.CurrentConnectedHostName != IPConfig.getBaseHostName() )
                    {
                        webDisplay.Navigate(IPConfig.CurrentConnectedHostName);
                        webDisplay.Visible = true;
                    }
                    else
                    {
                        webDisplay.Visible = false;
                    }
                    break;
                case "probe":
                    // Toggle Display stuff
                    webDisplay.Visible = txtDisplay.Visible = !(picDisplay.Visible = true);
                    _bDrawProbed = true;
                    _sProbeResults = result;
                    break;
                case "scan":
                    txtNetMap.Text = "TODO: DO post process for scan map";
                    break;
            }
            return;
        }
        private void addHistoryLines(string result)
        {
            List<string> lines = result.Split('\n').ToList();
            List<string> lines2 = txtBoxHistory.Text.Split('\n').ToList();

            txtBoxHistory.Text = "";
            if (lines2.Count <= 1)
            {
                for (int x = 0; x < _numLinesForCommandInputWindow- lines.Count; x++)
                    txtBoxHistory.Text += "\r\n";
            }
            else
            {
                for( int x = lines.Count; x <= _numLinesForCommandInputWindow; x++)
                    txtBoxHistory.Text += lines2[x] + "\r\n";
            }

            foreach ( string line in lines )
                txtBoxHistory.Text += line + "\r\n";

        }

        private void resetCommandText()
        {
            txtBoxCommand.Text = string.Format("{0}{1}{2}", IPConfig.CurrentConnectedIP, cp.getCurrentDirectory, PROMPTCHARS);

            this.txtBoxCommand.SelectionStart = this.txtBoxCommand.TextLength;
            _PromptLength = this.txtBoxCommand.TextLength;
            this.txtBoxCommand.SelectionLength = 0;
            this.txtBoxCommand.Focus();
        }

        private void txtBoxHistory_GotFocus(object sender, EventArgs e)
        {
            HideCaret(txtBoxHistory.Handle);
        }
        private void txtMemory_GotFocus(object sender, EventArgs e)
        {
            HideCaret(txtBoxHistory.Handle);
        }
        

        private void txtBoxHistory_Enter(object sender, EventArgs e)
        {
            txtBoxHistory.Enabled = false;
            txtBoxHistory.Enabled = true;
        }

        private void ExitApplication()
        {
            Application.Exit();
        }

        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if( e.KeyChar == (char) Keys.Escape )
            {
                ExitApplication();
            }

        }

        private bool m_bCommandMouseDown;
        private int m_endCharPosition;
        private void txtBoxCommand_MouseDown(object sender, MouseEventArgs e)
        {
            m_bCommandMouseDown = true;
            m_endCharPosition = this.txtBoxCommand.SelectionStart + this.txtBoxCommand.SelectionLength;
        }

        private void txtBoxCommand_MouseUp(object sender, MouseEventArgs e)
        {
            m_bCommandMouseDown = false;
            if (this.txtBoxCommand.SelectionStart < _PromptLength ||
                (this.txtBoxCommand.SelectionStart + this.txtBoxCommand.SelectionLength) < _PromptLength)
            {
                this.txtBoxCommand.SelectionStart = _PromptLength;
                this.txtBoxCommand.SelectionLength = 0;
                this.txtBoxCommand.Focus();
            }
        }

        private void txtBoxCommand_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_bCommandMouseDown)
            {
                if (this.txtBoxCommand.SelectionStart < _PromptLength)
                {
                    this.txtBoxCommand.SelectionStart = _PromptLength;
                    this.txtBoxCommand.SelectionLength = m_endCharPosition - _PromptLength;
                }
            }
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            drawFrames();
        }

        private void txtBoxHistory_MouseDown(object sender, MouseEventArgs e)
        {
            this.txtBoxCommand.SelectionStart = this.txtBoxCommand.TextLength;
            this.txtBoxCommand.SelectionLength = 0;
            this.txtBoxCommand.Focus();
        }

        private void picX_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void txtMemory_Enter(object sender, EventArgs e)
        {
            txtAppData.Enabled = false;
            txtAppData.Enabled = true;
        }

        private void txtBoxHistory_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
