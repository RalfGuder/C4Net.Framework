using System;
using System.Collections.Generic;
using C4Net.Framework.Core.Utils;
using C4Net.Framework.Data.DAO;
using MetaTool.Business.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Entities
{
    /// <summary>
    /// Class for the table ENT.
    /// </summary>
    [Serializable]
    public partial class Entity : IEntity, IDAO
    {
        #region - Fields - 

        /// <summary>
        /// Expression builder for this entity.
        /// </summary>
        public static readonly EntityExpression _ = new EntityExpression();

        #endregion

        #region - Properties -
		
        /// <summary>
        /// Gets or sets the value of the column ent_id
        /// </summary>
        /// <value>
        /// Value of the column ent_id
        /// </value>
        public double EntId { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column name_txt
        /// </summary>
        /// <value>
        /// Value of the column name_txt
        /// </value>
        public string NameTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column tab_name_txt
        /// </summary>
        /// <value>
        /// Value of the column tab_name_txt
        /// </value>
        public string TabNameTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column def_txt
        /// </summary>
        /// <value>
        /// Value of the column def_txt
        /// </value>
        public string DefTxt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column depen_code
        /// </summary>
        /// <value>
        /// Value of the column depen_code
        /// </value>
        public string DepenCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column depth_cnt
        /// </summary>
        /// <value>
        /// Value of the column depth_cnt
        /// </value>
        public double DepthCnt { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column stg_type_code
        /// </summary>
        /// <value>
        /// Value of the column stg_type_code
        /// </value>
        public string StgTypeCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column stdn_lvl_code
        /// </summary>
        /// <value>
        /// Value of the column stdn_lvl_code
        /// </value>
        public string StdnLvlCode { get; set; }
		
        /// <summary>
        /// Gets or sets the value of the column mod_lvl_code
        /// </summary>
        /// <value>
        /// Value of the column mod_lvl_code
        /// </value>
        public string ModLvlCode { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public List<Attribute> Attributes { get; set; }

        /// <summary>
        /// Gets or sets the sons.
        /// </summary>
        /// <value>
        /// The sons.
        /// </value>
        public List<SubtypeRelationship> Sons { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public SubtypeRelationship Parent { get; set; }

        /// <summary>
        /// Gets or sets the category attribute.
        /// </summary>
        /// <value>
        /// The category attribute.
        /// </value>
        public Attribute CategoryAttribute { get; set; }

        /// <summary>
        /// Gets or sets the son relations.
        /// </summary>
        /// <value>
        /// The son relations.
        /// </value>
        public List<CardinalityRelationship> SonRelations { get; set; }

        /// <summary>
        /// Gets or sets the parent relations.
        /// </summary>
        /// <value>
        /// The parent relations.
        /// </value>
        public List<CardinalityRelationship> ParentRelations { get; set; }

        public List<BusinessRule> BusinessRules { get; set; }

        public bool IsSelected { get; set; }

        public string ClassName
        {
            get { return StringUtil.Pascalize(this.NameTxt); }
        }


        public string PkName
        {
            get
            {
                if (this.NameTxt.Equals("ATTR"))
                {
                    return "PK_ATTR_PK";
                }
                return "PK_" + StringUtil.Pascalize(this.NameTxt);
            }
        }

        public bool IsCompleteRelationship { get; set; }

        #endregion

        #region - Constructors -

        public Entity()
        {
            this.Attributes = new List<Attribute>();
            this.Sons = new List<SubtypeRelationship>();
            this.Parent = null;
            this.CategoryAttribute = null;
            this.SonRelations = new List<CardinalityRelationship>();
            this.ParentRelations = new List<CardinalityRelationship>();
            this.BusinessRules = new List<BusinessRule>();
            this.IsSelected = false;
            this.IsCompleteRelationship = false;
        }

        #endregion

        #region - Methods -

        public void AddAttribute(Attribute attribute)
        {
            attribute.Entity = this;
            foreach (Attribute attr in this.Attributes)
            {
                if (attr.AttrSeqnrOrd == attribute.AttrSeqnrOrd)
                {
                    return;
                }
            }
            this.Attributes.Add(attribute);
        }

        public void AddSon(SubtypeRelationship rel)
        {
            this.Sons.Add(rel);
        }

        public void AddParent(SubtypeRelationship rel)
        {
            this.Parent = rel;
        }

        public void SetCategoryAttribute(int attrIx)
        {
            foreach (Attribute attr in this.Attributes)
            {
                if (attr.AttrIx == attrIx)
                {
                    this.CategoryAttribute = attr;
                    attr.IsCategoryAttribute = true;
                }
            }
        }

        public Attribute GetAttributeByIx(int attrIx)
        {
            foreach (Attribute attr in this.Attributes)
            {
                if (attr.AttrIx == attrIx)
                {
                    return attr;
                }
            }
            return null;
        }

        #endregion
    }
}
