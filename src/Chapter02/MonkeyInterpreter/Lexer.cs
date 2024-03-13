using System.Collections.Frozen;

namespace MonkeyInterpreter.Chapter02;

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

        SkipWhiteSpace();

        switch (CurrentChar)
        {
            //
            // Easily identifiable single character tokens.
            //

            case '=':
                if (PeekChar() == '=')
                {
                    // Save the first equals sign.
                    var firstEquals = CurrentChar;

                    // Read the subsequent equals sign and update the positions.
                    ReadChar();

                    // The literal is the combination of the two equals signs.
                    var literal = $"{firstEquals}{CurrentChar}";

                    token = new Token(TokenType: TokenTypes.EQ, Literal: literal);
                }
                else
                {
                    // The next character is not an equals sign. Treat the current character 
                    //   as an assignment operator.
                    token = NewToken(tokenType: TokenTypes.ASSIGN, currentChar: CurrentChar);
                }
                
                break;

            case '+':
                token = NewToken(tokenType: TokenTypes.PLUS, currentChar: CurrentChar);
                break;

            case '-':
                token = NewToken(tokenType: TokenTypes.MINUS, currentChar: CurrentChar);
                break;

            case '!':
                if (PeekChar() == '=')
                {
                    // Save the exclamation point.
                    var exclamationPoint = CurrentChar;

                    // Read the subsequent equals sign and update the positions.
                    ReadChar();

                    // The literal is the combination of the exclamation point and equals sign.
                    var literal = $"{exclamationPoint}{CurrentChar}";

                    token = new Token(TokenType: TokenTypes.NOT_EQ, Literal: literal);
                }
                else
                {
                    // The next character is not an equals sign. Treat the current character 
                    //   as an assignment operator.
                    token = NewToken(tokenType: TokenTypes.BANG, currentChar: CurrentChar);
                }
                
                break;

            case '*':
                token = NewToken(tokenType: TokenTypes.ASTERISK, currentChar: CurrentChar);
                break;

            case '/':
                token = NewToken(tokenType: TokenTypes.SLASH, currentChar: CurrentChar);
                break;

            case '<':
                token = NewToken(tokenType: TokenTypes.LT, currentChar: CurrentChar);
                break;

            case '>':
                token = NewToken(tokenType: TokenTypes.GT, currentChar: CurrentChar);
                break;

            case ';':
                token = NewToken(tokenType: TokenTypes.SEMICOLON, currentChar: CurrentChar);
                break;

            case ',':
                token = NewToken(tokenType: TokenTypes.COMMA, currentChar: CurrentChar);
                break;

            case '(':
                token = NewToken(tokenType: TokenTypes.LPAREN, currentChar: CurrentChar);
                break;

            case ')':
                token = NewToken(tokenType: TokenTypes.RPAREN, currentChar: CurrentChar);
                break;

            case '{':
                token = NewToken(tokenType: TokenTypes.LBRACE, currentChar: CurrentChar);
                break;

            case '}':
                token = NewToken(tokenType: TokenTypes.RBRACE, currentChar: CurrentChar);
                break;

            case '\0':
                // Reminder: If we have read the whole input string, ReadChar sets CurrentChar to 
                //   the null terminator character. C# strings don't have a null terminator.
                token = new Token(TokenType: TokenTypes.EOF, Literal: string.Empty);
                break;


            //
            // Identifiers or keywords
            //

            default:
                if (IsLetter(CurrentChar))
                {
                    // SPECIAL CASE: ReadIdentifier advances the character positions for us, so we return
                    //   here. We don't want to call ReadChar() again.
                    var identifier = ReadIdentifier();
                    var tokenType = TokenTypes.GetIdentifierTokenType(identifier);
                    return new Token(TokenType: tokenType, Literal: identifier);
                }
                else if (IsDigit(CurrentChar))
                {
                    // SPECIAL CASE: ReadNumber advances the character positions for us, so we return
                    //   here. We don't want to call ReadChar() again.
                    var number = ReadNumber();
                    return new Token(TokenType: TokenTypes.INT, Literal: number);
                }
                else
                {
                    // It's not a recognized single-character toke, and it's not an identifier. It's an
                    //   illegal character.
                    token = NewToken(tokenType: TokenTypes.ILLEGAL, currentChar: CurrentChar);
                }
                break;
        }

        // Advance to the next character in the input.
        ReadChar();

        return token;
    }


    //
    // Private Methods
    //

    private static readonly FrozenSet<char> _whiteSpaceChars = new HashSet<char>
    {
        ' ',
        '\t',
        '\n',
        '\r',
    }
    .ToFrozenSet();

    /// <summary>
    /// Read characters from input until we encounter a non-white space character.
    /// </summary>
    private void SkipWhiteSpace()
    {
        while (_whiteSpaceChars.Contains(CurrentChar))
        {
            ReadChar();
        }
    }

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

    /// <summary>
    /// Return the character immediately following the current character. If there is no next character, return
    /// the null terminator.
    /// </summary>
    private char PeekChar()
    {
        if (ReadPosition >= Input.Length)
        {
            // The "next character" position is past the end of the input string. There are no more characters
            //   to peek. Return a null terminator to indicate this.
            return '\0';
        }

        // Return the next character to be read.
        return Input[ReadPosition];
    }

    private string ReadIdentifier()
    {
        // Start of the identifier.
        var ixStart = Position;

        // Read the input until we find a non-letter character.
        while (IsLetter(CurrentChar))
        {
            ReadChar();
        }

        // One character past the end of the identifier.
        var ixEnd = Position;

        // Start of range is inclusive; end is exclusive. Gets the entire identifier.
        return Input[ixStart..ixEnd];
    }

    /// <summary>
    /// Helper method that creates a new <see cref="Token"/> instance, converting the <paramref name="currentChar"/>
    /// to a string.
    /// </summary>
    private Token NewToken(string tokenType, char currentChar)
        => new Token(TokenType: tokenType, Literal: currentChar.ToString());

    /// <summary>
    /// Returns true if <paramref name="currentChar"/> is in [a-zA-Z_]; false otherwise.
    /// </summary>
    private bool IsLetter(char currentChar)
    {
        if ('a' <= currentChar && currentChar <= 'z')
        {
            return true;
        }

        if ('A' <= currentChar && currentChar <= 'Z')
        {
            return true;
        }

        if (currentChar == '_')
        {
            return true;
        }

        return false;
    }

    private string ReadNumber()
    {
        // Start of the number string.
        var ixStart = Position;

        // Read the input until we find a non-digit character.
        while (IsDigit(CurrentChar))
        {
            ReadChar();
        }

        // One character past the end of the identifier.
        var ixEnd = Position;

        // Start of range is inclusive; end is exclusive. Gets the entire number.
        return Input[ixStart..ixEnd];
    }

    /// <summary>
    /// Returns true if <paramref name="currentChar"/> is in [0-9]; false otherwise.
    /// </summary>
    private bool IsDigit(char currentChar)
        => '0' <= currentChar && currentChar <= '9';
}
