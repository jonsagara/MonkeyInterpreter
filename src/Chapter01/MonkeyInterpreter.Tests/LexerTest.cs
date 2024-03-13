namespace MonkeyInterpreter.Tests;

public class LexerTest
{
    private readonly record struct ExpectedToken(string ExpectedType, string ExpectedLiteral);

    [Fact]
    public void TestNextToken()
    {
        const string input = "=+(){},;";

        ExpectedToken[] testTokens = [
            new ExpectedToken(TokenTypes.ASSIGN, "="),
            new ExpectedToken(TokenTypes.PLUS, "+"),
            new ExpectedToken(TokenTypes.LPAREN, "("),
            new ExpectedToken(TokenTypes.RPAREN, ")"),
            new ExpectedToken(TokenTypes.LBRACE, "{"),
            new ExpectedToken(TokenTypes.RBRACE, "}"),
            new ExpectedToken(TokenTypes.COMMA, ","),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.EOF, ""),
            ];

        var lexer = new Lexer(input);

        foreach (var (testToken, ixToken) in testTokens.Select((testToken, ixToken) => (testToken, ixToken)))
        {
            var token = lexer.NextToken();

            Assert.True(token.TokenType == testToken.ExpectedType, GetAssertionMessage($"tests[{ixToken}] - TokenType wrong", testToken, token));
            Assert.True(token.Literal == testToken.ExpectedLiteral, GetAssertionMessage($"tests[{ixToken}] - Literal wrong", testToken, token));
        }
    }

    [Fact]
    public void TestSmallProgram()
    {
        const string input = """
let five = 5;
let ten = 10;

let add = fn(x, y) {
    x + y;
};

let result = add(five, ten);
""";


        ExpectedToken[] testTokens = [
            new ExpectedToken(TokenTypes.LET, "let"),
            new ExpectedToken(TokenTypes.IDENT, "five"),
            new ExpectedToken(TokenTypes.ASSIGN, "="),
            new ExpectedToken(TokenTypes.INT, "5"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.LET, "let"),
            new ExpectedToken(TokenTypes.IDENT, "ten"),
            new ExpectedToken(TokenTypes.ASSIGN, "="),
            new ExpectedToken(TokenTypes.INT, "10"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.LET, "let"),
            new ExpectedToken(TokenTypes.IDENT, "add"),
            new ExpectedToken(TokenTypes.ASSIGN, "="),
            new ExpectedToken(TokenTypes.FUNCTION, "fn"),
            new ExpectedToken(TokenTypes.LPAREN, "("),
            new ExpectedToken(TokenTypes.IDENT, "x"),
            new ExpectedToken(TokenTypes.COMMA, ","),
            new ExpectedToken(TokenTypes.IDENT, "y"),
            new ExpectedToken(TokenTypes.RPAREN, ")"),
            new ExpectedToken(TokenTypes.LBRACE, "{"),
            new ExpectedToken(TokenTypes.IDENT, "x"),
            new ExpectedToken(TokenTypes.PLUS, "+"),
            new ExpectedToken(TokenTypes.IDENT, "y"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.RBRACE, "}"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.LET, "let"),
            new ExpectedToken(TokenTypes.IDENT, "result"),
            new ExpectedToken(TokenTypes.ASSIGN, "="),
            new ExpectedToken(TokenTypes.IDENT, "add"),
            new ExpectedToken(TokenTypes.LPAREN, "("),
            new ExpectedToken(TokenTypes.IDENT, "five"),
            new ExpectedToken(TokenTypes.COMMA, ","),
            new ExpectedToken(TokenTypes.IDENT, "ten"),
            new ExpectedToken(TokenTypes.RPAREN, ")"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.EOF, ""),
            ];

        var lexer = new Lexer(input);

        foreach (var (testToken, ixToken) in testTokens.Select((testToken, ixToken) => (testToken, ixToken)))
        {
            var token = lexer.NextToken();

            Assert.True(token.TokenType == testToken.ExpectedType, GetAssertionMessage($"tests[{ixToken}] - TokenType wrong", testToken, token));
            Assert.True(token.Literal == testToken.ExpectedLiteral, GetAssertionMessage($"tests[{ixToken}] - Literal wrong", testToken, token));
        }
    }


    //
    // Private methods
    //

    private string GetAssertionMessage(string message, ExpectedToken testToken, Token token)
        => $"{message}. ExpectedType={testToken.ExpectedType}, ActualType={token.TokenType}. ExpectedLiteral={testToken.ExpectedLiteral}, ActualLiteral={token.Literal}.";
}