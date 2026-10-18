using Spectre.Console;

var figlet = new FigletText("FlightWatcher")
{
    Justification = Justify.Center,
    Color = Color.DeepSkyBlue1

};
AnsiConsole.Write(figlet);
AnsiConsole.WriteLine();
var description = new Text("FlightWatcher tam kapsamlı bir uçuş izleme aracıdır bilgisayarınızda çalışırken  arkada sizin için takibi yapar istediğiniz lokasyonda istediğiniz fiyata düşen bilet olursa bildirimle hemen sizi uyarır.")
    .Centered();

AnsiConsole.Write(description);
AnsiConsole.WriteLine();
AnsiConsole.Write(new Text("\n\n"));

while (true)
{
    if (Console.IsInputRedirected || Console.IsOutputRedirected)
    {
        Console.Write(" -> ");
        if (Console.ReadLine() is null)
            break;

        continue;
    }

    var top = Console.WindowTop + Console.WindowHeight - 3;
    var left = Console.WindowLeft;
    var line = new string('─', Console.WindowWidth - 1);

    Console.SetCursorPosition(left, top);
    AnsiConsole.Write(new Text(line, new Style(Color.DeepSkyBlue1)));

    Console.SetCursorPosition(left, top + 1);
    Console.Write(" -> ");
    var inputLeft = Console.CursorLeft;
    Console.Write(new string(' ', line.Length - 4));

    Console.SetCursorPosition(left, top + 2);
    AnsiConsole.Write(new Text(line, new Style(Color.DeepSkyBlue1)));

    Console.SetCursorPosition(inputLeft, top + 1);

    var input = Console.ReadLine();
    if (input is null)
        break;
}
