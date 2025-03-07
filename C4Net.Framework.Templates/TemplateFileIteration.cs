namespace C4Net.Framework.Templates
{
    /// <summary>
    /// Define one file iteration for transforming using the template engine.
    /// This is a conversion of one to many, using the input file name to load data and the output path to
    /// write data. The output path can contain variables, so the name is generated for each iteration.
    /// </summary>
    public class TemplateFileIteration
    {
        #region - Properties -

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>
        /// The output path.
        /// </value>
        public string OutputPath { get; set; }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateFileIteration"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="outputPath">The output path.</param>
        public TemplateFileIteration(string fileName, string outputPath)
        {
            this.FileName = fileName;
            this.OutputPath = outputPath;
        }

        #endregion
    }
}
