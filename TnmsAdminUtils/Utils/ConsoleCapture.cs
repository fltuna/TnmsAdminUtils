using System.Text;

namespace TnmsAdminUtils.Utils;

public class ConsoleCapture : TextWriter
{
    private readonly TextWriter _originalOutput;
    private readonly StringBuilder _capturedText;

    public ConsoleCapture()
    {
        _originalOutput = Console.Out;
        _capturedText = new StringBuilder();
        Console.SetOut(this);
    }

    public override Encoding Encoding => _originalOutput.Encoding;

    public override void Write(char value)
    {
        _originalOutput.Write(value);
        _capturedText.Append(value);
    }

    public override void Write(string? value)
    {
        _originalOutput.Write(value);
        _capturedText.Append(value);
    }

    public string GetCapturedText()
    {
        return _capturedText.ToString();
    }

    public void Clear()
    {
        _capturedText.Clear();
    }

    public void Restore()
    {
        Console.SetOut(_originalOutput);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Restore();
        }
        base.Dispose(disposing);
    }
}