using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SlidingPuzzle
{
    class GameManager
    {
        GameGrid m_gameGrid;
        Label m_message;
        DateTime m_start_time;

        public GameManager(GameGrid gameGrid, Label message)
        {
            GameTile.GameTileClicked += OnTileClicked;

            m_gameGrid = gameGrid;
            m_message = message;
            NewGame(3);
        }

        private void NewGame(int gridSize)
        {
            
            m_gameGrid.SetGridSize(gridSize);
            m_gameGrid.FillGrid();
        }


        public bool IsSolved()
        {
            for (int i = 1; i < m_gameGrid.Size * m_gameGrid.Size - 1; i++)
            {
                if (m_gameGrid.GetTile((i-1) % m_gameGrid.Size, (i-1) / m_gameGrid.Size).Number > m_gameGrid.GetTile(i % m_gameGrid.Size, i / m_gameGrid.Size).Number)
                    return false;
            }


            return true;
        }

        private void OnTileClicked(object sender, EventArgs e)
        {
            GameTile gameTile = sender as GameTile;

            if(gameTile.TilePoint.X == m_gameGrid.EmptyTilePoint.X || gameTile.TilePoint.Y == m_gameGrid.EmptyTilePoint.Y)
            {
                if (Math.Abs(gameTile.TilePoint.X - m_gameGrid.EmptyTilePoint.X) == 1 || Math.Abs(gameTile.TilePoint.Y - m_gameGrid.EmptyTilePoint.Y) == 1)
                    m_gameGrid.SwapTiles(gameTile.TilePoint, m_gameGrid.EmptyTilePoint);
            }

            if (IsSolved())
            {
                OnGameSolved();
                NewGame(m_gameGrid.Size + 1);
            }
        }

        private void OnGameSolved()
        {
            m_message.Content = DateTime.Now - m_start_time;
        }
    }


}
