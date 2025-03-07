using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using C4Net.Framework.Core.Utils;
using C4Net.Framework.Data.Definitions;

namespace TestBed
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DefinitionManager.LoadFromXml("TestEntity.xml", "Definitions/Entities/Entity", "TestBed.Resources", "TestBed");
            //DefinitionManager.HasEntity<AbsolutePoint>("ABSOLUTE-POINT", "ABS_POINT")
            //    .WithDepth(2).SetIsLoggable().WithModule("APPL").WithDefinition("A POINT in a geodetic system.")
            //    .HasAttribute("Id").WithAttrIx(100001).WithName("absolute-point-id").WithColumnName("abs_point_id")
            //    .WithSequenceNumber(1).SetIsPrimaryKey().SetIsForeignKey().WithDataTypeStr("NUMBER").WithNetDataTypeStr("decimal")
            //    .WithDataLength(20).WithDomId(100000914).WithRelIx(1).WithSource(10000105, 100001).WithBase(10000063, 100001)
            //    .WithDefinition("The unique value, or set of characters, assigned to represent a specific LOCATION and to distinguish it from all other LOCATIONs.")
            //    .WithRoleDefinition("The point-id of a specific ABSOLUTE-POINT (a role name for location-id).");
        }
    }
}
