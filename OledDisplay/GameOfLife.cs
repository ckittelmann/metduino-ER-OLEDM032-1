namespace OledDisplay
{
    using System;

    public class GameOfLife : DisplayDriver
    {
        private static int width = 128;
        private static int height = 32;
        private bool[][] playingFieldActive = new bool[height][];
        private bool[][] playingFieldHidden = new bool[height][];

        protected override void StartAnimation()
        {
            InitPlayingField();
            SetDisplayData();
            RenderDisplayData();

            while(true)
            {
                ApplyRules();
                CopyHiddenToActive();
                SetDisplayData();
                RenderDisplayData();
            }            
        }

        private void ApplyRules()
        {
            for (int y = 0; y < playingFieldActive.Length; y++)
            {
                for (int x = 0; x < playingFieldActive[y].Length; x++)
                {
                    var aliveNeighbours = GetAliveNeightbours(y, x);
                    SetCellState(y, x, aliveNeighbours);
                }
            }
        }

        private int GetAliveNeightbours(int y, int x)
        {
            var count = 0;
            // upper left cell
            count += GetAliveStateOfCell(y - 1, x - 1);
            // upper cell
            count += GetAliveStateOfCell(y - 1, x);
            // upper right cell
            count += GetAliveStateOfCell(y - 1, x + 1);
            // left cell
            count += GetAliveStateOfCell(y, x - 1);
            // right cell
            count += GetAliveStateOfCell(y, x + 1);
            // bottom left cell
            count += GetAliveStateOfCell(y + 1, x - 1);
            // bottom cell
            count += GetAliveStateOfCell(y + 1, x);
            // bottom right cell
            count += GetAliveStateOfCell(y + 1, x + 1);

            return count;
        }

        private int GetAliveStateOfCell(int y, int x)
        {
            if (x < 0 || y < 0)
            {
                return 0;
            }

            if (y > playingFieldActive.Length -1 || x > playingFieldActive[y].Length -1)
            {
                return 0;
            }

            return playingFieldActive[y][x] ? 1 : 0;
        }

        private void SetCellState(int y, int x, int aliveNeighbours)
        {
            if (aliveNeighbours < 2 || aliveNeighbours > 3)
            {
                playingFieldHidden[y][x] = false;
            }
            else if (aliveNeighbours == 3)
            {
                playingFieldHidden[y][x] = true;
            }
            else
            {
                playingFieldHidden[y][x] = playingFieldActive[y][x];
            }
        }

        private void CopyHiddenToActive()
        {
            for (int i = 0; i < playingFieldActive.Length; i++)
            {
                playingFieldActive[i] = (bool[])playingFieldHidden[i].Clone();
            }
        }

        private void SetDisplayData()
        {
            var counter = 0;
            var bytesToWrite = new byte[playingFieldActive.Length * playingFieldActive[0].Length];

            for (int y = 0; y < playingFieldActive.Length; y++)
            {
                for (int x = 0; x < playingFieldActive[y].Length; x++)
                {
                    int firstCell = playingFieldActive[y][x] ? 0xFF : 0x00;
                    
                    byte byteValue = (byte)firstCell;

                    displayData[counter] = byteValue;
                    displayData[counter + 128] = byteValue;                   

                    counter += 1;
                }

                counter += 128;
            }
        }

        private void InitPlayingField()
        {
            for (int i = 0; i < playingFieldActive.Length; i++)
            {
                playingFieldActive[i] = new bool[width];
                playingFieldHidden[i] = new bool[width];
            }

            playingFieldActive[13][63] = true;
            playingFieldActive[13][64] = true;
            playingFieldActive[13][65] = true;
            playingFieldActive[14][63] = true;
            playingFieldActive[14][65] = true;
            playingFieldActive[15][63] = true;
            playingFieldActive[15][65] = true;

            playingFieldActive[17][63] = true;
            playingFieldActive[17][65] = true;
            playingFieldActive[18][63] = true;
            playingFieldActive[18][65] = true;
            playingFieldActive[19][63] = true;
            playingFieldActive[19][64] = true;
            playingFieldActive[19][65] = true;
        }
    }
}
