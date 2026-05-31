using System.Text.RegularExpressions;

namespace FinanceControl.Domain.ValueObjects;

/// <summary>
/// Value Object para uma cor em formato hexadecimal "#RRGGBB".
/// </summary>
public sealed partial record HexColor
{
    public static readonly HexColor Default = new("#6C757D");

    public string Value { get; }

    private HexColor(string value) => Value = value;

    public static HexColor Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Cor é obrigatória.", nameof(value));

        if (!HexRegex().IsMatch(value))
            throw new ArgumentException($"Cor '{value}' não está no formato #RRGGBB.", nameof(value));

        return new HexColor(value.ToUpperInvariant());
    }

    public override string ToString() => Value;
    public static implicit operator string(HexColor color) => color.Value;

    [GeneratedRegex("^#([0-9A-Fa-f]{6})$")]
    private static partial Regex HexRegex();
}
