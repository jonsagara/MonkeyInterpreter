namespace MonkeyInterpreter.Tests;

public class LexerTest
{
    private readonly record struct ExpectedToken(string ExpectedType, string ExpectedLiteral);

    [Fact]
    public void TestNextToken()
    {
        const string input = "=+(){},;";

        ExpectedToken[] tokenTests = [
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

        foreach (var (tokenTest, ixToken) in tokenTests.Select((tokenTest, ixToken) => (tokenTest, ixToken)))
        {
            var token = lexer.NextToken();

            Assert.True(token.TokenType == tokenTest.ExpectedType, $"tests[{ixToken}] - TokenType wrong. Expected={tokenTest.ExpectedType}, Actual={token.TokenType}.");
            Assert.True(token.Literal == tokenTest.ExpectedLiteral, $"tests[{ixToken}] - Literal wrong. Expected={tokenTest.ExpectedLiteral}, Actual={token.Literal}.");
        }
    }

//    [Fact]
//    public void TBD()
//    {
//        const string code = """
//let five = 5;
//let ten = 10;

//let add = fn(x, y) {
//    x + y;
//};

//let result = add(five, ten);
//""";

//    }
}