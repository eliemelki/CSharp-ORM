
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBLibrary.Mapper;

namespace Core.Tables
{
  
    
    public class Portal 
    {
        public Portal()
        {
          
        }

        public int id { set; get; }
        public String Title { set; get; }
        public String Http { set; get; }
    }

    public class PortalMap : ClassMap<Portal>
    {
        public PortalMap()
        {
            MapIdentity(m => m.id).SetColumn("PortalID");
            MapField(m => m.Title).SetColumn("PortalTitle");
            MapField(m => m.Http).SetColumn("PortalHTTP");
            SetTableName("Portal");

        }
    }
}