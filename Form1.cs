using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    public partial class Form1 : Form
    {
        TableLayoutPanel board = new TableLayoutPanel();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int columns = 6;
            int rows = 6;
            
            this.Width = 800;
            this.Height = 800;
            
            this.board.Name = "board";
            this.board.Location = new System.Drawing.Point(50, 50);
            this.board.Size = new System.Drawing.Size(50*columns+columns, 50*rows+rows);
            Controls.Add(this.board);

            for(; this.board.ColumnCount < columns; this.board.ColumnCount++) {
                this.board.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            }
            for (; this.board.RowCount < rows; this.board.RowCount++)
            {
                this.board.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            }
            Dictionary<int, Panel> boardPanels = new Dictionary<int, Panel>();
            int panelIndex = 1;
            for(int x = 0; x < columns; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    boardPanels.Add(panelIndex, new Panel());
                    this.board.Controls.Add(boardPanels[panelIndex],x,y);
                    panelIndex++;
                }

            }

            this.board.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            // this is the on click function for each panel in the tablelayoutpanel
            Console.WriteLine(this.board.Controls);
            foreach (Control c in this.board.Controls)
            {
                c.MouseClick += new MouseEventHandler(this.Clicked);
            }
        }

        private void Clicked(object sender, MouseEventArgs e)
        {
            Console.WriteLine("column" + this.board.GetColumn((Panel)sender));
            Console.WriteLine("row" + this.board.GetRow((Panel)sender));

        }


    }
}
