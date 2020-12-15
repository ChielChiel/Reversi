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
        public String playerOne;
        public String playerTwo;
        public int boardWidth = 6;
        public int boardHeight = 6;
        TableLayoutPanel board = new TableLayoutPanel();
        private int[,] boardState;
        int currentPlayer = 1;
        IDictionary<int, Color> pieceColor = new Dictionary<int, Color> { { 1, Color.Black }, { 2, Color.White } };

        private bool isHelping = false;
        
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
           
            //Get here the valid moves/flips for the current coordinates.
            placingCoord[] flips = getValidMoves(column, row, currentPlayer);
            if (flips.Length != 0)
            {
                this.boardState[column, row] = currentPlayer;
                foreach (placingCoord flip in flips)
                {
                    this.boardState[flip.X, flip.Y] = currentPlayer;
                }
                this.currentPlayer = (this.currentPlayer == 1 ? 2 : 1);
                this.isHelping = false;
                Console.WriteLine("helping? " + this.isHelping.ToString());
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

            if (this.boardState[column, row] != 0 && this.boardState[column,row] != 3)
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
                        if (this.boardState[column + places[dx], row + places[dy]] != currentPlayer && this.boardState[column + places[dx], row + places[dy]] != 0 )
                        {
                            //Check here whether the possible moves actually close-in pieces of the opponent.
                            List<placingCoord> tempFlipPieces = new List<placingCoord>();
                            try
                            {
                                int maxBoardDimension = (this.boardWidth <= this.boardHeight ? this.boardHeight : this.boardWidth);
                                for (int i = 1; i <= maxBoardDimension; i++) 
                                {
                                    int tempX = i * places[dx] + column;
                                    int tempY = i * places[dy] + row;

                                    if (this.boardState[tempX, tempY] == currentPlayer)
                                    {
                                        //stop searching
                                        i = maxBoardDimension;
                                    }
                                    else if (this.boardState[tempX, tempY] != currentPlayer && this.boardState[tempX, tempY] != 0)
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
            bool drawHelp = false;

            switch (this.boardState[tlcpea.Column, tlcpea.Row]) {
                case 1:
                    brushColor = new SolidBrush(pieceColor[1]);
                    break;
                case 2:
                    brushColor = new SolidBrush(pieceColor[2]);
                    break;
                case 3:
                    brushColor = new SolidBrush(Color.Yellow);
                    this.boardState[tlcpea.Column, tlcpea.Row] = 0;
                    drawHelp = true;
                    break;
                default:
                    break;
            }
            tlcpea.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            if (brushColor != null)
            {
                if (drawHelp)
                    tlcpea.Graphics.FillEllipse(brushColor, tlcpea.CellBounds.Location.X + 10, tlcpea.CellBounds.Location.Y + 10, tlcpea.CellBounds.Width - 20, tlcpea.CellBounds.Height - 20);
                else 
                    tlcpea.Graphics.FillEllipse(brushColor, tlcpea.CellBounds);
            }

            if (tlcpea.Column == this.boardWidth - 1 && tlcpea.Row == this.boardHeight - 1)
            {
                this.isHelping = false;
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
                    this.boardWidth = newGame.gameBoardSizeWidth;
                    this.boardHeight = newGame.gameBoardSizeHeight;
                    Console.WriteLine("New \"" + this.playerOne + "\"v\"" + this.playerTwo + "\" game");
                    this.NewGame(this.boardWidth, this.boardHeight);
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
                    this.boardWidth = newGame.gameBoardSizeWidth;
                    this.boardHeight = newGame.gameBoardSizeHeight;
                    Console.WriteLine("New \"" + this.playerOne + "\"v\"" + this.playerTwo + "\" game");
                    this.NewGame(this.boardWidth, this.boardHeight);
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
            List<placingCoord> possibleMoves = new List<placingCoord>();
            for (int x = 0; x < this.boardWidth; x++)
            {
                for (int y = 0; y < this.boardHeight; y++)
                {
                    if (this.boardState[x, y] == 0)
                    {
                        //Thus the position is not yet occupied by one of the two player's pieces.
                        if (this.getValidMoves(x, y, currentPlayer).Length != 0)
                        {
                            //Thus there are possible flips for this coordinate, so x,y is a valid move.
                            possibleMoves.Add(new placingCoord(x,y, 3));
                        }
                    }
                }
            }

            if (possibleMoves.Count != 0)
            {
                this.isHelping = true;
                foreach (placingCoord helpMove in possibleMoves)
                {
                    this.boardState[helpMove.X, helpMove.Y] = 3;
                }
                this.board.Invalidate();
            }
        }

        private void NewGame(int columns, int rows) 
        {
            //TODO: Initialise gameBoard with new values.
            this.playerOneIconName.Text = this.playerOne;
            this.playerTwoIconName.Text = this.playerTwo;
            
            this.boardState = new int[columns, rows];

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

            //Setup first 4 pieces in the middle of the board:
            this.boardState[columns / 2 - 1, rows / 2 - 1] = 1;
            this.boardState[columns / 2 - 1, rows / 2] = 2;
            this.boardState[columns / 2, rows / 2] = 1;
            this.boardState[columns / 2, rows / 2 - 1] = 2;
            Controls.Add(this.board);
        }

    }





}
