using System;
using System.Collections.Generic;

namespace C4Net.Framework.Templates.Parser
{
    /// <summary>
    /// Generic token for a parser. 
    /// </summary>
    public class ParserToken
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public ParserToken Parent { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public String Content { get; set; }

        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public int TokenType { get; set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public List<ParserToken> Parameters { get; private set; }

        /// <summary>
        /// Gets the sons.
        /// </summary>
        /// <value>
        /// The sons.
        /// </value>
        public List<ParserToken> Sons { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserToken"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="content">The content.</param>
        public ParserToken(int type, String content)
        {
            this.Parent = null;
            this.TokenType = type;
            this.Content = content;
            this.Parameters = new List<ParserToken>();
            this.Sons = new List<ParserToken>();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="token">The token.</param>
        public void AddParameter(ParserToken token)
        {
            token.Parent = this;
            this.Parameters.Add(token);
        }

        /// <summary>
        /// Adds the son.
        /// </summary>
        /// <param name="token">The token.</param>
        public void AddSon(ParserToken token)
        {
            token.Parent = this;
            this.Sons.Add(token);
        }

        #endregion
    }
}
