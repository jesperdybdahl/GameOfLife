// See https://aka.ms/new-console-template for more information
using System.Net.Http.Headers;
using System.Text;

Console.WriteLine("Hello, World!");

Console.SetWindowSize(160,55);
Console.Clear();

GameOfLifeConsole.GameOfLife gol = new GameOfLifeConsole.GameOfLife(80,50);
gol.OnGenerationComplete += Gol_OnGenerationComplete;

gol.Wait = 100;
gol.Run(1000);



void Gol_OnGenerationComplete(object? sender, GameOfLifeConsole.GameOfLifeGenerationCompleteEventArgs e)
{
    StringBuilder gridBuilder = new StringBuilder();

    for (int y = 0; y < e.Height; y++)
    {
        for (int x = 0; x < e.Width; x++)
        {
            gridBuilder.Append(e.Grid[x, y] ? "██" : "░░");
        }
        gridBuilder.AppendLine();
    }
    Console.SetCursorPosition(0, 0);
    Console.WriteLine(gridBuilder.ToString());
}
