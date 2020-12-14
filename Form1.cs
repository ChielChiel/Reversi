﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        IDictionary<int, Color> pieceColor = new Dictionary<int, Color> { { 1, Color.Black }, { 2, Color.White } };

        public MainWindow()

        {
            InitializeComponent();
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, 50, 50);

            this.playerOneIcon.BackColor = pieceColor[1];
            this.playerOneIcon.Region = new Region(path);
            this.playerOneIconName.ForeColor = Color.Red;
            
            this.playerTwoIcon.BackColor = pieceColor[2];
            this.playerTwoIcon.Region = this.playerOneIcon.Region;
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
            this.board.Size = new System.Drawing.Size(50 * boardSize + boardSize, 50 * boardSize + boardSize);
            this.board.BackColor = Color.DarkGreen;

            this.board.CellPaint += new TableLayoutCellPaintEventHandler(this.draw);

            for (; this.board.ColumnCount < columns; this.board.ColumnCount++)
            {
                this.board.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            }
            for (; this.board.RowCount < rows; this.board.RowCount++)
            {
                this.board.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            }
            //this code adds a control to each panel of the board
            Dictionary<int, Panel> boardPanels = new Dictionary<int, Panel>();
            int panelIndex = 1;
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    boardPanels.Add(panelIndex, new Panel());
                    this.board.Controls.Add(boardPanels[panelIndex], x, y);
                    panelIndex++;
                }

            }

            //Setup first 4 pieces in the middle of the board:
            boardState[columns / 2 - 1, rows / 2 - 1] = 1;
            boardState[columns / 2 - 1, rows / 2] = 2;
            boardState[columns / 2, rows / 2] = 1;
            boardState[columns / 2, rows / 2 - 1] = 2;


            this.board.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            // this is the on click function for each panel in the tablelayoutpanel, it does this by adding a control to each panel
            Controls.Add(this.board);
            foreach (Control c in this.board.Controls)
            {
                c.MouseClick += new MouseEventHandler(this.Clicked);
            }

        }

        private void Clicked(object sender, MouseEventArgs mea)
        {
            int x = this.board.GetColumn((Panel)sender);
            int y = this.board.GetRow((Panel)sender);
            Console.WriteLine(x + "," + y);

            //Get here the valid moves/flips for the current coordinates.
            placingCoord[] flips = getValidMoves(x, y, currentPlayer);
            Console.WriteLine("def flips: " + flips.Length);
            if (flips.Length != 0)
            {
                boardState[x, y] = currentPlayer;
                foreach (placingCoord flip in flips)
                {
                    boardState[flip.X, flip.Y] = currentPlayer;
                }
                this.currentPlayer = (this.currentPlayer == 1 ? 2 : 1);

            }
            else 
            {
                //There are no pieces flippable so the current player has to do another turn
                //TODO: Add a warning message
            }
            if(this.currentPlayer == 1)
            {
                this.playerOneIconName.ForeColor = Color.Red;
                this.playerTwoIconName.ForeColor = Color.Black;
            }
            else
            {
                this.playerOneIconName.ForeColor = Color.Black;
                this.playerTwoIconName.ForeColor = Color.Red;

            }
            this.board.Invalidate();

        }

        private placingCoord[] getValidMoves(int column, int row, int currentPlayer)
        {
            int[] places = new int[] { -1, 0, 1 };

            List<placingCoord> flipsToReturn = new List<placingCoord>();

            if (boardState[column, row] != 0)
            {
                //This means that the place is already occupied by one of the player's pieces.
                return flipsToReturn.ToArray();
            }

            //Loop through every piece surrounding the clicked one.
            for (int dx = 0; dx < places.Length; dx++) 
            {
                for (int dy = 0; dy < places.Length; dy++) 
                {
                    try
                    {
                        //Console.WriteLine("x: " + (column + places[dx]) + ", y: " + (row + places[dy]) + "; " + boardState[column + places[dx], row + places[dy]]);
                        if (boardState[column + places[dx], row + places[dy]] != currentPlayer && boardState[column + places[dx], row + places[dy]] != 0)
                        {
                            //Console.WriteLine("x: " +  column + ", y: " + row + "; is a possible move");
                            //Console.WriteLine("x: " + (column + places[dx]) + ", y: " + (row + places[dy]) + "; could be closed in");
                            
                            //Check here whether the possible moves actually close-in pieces of the opponent.
                            List<placingCoord> tempFlipPieces = new List<placingCoord>();

                            try
                            {
                                for (int i = 1; i < this.boardSize; i++) 
                                {
                                    int tempX = i * places[dx] + column;
                                    int tempY = i * places[dy] + row;
                                    Console.WriteLine("searching x: " + tempX + ", y: " + tempY + "; for a possible move");
                                    if (boardState[tempX, tempY] == currentPlayer)
                                    {
                                        //stop searching
                                        i = this.boardSize;
                                    }
                                    else if (boardState[tempX, tempY] != currentPlayer && boardState[tempX, tempY] != 0)
                                    {
                                        tempFlipPieces.Add(new placingCoord(tempX, tempY, currentPlayer));

                                    }
                                    else
                                    {
                                        tempFlipPieces.Clear();
                                    }
                                }
                                flipsToReturn.AddRange(tempFlipPieces);
                                //Console.WriteLine("flips to return?: " + flipsToReturn.Count);

                            }
                            catch (IndexOutOfRangeException)
                            {
                                //Out of range. But nothing has to be done
                            }

                        }
                    }
                    catch (IndexOutOfRangeException) {
                        //Out of range. But nothing has to be done
                    }


                }
            }
            //Return all the pieces that will be flipped.
            return flipsToReturn.ToArray();
        }

        //Struct holding the x and y coordinates of a (possible) flip as well as the current player.
        public struct placingCoord
        {
            public placingCoord(int x, int y, int player)
            {
                X = x;
                Y = y;
                currentPlayer = player;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int currentPlayer { get; set; }
           //Custom ToString method for easy debugging.
            public override string ToString() => $"({X}, {Y}) now for: player{currentPlayer}";
        }

        private void draw(object sender, TableLayoutCellPaintEventArgs tlcpea)
        {
            Brush brushColor = null;

            switch (boardState[tlcpea.Column, tlcpea.Row]) {
                case 1:
                    brushColor = new SolidBrush(pieceColor[1]);
                    break;
                case 2:
                    brushColor = new SolidBrush(pieceColor[2]);
                    break;
                default:
                    break;
            }

            if (brushColor != null)
            {
                tlcpea.Graphics.FillEllipse(brushColor, tlcpea.CellBounds);
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
            //Simply get all the valid moves for every piece of the current player:
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (boardState[x, y] == currentPlayer)
                    {
                        placingCoord[] helpMoves = this.getValidMoves(x, y, currentPlayer);
                        if (helpMoves.Length != 0)
                        { 
                            //Thus there are possible flips for this coordinate, so x,y is a valid move.
                            //TODO: Draw a circle or some other indicator on the gameboard.
                        }
                    }
                }
            }


        }

        private void NewGame()
        {
            //TODO: Initialise gameBoard with new values.
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
