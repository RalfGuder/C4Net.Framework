using System.Collections.Generic;

namespace C4Net.Framework.Templates
{
    /// <summary>
    /// Class for one rule of the template engine. One rule contains the iterator path over the container and a
    /// list of TemplateFileIteration that defines the template files to use with this iterator, and the output
    /// file names.
    /// </summary>
    public class TemplateEngineItem
    {
        #region - Properties -

        /// <summary>
        /// Gets the iterator path.
        /// </summary>
        /// <value>
        /// The iterator path.
        /// </value>
        public string IteratorPath { get; private set; }

        /// <summary>
        /// Gets the file iterators.
        /// </summary>
        /// <value>
        /// The file iterators.
        /// </value>
        public List<TemplateFileIteration> FileIterators { get; private set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngineItem"/> class.
        /// </summary>
        /// <param name="iteratorPath">The iterator path.</param>
        public TemplateEngineItem(string iteratorPath)
        {
            this.IteratorPath = iteratorPath;
            this.FileIterators = new List<TemplateFileIteration>();
        }

        #endregion

        #region - Method -

        /// <summary>
        /// Adds one iteration.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="outputFileName">Name of the output file.</param>
        /// <returns></returns>
        public TemplateFileIteration AddIteration(string fileName, string outputFileName)
        {
            TemplateFileIteration item = new TemplateFileIteration(fileName, outputFileName);
            this.FileIterators.Add(item);
            return item;
        }

        #endregion
    }
}
