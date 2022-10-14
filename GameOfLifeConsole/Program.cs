// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Console.SetWindowSize(160,50);
Console.Clear();

GameOfLifeConsole.GameOfLife gol = new GameOfLifeConsole.GameOfLife(40,40);
gol.DrawCell += Gol_DrawCell;
gol.Wait = 500;
gol.Run(100);

void Gol_DrawCell(object? sender, GameOfLifeConsole.GameOfLifeDrawEventArgs e)
{
    Console.SetCursorPosition(e.X*2, e.Y);
    Console.Write(e.Alive ? "██" : "░░");
}