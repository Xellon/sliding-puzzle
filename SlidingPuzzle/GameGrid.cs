using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SlidingPuzzle
{
    class GameGrid : Grid
    {
        private const int m_minGridSize = 3;
        private const int m_maxGridSize = 10;
        private int m_gridSize;

        public int Size { get => m_gridSize; }

        public Point EmptyTilePoint {
            get
            {
                for (int y = 0; y < m_gridSize; y++)
                {
                    for (int x = 0; x < m_gridSize; x++)
                    {
                        if (m_tileArray[x, y].Number == 0)
                            return new Point(x,y);
                    }
                }
                return new Point(-1,-1);
            }
            
        }

        private List<GameTile> m_tileList = new List<GameTile>();
        private GameTile[,] m_tileArray = new GameTile[m_maxGridSize, m_maxGridSize];

        public GameGrid()
        {
            m_gridSize = m_minGridSize;
            //SetGridSize(m_minGridSize);
        }

        public GameGrid(int size)
        {
            m_gridSize = m_minGridSize;
            //SetGridSize(size);
        }

        public void SetGridSize(int size)
        {
            m_gridSize = size;

            if (size < m_minGridSize)
                throw new Exception("Trying to set grid size to lower than " + m_minGridSize + ".");
            else if (size > m_maxGridSize)
                throw new Exception("Trying to set grid size to higher than " + m_maxGridSize + ".");

            this.RowDefinitions.Clear();
            this.ColumnDefinitions.Clear();
            for (int i = 0; i < size; i++)
            {
                RowDefinition rowDefinitionTemplate = new RowDefinition();
                rowDefinitionTemplate.Height = new GridLength(1, GridUnitType.Star);
                this.RowDefinitions.Add(rowDefinitionTemplate);

                ColumnDefinition columnDefinitionTemplate = new ColumnDefinition();
                columnDefinitionTemplate.Width = new GridLength(1, GridUnitType.Star);
                this.ColumnDefinitions.Add(columnDefinitionTemplate);
            }

            PopulateTileList();
        }

        private void PopulateTileList()
        {
            m_tileList.Clear();

            for (int i = 0; i < m_gridSize * m_gridSize; i++)
            {
                m_tileList.Add(new GameTile(i));
            }

            do
            {
                ShuffleTileList();
            } while (!IsSolvable());
            
        }

        public void FillGrid()
        {
            for (int y = 0; y < m_gridSize; y++)
            {
                for (int x = 0; x < m_gridSize; x++)
                {
                    m_tileArray[x, y] = m_tileList[y * m_gridSize + x];
                    m_tileArray[x, y].SetTilePoint(new Point(x,y));
                }
            }
            SyncUIWithArray();
        }

        public void UpdateGrid()
        {
            SyncUIWithArray();
        }

        private void SyncUIWithArray()
        {
            this.Children.Clear();
            for (int y = 0; y < m_gridSize; y++)
            {
                for (int x = 0; x < m_gridSize; x++)
                {
                    if (m_tileArray[x, y].Number != 0)
                    {
                        GameGrid.SetRow(m_tileArray[x, y], y);
                        GameGrid.SetColumn(m_tileArray[x, y], x);
                        this.Children.Add(m_tileArray[x, y]);
                    }
                }
            }
        }

        public void SwapTiles(Point first, Point second)
        {
            GameTile tempTile = m_tileArray[(int)first.X, (int)first.Y];
            m_tileArray[(int)first.X, (int)first.Y]  =  m_tileArray[(int)second.X, (int)second.Y];
            m_tileArray[(int)second.X, (int)second.Y] = tempTile;

            m_tileArray[(int)first.X, (int)first.Y].SetTilePoint(first);
            m_tileArray[(int)second.X, (int)second.Y].SetTilePoint(second);
            SyncUIWithArray();
        }

        private void ShuffleTileList()
        {
            int n = m_tileList.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                GameTile tile = m_tileList[k];
                m_tileList[k] = m_tileList[n];
                m_tileList[n] = tile;
            }
        }

        //Source https://www.cs.bham.ac.uk/~mdr/teaching/modules04/java2/TilesSolvability.html
        private bool IsSolvable()
        {
            FillGrid();
            int inversions = 0;
            for (int i = 0; i < m_tileList.Count; i++)
            {
                if (m_tileList[i].Number != 0)
                {
                    for (int j = i; j < m_tileList.Count; j++)
                    {
                        if (m_tileList[i].Number > m_tileList[j].Number && m_tileList[j].Number != 0)
                            inversions++;
                    }
                }
            }

            if ((!IsEven(m_gridSize) && IsEven(inversions)) || (IsEven(m_gridSize) && (IsBlankOnOddRowFromBottom() == IsEven(inversions))))
                return true;
            else
                return false;
            //if (m_gridSize % 2 != 0 && inversions % 2 == 0)
            //    return true;
            //else if (m_gridSize % 2 == 0 && inversions % 2 == 0 && (m_gridSize - ((int)this.EmptyTilePoint.Y + 1)) % 2 == 0)
            //    return true;
            //else if (m_gridSize % 2 == 0 && inversions % 2 != 0 && (m_gridSize - ((int)this.EmptyTilePoint.Y + 1)) % 2 != 0)
            //    return true;
            //else
            //    return true;
        }

        bool IsEven(int value)
        {
            return value % 2 == 0 ? true : false;
        }

        bool IsBlankOnOddRowFromBottom()
        {
            return (m_gridSize - ((int)EmptyTilePoint.Y+1)) % 2 == 0 ? true : false;
        }

        public GameTile GetTile(int x, int y)
        {
            return m_tileArray[x, y];
        }
    }
}
