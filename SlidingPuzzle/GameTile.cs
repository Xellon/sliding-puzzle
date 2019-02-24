using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SlidingPuzzle
{
    class GameTile: Button
    {
        public static event EventHandler GameTileClicked;

        int m_number;

        public Point TilePoint { get; private set; }
        public int Number { get => m_number; }


        public GameTile(int number)
        {
            m_number = number;
            this.Content = m_number.ToString();
        }

        protected override void OnClick()
        { 
            OnGameTileClicked(new EventArgs());
            base.OnClick();
        }

        protected virtual void OnGameTileClicked(EventArgs e)
        {
            GameTileClicked?.Invoke(this, e);
        }

        public void SetTilePoint(Point point){ TilePoint = point; }
    }
}
