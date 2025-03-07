using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C4Net.Framework.Expressions;
using MetaTool.Business.Interfaces;

namespace MetaTool.Business.Expressions
{
    /// <summary>
    /// Expression class for query the entity Dom.
    /// </summary>
    public class DomainExpression : BaseEntityExpression<DomainExpression>
    {
        #region - Properties -

        /// <summary>
        /// Expression for the column dom_id.
        /// </summary>
        /// <value>
        /// The value of the column dom_id.
        /// </value>
        public ColumnExpression DomId
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
        /// Expression for the column def_txt.
        /// </summary>
        /// <value>
        /// The value of the column def_txt.
        /// </value>
        public ColumnExpression DefTxt
        {
            get { return this.columns[2]; }
        }

        /// <summary>
        /// Expression for the column class_name_txt.
        /// </summary>
        /// <value>
        /// The value of the column class_name_txt.
        /// </value>
        public ColumnExpression ClassNameTxt
        {
            get { return this.columns[3]; }
        }

        /// <summary>
        /// Expression for the column restr_type_code.
        /// </summary>
        /// <value>
        /// The value of the column restr_type_code.
        /// </value>
        public ColumnExpression RestrTypeCode
        {
            get { return this.columns[4]; }
        }

        /// <summary>
        /// Expression for the column meas_unit_descr_txt.
        /// </summary>
        /// <value>
        /// The value of the column meas_unit_descr_txt.
        /// </value>
        public ColumnExpression MeasUnitDescrTxt
        {
            get { return this.columns[5]; }
        }

        /// <summary>
        /// Expression for the column pa_dom_id.
        /// </summary>
        /// <value>
        /// The value of the column pa_dom_id.
        /// </value>
        public ColumnExpression PaDomId
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

        /// <summary>
        /// Expression for the column def_src_txt.
        /// </summary>
        /// <value>
        /// The value of the column def_src_txt.
        /// </value>
        public ColumnExpression DefSrcTxt
        {
            get { return this.columns[9]; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainExpression"/> class.
        /// </summary>
        public DomainExpression()
            : base("[%LogicalName%%]", typeof(IDomain))
        {
        }

        #endregion
    }
}
