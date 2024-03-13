﻿namespace MonkeyInterpreter;

public static class TokenTypes
{
    public const string ILLEGAL = "ILLEGAL";
    public const string EOF = "EOF";

    // Identifiers + literals;
    public const string IDENT = "IDENT"; // add, foobar, x, y, ...
    public const string INT = "INT";   // 1343456

    // Operators;
    public const string ASSIGN = "=";
    public const string PLUS = "+";
    public const string MINUS = "-";
    public const string BANG = "!";
    public const string ASTERISK = "*";
    public const string SLASH = "/";

    public const string LT = "<";
    public const string GT = ">";

    public const string EQ = "==";
    public const string NOT_EQ = "!=";

    // Delimiters;
    public const string COMMA = ",";
    public const string SEMICOLON = ";";

    public const string LPAREN = "(";
    public const string RPAREN = ")";
    public const string LBRACE = "{";
    public const string RBRACE = "}";

    // Keywords;
    public const string FUNCTION = "FUNCTION";
    public const string LET = "LET";
    public const string TRUE = "TRUE";
    public const string FALSE = "FALSE";
    public const string IF = "IF";
    public const string ELSE = "ELSE";
    public const string RETURN = "RETURN";
}