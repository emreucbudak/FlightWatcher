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
    AnsiConsole.Write(new Rule().RuleStyle("deepskyblue1"));
    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule().RuleStyle("deepskyblue1"));


    AnsiConsole.Cursor.MoveUp(2);
    var input = AnsiConsole.Prompt(
        new TextPrompt<string>(" -> ")
            .AllowEmpty());
    AnsiConsole.Cursor.MoveDown(1);
}
