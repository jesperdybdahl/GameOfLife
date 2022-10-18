using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GameOfLifeConsole;

public class GameOfLifeDrawEventArgs : EventArgs
{
	public int X { get; set; }
	public int Y { get; set; }
	public bool Alive { get; set; }
}

public class GameOfLifeGenerationCompleteEventArgs : EventArgs
{
	public bool[,] Grid { get; set; }
	public int Width { get; set; } = 0;
	public int Height { get; set; } = 0;
}

public class GameOfLife
{
	public int Wait { get; set; } = 1000;
	private int width;
	private int height;

    public bool[,] Grid { get; set; }

	public event EventHandler<GameOfLifeDrawEventArgs> DrawCell;
	public event EventHandler<GameOfLifeGenerationCompleteEventArgs> OnGenerationComplete;	

	public GameOfLife(int width = 40, int height = 20)
	{
		this.width = width;
		this.height = height;
		Grid = new bool[width, height];
	}

	public void BuildRandom()
	{
		Random rand = new Random((int)DateTime.Now.Ticks);
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				Grid[x,y] = rand.Next(0,2) == 1;
			}
		}
	}

	protected int CountNeighbours(bool[,] grid, int x, int y)
	{
		int neighbours = 0;

		for (int ny = -1; ny <= 1; ny++)
		{
			for (int nx = -1; nx <= 1; nx++)
			{
				if (nx == 0 && ny == 0)
				{
					continue;
				}

				int nPosX = x + nx;
				int nPosY = y + ny;

				if (nPosX >= 0 && nPosX < this.width && nPosY >= 0 && nPosY < this.height)
				{ 
					if (grid[nPosX, nPosY] == true)
					{
						neighbours++;
					}
				}
			}
		}

		return neighbours;
	}

	protected void NextGeneration()
	{
		bool[,] nextGen = new bool[width, height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
                nextGen[x, y] = false;
                
				int n = this.CountNeighbours(Grid, x, y);
				
				if (Grid[x,y] == true)
				{
					if (n == 2 || n == 3)
					{
						nextGen[x,y] = true;
					}
				}
				else
				{
					if (n == 3)
					{
						nextGen[x,y] = true;
					}
				}
			}
		}

		Grid = nextGen;
	}

	protected void GenerationComplete()
	{
		OnGenerationComplete?.Invoke(this, new GameOfLifeGenerationCompleteEventArgs
		{
			Grid = this.Grid,
			Width = this.width,
			Height = this.height
		});
	}

    protected void Draw()
	{
		this.GenerationComplete();

		if (DrawCell != null)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					GameOfLifeDrawEventArgs args = new GameOfLifeDrawEventArgs { X = x, Y = y };
					args.Alive = Grid[x, y];

					DrawCell?.Invoke(this, args);
				}
			}
		}
	}

	public void Run(int generations = 10)
	{
		this.BuildRandom();
		this.Draw();

		for (int gen = 1; gen <= generations; gen++)
		{
			System.Threading.Thread.Sleep(Wait);
			this.NextGeneration();
			this.Draw();
		}
	}

}
