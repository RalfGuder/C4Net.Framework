using System.Xml;

namespace C4Net.Framework.Core.Utils
{
    /// <summary>
    /// Abstract class for the Base item of a Xml Manager.
    /// </summary>
    public abstract class BaseXmlManagerItem : IXmlManagerItem
    {
        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseXmlManagerItem"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        public BaseXmlManagerItem(XmlNode node)
        {
            this.LoadFromNode(node);
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Inner load of the item.
        /// </summary>
        protected abstract void InnerLoad(NodeAttributes attributes);

        /// <summary>
        /// Loads from node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void LoadFromNode(XmlNode node)
        {
            if (node != null)
            {
                this.InnerLoad(new NodeAttributes(node));
            }
        }

        #endregion
    }
}
