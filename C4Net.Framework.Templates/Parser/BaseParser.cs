using System.Collections.Generic;
using System.Text;

namespace C4Net.Framework.Templates.Parser
{
    public abstract class BaseParser
    {
        #region - Fields -

        /// <summary>
        /// The flow definition of the state machine.
        /// </summary>
        private Dictionary<int, ParserStateActions> flow = new Dictionary<int, ParserStateActions>();

        /// <summary>
        /// The separator token type
        /// </summary>
        private int separatorTokenType;

        /// <summary>
        /// The root token type
        /// </summary>
        private int rootTokenType;

        /// <summary>
        /// The initial state
        /// </summary>
        private int initialState;

        /// <summary>
        /// The source
        /// </summary>
        private string source;

        /// <summary>
        /// The current position
        /// </summary>
        private int currentPosition;

        /// <summary>
        /// The current char.
        /// </summary>
        private char currentChar = '\0';

        /// <summary>
        /// The source length
        /// </summary>
        private int sourceLength;

        /// <summary>
        /// The current state.
        /// </summary>
        private int state;

        /// <summary>
        /// The string builder.
        /// </summary>
        private StringBuilder builder = new StringBuilder();

        /// <summary>
        /// The tokens
        /// </summary>
        protected List<ParserToken> tokens = new List<ParserToken>();

        /// <summary>
        /// The root
        /// </summary>
        protected ParserToken root = null;

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseParser"/> class.
        /// </summary>
        /// <param name="separatorTokenType">Type of the separator token.</param>
        /// <param name="initialState">The initial state.</param>
        public BaseParser(int separatorTokenType, int rootTokenType, int initialState = 0)
        {
            this.separatorTokenType = separatorTokenType;
            this.rootTokenType = rootTokenType;
            this.initialState = initialState;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Initializes the actions defining the flow.
        /// </summary>
        protected abstract void InitializeActions();

        /// <summary>
        /// Builds the token tree.
        /// </summary>
        protected abstract void BuildTree();

        /// <summary>
        /// Parses the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public ParserToken Parse(string source)
        {
            this.source = source;
            this.InitParsing();
            while (this.GetChar())
            {
                if (!ResolveAction())
                {
                    this.Append();
                }
            }
            this.ResolveEndAction();
            this.root = new ParserToken(rootTokenType, "");
            this.BuildTree();
            return root;
        }

        /// <summary>
        /// Initialization of the parsing.
        /// </summary>
        private void InitParsing()
        {
            this.flow.Clear();
            this.InitializeActions();
            this.currentPosition = 0;
            this.sourceLength = this.source.Length;
            this.state = this.initialState;
            this.builder.Clear();
            this.tokens.Clear();
        }

        /// <summary>
        /// Pop one char.
        /// </summary>
        /// <returns></returns>
        private bool GetChar()
        {
            if (currentPosition < sourceLength)
            {
                this.currentChar = this.source[currentPosition++];
                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the cursor.
        /// </summary>
        /// <param name="i">The i.</param>
        private void MoveBy(int i)
        {
            this.currentPosition += i;
        }

        /// <summary>
        /// Gives the next char with a defined offset.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        private char NextChar(int i = 0)
        {
            if ((currentPosition + i) < sourceLength)
            {
                return this.source[currentPosition + i];
            }
            return '\0';
        }

        /// <summary>
        /// Gets the sequence existing from the cursor.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        private string NextSequence(int length)
        {
            if (length <= 0)
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(this.currentChar);
            for (int i = 1; i < length; i++)
            {
                sb.Append(this.NextChar(i - 1));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Determines whether the specified s is the next sequence.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns>
        ///   <c>true</c> if the specified s has sequence; otherwise, <c>false</c>.
        /// </returns>
        private bool HasSequence(string s)
        {
            return this.NextSequence(s.Length).Equals(s);
        }

        /// <summary>
        /// Pushes one token.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        private ParserToken PushToken(int type, string content)
        {
            ParserToken result = new ParserToken(type, content);
            this.tokens.Add(result);
            return result;
        }

        /// <summary>
        /// Pushes one token.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private ParserToken PushToken(int type)
        {
            ParserToken result = new ParserToken(type, builder.ToString());
            this.tokens.Add(result);
            return result;
        }

        /// <summary>
        /// Appends the current character to the builder.
        /// </summary>
        private void Append()
        {
            this.builder.Append(this.currentChar);
        }

        /// <summary>
        /// Resolves one action over the flow.
        /// </summary>
        /// <returns></returns>
        private bool ResolveAction()
        {
            if (this.flow.ContainsKey(this.state))
            {
                ParserStateActions actions = this.flow[this.state];
                foreach (ParserAction action in actions.Actions)
                {
                    if (action.Pattern.Equals(string.Empty))
                    {
                        continue;
                    }
                    if (this.HasSequence(action.Pattern))
                    {
                        this.MoveBy(action.Pattern.Length - 1);
                        if (action.TokenType > -1)
                        {
                            this.PushToken(action.TokenType);
                            builder.Clear();
                        }
                        if (separatorTokenType > -1)
                        {
                            this.PushToken(separatorTokenType, action.Pattern);
                        }
                        if (action.NewState > -1)
                        {
                            this.state = action.NewState;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Resolves the end action that occurs when the parser locates the EOF and there is still information in the buffer.
        /// </summary>
        private void ResolveEndAction()
        {
            if (this.flow.ContainsKey(this.state))
            {
                ParserStateActions actions = this.flow[this.state];
                foreach (ParserAction action in actions.Actions)
                {
                    if (action.Pattern.Equals(string.Empty))
                    {
                        if (action.TokenType > -1)
                        {
                            this.PushToken(action.TokenType);
                            builder.Clear();
                        }
                        if (action.NewState > -1)
                        {
                            this.state = action.NewState;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Registers the action.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="newState">The new state.</param>
        protected void RegisterAction(int state, string pattern, int tokenType, int newState)
        {
            ParserStateActions actions;
            if (this.flow.ContainsKey(state))
            {
                actions = this.flow[state];
            }
            else
            {
                actions = new ParserStateActions(state);
                this.flow.Add(state, actions);
            }
            actions.Actions.Add(new ParserAction(pattern, tokenType, newState));
        }

        #endregion
    }
}
