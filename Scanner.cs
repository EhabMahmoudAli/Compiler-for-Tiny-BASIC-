using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public enum Token_Class
{
    Integer, LParanthesis, Comma, RParanthesis, LCurlybraces, RCurlybraces, Return, Semicolon,
    RightCurlybraces, Read, Repeat, MinusOp, Number, Write, Doublequot, EndL, Colon,
    PlusOp, Until, EqualOp, String, Float, Dot, DivideOp, IF, GreaterThanOp, Error,
    OrOp, LessThanOP, AndOp, Then, ElseIF, Else, End, MultiplyOp, NotEqual, Assign, Main, Identefier, Comment
}
namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        bool tok_flag = false;
        public List<Token> Tokens = new List<Token>();
        public List<Token> errors_list = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("int", Token_Class.Integer);
            ReservedWords.Add("main", Token_Class.Main);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("endl", Token_Class.EndL);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("if", Token_Class.IF);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("elseif", Token_Class.ElseIF);


            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOP);
            Operators.Add("<>", Token_Class.NotEqual);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("{", Token_Class.LCurlybraces);
            Operators.Add("}", Token_Class.RCurlybraces);
            Operators.Add(":=", Token_Class.Assign);
            Operators.Add(":", Token_Class.Colon);
            Operators.Add("||", Token_Class.OrOp);
            Operators.Add("&&", Token_Class.AndOp);




        }

        public void StartScanning(string SourceCode)
        {

            string tmp = "";
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = "";

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'Z' || CurrentChar >= 'a' && CurrentChar <= 'z') //if you read a character
                {

                    for (j = i; j < SourceCode.Length; j++) // WORDS AND IDENTEFIERS
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar >= 'A' && CurrentChar <= 'Z' || CurrentChar >= 'a' && CurrentChar <= 'z' || CurrentChar >= '0' && CurrentChar <= '9')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        else
                            break;
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                    /*if (tok_flag == false)
                    {
                        Errors.Error_List.Add(CurrentLexeme.ToString());
                    }*/
                }

                else if (CurrentChar >= '0' && CurrentChar <= '9') //NUMBERS
                {
                    for (j = i; j < SourceCode.Length; j++)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar >= '0' && CurrentChar <= '9' || CurrentChar == '.' || (CurrentChar >= 'A' && CurrentChar <= 'z'))
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        else
                            break;
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);

                }
                else if (CurrentChar == '"')// STRINGS
                {
                    j = i;
                    CurrentLexeme = "";
                    CurrentLexeme += CurrentChar;
                    CurrentChar = SourceCode[++j];
                    for (; CurrentChar != '"';)
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        CurrentChar = SourceCode[j];
                    }
                    CurrentLexeme += CurrentChar.ToString();
                    i = j;
                    FindTokenClass(CurrentLexeme);

                }
                /*else if (CurrentChar == '{') // BLOCK OF CODE
                {
                    j = i;
                    CurrentChar = SourceCode[++j];
                    CurrentLexeme += CurrentChar.ToString();
                    for(;CurrentChar != '}';j++)
                    {
                        
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    i = j - 1;
                    FindTokenClass(CurrentLexeme);
                   *//*if(tok_flag == false)
                    {
                        Errors.Error_List.Add(CurrentLexeme.ToString());
                    }*//*
                }*/
                /*else if (CurrentChar == '/') // COMMENT STATMENT
                {
                    j = i;
                    int k = i;
                    int z;
                    CurrentLexeme += CurrentChar.ToString();
                    CurrentChar = SourceCode[++j];// TO be revised
                    if (CurrentChar == '*')
                    {
                    A:
                        //CurrentLexeme += CurrentChar.ToString();
                        CurrentChar = SourceCode[++j];
                        //CurrentLexeme += CurrentChar.ToString();
                        while (CurrentChar != '*')
                        {
                            j++;
                            CurrentChar = SourceCode[j];
                            //CurrentLexeme += CurrentChar.ToString();
                        }
                        //CurrentLexeme += CurrentChar.ToString();
                        CurrentChar = SourceCode[++j];
                        if (CurrentChar == '/')
                        {
                            z = j;
                            CurrentLexeme += SourceCode.Substring(k-1,j-k);
                            FindTokenClass(CurrentLexeme);
                            
                        }
                        else
                        {
                            goto A;
                        }
                    }*/
                else if (j + 1 < SourceCode.Length && SourceCode[j] == '/' && SourceCode[j + 1] == '*')  ///detect comments
                {
                    tmp += "/*";
                    j += 2;

                    for (; j < SourceCode.Length && SourceCode[j] != '*';)
                    {
                        //if (SourceCode[j] == '\r' || SourceCode[j] == '\n') ///if the line ends you can detect other things
                        //{
                        //    break;
                        //}

                        tmp += SourceCode[j++];
                    }

                    for (; j + 1 < SourceCode.Length && (SourceCode[j] != '*' || SourceCode[j + 1] != '/');)
                    {
                        //if (SourceCode[j] == '\r' || SourceCode[j] == '\n')
                        //{
                        //    break;
                        //}

                        tmp += SourceCode[j++];
                    }

                    if (j + 1 < SourceCode.Length /* && SourceCode[j] == '*' && SourceCode[j + 1] == '/' */)
                    {
                        tmp += "*/";
                    }
                    else
                    {
                        if (j < SourceCode.Length /* && SourceCode[j] != '\r' && SourceCode[j] == '\n' */) //edit
                        {
                            tmp += SourceCode[j];
                        }

                        //errors_list.Add(tmp);
                    }

                    i = j + 1;
                    string pattern_comment = @"/\*[\s\S]*?\*/";
                    Regex re_for_comment = new Regex(pattern_comment);

                    if (re_for_comment.IsMatch(tmp))
                    {
                        tmp = "";
                        continue;
                    }

                    FindTokenClass(tmp);
                    tmp = "";


                }


                //////////////////////////////////
                /* else
                     {
                         CurrentChar = SourceCode[i];
                         FindTokenClass(CurrentLexeme);
                     }
                     //////////////////////////////////
                 }*/
                /*else if (CurrentChar == '+' || CurrentChar == '-' || CurrentChar == '*') // (+ | / | - | *)          //  ( < | > | = | <> )
                {
                    CurrentLexeme += CurrentChar.ToString();
                    FindTokenClass(CurrentLexeme);
                    *//*if (tok_flag == false)
                    {
                        Errors.Error_List.Add(CurrentLexeme.ToString());
                    }*//*
                }*/
                else if (CurrentChar == '<' || CurrentChar == '>' || CurrentChar == '=') // (+ | / | - | *)          //  ( < | > | = | <> )
                {
                    j = i;
                    CurrentLexeme += CurrentChar.ToString();
                    if (CurrentChar == '<')
                    {
                        CurrentChar = SourceCode[++j];// To be revised
                        if (CurrentChar == '>')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                            i++;
                            //FindTokenClass(CurrentLexeme);
                        }
                    }
                    if (CurrentChar == '>')
                    {
                        CurrentChar = SourceCode[++j];// To be revised
                        if (CurrentChar == '=')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                            i++;
                            //FindTokenClass(CurrentLexeme);
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                    /*if (tok_flag == false)
                    {
                        Errors.Error_List.Add(CurrentLexeme.ToString());
                    }*/
                }
                else if (CurrentChar == '&') //&&
                {
                    j = i;
                    CurrentLexeme += CurrentChar.ToString();
                    CurrentChar = SourceCode[++j];
                    if (CurrentChar == '&')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        //FindTokenClass(CurrentLexeme);
                        i++;
                    }
                    FindTokenClass(CurrentLexeme);
                    /*if (tok_flag == false)
                    {
                        Errors.Error_List.Add(CurrentLexeme.ToString());
                    }*/
                }
                else if (CurrentChar == '|')//               ||
                {
                    j = i;
                    CurrentLexeme += CurrentChar.ToString();
                    CurrentChar = SourceCode[++j];
                    if (CurrentChar == '|')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        i++;
                        // FindTokenClass(CurrentLexeme);
                    }
                    FindTokenClass(CurrentLexeme);
                    /*if (tok_flag == false)
                    {
                        Errors.Error_List.Add(CurrentLexeme.ToString());
                    }*/
                }
                else if (CurrentChar == ':')//           :=
                {
                    j = i;
                    CurrentLexeme += CurrentChar.ToString();
                    CurrentChar = SourceCode[++j];
                    if (CurrentChar == '=')
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        //FindTokenClass(CurrentLexeme);
                        // continue;
                        i++;
                    }
                    FindTokenClass(CurrentLexeme);
                    /*if (tok_flag == false)
                    {
                        Errors.Error_List.Add(CurrentLexeme.ToString());
                    }*/
                }
                else if (CurrentChar == '_')
                {
                    CurrentLexeme += CurrentChar.ToString();
                    FindTokenClass(CurrentLexeme);
                }
                else if (CurrentChar == '.')
                {
                    j = i;
                    CurrentLexeme += CurrentChar.ToString();
                    CurrentChar = SourceCode[++j];
                    for (; CurrentChar != ' ';)
                    {
                        CurrentLexeme += CurrentChar.ToString();
                        j++;
                        CurrentChar = SourceCode[j];
                    }
                    //CurrentLexeme += CurrentChar.ToString();
                    i = j;
                    FindTokenClass(CurrentLexeme);
                }
                else// ANY THING ELSE
                {
                    //CurrentLexeme += CurrentChar.ToString();

                    CurrentLexeme += CurrentChar.ToString();
                    FindTokenClass(CurrentLexeme);


                    /*if (tok_flag == false)
                    {
                        Errors.Error_List.Add(CurrentLexeme.ToString());
                    }*/
                }
            }

            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if (isasdString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
                //tok_flag = true;
                //  return;
            }
            else if (ReservedWords.ContainsKey(Lex))
            {

                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);

            }
            else if (Operators.ContainsKey(Lex))
            {

                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
                //return;
            }
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identefier;
                Tokens.Add(Tok);
                //tok_flag = true;

            }
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
                //tok_flag = true;

            }
            else
            {
                /*ok.token_type = Token_Class.Error;
                Tokens.Add(Tok);
                Console.WriteLine("svsds");*/
                Errors.Error_List.Add(Lex);
                //Tok.token_type = Token_Class.Error;
                //Tokens.Add(Tok);
                //tok_flag = false;*/

                //return;
            }
            /*else if(isComment(Lex))
            {
                *//*Tok.token_type = Token_Class.Comment;
               // Errors.Error_List.Add(Lex);*//*
               
                //tok_flag = true;
                //return;
            }*/
        }



        bool isIdentifier(string Lex)
        {
            bool isValid = true;
            // Check if the lex is an identifier or not
            // Regex re_for_id = new Regex(@"^[a-zA-Z][0-9a-zA-Z]*$");//1231asdasd not Handled
            Regex re_for_id = new Regex(@"^[a-zA-Z][0-9a-zA-Z]*$");//1231asdasd not Handled
            if (re_for_id.IsMatch(Lex) == false)
            {
                isValid = false;
            }

            return isValid;
        }
        bool isNumber(string Lex)
        {
            bool isValid = true;
            Regex re_for_const = new Regex(@"^[0-9]+(\.[0-9]+)?$");
            if (re_for_const.IsMatch(Lex) == false)
            {
                isValid = false;
            }
            // Check if the lex is a constant (Number) or not.

            return isValid;
        }
        bool isasdString(string Lex)
        {
            bool isValid = true;
            string pattern = "^\"([^\"]+)\"$";
            //string pattern = @"^[A-Za-z0-9]+$";
            Regex re_for_string = new Regex(pattern);
            if (re_for_string.IsMatch(Lex) == false)
            {
                isValid = false;
            }
            return isValid;
        }
        bool isComment(string Lex)
        {
            bool isValid = true;
            Regex re_for_comment = new Regex(@"^/\*(.*\n*.*)*\*//*$");
            if (re_for_comment.IsMatch(Lex) == true)
            {
                isValid = false;
            }
            return isValid;
        }
    }
}
