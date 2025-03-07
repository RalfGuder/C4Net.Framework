using System.Collections.Generic;
using System.Text;
using C4Net.Framework.Core.Utils;
using C4Net.Framework.Templates.Parser;

namespace C4Net.Framework.Templates.Language
{
    /// <summary>
    /// Delegate for a function, evaluates a token and returns a string.
    /// </summary>
    /// <param name="token">The token.</param>
    /// <param name="container">The container.</param>
    /// <returns></returns>
    public delegate string TokenAction(ParserToken token, TemplateContainer container);

    public class LanguageParser : BaseParser
    {
        #region - Fields -

        /// <summary>
        /// The string builder for the evaluation.
        /// </summary>
        StringBuilder builder = new StringBuilder();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets or sets the command begin string.
        /// </summary>
        /// <value>
        /// The command begin.
        /// </value>
        public string CommandBegin { get; set; }

        /// <summary>
        /// Gets or sets the command end string.
        /// </summary>
        /// <value>
        /// The command end.
        /// </value>
        public string CommandEnd { get; set; }

        /// <summary>
        /// Gets or sets the parameters begin string.
        /// </summary>
        /// <value>
        /// The parameters begin.
        /// </value>
        public string ParametersBegin { get; set; }

        /// <summary>
        /// Gets or sets the parameters end string.
        /// </summary>
        /// <value>
        /// The parameters end.
        /// </value>
        public string ParametersEnd { get; set; }

        /// <summary>
        /// Gets or sets the parameters separator string.
        /// </summary>
        /// <value>
        /// The parameters separator.
        /// </value>
        public string ParametersSeparator { get; set; }

        /// <summary>
        /// Gets or sets the literal delimiter string.
        /// </summary>
        /// <value>
        /// The literal delimiter.
        /// </value>
        public string LiteralDelimiter { get; set; }

        /// <summary>
        /// Gets the list of commands strings and the command that they represent. The same command can be represented by
        /// several command strings.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        public Dictionary<string, LanguageCommandType> Commands { get; private set; }

