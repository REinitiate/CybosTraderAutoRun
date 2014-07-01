using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CybosAutoLogin
{
    /*
     * 로그인 ID    : 156
     * 비밀번호     : 157
     * 로그인 버튼  : 203
    */

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            
        }

        private void buttonGetColor_Click(object sender, EventArgs e)
        {
            IntPtr windowHandler = Module.FindWindowByName("CYBOS Starter");
            Win32.Rect rect = Module.WindowPosisionByName(windowHandler);
            //Ut.Log("top:" + rect.Top + " left:" + rect.Left + " bottom:" + rect.Bottom + " right:" + rect.Right);

            // 685, 715
            // 6, 15

            double argb = 0;
            int counter = 0;

            for(int i=6; i<16; i++)
            {
                for (int j = 685; j < 716; j++)
                {
                    int x = rect.Left + j;
                    int y = rect.Top + i;
                    argb = argb + Win32.GetPixelColor(x, y).ToArgb();
                    counter++;
                }
            }

            Ut.Log((argb / Convert.ToDouble(counter)).ToString());

            return;
        }

        private void buttonBtnClick_Click(object sender, EventArgs e)
        {
            IntPtr windowHandler = Module.FindWindowByName("CYBOS Starter");

            try
            {
                Convert.ToInt32(textBoxItemClick.Text);
            }
            catch
            {
                Ut.Log("아이템 번호가 이상합니다.");
            }

            if(textBoxItemClick.Text != "")
                Module.ButtonClick(windowHandler, Convert.ToInt32(textBoxItemClick.Text));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr windowHandler = Module.FindWindowByName("CYBOS Starter");

            try
            {
                Convert.ToInt32(textBoxItemClick.Text);
            }
            catch
            {
                Ut.Log("아이템 번호가 이상합니다.");
            }

            if (textBoxItemClick.Text != "")
                Module.SetTextInEdit(windowHandler, Convert.ToInt32(textBoxItemClick.Text), textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 매크로 동작            
            //CpStart
            //ncStarter
            Automation automation = new Automation();
            automation.Action();
        }

        
    }
}
