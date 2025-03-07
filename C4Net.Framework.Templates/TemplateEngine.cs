using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using C4Net.Framework.Core.Utils;
using C4Net.Framework.Templates.Language;

namespace C4Net.Framework.Templates
{
    /// <summary>
    /// Class for one template engine, able to load a configuration from xml and iterate generating code.
    /// </summary>
    public class TemplateEngine
    {
        #region - Fields -

        /// <summary>
        /// The file content dictionary.
        /// </summary>
        private Dictionary<string, string> files = new Dictionary<string, string>();

        /// <summary>
        /// The engine rules list.
        /// </summary>
        private List<TemplateEngineItem> rules = new List<TemplateEngineItem>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Method to get the file content of files. If the file was already loaded, use the value at the dictionary.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private string GetFile(string fileName)
        {
            if (this.files.ContainsKey(fileName))
            {
                return this.files[fileName];
            }
            string result = StringUtil.LoadFile(fileName);
            this.files.Add(fileName, result);
            return result;
        }

        /// <summary>
        /// Adds one engine rule.
        /// </summary>
        /// <param name="iteratorPath">The iterator path.</param>
        /// <returns></returns>
        public TemplateEngineItem AddRule(string iteratorPath)
        {
            TemplateEngineItem rule = new TemplateEngineItem(iteratorPath);
            this.rules.Add(rule);
            return rule;
        }

        /// <summary>
        /// Replaces the specified string iterating the container.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        private string Replace(string str, TemplateContainer container)
        {
            return LanguageParser.ReplaceString(str, container);
        }

        /// <summary>
        /// Iterates one rule rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="container">The container.</param>
        private void IterateRule(TemplateEngineItem rule, TemplateContainer container)
        {
            foreach (TemplateFileIteration iterator in rule.FileIterators)
            {
                string file = this.GetFile(iterator.FileName);
                if (!string.IsNullOrEmpty(file))
                {
                    string outputPath = this.Replace(iterator.OutputPath, container);
                    container.AddAttribute("CurrentFile", Path.GetFileName(outputPath));
                    string outputFile = this.Replace(file, container);
                    StringUtil.WriteFile(outputPath, outputFile);
                }
            }
        }

        /// <summary>
        /// Iterates all the rules of the engine.
        /// </summary>
        /// <param name="container">The container.</param>
        public void Iterate(TemplateContainer container)
        {
            foreach (TemplateEngineItem rule in this.rules)
            {
                Object pathItem = container.GetByPath(rule.IteratorPath);
                if ((pathItem != null) && (pathItem is TemplateContainer))
                {
                    TemplateContainer root = (TemplateContainer)pathItem;
                    if (root.IsArray)
                    {
                        int i = 0;
                        string maxIndex = (root.ArrayValues.Count - 1).ToString();
                        foreach (TemplateContainer son in root.ArrayValues)
                        {
                            son.AddAttribute("Index", i.ToString());
                            son.AddAttribute("MaxIndex", maxIndex);
                            this.IterateRule(rule, son);
                            i++;
                        }
                    }
                    else
                    {
                        this.IterateRule(rule, root);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the rules from XML.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool LoadRulesFromXml(string fileName, string path)
        {
            XmlDocument document = XmlResources.GetFromResource(fileName);
            if (document != null)
            {
                foreach (XmlNode node in document.SelectNodes("template/rules/rule"))
                {
                    NodeAttributes attributes = new NodeAttributes(node);
                    TemplateEngineItem item = this.AddRule(attributes.AsString("iterator"));
                    foreach (XmlNode sonnode in node.SelectNodes("iteration"))
                    {
                        attributes = new NodeAttributes(sonnode);
                        item.AddIteration(path+"\\"+attributes.AsString("fileName"), attributes.AsString("outputFileName"));
                    }
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}
