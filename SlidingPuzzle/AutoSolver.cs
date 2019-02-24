using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SlidingPuzzle
{
    class AutoSolver
    {
        GameManager m_gameManager;
        GameGrid m_gameGrid;

        enum Direction
        {
            N,E,S,W
        }

        AutoSolver(GameManager gameManager, GameGrid gameGrid)
        {
            m_gameManager = gameManager;
        }

        private void MoveTileTo(Point point)
        {

        }

        private void MoveEmptyTileTo(Point point)
        {
            Point emptyTilePoint = m_gameGrid.EmptyTilePoint;
            foreach(Direction direction in GetPath(emptyTilePoint, point))
            {
                Swap(emptyTilePoint, direction);
            }
        }
        private void Swap(Point point, Direction direction)
        {
            int x=0;
            int y=0;
            if (direction == Direction.E)
                x = 1;
            else if (direction == Direction.W)
                x = -1;
            if (direction == Direction.N)
                y = 1;
            else if (direction == Direction.S)
                y = -1;

            var new_point = new Point(point.X + x,point.Y + y);
            m_gameGrid.SwapTiles(point,new_point);
            point = new_point;
        }

        private List<Direction> GetPath(Point start, Point finish)
        {
            List<Direction> path = new List<Direction>();
            var direction_vector = (finish - start);
            
            for (int x = 0; x < (int)Math.Abs(Math.Round(direction_vector.X)); x++)
            {
                path.Add(direction_vector.X >= 0 ? Direction.E : Direction.W);
            }
            for (int y = 0; y < (int)Math.Abs(Math.Round(direction_vector.Y)); y++)
            {
                path.Add(direction_vector.Y >= 0 ? Direction.N : Direction.S);
            }
            return path;
        }

        private Point FindTile(int id)
        {
            for (int y = 0; y < m_gameGrid.Size; y++)
                for (int x = 0; x < m_gameGrid.Size; x++)
                    if (m_gameGrid.GetTile(x, y).Name == id.ToString())
                        return m_gameGrid.GetTile(x, y).TilePoint;
            return new Point(-1, -1);
        }
    }
}
