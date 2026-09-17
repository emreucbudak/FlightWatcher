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
AnsiConsole.Write(new Text("\n\n\n\n"));

while (true)
{
    await Task.Delay(1000);
}
