using System.Text;

namespace Unitz.Core.Generators.Helpers;

internal sealed class CodeWriter(StringBuilder builder, string indentText = CodeWriter.DefaultIndentText)
{
    private const string DefaultIndentText = "    ";

    private readonly StringBuilder _builder = builder ?? throw new ArgumentNullException(nameof(builder));
    private readonly string _indentText = indentText ?? throw new ArgumentNullException(nameof(indentText));

    private int _indentLevel;
    private bool _isLineStart = true;

    public int IndentLevel => _indentLevel;

    public void Indent()
    {
        _indentLevel++;
    }

    public void Unindent()
    {
        if (_indentLevel == 0)
            throw new InvalidOperationException("Indentation level cannot be negative.");

        _indentLevel--;
    }

    public void Write(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        WriteIndentIfNeeded();
        _builder.Append(text);
    }

    public void WriteLine()
    {
        _builder.AppendLine();
        _isLineStart = true;
    }

    public void WriteLine(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            WriteIndentIfNeeded();
            _builder.Append(text);
        }

        _builder.AppendLine();
        _isLineStart = true;
    }

    public IDisposable Block()
    {
        WriteLine("{");
        Indent();

        return new BlockScope(this);
    }
    public void WriteEmptyBlock()
    {
        using (Block())
        {
        }
    }

    public override string ToString()
    {
        return _builder.ToString();
    }

    private void WriteIndentIfNeeded()
    {
        if (!_isLineStart)
            return;

        for (var i = 0; i < _indentLevel; i++)
            _builder.Append(_indentText);

        _isLineStart = false;
    }

    private sealed class BlockScope : IDisposable
    {
        private readonly CodeWriter _writer;
        private bool _disposed;

        public BlockScope(CodeWriter writer)
        {
            _writer = writer;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _writer.Unindent();
            _writer.WriteLine("}");
            _disposed = true;
        }
    }
}