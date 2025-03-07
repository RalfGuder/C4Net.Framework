using System.Xml;

namespace C4Net.Framework.Core.Utils
{
    /// <summary>
    /// Abstract class for the base of Xml managers.
    /// </summary>
    public abstract class BaseXmlManager
    {
        #region - Methods -

        /// <summary>
        /// Loads the elements.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="nodePath">The node path.</param>
        public virtual bool LoadElements(string fileName, string nodePath)
        {
            XmlDocument document = XmlResources.GetFromResource(fileName);
            if (document != null)
            {
                foreach (XmlNode node in document.SelectNodes(nodePath))
                {
                    this.ProcessNode(node);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Process the node.
        /// </summary>
        /// <param name="node">The node.</param>
        public abstract void ProcessNode(XmlNode node);

        #endregion
    }
}
