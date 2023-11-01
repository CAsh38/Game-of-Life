using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Game_of_life
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //Iniatilizing Components
            InitializeComponent();
            //Initializing the game
            this.Game = new Game();
            /*this.Game.Create_Glider(0, 0);*/
            this.Game.Populate_with_cells(250);
            //Initializing the second thread
            this.Run_Main_Loop();
        }
        private void Run_Main_Loop()
        {
            this.BackgroundWorker1.RunWorkerAsync();
            this.BackgroundWorker1.DoWork += new DoWorkEventHandler(this.BackgroundWorker_Run);
            this.Panel.Paint += new PaintEventHandler(this.Draw_Panel);
        }
        private void BackgroundWorker_Run(object sender, DoWorkEventArgs e)
        {
            while (!IsDisposed)
            {
                Thread.Sleep(500);

                //here we are getting the cells alive after the laws of the simulation 
                this.Game.Update_Live_Cells();

                /*this.Game.Populate_with_cells(100);*/
                this.Invoke((MethodInvoker)(() => this.Controls.Remove(this.Panel)));
                this.Invoke((MethodInvoker)(() => this.Controls.Add(this.Panel)));
            }
            
        }
        public void Draw_Panel(object sender, PaintEventArgs e)
        {
            //Cells can take positions from 0 to 510 on Y and from 0 to 740
            foreach (Cell cell in this.Game.Live_Cells)
            {
                e.Graphics.FillRectangle(this.Game.Brush, cell.X * 10, cell.Y * 10, cell.Edge, cell.Edge);
            }
            this.Invoke((MethodInvoker)(() => this.Controls.Add(this.Panel)));
        }
    }
}
/*this.Invoke((MethodInvoker)(() => this.Panel.Paint += new PaintEventHandler(this.Draw_Panel)));*/
/*InvokePaintBackground(this.Controls.ControlCollection,this.Panel);*/
/*this.Panel.Paint += delegate (object sender1, PaintEventArgs e1) { this.Draw_Panel(sender1, e1); };*/
/*this.Invalidate();*/
/*this.Panel.Paint += new PaintEventHandler(this.Draw_Panel);*/