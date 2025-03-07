using System;
using System.Collections.Generic;
using C4Net.Framework.Core.IoC;
using C4Net.Framework.Core.Utils;
using C4Net.Framework.Data;
using C4Net.Framework.Data.Configuration;
using C4Net.Framework.Data.Definitions;
using C4Net.Framework.Templates;
using MetaTool.Business.Entities;

namespace MetaTool.Business
{
    /// <summary>
    /// Class for the ModelLoader.
    /// </summary>
    public class ModelLoader
    {
        #region - Fields -
        
        /// <summary>
        /// The adapter
        /// </summary>
        private DataContext context = null;

        /// <summary>
        /// The MDB file name
        /// </summary>
        private string mdbFileName = string.Empty;

        /// <summary>
        /// Dictionary of entities.
        /// </summary>
        private Dictionary<string, Entity> entities = new Dictionary<string, Entity>();

        /// <summary>
        /// Dictionary of subtypes.
        /// </summary>
        private Dictionary<string, SubtypeRelationship> subtypes = new Dictionary<string, SubtypeRelationship>();

        /// <summary>
        /// Dictioanry of attributes.
        /// </summary>
        private Dictionary<string, MetaTool.Business.Entities.Attribute> attributes = new Dictionary<string, MetaTool.Business.Entities.Attribute>();

        /// <summary>
        /// Dictionary of domains.
        /// </summary>
        private Dictionary<string, Domain> domains = new Dictionary<string, Domain>();

        /// <summary>
        /// Dictionary of cardinalities.
        /// </summary>
        private Dictionary<string, CardinalityRelationship> cardinalities = new Dictionary<string, CardinalityRelationship>();

