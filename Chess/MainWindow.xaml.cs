using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Image[,] imageControls = new Image[8, 8];
        Border[,] borderControls = new Border[8, 8];

        List<ChessPiece> chessPieces = new List<ChessPiece>();

        ChessPoint selectedPoint;
        bool isBlackMove = false;

        List<ChessPoint> avaliableMoves;
        List<ChessPoint> avaliableHits;
        ChessPiece selectedPiece = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetupBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                Pawn p1 = new Pawn(i, 1, true);
                chessPieces.Add(p1);
                Pawn p2 = new Pawn(i, 6, false);
                chessPieces.Add(p2);
            }
        }

        private void DrawChessBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    imageControls[i,j].Source = null;
                }
            }
            foreach(ChessPiece cp in chessPieces){
                Image i = imageControls[cp.position.Y, cp.position.X];
                BitmapImage bi = new BitmapImage(new Uri(cp.imageName(),UriKind.Relative));
                i.Source = bi;
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                ChessGrid.ColumnDefinitions.Add(new ColumnDefinition());
                ChessGrid.RowDefinitions.Add(new RowDefinition());
            }
            ChessGrid.ShowGridLines = true;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Border bd = new Border();
                    if((i + j) % 2 == 0)
                    {
                        bd.Background = new SolidColorBrush(Color.FromRgb(100,100,100));  
                    }
                    else
                    {
                        bd.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                    }
                    Grid.SetRow(bd, i);
                    Grid.SetColumn(bd, j);

                    bd.MouseDown += new MouseButtonEventHandler((s, me) => CellClicked(me));

                    Image img = new Image();
                    imageControls[i, j] = img;
                    bd.Child = img;

                    borderControls[i, j] = bd;
                    ChessGrid.Children.Add(bd);
                }
            }

            SetupBoard();
            DrawChessBoard();
        }

        private void CellClicked(MouseButtonEventArgs m)
        {
            int ym = (int)(m.GetPosition(this).X / 64);
            int xm = (int)(m.GetPosition(this).Y / 64);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        borderControls[i,j].Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                    }
                    else
                    {
                        borderControls[i, j].Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                    }
                }
            }

            if(avaliableMoves != null) { 
                foreach(ChessPoint cpt in avaliableMoves)
                {
                    if (ym == cpt.X && xm == cpt.Y )
                    {
                        selectedPiece.position = cpt;
                        DrawChessBoard();
                        isBlackMove = !isBlackMove;
                        break;
                    }
                }
            }

            if(avaliableHits != null)
            {
                foreach (ChessPoint cpt in avaliableHits)
                {
                    if (ym == cpt.X && xm == cpt.Y)
                    {
                        ChessPiece pieceToRemove = null;
                        foreach (ChessPiece cpth in chessPieces)
                        {
                            if(cpth.position.X == cpt.X && cpth.position.Y == cpt.Y)
                            {
                                pieceToRemove = cpth;
                            }
                        }
                        chessPieces.Remove(pieceToRemove);
                        selectedPiece.position = cpt;
                        DrawChessBoard();
                        isBlackMove = !isBlackMove;
                        break;
                    }
                }
            }

            bool isValid = false;
            selectedPiece = null;
            avaliableMoves = null;
            avaliableHits = null;
            foreach(ChessPiece cp in chessPieces)
            {
                if(ym == cp.position.X && xm == cp.position.Y && cp.isBlack == isBlackMove)
                {
                    isValid = true;
                    selectedPiece = cp;
                }
            }

            if (isValid)
            {
                selectedPoint = new ChessPoint(ym, xm);
                Console.WriteLine("Selected : " + selectedPiece.position.X + " , " + selectedPiece.position. Y);
                borderControls[xm, ym].Background = Brushes.LimeGreen;

                avaliableMoves = selectedPiece.getPossibleMoves(chessPieces);
                foreach (ChessPoint cpt in avaliableMoves)
                {
                    borderControls[cpt.Y, cpt.X].Background = Brushes.SkyBlue;
                }

                avaliableHits = selectedPiece.getPossibleHits(chessPieces);
                Console.WriteLine("Hits : " + avaliableHits.Count);
                foreach(ChessPoint cpt in avaliableHits)
                {
                    borderControls[cpt.Y, cpt.X].Background = Brushes.Red;
                }
            }
        }
    }
}
