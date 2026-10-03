namespace RefactoringLab.Part02.Reports;

public class TextReportExporter
{
    public void Export(string path)
    {
        var rows = Load();
        if (!Validate(rows))
            throw new InvalidOperationException("Invalid data");
        var content = Format(rows);
        Save(path, content);
    }

    private List<string[]> Load() =>
    [
        ["Id", "Name"],
        ["1", "Keyboard"],
        ["2", "Mouse"]
    ];

    private bool Validate(List<string[]> rows) =>
        rows.Count > 1 && rows[0].Length > 0;

    private string Format(List<string[]> rows) =>
        string.Join(Environment.NewLine, rows.Select(r => string.Join(" | ", r)));

    private void Save(string path, string content) =>
        File.WriteAllText(path, content);
}