        /// <summary>
        /// Dictionary of business rules.
        /// </summary>
        private Dictionary<string, BusinessRule> businessRules = new Dictionary<string, BusinessRule>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public Dictionary<string, Entity> Entities
        {
            get { return this.entities; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelLoader"/> class.
        /// </summary>
        public ModelLoader()
        {
            DefinitionManager.LoadFromXml("Definitions.xml", "Definitions/Entities/Entity", "MetaTool.Business.Definitions", "MetaTool.Business");
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Sets the connection.
        /// </summary>
        /// <param name="mdbFileName">Name of the MDB file.</param>
        private void SetConnection(string mdbFileName)
        {
            this.mdbFileName = mdbFileName;
            IDbConnectionManager connectionManager = IoCDefault.Get<IDbConnectionManager>();
            DbConnectionInfo info = connectionManager.GetConnectionInfo("mdb");
            if (info == null)
            {
                info = new DbConnectionInfo();
                info.Name = "mdb";
                info.IsDefault = false;
                info.ProviderName = "OleDb";
                connectionManager.RegisterConnection(info);
            }
            info.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFileName + ";Persist Security Info=False;";
            info.Initialize();
            this.context = new DataContext("mdb");
        }

        /// <summary>
        /// Loads the tables from.
        /// </summary>
        /// <param name="mdbFileName">Name of the MDB file.</param>
        /// <param name="includeMeta">if set to <c>true</c> [include meta].</param>
        public void LoadTablesFrom(string mdbFileName, bool includeMeta)
        {
            this.SetConnection(mdbFileName);
            this.entities.Clear();
            this.subtypes.Clear();
            this.attributes.Clear();
            this.domains.Clear();
            this.cardinalities.Clear();
            this.businessRules.Clear();
            IList<Entity> entityList;
            if (includeMeta)
            {
                entityList = this.context.SelectList<Entity>(Entity._.OrderBy(Entity._.DepthCnt).OrderBy(Entity._.NameTxt));
            }
            else
            {
                entityList = this.context.SelectList<Entity>(Entity._.ModLvlCode != "META" ^ Entity._.DepthCnt ^ Entity._.NameTxt);
            }
            foreach (Entity entity in entityList) 
            {
                this.entities.Add(entity.EntId.ToString(), entity);
            }
            IList<SubtypeRelationship> subtypeList = this.context.SelectList<SubtypeRelationship>();
            foreach (SubtypeRelationship subtype in subtypeList)
            {
                this.subtypes.Add(subtype.SupEntId.ToString() + "|" + subtype.SubEntId.ToString(), subtype);
            }
        }

        public SubtypeRelationship GetSubtypeWhereSonIs(Entity entity)
        {
            foreach (SubtypeRelationship subtype in this.subtypes.Values)
            {
                if (subtype.SubEntId == entity.EntId)
                {
                    return subtype;
                }
            }
            return null;
        }

        private SubtypeRelationship GetSubtypeWhereParentIs(Entity entity)
        {
            foreach (SubtypeRelationship subtype in this.subtypes.Values)
            {
                if (subtype.SupEntId == entity.EntId)
                {
                    return subtype;
                }
            }
            return null;
        }

        private bool IsPlainEntity(Entity entity)
        {
            SubtypeRelationship rel = this.GetSubtypeWhereParentIs(entity);
            if (rel == null)
            {
                return true;
            }
            return false;
        }

        public void LoadMetamodel()
        {
            // 1. Load Attributes.
            IList<MetaTool.Business.Entities.Attribute> attributeList = this.context.SelectList<MetaTool.Business.Entities.Attribute>(MetaTool.Business.Entities.Attribute._.OrderBy(MetaTool.Business.Entities.Attribute._.EntId).OrderBy(MetaTool.Business.Entities.Attribute._.AttrSeqnrOrd).OrderBy(MetaTool.Business.Entities.Attribute._.AttrIx));
            double lastEntId = 0;
            double lastSeqnr = 0;
            foreach (MetaTool.Business.Entities.Attribute item in attributeList)
            {
                this.attributes.Add(item.EntId.ToString() + "|" + item.AttrIx.ToString(), item);
                if (!((item.EntId == lastEntId) && (item.AttrSeqnrOrd == lastSeqnr)))
                {
                    if (this.entities.ContainsKey(item.EntId.ToString()))
                    {
                        Entity entity = this.entities[item.EntId.ToString()];
                        entity.AddAttribute(item);
                    }
                }
            }
            IList<NonKeyAttribute> nkAttributeList = this.context.SelectList<NonKeyAttribute>();
            foreach (NonKeyAttribute item in nkAttributeList)
            {
                if (this.attributes.ContainsKey(item.AttrIx.ToString()+"|"+item.AttrIx.ToString()))
                {
                    this.attributes[item.AttrIx.ToString() + "|" + item.AttrIx.ToString()].Apply(item);
                }
            }
            IList<BaseAttribute> baseAttributeList = this.context.SelectList<BaseAttribute>();
            foreach (BaseAttribute item in baseAttributeList)
            {
                if (this.attributes.ContainsKey(item.EntId.ToString() + "|" + item.AttrIx.ToString()))
                {
                    this.attributes[item.EntId.ToString() + "|" + item.AttrIx.ToString()].Apply(item);
                }
            }
            IList<ForeignKeyAttribute> fkAttributeList = this.context.SelectList<ForeignKeyAttribute>();
            foreach (ForeignKeyAttribute item in fkAttributeList)
            {
                if (this.attributes.ContainsKey(item.HostEntId.ToString() + "|" + item.AttrIx.ToString()))
                {
                    this.attributes[item.HostEntId.ToString() + "|" + item.AttrIx.ToString()].Apply(item);
                }
            }
            foreach (MetaTool.Business.Entities.Attribute item in this.attributes.Values)
            {
                if (item.IsForeignKey)
                {
                    if (this.attributes.ContainsKey(item.BaseEntId.ToString() + "|" + item.BaseAttrIx.ToString()))
                    {
                        item.ApplyBase(this.attributes[item.BaseEntId.ToString() + "|" + item.BaseAttrIx.ToString()]);
                    }
                    if (this.attributes.ContainsKey(item.SourceEntId.ToString() + "|" + item.SourceAttrIx.ToString()))
                    {
                        item.ApplySource(this.attributes[item.SourceEntId.ToString() + "|" + item.SourceAttrIx.ToString()]);
                    }
                }
            }
            //2. Load domains
            IList<Domain> domainList = this.context.SelectList<Domain>(Domain._.OrderBy(Domain._.DomId));
            foreach (Domain item in domainList)
            {
                if (item.RestrTypeCode == null)
                {
                    item.RestrTypeCode = string.Empty;
                }
                this.domains.Add(item.DomId.ToString(), item);
            }
            IList<DomainValue> domValueList = this.context.SelectList<DomainValue>(DomainValue._.OrderBy(DomainValue._.DomId).OrderBy(DomainValue._.DomValIx));
            foreach (DomainValue item in domValueList)
            {
                if (this.domains.ContainsKey(item.DomId.ToString()))
                {
                    this.domains[item.DomId.ToString()].AddValue(item);
                }
            }

            //3. Subtyping
            foreach (SubtypeRelationship item in this.subtypes.Values)
            {
                if (this.domains.ContainsKey(item.DomId.ToString()))
                {
                    item.DomainValue = this.domains[item.DomId.ToString()].GetValue(Convert.ToInt32(item.DomValIx));
                }
                if (this.entities.ContainsKey(item.SupEntId.ToString()))
                {
                    if (this.entities.ContainsKey(item.SubEntId.ToString()))
                    {
                        Entity parent = this.entities[item.SupEntId.ToString()];
                        Entity son = this.entities[item.SubEntId.ToString()];
                        item.Parent = parent;
                        item.Son = son;
                        parent.AddSon(item);
                        son.AddParent(item);
                    }
                }
            }
            IList<CategoryRelationship> catList = this.context.SelectList<CategoryRelationship>();
            foreach (CategoryRelationship item in catList)
            {
                if (this.entities.ContainsKey(item.SupEntId.ToString()))
                {
                    Entity entity = this.entities[item.SupEntId.ToString()];
                    entity.SetCategoryAttribute(Convert.ToInt32(item.DiscrIx));
                    entity.IsCompleteRelationship = item.ComplIndCode.Equals("CC");
                    
                }
            }
            foreach (SubtypeRelationship rel in this.subtypes.Values)
            {
                rel.CategoryAttribute = rel.Parent.CategoryAttribute;
            }
            //4. Cardinality
            IList<CardinalityRelationship> cardList = this.context.SelectList<CardinalityRelationship>(CardinalityRelationship._.OrderBy(CardinalityRelationship._.PaEntId).OrderBy(CardinalityRelationship._.ChEntId).OrderBy(CardinalityRelationship._.RelIx));
            foreach (CardinalityRelationship item in cardList)
            {
                item.SetCardinality();
                if ((this.entities.ContainsKey(item.PaEntId.ToString())) && (this.entities.ContainsKey(item.ChEntId.ToString())))
                {
                    Entity parent = this.entities[item.PaEntId.ToString()];
                    Entity son = this.entities[item.ChEntId.ToString()];
                    parent.SonRelations.Add(item);
                    son.ParentRelations.Add(item);
                    item.Parent = parent;
                    item.Son = son;
                    foreach (MetaTool.Business.Entities.Attribute attr in son.Attributes)
                    {
                        if ((attr.IsForeignKey) && (attr.RelIx == Convert.ToInt32(item.RelIx)) && (attr.SourceAttribute.Entity == parent))
                        {
                            item.AddAttributes(attr.SourceAttribute, attr);
                        }
                    }
                }
            }
            //5. Business rules
            IList<BusinessRule> brList = this.context.SelectList<BusinessRule>(BusinessRule._.OrderBy(BusinessRule._.BrId));
            foreach (BusinessRule item in brList)
            {
                this.businessRules.Add(item.BrId.ToString(), item);
            }
            IList<BusinessRuleEntity> brEntityList = this.context.SelectList<BusinessRuleEntity>(BusinessRuleEntity._.OrderBy(BusinessRuleEntity._.BrId).OrderBy(BusinessRuleEntity._.BrEntIx));
            IList<BusinessRuleEntityAttributeComposite> brCompositeList = this.context.SelectList<BusinessRuleEntityAttributeComposite>(BusinessRuleEntityAttributeComposite._.OrderBy(BusinessRuleEntityAttributeComposite._.EntId).OrderBy(BusinessRuleEntityAttributeComposite._.BrEntIx).OrderBy(BusinessRuleEntityAttributeComposite._.BrEntAttrCompIx));
            IList<BusinessRuleEntityAttributeCompositeDomainValue> brValueList = this.context.SelectList<BusinessRuleEntityAttributeCompositeDomainValue>(BusinessRuleEntityAttributeCompositeDomainValue._.OrderBy(BusinessRuleEntityAttributeCompositeDomainValue._.BrId).OrderBy(BusinessRuleEntityAttributeCompositeDomainValue._.BrEntIx).OrderBy(BusinessRuleEntityAttributeCompositeDomainValue._.BrEntAttrCompIx).OrderBy(BusinessRuleEntityAttributeCompositeDomainValue._.BrEntAttrCompDomValIx));
            // Get entity of interest of each business rule.
            long lastBrId = 0;
            Entity currentEntity = null;
            BusinessRule currentBr = null;
            foreach (BusinessRuleEntity item in brEntityList)
            {
                if (lastBrId != Convert.ToInt64(item.BrId))
                {
                    currentEntity = this.entities[item.EntOfInterestId.ToString()];
                    currentBr = this.businessRules[item.BrId.ToString()];
                    currentBr.EntOfInterest = currentEntity;
                    currentBr.EntOfInterestId = currentEntity.EntId;
                    currentEntity.BusinessRules.Add(currentBr);
                    lastBrId = Convert.ToInt64(item.BrId);
                }
                currentBr.EntityRules.Add(item);
            }
            // Build tree structure
            BusinessRuleEntity currentBrEntity;
            foreach (BusinessRuleEntityAttributeComposite item in brCompositeList)
            {
                currentBr = this.businessRules[item.BrId.ToString()];
                currentBrEntity = currentBr.GetEntityRule(Convert.ToInt64(item.BrEntIx));
                currentBrEntity.CompositeRules.Add(item);
            }
            BusinessRuleEntityAttributeComposite currentBrComposite;
            foreach (BusinessRuleEntityAttributeCompositeDomainValue item in brValueList)
            {
                currentBr = this.businessRules[item.BrId.ToString()];
                currentBrEntity = currentBr.GetEntityRule(Convert.ToInt64(item.BrEntIx));
                currentBrComposite = currentBrEntity.GetCompositeRule(Convert.ToInt64(item.BrEntAttrCompIx));
                currentBrComposite.DomainValues.Add(item);
            }
            // Build the valid combinations
            foreach (BusinessRule rule in brList)
            {
                rule.BuildValidCombinations(this.domains);
            }
        }

        public void SelectEntity(double entId)
        {
            Entity ent = this.entities[entId.ToString()];
            ent.IsSelected = true;
            foreach (MetaTool.Business.Entities.Attribute attribute in ent.Attributes)
            {
                this.domains[attribute.DomId.ToString()].IsSelected = true;
            }
            foreach (BusinessRule rule in ent.BusinessRules)
            {
                rule.IsSelected = true;
            }
            foreach (CardinalityRelationship relation in ent.ParentRelations)
            {
                relation.IsSelected = true;
            }
            foreach (CardinalityRelationship relation in ent.SonRelations)
            {
                relation.IsSelected = true;
            }
        }

        private string GetAttributeName(MetaTool.Business.Entities.Attribute attribute)
        {
            if (attribute.NameTxt.Equals("physical model only"))
            {
                return StringUtil.Pascalize(attribute.ColNameTxt);
            }
            string result = StringUtil.Pascalize(attribute.NameTxt);
            Domain dom = this.domains[attribute.DomId.ToString()];
            if ((dom.RestrTypeCode != null) && (dom.RestrTypeCode.Equals("EN")))
            {
                if (result.EndsWith("Code"))
                {
                    result = result.Remove(result.Length - 4, 4);
                }
            }
            string parentEntityName = StringUtil.Pascalize(attribute.Entity.NameTxt);
            if (result.StartsWith(parentEntityName))
            {
                if (result.Length > parentEntityName.Length)
                {
                    result = result.Remove(0, parentEntityName.Length);
                }
            }
            if (result.Equals(parentEntityName))
            {
                result = result + "Code";
            }
            if ((result.Equals("Category")) && (dom.RestrTypeCode.Equals("EN")))
            {
                result = StringUtil.Pascalize(attribute.Entity.NameTxt) + "Category";
            }
            if ((result.Equals("Subcategory")) && (dom.RestrTypeCode.Equals("EN")))
            {
                result = StringUtil.Pascalize(attribute.Entity.NameTxt) + "Subcategory";
            }
            return result;
        }

        private string GetDomainName(Domain dom)
        {
            string name = StringUtil.Pascalize(dom.NameTxt);
            if (name.EndsWith("Code"))
            {
                name = name.Remove(name.Length - 4, 4);
            }
            if (dom.RestrTypeCode.Equals("EN"))
            {
                name = name + "Enum";
            }
            return name;
        }

        private string GetNetType(MetaTool.Business.Entities.Attribute attr)
        {
            Domain dom = this.domains[attr.DomId.ToString()];
            if (dom.NameTxt.Equals("datetime"))
            {
                return "DateTime";
            }
            if (dom.RestrTypeCode.Equals("EN"))
            {
                return this.GetDomainName(dom);
            }
            return this.GetBaseNetType(attr);
        }

        private string GetBaseNetType(MetaTool.Business.Entities.Attribute attr)
        {
            if (attr.DataType.Equals("VARCHAR"))
            {
                return "string";
            }
            if (attr.DataType.Equals("NUMBER"))
            {
                if (attr.DataDecimals > 0)
                {
                    return "double";
                }
                else
                {
                    if (attr.DataLength > 18)
                    {
                        return "decimal";
                    }
                    else if (attr.DataLength > 9)
                    {
                        return "long";
                    }
                    else if (attr.DataLength > 4)
                    {
                        return "int";
                    }
                    else
                    {
                        return "short";
                    }
                }
            }
            return "string";
        }

        private string GetSQLServerType(MetaTool.Business.Entities.Attribute attr)
        {
            if (attr.DataType.Equals("VARCHAR"))
            {
                return "[NVARCHAR](" + attr.DataLength.ToString() + ")";
            }
            if (attr.DataType.Equals("NUMBER"))
            {
                return "[NUMERIC](" + attr.DataLength.ToString() + "," + attr.DataDecimals.ToString() + ")";
            }
            if (attr.DataType.Equals("CHAR"))
            {
                return "[CHAR](" + attr.DataLength.ToString() + ")";
            }
            return "";
        }

        private bool ExistsInEntity(Entity entity, string attributeName)
        {
            foreach (MetaTool.Business.Entities.Attribute attribute in entity.Attributes)
            {
                if (this.GetAttributeName(attribute).Equals(attributeName))
                {
                    return true;
                }
            }
            return false;
        }

        private bool ExistsInParent(MetaTool.Business.Entities.Attribute attr)
        {
            string attributeName = this.GetAttributeName(attr);
            Entity entity = attr.Entity;
            while (entity.Parent != null)
            {
                entity = entity.Parent.Parent;
                if (this.ExistsInEntity(entity, attributeName))
                {
                    return true;
                }
            }
            return false;
        }

        private void GenerateAttribute(TemplateContainer container, MetaTool.Business.Entities.Attribute item)
        {
            container.AddAttribute("EntId", item.EntId);
            container.AddAttribute("AttrIx", item.AttrIx);
            container.AddAttribute("LogicalName", item.NameTxt);
            container.AddAttribute("PhysicalName", item.ColNameTxt);
            container.AddAttribute("Name", this.GetAttributeName(item));
            container.AddAttribute("SequenceNumber", item.AttrSeqnrOrd);
            container.AddAttribute("StandardisationLevel", item.StdnLvlCode);
            container.AddAttribute("IsPrimaryKey", item.IsPrimaryKey);
            container.AddAttribute("IsMandatory", item.IsMandatory);
            container.AddAttribute("IsForeignKey", item.IsForeignKey);
            container.AddAttribute("IsCategory", item.IsCategoryAttribute);
            container.AddAttribute("BaseEntId", item.BaseEntId);
            container.AddAttribute("BaseAttrIx", item.BaseAttrIx);
            container.AddAttribute("Definition", item.Definition);
            container.AddAttribute("DataType", item.DataType);
            container.AddAttribute("DataLength", item.DataLength);
            container.AddAttribute("DataDecimals", item.DataDecimals);
            container.AddAttribute("DomId", item.DomId.ToString());
            container.AddAttribute("NetType", this.GetNetType(item));
            container.AddAttribute("BaseNetType", this.GetNetType(item));
            container.AddAttribute("SQLServerType", this.GetSQLServerType(item));
            container.AddAttribute("ExistsInParent", this.ExistsInParent(item).ToString());
            container.AddAttribute("IsLoggable", item.ColNameTxt.Equals("creator_id") || item.ColNameTxt.Equals("update_seqnr"));
            this.GenerateDomain(container.AddChild("Domain"), this.domains[item.DomId.ToString()]);
        }

        private void GenerateSubtype(TemplateContainer container, SubtypeRelationship subtype)
        {
            container.AddAttribute("SupEntid", subtype.SupEntId);
            container.AddAttribute("SubEntId", subtype.SubEntId);
            container.AddAttribute("DomId", subtype.DomId);
            container.AddAttribute("DomValIx", subtype.DomValIx);
            this.GenerateEntity(container.AddChild("Parent"), subtype.Parent);
            this.GenerateEntity(container.AddChild("Son"), subtype.Son);
            this.GenerateAttribute(container.AddChild("CategoryAttribute"), subtype.CategoryAttribute);
            this.GenerateDomainValue(container.AddChild("DomainValue"), subtype.DomainValue);
        }

        private void GenerateCardinality(TemplateContainer container, CardinalityRelationship relation)
        {
            container.AddAttribute("PaEntId", relation.PaEntId);
            container.AddAttribute("ChEntId", relation.ChEntId);
            container.AddAttribute("VerbName", relation.VerbNameTxt);
            container.AddAttribute("InvVerbName", relation.InvVerbNameTxt);
            container.AddAttribute("Cardinality", relation.CardinalityType.ToString());
            container.AddAttribute("ToOne", (relation.CardinalityType == CardinalityTypeEnum.OneToZeroOrOne) || (relation.CardinalityType == CardinalityTypeEnum.ZeroToZeroOrOne));
            container.AddAttribute("ToMany", !((relation.CardinalityType == CardinalityTypeEnum.OneToZeroOrOne) || (relation.CardinalityType == CardinalityTypeEnum.ZeroToZeroOrOne)));
            string name = relation.Verb;
            //string inverseName = "Inv"+relation.Verb;
            string inverseName = relation.InverseVerb;
            container.AddAttribute("Verb", name);
            container.AddAttribute("InverseVerb", inverseName);
            container.AddAttribute("ParentClass", "I"+relation.Parent.ClassName);
            container.AddAttribute("ParentDtoClass", relation.Parent.ClassName+"DTO");
            if ((relation.CardinalityType == CardinalityTypeEnum.OneToZeroOrOne) || (relation.CardinalityType == CardinalityTypeEnum.ZeroToZeroOrOne))
            {
                container.AddAttribute("SonClass", "I"+relation.Son.ClassName);
                container.AddAttribute("SonDtoClass", relation.Son.ClassName+"DTO");
            }
            else
            {
                container.AddAttribute("SonDtoClass", "IList<" + relation.Son.ClassName + "DTO>");
            }
            container.AddAttribute("ParentPhysicalName", relation.Parent.TabNameTxt);
            container.AddAttribute("SonPhysicalName", relation.Son.TabNameTxt);
        }

        private void CalculateCardinalityNames(Entity item)
        {
            foreach (CardinalityRelationship rel in item.SonRelations)
            {
                rel.Verb = StringUtil.Pascalize(rel.VerbNameTxt);
            }
            List<CardinalityRelationship> collisions = new List<CardinalityRelationship>();
            for (int i = 0; i < item.SonRelations.Count; i++)
            {
                for (int j = i + 1; j < item.SonRelations.Count; j++)
                {
                    if (item.SonRelations[i].Verb == item.SonRelations[j].Verb)
                    {
                        if (!collisions.Contains(item.SonRelations[i]))
                        {
                            collisions.Add(item.SonRelations[i]);
                        }
                        if (!collisions.Contains(item.SonRelations[j]))
                        {
                            collisions.Add(item.SonRelations[j]);
                        }
                    }
                }
            }
            if (collisions.Count == 0) 
            {
                return;
            }
            foreach (CardinalityRelationship rel in collisions)
            {
                rel.Verb = rel.Verb + rel.Son.ClassName;
            }
            collisions.Clear();
            for (int i = 0; i < item.SonRelations.Count; i++)
            {
                for (int j = i + 1; j < item.SonRelations.Count; j++)
                {
                    if (item.SonRelations[i].Verb == item.SonRelations[j].Verb)
                    {
                        if (!collisions.Contains(item.SonRelations[i]))
                        {
                            collisions.Add(item.SonRelations[i]);
                        }
                        if (!collisions.Contains(item.SonRelations[j]))
                        {
                            collisions.Add(item.SonRelations[j]);
                        }
                    }
                }
            }
            if (collisions.Count == 0) 
            {
                return;
            }
            foreach (CardinalityRelationship rel in collisions)
            {
                if (rel.RelIx > 1)
                {
                    rel.Verb = rel.Verb + rel.RelIx.ToString();
                }
            }
        }

        private void CalculateInvCardinalityNames(Entity item)
        {
            foreach (CardinalityRelationship rel in item.ParentRelations)
            {
                if (string.IsNullOrEmpty(rel.InvVerbNameTxt))
                {
                    rel.InverseVerb = "HasParent";
                }
                else
                {
                    rel.InverseVerb = StringUtil.Pascalize(rel.InvVerbNameTxt);
                }
                if (rel.InverseVerb == "Has")
                {
                    rel.InverseVerb = "HasParent";
                }
            }
            List<CardinalityRelationship> collisions = new List<CardinalityRelationship>();
            for (int i = 0; i < item.ParentRelations.Count; i++)
            {
                for (int j = i + 1; j < item.ParentRelations.Count; j++)
                {
                    if (item.ParentRelations[i].InverseVerb == item.ParentRelations[j].InverseVerb)
                    {
                        if (!collisions.Contains(item.ParentRelations[i]))
                        {
                            collisions.Add(item.ParentRelations[i]);
                        }
                        if (!collisions.Contains(item.ParentRelations[j]))
                        {
                            collisions.Add(item.ParentRelations[j]);
                        }
                    }
                }
            }
            if (collisions.Count == 0)
            {
                return;
            }
            foreach (CardinalityRelationship rel in collisions)
            {
                rel.InverseVerb = rel.InverseVerb + rel.Parent.ClassName;
            }
            collisions.Clear();
            for (int i = 0; i < item.ParentRelations.Count; i++)
            {
                for (int j = i + 1; j < item.ParentRelations.Count; j++)
                {
                    if (item.ParentRelations[i].InverseVerb == item.ParentRelations[j].InverseVerb)
                    {
                        if (!collisions.Contains(item.ParentRelations[i]))
                        {
                            collisions.Add(item.ParentRelations[i]);
                        }
                        if (!collisions.Contains(item.ParentRelations[j]))
                        {
                            collisions.Add(item.ParentRelations[j]);
                        }
                    }
                }
            }
            if (collisions.Count == 0)
            {
                return;
            }
            foreach (CardinalityRelationship rel in collisions)
            {
                if (rel.RelIx > 1)
                {
                    rel.InverseVerb = rel.InverseVerb + rel.RelIx.ToString();
                }
            }
        }

        private void GenerateEntity(TemplateContainer container, Entity item, bool full = false)
        {
            container.AddAttribute("EntId", item.EntId);
            container.AddAttribute("LogicalName", item.NameTxt);
            container.AddAttribute("PhysicalName", item.TabNameTxt);
            container.AddAttribute("Name", item.ClassName);
            container.AddAttribute("ClassName", item.ClassName);
            container.AddAttribute("InterfaceName", "I" + item.ClassName);
            container.AddAttribute("Definition", item.DefTxt);
            container.AddAttribute("DependencyCode", item.DepenCode);
            container.AddAttribute("Depth", item.DepthCnt);
            container.AddAttribute("StorageType", item.StgTypeCode);
            container.AddAttribute("StandardisationLevel", item.StdnLvlCode);
            container.AddAttribute("ModelLevel", item.ModLvlCode);
            container.AddAttribute("PkName", item.PkName);
            container.AddAttribute("IsLoggable", item.StgTypeCode.Equals("LOG"));
            container.AddAttribute("HasNavigationProperties", ((item.ParentRelations.Count > 0) || (item.SonRelations.Count > 0)).ToString());
            if (item.Parent != null)
            {
                container.AddAttribute("PaEntId", item.Parent.Parent.EntId);
                container.AddAttribute("InheritsFrom", item.Parent.Parent.ClassName);
                if (full)
                {
                    this.GenerateEntity(container.AddChild("ParentEntity"), item.Parent.Parent, false);
                }
            }
            else
            {
                container.AddAttribute("PaEntId", "");
                container.AddAttribute("InheritsFrom", "");
            }
            if (full)
            {
                TemplateContainer sonsContainer = container.AddChild("Sons");
                foreach (SubtypeRelationship son in item.Sons)
                {
                    this.GenerateSubtype(sonsContainer.AddArrayValue(), son);
                }
                TemplateContainer parentRelationsContainer = container.AddChild("ParentRelations");
                foreach (CardinalityRelationship parent in item.ParentRelations)
                {
                    this.GenerateCardinality(parentRelationsContainer.AddArrayValue(), parent);
                }

                TemplateContainer sonRelationsContainer = container.AddChild("SonRelations");
                foreach (CardinalityRelationship son in item.SonRelations)
                {
                    this.GenerateCardinality(sonRelationsContainer.AddArrayValue(), son);
                }
            }
            TemplateContainer attrContainer = container.AddChild("Attributes");
            foreach (MetaTool.Business.Entities.Attribute attribute in item.Attributes)
            {
                this.GenerateAttribute(attrContainer.AddArrayValue(), attribute);
            }
        }

        private void GenerateEntities(TemplateContainer container)
        {
            foreach (Entity item in this.entities.Values)
            {
                this.CalculateCardinalityNames(item);
                this.CalculateInvCardinalityNames(item);
                if (item.IsSelected)
                {
                    this.GenerateEntity(container.AddArrayValue(), item, true);
                }
            }
        }

        private void GenerateCategoryValue(TemplateContainer container, SubtypeRelationship subtype)
        {
            container.AddAttribute("LogicalName", subtype.CategoryAttribute.NameTxt);
            container.AddAttribute("PhysicalName", subtype.CategoryAttribute.ColNameTxt);
            container.AddAttribute("Name", this.GetAttributeName(subtype.CategoryAttribute));
            this.GenerateDomainValue(container.AddChild("DomainValue"), subtype.DomainValue);
        }

        private void GeneratePlainEntity(TemplateContainer container, Entity item, bool full)
        {
            List<Entity> line = new List<Entity>();
            Entity current = item;
            while (current != null)
            {
                line.Insert(0, current);
                if (current.Parent != null)
                {
                    current = current.Parent.Parent;
                }
                else
                {
                    current = null;
                }
            }
            container.AddAttribute("EntId", item.EntId);
            container.AddAttribute("LogicalName", item.NameTxt);
            container.AddAttribute("PhysicalName", item.TabNameTxt);
            container.AddAttribute("Name", item.ClassName);
            container.AddAttribute("ClassName", item.ClassName);
            container.AddAttribute("InterfaceName", "I" + item.ClassName);
            container.AddAttribute("Definition", item.DefTxt);
            container.AddAttribute("DependencyCode", item.DepenCode);
            container.AddAttribute("Depth", item.DepthCnt);
            container.AddAttribute("StorageType", item.StgTypeCode);
            container.AddAttribute("StandardisationLevel", item.StdnLvlCode);
            container.AddAttribute("ModelLevel", item.ModLvlCode);
            container.AddAttribute("PkName", item.PkName);
            container.AddAttribute("IsLoggable", item.StgTypeCode.Equals("LOG"));
            container.AddAttribute("HasNavigationProperties", ((item.ParentRelations.Count > 0) || (item.SonRelations.Count > 0)).ToString());
            if (item.Parent != null)
            {
                container.AddAttribute("PaEntId", item.Parent.Parent.EntId);
                container.AddAttribute("InheritsFrom", item.Parent.Parent.ClassName);
                if (full)
                {
                    this.GenerateEntity(container.AddChild("ParentEntity"), item.Parent.Parent, false);
                }
            }
            else
            {
                container.AddAttribute("PaEntId", "");
                container.AddAttribute("InheritsFrom", "");
            }
            if (full)
            {
                // Add son subtypes. It's only for this item.
                TemplateContainer sonsContainer = container.AddChild("Sons");
                foreach (SubtypeRelationship son in item.Sons)
                {
                    this.GenerateSubtype(sonsContainer.AddArrayValue(), son);
                }
                TemplateContainer parentRelationsContainer = container.AddChild("ParentRelations");
                TemplateContainer sonRelationsContainer = container.AddChild("SonRelations");
                foreach (Entity entity in line)
                {
                    // Add parent cardinalities. It's for all the line.
                    foreach (CardinalityRelationship parent in entity.ParentRelations)
                    {
                        this.GenerateCardinality(parentRelationsContainer.AddArrayValue(), parent);
                    }
                    // Add son cardinalities. It's for all the line.
                    foreach (CardinalityRelationship son in entity.SonRelations)
                    {
                        this.GenerateCardinality(sonRelationsContainer.AddArrayValue(), son);
                    }
                }
            }
            TemplateContainer attrContainer = container.AddChild("Attributes");
            TemplateContainer catContainer = container.AddChild("Categories");
            foreach (Entity entity in line)
            {
                // Add attributes. It's for all the line, skip categories.
                foreach (MetaTool.Business.Entities.Attribute attribute in entity.Attributes)
                {
                    if ((attribute != entity.CategoryAttribute) && (!this.ExistsInParent(attribute)))
                    {
                        this.GenerateAttribute(attrContainer.AddArrayValue(), attribute);
                    }
                }
                if (entity.Parent != null)
                {
                    this.GenerateCategoryValue(catContainer.AddArrayValue(), entity.Parent);
                }
            }
            

        }

        private void GeneratePlainEntities(TemplateContainer container)
        {
            foreach (Entity item in this.entities.Values)
            {
                if ((item.IsSelected))
                {
                    this.GeneratePlainEntity(container.AddArrayValue(), item, true);
                }
            }
        }

        private void GenerateDomainValue(TemplateContainer container, DomainValue item)
        {
            container.AddAttribute("DomId", item.DomId);
            container.AddAttribute("DomValIx", item.DomValIx);
            container.AddAttribute("Description", item.DescrTxt);
            container.AddAttribute("LogicalName", item.NameTxt);
            string name = StringUtil.Pascalize(item.NameTxt);
            int number;
            if (int.TryParse(name.Substring(0, 1), out number))
            {
                name = "_" + name;
            }
            container.AddAttribute("Name", name);
            container.AddAttribute("Definition", item.DefTxt);
            container.AddAttribute("StandardisationLevel", item.StdnLvlCode);
            container.AddAttribute("Source", item.SrcTxt);
        }

        private void GenerateDomain(TemplateContainer container, Domain item)
        {
            container.AddAttribute("DomId", item.DomId.ToString());
            container.AddAttribute("LogicalName", item.NameTxt);
            container.AddAttribute("Name", this.GetDomainName(item));
            container.AddAttribute("Definition", item.DefTxt);
            container.AddAttribute("ClassName", item.ClassNameTxt);
            container.AddAttribute("DomainType", item.RestrTypeCode);
            container.AddAttribute("MeasurementUnit", item.MeasUnitDescrTxt);
            container.AddAttribute("ParentDomId", item.PaDomId);
            container.AddAttribute("StandardisationLevel", item.StdnLvlCode);
            container.AddAttribute("ModelLevel", item.ModLvlCode);
            container.AddAttribute("DefinitionSource", item.DefSrcTxt);
            container.AddAttribute("MinValue", "");
            container.AddAttribute("MaxValue", "");
            if (item.RestrTypeCode.Equals("RA"))
            {
                foreach (DomainValue value in item.Values)
                {
                    if (value.TypeCode.Equals("MIN-IN"))
                    {
                        container.AddAttribute("MinValue", value.DescrTxt);
                    }
                    if (value.TypeCode.Equals("MAX-IN"))
                    {
                        container.AddAttribute("MaxValue", value.DescrTxt);
                    }
                }
            }
            else if (item.RestrTypeCode.Equals("EN"))
            {
                TemplateContainer valuesContainer = container.AddChild("Values");
                foreach (DomainValue value in item.Values)
                {
                    this.GenerateDomainValue(valuesContainer.AddArrayValue(), value);
                }
            }
        }

        private void GenerateDomains(TemplateContainer container)
        {
            foreach (Domain item in this.domains.Values)
            {
                this.GenerateDomain(container.AddArrayValue(), item);
            }
        }

        private void GenerateEnums(TemplateContainer container)
        {
            foreach (Domain item in this.domains.Values)
            {
                if ((item.RestrTypeCode.Equals("EN")) && (item.Values.Count > 0))
                {
                    this.GenerateDomain(container.AddArrayValue(), item);
                }
            }
        }

        private void GenerateBusinessRules(TemplateContainer container)
        {
        }

        public TemplateContainer BuildContainer()
        {
            TemplateContainer result = new TemplateContainer();
            TemplateContainer entityContainer = result.AddChild("Entities");
            TemplateContainer plainEntityContainer = result.AddChild("PlainEntities");
            TemplateContainer domainContainer = result.AddChild("Domains");
            TemplateContainer enumContainer = result.AddChild("Enumerateds");
            TemplateContainer brContainer = result.AddChild("BusinessRules");
            this.GenerateEntities(entityContainer);
            this.GeneratePlainEntities(plainEntityContainer);
            this.GenerateDomains(domainContainer);
            this.GenerateEnums(enumContainer);
            this.GenerateBusinessRules(brContainer);
            return result;
        }

        #endregion
    }
}
