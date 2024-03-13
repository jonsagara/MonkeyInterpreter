using MonkeyInterpreter;

const string Prompt = ">> ";

Console.WriteLine("Hello! This is the Monkey programming language!");
Console.WriteLine("Feel free to type in commands");

string? input = null;
Console.Write(Prompt);

while ((input = Console.ReadLine()) != null)
{
    var lexer = new Lexer(input);

    for (var token = lexer.NextToken(); token.TokenType != TokenTypes.EOF; token = lexer.NextToken())
    {
        Console.WriteLine($"{token}");
    }

    Console.Write(Prompt);
}
