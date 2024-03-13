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

    [Fact]
    public void TestNextTokenPeeker()
    {
        const string input = """
let five = 5;
let ten = 10;

let add = fn(x, y) {
    x + y;
};

let result = add(five, ten);
!-/*5;
5 < 10 > 5;

if (5 < 10) {
    return true;
} else {
    return false;
}

10 == 10;
10 != 9;
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
            new ExpectedToken(TokenTypes.BANG, "!"),
            new ExpectedToken(TokenTypes.MINUS, "-"),
            new ExpectedToken(TokenTypes.SLASH, "/"),
            new ExpectedToken(TokenTypes.ASTERISK, "*"),
            new ExpectedToken(TokenTypes.INT, "5"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.INT, "5"),
            new ExpectedToken(TokenTypes.LT, "<"),
            new ExpectedToken(TokenTypes.INT, "10"),
            new ExpectedToken(TokenTypes.GT, ">"),
            new ExpectedToken(TokenTypes.INT, "5"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.IF, "if"),
            new ExpectedToken(TokenTypes.LPAREN, "("),
            new ExpectedToken(TokenTypes.INT, "5"),
            new ExpectedToken(TokenTypes.LT, "<"),
            new ExpectedToken(TokenTypes.INT, "10"),
            new ExpectedToken(TokenTypes.RPAREN, ")"),
            new ExpectedToken(TokenTypes.LBRACE, "{"),
            new ExpectedToken(TokenTypes.RETURN, "return"),
            new ExpectedToken(TokenTypes.TRUE, "true"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.RBRACE, "}"),
            new ExpectedToken(TokenTypes.ELSE, "else"),
            new ExpectedToken(TokenTypes.LBRACE, "{"),
            new ExpectedToken(TokenTypes.RETURN, "return"),
            new ExpectedToken(TokenTypes.FALSE, "false"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.RBRACE, "}"),
            new ExpectedToken(TokenTypes.INT, "10"),
            new ExpectedToken(TokenTypes.EQ, "=="),
            new ExpectedToken(TokenTypes.INT, "10"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.INT, "10"),
            new ExpectedToken(TokenTypes.NOT_EQ, "!="),
            new ExpectedToken(TokenTypes.INT, "9"),
            new ExpectedToken(TokenTypes.SEMICOLON, ";"),
            new ExpectedToken(TokenTypes.EOF, ""),
            ];

        var lexer = new Lexer(input);

        foreach (var (testToken, ixToken) in testTokens.Select((testToken, ixToken) => (testToken, ixToken)))
        {
            var token = lexer.NextToken();

            Assert.True(token.TokenType == testToken.ExpectedType, GetAssertionMessage($"tests[{ixToken}] - TokenType wrong.", testToken, token));
            Assert.True(token.Literal == testToken.ExpectedLiteral, GetAssertionMessage($"tests[{ixToken}] - Literal wrong.", testToken, token));
        }
    }


    //
    // Private methods
    //

    private string GetAssertionMessage(string message, ExpectedToken testToken, Token token)
        => $"{message}\nExpectedType: {testToken.ExpectedType}, ActualType: {token.TokenType}.\nExpectedLiteral: {testToken.ExpectedLiteral}, ActualLiteral: {token.Literal}.";
}