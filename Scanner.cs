﻿using System;
using System.Collections.Generic;

namespace jloxcs
{
    class Scanner
    {
        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>();
        private static bool _keywordsInitialized = false;

        static void _initializeKeywords()
        {
            if (_keywordsInitialized)
                return;

            //keywords = new Dictionary<string, TokenType>();
            keywords.Add("and", TokenType.AND);
            keywords.Add("class", TokenType.CLASS);
            keywords.Add("else", TokenType.ELSE);
            keywords.Add("false", TokenType.FALSE);
            keywords.Add("for", TokenType.FOR);
            keywords.Add("fun", TokenType.FUN);
            keywords.Add("if", TokenType.IF);
            keywords.Add("nil", TokenType.NIL);
            keywords.Add("or", TokenType.OR);
            keywords.Add("print", TokenType.PRINT);
            keywords.Add("return", TokenType.RETURN);
            keywords.Add("super", TokenType.SUPER);
            keywords.Add("this", TokenType.THIS);
            keywords.Add("true", TokenType.TRUE);
            keywords.Add("var", TokenType.VAR);
            keywords.Add("while", TokenType.WHILE);

            _keywordsInitialized = true;
        }

        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        public Scanner(string source)
        {
            this.source = source;
            _initializeKeywords();
        }

        public List<Token> scanTokens()
        {
            while (!isAtEnd())
            {
                // We are at the beginning of the next lexeme.
                start = current;
                scanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '(':
                    addToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    addToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    addToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    addToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    addToken(TokenType.COMMA);
                    break;
                case '.':
                    addToken(TokenType.DOT);
                    break;
                case '-':
                    addToken(TokenType.MINUS);
                    break;
                case '+':
                    addToken(TokenType.PLUS);
                    break;
                case ';':
                    addToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    addToken(TokenType.STAR);
                    break;
                // two-char-tokens
                case '!':
                    addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (match('/'))
                    {
                        // a comment goes until the end of the line.
                        while (peek() != '\n' && !isAtEnd())
                            advance();
                    }
                    else
                    {
                        addToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // ignore  whitespace
                    break;
                case '\n':
                    line++;
                    break;
                case '"':
                    string_();
                    break;
                default:
                    if (isDigit(c))
                    {
                        number();
                    }
                    else if (isAlpha(c))
                    {
                        identifier();
                    }
                    else
                    {
                        Lox.error(line, "Unexpected character.");
                    }
                    break;
            }
        } //scanToken

        private void identifier()
        {
            while (isAlphaNumeric(peek()))
                advance();

            // See if the identifier is a reserved word
            string text = source.Substring(start, current - start); // second argument is length in Csharp

            TokenType type;
            if (!keywords.ContainsKey(text))
                type = TokenType.IDENTIFIER;
            else
                type = keywords[text];

            addToken(type);
        }

        private void number()
        {
            while (isDigit(peek()))
                advance();

            // Look for a fractional part.
            if (peek() == '.' && isDigit(peekNext()))
            {
                // Consue the "."
                advance();
                while (isDigit(peek()))
                    advance();
            }

            addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
        }

        private void string_()
        {
            while (peek() != '"' && !isAtEnd())
            {
                if (peek() == '\n')
                    line++;
                advance();
            }

            // Unterminated string.
            if (isAtEnd())
            {
                Lox.error(line, "Unterminated string.");
                return;
            }

            // The closing "
            advance();

            // Trim the surrounding quotes
            string value = source.Substring(start + 1, current - start - 2);
            addToken(TokenType.STRING, value);
        }

        bool match(char expected)
        {
            if (isAtEnd())
                return false;
            if (source[current] != expected)
                return false;

            current++;
            return true;
        }

        private char peek()
        {
            if (isAtEnd())
                return '\0';
            return source[current];
        }

        private char peekNext()
        {
            if (current + 1 >= source.Length)
                return '\0';
            return source[current + 1];
        }

        private bool isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }

        private bool isAlphaNumeric(char c)
        {
            return isAlpha(c) || isDigit(c);
        }

        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool isAtEnd()
        {
            return current >= source.Length;
        }

        private char advance()
        {
            current++;
            return source[current - 1];
        }

        private void addToken(TokenType type)
        {
            addToken(type, null);
        }

        private void addToken(TokenType type, object literal)
        {
            string text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }
    }
}
