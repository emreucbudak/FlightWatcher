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

    var input = ReadBottomPrompt();
}

static string ReadBottomPrompt()
{
    const string prefix = " -> ";
    var input = new System.Text.StringBuilder();
    var cursor = 0;
    var viewport = (Left: -1, Top: -1, Width: -1, Height: -1);
    var dirty = true;

    while (true)
    {
        var current = (Left: Console.WindowLeft, Top: Console.WindowTop,
            Width: Console.WindowWidth, Height: Console.WindowHeight);

        if (dirty || current != viewport)
        {
            // A resize can happen between reading the dimensions and moving the cursor.
            try
            {
                var width = Math.Max(0, current.Width - 1);
                var footerTop = current.Top + Math.Max(0, current.Height - 3);
                if (viewport.Height > 0)
                {
                    var previousTop = viewport.Top + Math.Max(0, viewport.Height - 3);
                    for (var row = Math.Max(current.Top, previousTop);
                         row < Math.Min(current.Top + current.Height, viewport.Top + viewport.Height);
                         row++)
                    {
                        Console.SetCursorPosition(current.Left, row);
                        Console.Write(new string(' ', width));
                    }
                }

                var inputRow = footerTop + (current.Height >= 2 ? 1 : 0);
                if (current.Height >= 2)
                {
                    Console.SetCursorPosition(current.Left, footerTop);
                    AnsiConsole.Write(new Text(new string('─', width), new Style(Color.DeepSkyBlue1)));
                }

                if (current.Height >= 3)
                {
                    Console.SetCursorPosition(current.Left, footerTop + 2);
                    AnsiConsole.Write(new Text(new string('─', width), new Style(Color.DeepSkyBlue1)));
                }

                var visiblePrefix = prefix[..Math.Min(prefix.Length, width)];
                var available = Math.Max(0, width - visiblePrefix.Length);
                var offset = Math.Max(0, cursor - Math.Max(0, available - 1));
                var visibleInput = input.ToString(offset, Math.Min(available, input.Length - offset));
                Console.SetCursorPosition(current.Left, inputRow);
                Console.Write((visiblePrefix + visibleInput).PadRight(width));
                Console.SetCursorPosition(current.Left + visiblePrefix.Length + cursor - offset, inputRow);
                viewport = current;
                dirty = false;
            }
            catch (ArgumentOutOfRangeException)
            {
                dirty = true;
            }
        }

        if (!Console.KeyAvailable)
        {
            Thread.Sleep(40);
            continue;
        }

        var key = Console.ReadKey(intercept: true);
        switch (key.Key)
        {
            case ConsoleKey.Enter:
                return input.ToString();
            case ConsoleKey.Backspace when cursor > 0:
                input.Remove(--cursor, 1);
                break;
            case ConsoleKey.Delete when cursor < input.Length:
                input.Remove(cursor, 1);
                break;
            case ConsoleKey.LeftArrow:
                cursor = Math.Max(0, cursor - 1);
                break;
            case ConsoleKey.RightArrow:
                cursor = Math.Min(input.Length, cursor + 1);
                break;
            case ConsoleKey.Home:
                cursor = 0;
                break;
            case ConsoleKey.End:
                cursor = input.Length;
                break;
            default:
                if (!char.IsControl(key.KeyChar))
                    input.Insert(cursor++, key.KeyChar);
                break;
        }

        dirty = true;
    }
}
