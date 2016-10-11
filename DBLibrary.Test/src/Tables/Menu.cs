using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBLibrary.Mapper;

namespace Core.Tables
{
    public class Menu
    {
        public int id { set; get; }
        public String Icon { set; get; }
        public String Name { set; get; }
        public String Link { set; get; }
        public bool? Status { set; get; }
        public int? ParentId { set; get; }
        public int? PortalId { set; get; }
    }

    public class MenuMap : ClassMap<Menu>
    {
        public MenuMap()
        {
            MapIdentity(m => m.id).SetColumn("ID");
            MapField(m => m.Icon).SetColumn("ICON");
            MapField(m => m.Name).SetColumn("NAME");
            MapField(m => m.Link).SetColumn("LINK");
            MapField(m => m.Status).SetColumn("STATUS");
            MapField(m => m.ParentId).SetColumn("PARENTID");
            MapField(m => m.PortalId).SetColumn("PORTALID");
            SetTableName("MENUE");
        }
    }
}