using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    class ReversiBot
    {

        private int[,] boardState;
        private int defVoorsprong = -20;
        private placingCoord defMove;

        //bot is always player 2. So no variable needed there.

        public ReversiBot(int[,] currentGameState)
        {
            this.boardState = currentGameState;
            Console.WriteLine("start searching:...");
            this.getBestMove(currentGameState, 2, 4);
            Console.WriteLine("done searching:...");
    
        }


        private void getBestMove(int[,] gameState, int player, int searchDepth)
        {
            //0. 
            /*
             * if(searchDepth == 0) { //Klaar met zoeken, de put is bereikt
             *      int voorsprong = -20;
             *      placingCoord bestMove;
             * 
             *      placingCoord[] possibleMoves = viableMoves(gameState, player).ToArray();
             *      foreach(placingCoord move in possibleMoves) { //Loop door alle mogelijke moves
             *          int[,] tempGameState = gameState;
             *          tempGameState[move.X, move.Y] = player
             *          placingCoord[] flips = getValidMoves(move.X, move.Y);
             *          foreach(placingCoord flip in flips) {
             *              tempGameState[flip.X, flip.Y] = player
             *          }
             *          
             *          int TempVoorsprong = occurencies(tempGameState, 2) - occurencies(tempGameState, 1);
             *          if(TempVoorSprong > voorsprong) { //check of de voorsprong die je met deze move bereikt groter is dan de vorige
             *              voorsprong = TempVoorSprong
             *              bestMove = move;
             *          }
             *      
             *      if(voorsprong > this.defVoorsprong) {
             *          this.defVoorsprong = voorsprong;
             *          this.defMove = bestMove;
             *      }
             *      
             */

            //1. Haal alle mogelijke zetten op met deze gameState
            //placingCoord[] possibleMoves = viableMoves(gameState, player).ToArray();

            //2. Loop through each of these moves
            // foreach(placingCoord move in possibleMoves) {
            /*      gameState[move.X, move.Y] = player
             *      placingCoord[] flips = getValidMoves(move.X, move.Y);
             *      foreach(placingCoord flip in flips) {
             *          gameState[flip.X, flip.Y] = player
             *      }
             *      
             *      //Nu zit dus in gameState de nieuw hypothethische state van deze move.
             *      //run dan nu getBestMove nog een keer voor de tegenpartij.
             *      getBestMove(gameState, turnOf(player), searchDepth - 1)
             *      
             */   

        }

        //Whose turn is it?
        private int turnOf(int player)
        {
            return (player == 1 ? 2 : 1);
        }





        //Only here for initial setup. 
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
    }
}