        /// <summary>
        /// Dictionary of known functions.
        /// </summary>
        /// <value>
        /// The functions.
        /// </value>
        public Dictionary<string, TokenAction> Functions { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageParser"/> class.
        /// </summary>
        public LanguageParser()
            : base((int)LanguageTokenType.Separator, (int)LanguageTokenType.Root, (int)LanguageParserState.Text)
        {
            this.Commands = new Dictionary<string, LanguageCommandType>();
            this.Functions = new Dictionary<string, TokenAction>();
            this.CommandBegin = "[%";
            this.CommandEnd = "%]";
            this.ParametersBegin = "(";
            this.ParametersEnd = ")";
            this.ParametersSeparator = ",";
            this.LiteralDelimiter = "\"";
            this.Commands.Add("foreach", LanguageCommandType.Foreach);
            this.Commands.Add("if", LanguageCommandType.If);
            this.Commands.Add("else", LanguageCommandType.Else);
            this.Commands.Add("end", LanguageCommandType.End);
            this.Functions.Add("IsLast", this.IsLast);
            this.Functions.Add("IsFirst", this.IsFirst);
            this.Functions.Add("IsIndex", this.IsIndex);
            this.Functions.Add("Camelize", this.Camelize);
            this.Functions.Add("Pascalize", this.Pascalize);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Replaces the string using the container.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public static string ReplaceString(string source, TemplateContainer container)
        {
            LanguageParser parser = new LanguageParser();
            return parser.Evaluate(source, container);
        }

        /// <summary>
        /// Initializes the actions defining the flow.
        /// </summary>
        protected override void InitializeActions()
        {
            this.RegisterAction((int)LanguageParserState.Text, this.CommandBegin, (int)LanguageTokenType.Text, (int)LanguageParserState.Command);
            this.RegisterAction((int)LanguageParserState.Command, this.CommandEnd, (int)LanguageTokenType.Identifier, (int)LanguageParserState.Text);
            this.RegisterAction((int)LanguageParserState.Command, this.ParametersBegin, (int)LanguageTokenType.Identifier, (int)LanguageParserState.Command);
            this.RegisterAction((int)LanguageParserState.Command, this.ParametersSeparator, (int)LanguageTokenType.Identifier, (int)LanguageParserState.Command);
            this.RegisterAction((int)LanguageParserState.Command, this.ParametersEnd, (int)LanguageTokenType.Identifier, (int)LanguageParserState.Command);
            this.RegisterAction((int)LanguageParserState.Command, this.LiteralDelimiter, (int)LanguageTokenType.Identifier, (int)LanguageParserState.Literal);
            this.RegisterAction((int)LanguageParserState.Literal, this.LiteralDelimiter, (int)LanguageTokenType.Literal, (int)LanguageParserState.Command);
            this.RegisterAction((int)LanguageParserState.Text, string.Empty, (int)LanguageTokenType.Text, (int)LanguageTokenType.Text);
        }

        /// <summary>
        /// Builds the token tree.
        /// </summary>
        protected override void BuildTree()
        {
            ParserToken listToken = new ParserToken((int)LanguageTokenType.Root, "");
            ParserToken currentToken = listToken;
            foreach (ParserToken token in this.tokens)
            {
                switch (token.TokenType)
                {
                    case (int)LanguageTokenType.Root: break;
                    case (int)LanguageTokenType.Separator:
                        if (token.Content.Equals(this.ParametersBegin))
                        {
                            currentToken = currentToken.Parameters[currentToken.Parameters.Count - 1];
                        }
                        else if (token.Content.Equals(this.ParametersEnd))
                        {
                            currentToken = currentToken.Parent;
                        }
                        break;
                    case (int)LanguageTokenType.Text:
                    case (int)LanguageTokenType.Identifier:
                        if (token.Content != string.Empty)
                        {
                            currentToken.AddParameter(token);
                        }
                        break;
                    case (int)LanguageTokenType.Literal:
                        currentToken.AddParameter(token);
                        break;
                }
            }

            Stack<ParserToken> tokenStack = new Stack<ParserToken>();
            currentToken = this.root;
            foreach (ParserToken token in listToken.Parameters)
            {
                if (token.TokenType == (int)LanguageTokenType.Identifier)
                {
                    if (this.Commands.ContainsKey(token.Content))
                    {
                        LanguageCommandType commandType = this.Commands[token.Content];
                        switch (commandType)
                        {
                            case LanguageCommandType.Foreach:
                            case LanguageCommandType.If:
                            case LanguageCommandType.Else:
                                currentToken.AddSon(token);
                                tokenStack.Push(currentToken);
                                currentToken = token;
                                break;
                            case LanguageCommandType.End:
                                currentToken = tokenStack.Pop();
                                if (this.Commands.ContainsKey(currentToken.Content))
                                {
                                    commandType = this.Commands[currentToken.Content];
                                    if (commandType == LanguageCommandType.Else)
                                    {
                                        currentToken = tokenStack.Pop();
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        currentToken.AddSon(token);
                    }
                }
                else
                {
                    currentToken.AddSon(token);
                }
            }
        }

        /// <summary>
        /// Evaluates the string parsing it, and then the value using the template container.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private string Evaluate(string str, TemplateContainer container)
        {
            builder.Clear();
            ParserToken rootToken = this.Parse(str);
            this.Visit(container, rootToken);
            return builder.ToString();
        }

        /// <summary>
        /// Visits the specified token using the container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="token">The token.</param>
        private void Visit(TemplateContainer container, ParserToken token)
        {
            if (token.TokenType == (int)LanguageTokenType.Root)
            {
                foreach (ParserToken nested in token.Sons)
                {
                    this.Visit(container, nested);
                }
            }
            if (token.TokenType == (int)LanguageTokenType.Text)
            {
                this.builder.Append(token.Content);
            }
            else if (token.TokenType == (int)LanguageTokenType.Identifier)
            {
                this.EvaluateIdentifier(container, token);
            }
        }

        /// <summary>
        /// Evaluates one identifier token.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="token">The token.</param>
        private void EvaluateIdentifier(TemplateContainer container, ParserToken token)
        {
            if (this.Commands.ContainsKey(token.Content))
            {
                LanguageCommandType commandType = this.Commands[token.Content];
                switch (commandType)
                {
                    case LanguageCommandType.Foreach:
                        if (token.Parameters.Count > 0)
                        {
                            object obj = container.GetByPath(token.Parameters[0].Content);
                            if ((obj != null) && (obj is TemplateContainer))
                            {
                                TemplateContainer sonContainer = (TemplateContainer)obj;
                                if (sonContainer.IsArray)
                                {
                                    int index = 0;
                                    string maxIndex = (sonContainer.ArrayValues.Count - 1).ToString();
                                    foreach (TemplateContainer gransonContainer in sonContainer.ArrayValues)
                                    {
                                        gransonContainer.AddAttribute("Index", index.ToString());
                                        gransonContainer.AddAttribute("MaxIndex", maxIndex);
                                        foreach (ParserToken son in token.Sons)
                                        {
                                            this.Visit(gransonContainer, son);
                                        }
                                        index++;
                                    }
                                }
                                else
                                {
                                    //foreach (ParserToken son in token.Sons)
                                    //{
                                    //    this.Visit(sonContainer, son);
                                    //}
                                }
                            }
                        }
                        break;
                    case LanguageCommandType.If:
                        if (token.Parameters.Count > 0)
                        {
                            if (EvaluateCondition(token.Parameters, container))
                            {
                                foreach (ParserToken son in token.Sons)
                                {
                                    this.Visit(container, son);
                                }
                            }
                        }
                        break;
                    case LanguageCommandType.Else:
                        break;
                }
            }
            else
            {
                object obj = container.GetByPath(token.Content);
                if (obj != null)
                {
                    builder.Append(obj.ToString());
                }
                else
                {
                    if (this.Functions.ContainsKey(token.Content))
                    {
                        builder.Append(this.Functions[token.Content](token, container));
                    }
                }
            }
        }

        /// <summary>
        /// Pops a token from a string.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private string Pop(ref string token)
        {
            if (token != string.Empty)
            {
                int i = token.IndexOf(' ');
                string result = string.Empty;
                if (i > 0)
                {
                    result = token.Substring(0, i);
                    token = token.Remove(0, i + 1);
                    return result;
                }
                else if (i == 0)
                {
                    token = token.Remove(0, 1);
                    return this.Pop(ref token);
                }
                else
                {
                    result = token;
                    token = string.Empty;
                    return result;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Parses the condition token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private List<string> ParseConditionToken(string token)
        {
            List<string> result = new List<string>();
            while (token != string.Empty)
            {
                result.Add(this.Pop(ref token));
            }
            return result;
        }

        /// <summary>
        /// Evaluates a condition.
        /// </summary>
        /// <param name="tokens">The tokens.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private bool EvaluateCondition(List<ParserToken> tokens, TemplateContainer container)
        {
            List<string> parameters = new List<string>();
            foreach (ParserToken token in tokens)
            {
                if (token.TokenType == (int)LanguageTokenType.Identifier)
                {
                    List<string> list = this.ParseConditionToken(token.Content);
                    foreach (string item in list)
                    {
                        object obj = container.GetByPath(item);
                        if (obj != null)
                        {
                            parameters.Add(obj.ToString());
                        }
                        else
                        {
                            if (this.Functions.ContainsKey(item))
                            {
                                parameters.Add(this.Functions[item](token, container));
                            }
                            else
                            {
                                parameters.Add(item);
                            }
                        }
                    }
                }
                else if (token.TokenType == (int)LanguageTokenType.Literal)
                {
                    parameters.Add(token.Content);
                }
            }
            if (parameters.Count == 1)
            {
                return ((parameters[0].ToLower() == "true") || (parameters[0] == "1"));
            }
            else if (parameters[0] == "!")
            {
                return (!((parameters[1].ToLower() == "true") || (parameters[1] == "1")));
            }
            else
            {
                if (parameters[1] == "==")
                {
                    return (parameters[0] == parameters[2]);
                }
                else if (parameters[1] == "!=")
                {
                    return (parameters[0] != parameters[2]);
                }
            }
            return true;
        }

        /// <summary>
        /// Evaluates one parameter, resolving to a string.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private string EvaluateParameter(TemplateContainer container, ParserToken parameter)
        {
            object obj = container.GetByPath(parameter.Content);
            if (obj != null)
            {
                return obj.ToString();
            }
            else
            {
                if (this.Functions.ContainsKey(parameter.Content))
                {
                    return this.Functions[parameter.Content](parameter, container);
                }
                else
                {
                    return parameter.Content;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified token is last.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private string IsLast(ParserToken token, TemplateContainer container)
        {
            return (container.Attributes["Index"] == container.Attributes["MaxIndex"]).ToString();
        }

        /// <summary>
        /// Determines whether the specified token is first.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private string IsFirst(ParserToken token, TemplateContainer container)
        {
            return (container.Attributes["Index"] == "0").ToString();
        }

        /// <summary>
        /// Determines whether the specified token has the specified index.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private string IsIndex(ParserToken token, TemplateContainer container)
        {
            string value = this.EvaluateParameter(container, token.Parameters[0]);
            return (container.Attributes["Index"] == value).ToString();
        }

        /// <summary>
        /// Camelizes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private string Camelize(ParserToken token, TemplateContainer container)
        {
            string value = this.EvaluateParameter(container, token.Parameters[0]);
            return StringUtil.Camelize(value);
        }

        /// <summary>
        /// Pascalizes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private string Pascalize(ParserToken token, TemplateContainer container)
        {
            string value = this.EvaluateParameter(container, token.Parameters[0]);
            return StringUtil.Pascalize(value);
        }

        #endregion
    }
}
