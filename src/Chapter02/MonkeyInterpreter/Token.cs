namespace MonkeyInterpreter.Chapter02;

public readonly record struct Token(
    string TokenType,
    string Literal
    )
{
    public static readonly Token Empty = new Token(TokenType: string.Empty, Literal: string.Empty);
}
