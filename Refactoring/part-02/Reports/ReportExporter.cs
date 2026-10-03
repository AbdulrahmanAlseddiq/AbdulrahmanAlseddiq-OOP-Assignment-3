namespace RefactoringLab.Part02.Reports;

/// <summary>
/// Template Method: <see cref="Export"/> defines the export steps and their order in ONE place.
/// Subclasses only supply what is different — the format.
/// </summary>
public abstract class ReportExporter
{
    // The template method — not virtual, so no subclass can reorder or skip a step.
    public void Export(string path)
    {
        var rows = Load();
        if (!Validate(rows))
            throw new InvalidOperationException("Invalid data");
        var content = Format(rows);
        Save(path, content);
    }

    // Shared steps (same for every report).
    protected virtual List<string[]> Load() =>
    [
        ["Id", "Name"],
        ["1", "Keyboard"],
        ["2", "Mouse"]
    ];

    protected virtual bool Validate(List<string[]> rows) =>
        rows.Count > 1 && rows[0].Length > 0;

    private static void Save(string path, string content) =>
        File.WriteAllText(path, content);

    // The only step every exporter must provide.
    protected abstract string Format(List<string[]> rows);
}
