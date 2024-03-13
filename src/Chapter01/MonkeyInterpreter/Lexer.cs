namespace MonkeyInterpreter;

public class Lexer
{
    public string Input { get; }

    /// <summary>
    /// Current position in input (points to current char).
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    /// Current reading position in input (after current char).
    /// </summary>
    public int ReadPosition { get; set; }

    /// <summary>
    /// Current char under examination.
    /// </summary>
    public char CurrentChar { get; set; }

    public Lexer(string input)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(input);

        Input = input;

        // Read the first character so that our lexer is fully initialized.
        ReadChar();
    }


    //
    // Public methods
    //

    public Token NextToken()
    {
        // Tokenize the current character.
        Token token = Token.Empty;

        switch (CurrentChar)
        {
            case '=':
                token = NewToken(tokenType: TokenTypes.ASSIGN, currentChar: CurrentChar);
                break;

            case ';':
                token = NewToken(tokenType: TokenTypes.SEMICOLON, currentChar: CurrentChar);
                break;

            case '(':
                token = NewToken(tokenType: TokenTypes.LPAREN, currentChar: CurrentChar);
                break;

            case ')':
                token = NewToken(tokenType: TokenTypes.RPAREN, currentChar: CurrentChar);
                break;

            case ',':
                token = NewToken(tokenType: TokenTypes.COMMA, currentChar: CurrentChar);
                break;

            case '+':
                token = NewToken(tokenType: TokenTypes.PLUS, currentChar: CurrentChar);
                break;

            case '{':
                token = NewToken(tokenType: TokenTypes.LBRACE, currentChar: CurrentChar);
                break;

            case '}':
                token = NewToken(tokenType: TokenTypes.RBRACE, currentChar: CurrentChar);
                break;

            case '\0':
                token = new Token(TokenType: TokenTypes.EOF, Literal: string.Empty);
                break;
        }

        // Advance to the next character in the input.
        ReadChar();

        return token;
    }


    //
    // Private Methods
    //

    /// <summary>
    /// Read the next character and advance the current and next character positions.
    /// </summary>
    private void ReadChar()
    {
        if (ReadPosition >= Input.Length)
        {
            // We have read all characters. Set CurrentChar to the null terminator.
            CurrentChar = '\0';
        }
        else
        {
            // Read the next character.
            CurrentChar = Input[ReadPosition];
        }

        // Advance the current character and the next character positions.
        Position = ReadPosition;
        ReadPosition++;
    }

    private Token NewToken(string tokenType, char currentChar)
        => new Token(TokenType: tokenType, Literal: currentChar.ToString());
}
