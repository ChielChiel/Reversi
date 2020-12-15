using System;
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
        public int[,] boardState;
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


            this.Width = 800;
            this.Height = 800;

            
        }

        private void Clicked(object sender, MouseEventArgs mea)
        {
            Point translatedPoint = this.getPointFromMEA(mea.Location);
            int column = translatedPoint.X;
            int row = translatedPoint.Y;
            
          //  Console.WriteLine(column + "," + row);

            //Get here the valid moves/flips for the current coordinates.
            placingCoord[] flips = getValidMoves(column, row, currentPlayer);
           // Console.WriteLine("def flips: " + flips.Length);
            if (flips.Length != 0)
            {
                boardState[column, row] = currentPlayer;
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


        private Point getPointFromMEA(Point meaPoint) 
        {
            int x = (int)(meaPoint.X / 50f);
            int y = (int)(meaPoint.Y / 50f);
            return new Point(x, y);
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
                        if (boardState[column + places[dx], row + places[dy]] != currentPlayer && boardState[column + places[dx], row + places[dy]] != 0 )
                        {
                            
                            //Check here whether the possible moves actually close-in pieces of the opponent.
                            List<placingCoord> tempFlipPieces = new List<placingCoord>();

                            try
                            {
                                for (int i = 1; i <= this.boardSize; i++) 
                                {
                                    int tempX = i * places[dx] + column;
                                    int tempY = i * places[dy] + row;

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
                                
                            }
                            catch (IndexOutOfRangeException)
                            {
                                //Out of range. Clearing temp pieces
                                tempFlipPieces.Clear();
                            }
                            flipsToReturn.AddRange(tempFlipPieces);
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
            tlcpea.Graphics.SmoothingMode = SmoothingMode.HighQuality;
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
                    this.NewGame(this.boardSize, this.boardSize);
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
                    this.NewGame(this.boardSize, this.boardSize);
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

        private void NewGame(int columns, int rows) 
        {
            //TODO: Initialise gameBoard with new values.
            this.playerOneIconName.Text = this.playerOne;
            this.playerTwoIconName.Text = this.playerTwo;
            boardState = new int[columns, rows];
            this.board.Name = "board";
            this.board.Location = new Point(200, 200);
            this.board.Size = new Size(50 * columns + columns, 50 * rows + rows);
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
            this.board.MouseClick += new MouseEventHandler(this.Clicked);



            this.board.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            Controls.Add(this.board);


            //Setup first 4 pieces in the middle of the board:
            boardState[columns / 2 - 1, rows / 2 - 1] = 1;
            boardState[columns / 2 - 1, rows / 2] = 2;
            boardState[columns / 2, rows / 2] = 1;
            boardState[columns / 2, rows / 2 - 1] = 2;
        }

    }





}
