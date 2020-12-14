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
    public partial class MainWindow : Form
    {
        private PopupWindow Popup;
        public String playerOne;
        public String playerTwo;
        public int boardSize = 6;
        TableLayoutPanel board = new TableLayoutPanel();
        public int[,] boardState = new int[6, 6];
        int currentPlayer = 1;

        public MainWindow()

        {
            InitializeComponent();
            this.playerOneIcon.BackColor = Color.Black;
            this.playerTwoIcon.BackColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Text = "Reversi";

            int columns = 6;
            int rows = 6;
            
            this.Width = 800;
            this.Height = 800;
            
            this.board.Name = "board";
            this.board.Location = new System.Drawing.Point(200, 200);
            this.board.Size = new System.Drawing.Size(50* boardSize + boardSize, 50* boardSize + boardSize);

            this.board.CellPaint += new TableLayoutCellPaintEventHandler(this.draw);

            for (; this.board.ColumnCount < columns; this.board.ColumnCount++) {
                this.board.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            }
            for (; this.board.RowCount < rows; this.board.RowCount++)
            {
                this.board.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            }
            //this code adds a control to each panel of the board
            Dictionary<int, Panel> boardPanels = new Dictionary<int, Panel>();
            int panelIndex = 1;
            for(int x = 0; x < boardSize; x++)
            {
                for(int y = 0; y < boardSize; y++)
                {
                    boardPanels.Add(panelIndex, new Panel());
                    this.board.Controls.Add(boardPanels[panelIndex],x,y);
                    panelIndex++;
                }

            }

            this.board.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            // this is the on click function for each panel in the tablelayoutpanel, it does this by adding a control to each panel
            Controls.Add(this.board);
            Console.WriteLine(this.board.Controls);
            Controls.Add(this.board);
            foreach (Control c in this.board.Controls)
            {
                c.MouseClick += new MouseEventHandler(this.Clicked);
            }
            
        }

        private void Clicked(object sender, MouseEventArgs mea)
        {
            int column = this.board.GetColumn((Panel)sender);
            int row = this.board.GetRow((Panel)sender);

            boardState[column, row] = currentPlayer;
            if(currentPlayer == 2)
            {
                currentPlayer = 1;
            }
            else
            {
                currentPlayer = 2;
            }

            this.board.Invalidate();
            
        }
        private void draw(object sender, TableLayoutCellPaintEventArgs tlcpea)
        {
          if(boardState[tlcpea.Column,tlcpea.Row] == 0)
            {
               
            }else if (boardState[tlcpea.Column, tlcpea.Row] == 1)
            {
                tlcpea.Graphics.FillEllipse(Brushes.Red, tlcpea.CellBounds);
            }
            else
            {
                tlcpea.Graphics.FillEllipse(Brushes.Blue, tlcpea.CellBounds);

            }
        }
        //Buttons
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            using (PopupWindow newGame = new PopupWindow(true))
            {
                var result = newGame.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.playerOne = newGame.namePlayerOne;
                    this.playerTwo = newGame.namePlayerTwo;
                    this.boardSize = newGame.gameBoardSize;
                    Console.WriteLine("New \"" + this.playerOne + "\"v\"" + this.playerTwo + "\" game");
                    this.NewGame();
                }
                else
                {
                    Console.WriteLine("No new game");
                }
            }
        }

        private void BotButton_Click(object sender, EventArgs e)
        {
            using (PopupWindow newGame = new PopupWindow(false))
            {
                var result = newGame.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.playerOne = newGame.namePlayerOne;
                    this.playerTwo = newGame.namePlayerTwo;
                    this.boardSize = newGame.gameBoardSize;
                    Console.WriteLine("New \"" + this.playerOne + "\"v\"" + this.playerTwo + "\" game");
                    this.NewGame();
                }
                else
                {
                    Console.WriteLine("No new game");
                }
            }
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
        }

        private void NewGame()
        {
            this.playerOneIconName.Text = this.playerOne;
            this.playerTwoIconName.Text = this.playerTwo;
        }

    }


    public partial class PopupWindow : Form
    {
        public bool isPerson;
        private TextBox playerOneTextBox;
        private TextBox playerTwoTextBox;
        private TextBox gameBoardTextBox;

        public String namePlayerOne;
        public String namePlayerTwo;
        public int gameBoardSize;

        public PopupWindow(bool isPerson)
        {
            this.isPerson = isPerson;
            this.ControlBox = false;
            this.Size = new Size(250, 300);
            this.Text = "Stel game in";

            //Player one textbox
            this.playerOneTextBox = new TextBox();
            this.playerOneTextBox.Size = new Size(200, 100);
            this.playerOneTextBox.Location = new Point(20, 40);
            this.playerOneTextBox.Name = "playerOne";
            this.Controls.Add(this.playerOneTextBox);

            Label playerOneLabel = new Label();
            playerOneLabel.Text = "Naam speler 1";
            playerOneLabel.Location = new Point(20, 20);
            playerOneLabel.Name = "labelPlayerOne";
            this.Controls.Add(playerOneLabel);

            //Player two textbox
            if (isPerson)
            {
                Label playerTwoLabel = new Label();
                playerTwoLabel.Text = "Naam speler 2";
                playerTwoLabel.Location = new Point(20, 75);
                playerTwoLabel.Name = "labelPlayerTwo";
                this.Controls.Add(playerTwoLabel);

                this.playerTwoTextBox = new TextBox();
                this.playerTwoTextBox.Size = new Size(200, 100);
                this.playerTwoTextBox.Location = new Point(20, 100);
                this.playerTwoTextBox.Name = "playerTwo";
                this.Controls.Add(this.playerTwoTextBox);
            }

            Label boardSizeLabel = new Label();
            boardSizeLabel.Text = "Afmeting van het spelbord";
            boardSizeLabel.Location = new Point(20, 130);
            boardSizeLabel.Size = new Size(200, 20);
            boardSizeLabel.Name = "labelBoardSize";
            this.Controls.Add(boardSizeLabel);

            this.gameBoardTextBox = new TextBox();
            this.gameBoardTextBox.Size = new Size(50, 100);
            this.gameBoardTextBox.Location = new Point(20, 150);
            this.gameBoardTextBox.Name = "gameBoardTextBox";
            this.Controls.Add(this.gameBoardTextBox);



            //Cancel button
            Button cancelButton = new Button();
            cancelButton.Size = new Size(80, 50);
            cancelButton.Location = new Point(20, 180);
            cancelButton.Name = "cancelButton";
            cancelButton.Text = "Annuleer";
            cancelButton.Click += this.cancelClick;
            this.CancelButton = cancelButton;

            this.Controls.Add(cancelButton);

            //Submit button
            Button submitButton = new Button();
            submitButton.Size = new Size(80, 50);
            submitButton.Location = new Point(140, 180);
            submitButton.Name = "submitButton";
            submitButton.Text = "Start game";
            submitButton.Click += this.submitClick;
            this.AcceptButton = submitButton;

            this.Controls.Add(submitButton);



        }

        private void submitClick(object o, EventArgs e)
        {
            bool allOk = true;
            string playerOne = this.playerOneTextBox.Text;
            string playerTwo = (this.isPerson ? this.playerTwoTextBox.Text : "Bot Frank");
            int BoardSize;
            if (int.TryParse(this.gameBoardTextBox.Text, out BoardSize))
            {
                //parsing successful 

                if (BoardSize < 3 && BoardSize > 10)
                {
                    //Board heeft juiste afmetingen 
                    allOk = false;
                }
            }
            else
            {
                //parsing failed.
                allOk = false;
            }

            if (String.IsNullOrEmpty(this.playerOneTextBox.Text))
            {
                allOk = false;
            }

            if (this.isPerson && String.IsNullOrEmpty(this.playerOneTextBox.Text))
            {
                allOk = false;
            }


            if (allOk)
            {
                this.namePlayerOne = playerOne;
                this.namePlayerTwo = playerTwo;
                this.gameBoardSize = BoardSize;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                Console.WriteLine("Fix je inputs");
            }


        }

        private void cancelClick(object o, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }


}
