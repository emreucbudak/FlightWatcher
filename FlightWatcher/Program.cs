using Spectre.Console;

var figlet = new FigletText("FlightWatcher")
{
    Justification = Justify.Center,
    Color = Color.DeepSkyBlue1

};
AnsiConsole.Write(figlet);
AnsiConsole.WriteLine();
AnsiConsole.WriteLine("FlightWatcher tam kapsamlı bir uçuş izleme aracıdır bilgisayarınızda çalışırken  arkada sizin için takibi yapar istediğiniz lokasyonda istediğiniz fiyata düşen bilet olursa bildirimle hemen sizi uyarır.");