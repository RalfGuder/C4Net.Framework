using System.Xml;

namespace C4Net.Framework.Core.Utils
{
    /// <summary>
    /// Interface for an item of a manager that loads its information from a xml node.
    /// </summary>
    public interface IXmlManagerItem
    {
        #region - Methods -

        /// <summary>
        /// Loads from node.
        /// </summary>
        /// <param name="node">The node.</param>
        void LoadFromNode(XmlNode node);

        #endregion
    }
}
