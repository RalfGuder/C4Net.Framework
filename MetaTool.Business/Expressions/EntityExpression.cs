using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity Ent.
    /// </summary>
    public class EntityExpression : BaseEntityExpression<EntityExpression>
    {
        #region - Properties -
		
        /// <summary>
        /// Expression for the column ent_id.
        /// </summary>
        /// <value>
        /// The value of the column ent_id.
        /// </value>
        public ColumnExpression EntId 
        { 
            get { return this.columns[0]; } 
        }
		
        /// <summary>
        /// Expression for the column name_txt.
        /// </summary>
        /// <value>
        /// The value of the column name_txt.
        /// </value>
        public ColumnExpression NameTxt 
        { 
            get { return this.columns[1]; } 
        }
		
        /// <summary>
        /// Expression for the column tab_name_txt.
        /// </summary>
        /// <value>
        /// The value of the column tab_name_txt.
        /// </value>
        public ColumnExpression TabNameTxt 
        { 
            get { return this.columns[2]; } 
        }
		
        /// <summary>
        /// Expression for the column def_txt.
        /// </summary>
        /// <value>
        /// The value of the column def_txt.
        /// </value>
        public ColumnExpression DefTxt 
        { 
            get { return this.columns[3]; } 
        }
		
        /// <summary>
        /// Expression for the column depen_code.
        /// </summary>
        /// <value>
        /// The value of the column depen_code.
        /// </value>
        public ColumnExpression DepenCode 
        { 
            get { return this.columns[4]; } 
        }
		
        /// <summary>
        /// Expression for the column depth_cnt.
        /// </summary>
        /// <value>
        /// The value of the column depth_cnt.
        /// </value>
        public ColumnExpression DepthCnt 
        { 
            get { return this.columns[5]; } 
        }
		
        /// <summary>
        /// Expression for the column stg_type_code.
        /// </summary>
        /// <value>
        /// The value of the column stg_type_code.
        /// </value>
        public ColumnExpression StgTypeCode 
        { 
            get { return this.columns[6]; } 
        }
		
        /// <summary>
        /// Expression for the column stdn_lvl_code.
        /// </summary>
        /// <value>
        /// The value of the column stdn_lvl_code.
        /// </value>
        public ColumnExpression StdnLvlCode 
        { 
            get { return this.columns[7]; } 
        }
		
        /// <summary>
        /// Expression for the column mod_lvl_code.
        /// </summary>
        /// <value>
        /// The value of the column mod_lvl_code.
        /// </value>
        public ColumnExpression ModLvlCode 
        { 
            get { return this.columns[8]; } 
        }
		
        #endregion 

        #region - Constructors -
		
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityExpression"/> class.
        /// </summary>
        public EntityExpression()
            : base("[%LogicalName%%]", typeof(IEntity))
        {
        }

        #endregion
    }
}
