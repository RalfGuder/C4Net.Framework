using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using C4Net.Framework.Core.Utils;

namespace C4Net.Framework.Data.Definitions
{
    public static class DefinitionManager
    {
        #region - Fields -

        /// <summary>
        /// The table definitions dictionary.
        /// </summary>
        private static Dictionary<Type, EntityDefinition> definitions = new Dictionary<Type, EntityDefinition>();

        #endregion

        #region - Methods -

        /// <summary>
        /// Registers the definition.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="definition">The definition.</param>
        public static void RegisterDefinition(Type type, EntityDefinition definition)
        {
            DefinitionManager.definitions[type] = definition;
        }

        /// <summary>
        /// Unregisters the definition.
        /// </summary>
        /// <param name="type">The type.</param>
        public static void UnregisterDefinition(Type type)
        {
            DefinitionManager.definitions.Remove(type);
        }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static EntityDefinition GetDefinition(Type type)
        {
            if (DefinitionManager.definitions.ContainsKey(type))
            {
                return DefinitionManager.definitions[type];
            }
            return null;
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="nodePath">The node path.</param>
        /// <param name="xmlAssembly">The XML assembly.</param>
        /// <param name="typesAssembly">The types assembly.</param>
        public static void LoadFromXml(string fileName, string nodePath, string xmlAssembly, string typesAssembly) 
        {
            if (!string.IsNullOrEmpty(xmlAssembly))
            {
                fileName = xmlAssembly + "." + fileName;
            }
            XmlDocument document = XmlResources.GetFromEmbeddedResource(fileName);
            if (document != null)
            {
                foreach (XmlNode node in document.SelectNodes(nodePath))
                {
                    EntityDefinition definition = new EntityDefinition();
                    definition.LoadFromXml(node);
                    definition.Recalculate(typesAssembly + ".Interfaces", typesAssembly + ".Entities");
                    if (definition.ClassType != null)
                    {
                        DefinitionManager.RegisterDefinition(definition.ClassType, definition);
                    }
                }
                DefinitionManager.Build();
            }
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        public static void Build()
        {
            foreach (EntityDefinition entity in DefinitionManager.definitions.Values)
            {
                entity.Build();
            }
        }

        /// <summary>
        /// Fluent define a new entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static EntityDefinition HasEntity<T>(string name, string tableName) 
        {
            Type type = typeof(T);
            if (DefinitionManager.definitions.ContainsKey(type))
            {
                return (EntityDefinition)DefinitionManager.definitions[type];
            }
            EntityDefinition result = new EntityDefinition();
            DefinitionManager.definitions.Add(type, result);
            result.Name = name;
            result.TableName = tableName;
            return result;
        }

        /// <summary>
        /// Fluent define a new entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static EntityDefinition HasEntity<T>(string name)
        {
            Type type = typeof(T);
            if (DefinitionManager.definitions.ContainsKey(type))
            {
                return (EntityDefinition)DefinitionManager.definitions[type];
            }
            EntityDefinition result = new EntityDefinition();
            DefinitionManager.definitions.Add(type, result);
            result.Name = name;
            return result;
        }

        /// <summary>
        /// Fluent define a new entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static EntityDefinition HasEntity<T>()
        {
            Type type = typeof(T);
            if (DefinitionManager.definitions.ContainsKey(type))
            {
                return (EntityDefinition)DefinitionManager.definitions[type];
            }
            EntityDefinition result = new EntityDefinition();
            if (type.GetType().IsInterface)
            {
                result.InterfaceType = type;
                result.InterfaceName = type.GetType().Name;
            }
            else if (type.GetType().IsClass)
            {
                result.ClassType = type;
                result.ClassName = type.GetType().Name;
                foreach (Type interfaceType in type.GetType().GetInterfaces())
                {
                    if (interfaceType.Name.Equals("I"+result.ClassName))
                    {
                        result.InterfaceType = interfaceType;
                        result.InterfaceName = interfaceType.Name;
                    }
                }
            }
            DefinitionManager.definitions.Add(type, result);
            return result;
        }

        #endregion
    }
}
