using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Pawn : ChessPiece
    {

        public Pawn(int x , int y , bool isBlack) : base(x, y, isBlack) { }

        public override string imageName()
        {
            if (isBlack) return "b_pawn.png";
            else return "w_pawn.png";
        }  

        public override List<ChessPoint> getPossibleMoves(List<ChessPiece> allPieces)
        {
            List<ChessPoint> result = new List<ChessPoint>();
            if (isBlack)
            {
                ChessPoint move1 = new ChessPoint(position.X, position.Y + 1);
                result.Add(move1);
                if (isFrist)
                {
                    ChessPoint move2 = new ChessPoint(position.X, position.Y + 2);
                    result.Add(move2);
                }
            }
            else
            {
                ChessPoint move1 = new ChessPoint(position.X, position.Y - 1);
                result.Add(move1);
                if (isFrist)
                {
                    ChessPoint move2 = new ChessPoint(position.X, position.Y - 2);
                    result.Add(move2);
                }
            }
            List<ChessPoint> invalidMoves = new List<ChessPoint>();
            foreach(ChessPiece cp in allPieces)
            {
                foreach(ChessPoint possibleMove in result)
                {
                    if(cp.position.X == possibleMove.X && cp.position.Y == possibleMove.Y)
                    {
                        invalidMoves.Add(possibleMove);
                    }
                }
            }
            foreach(ChessPoint invalidMove in invalidMoves)
            {
                result.Remove(invalidMove);
            }
            return result;
        }

        public override List<ChessPoint> getPossibleHits(List<ChessPiece> allPieces)
        {
            List<ChessPoint> result = new List<ChessPoint>();
            foreach (ChessPiece cp in allPieces)
            {
                Console.WriteLine("Checking Hit Between " + position.X + " , " + position.Y + " and  " + cp.position.X + " , " + cp.position.Y);
                if (!isBlack)
                {
                    if (((position.X == cp.position.X + 1) || (position.X == cp.position.X - 1)) && (position.Y == cp.position.Y + 1))
                    {
                        result.Add(cp.position);
                    }
                }
                else
                {
                    if (((position.X == cp.position.X + 1) || (position.X == cp.position.X - 1)) && (position.Y == cp.position.Y - 1))
                    {
                        result.Add(cp.position);
                    }
                }
            }
            return result;
        }

    }
}
