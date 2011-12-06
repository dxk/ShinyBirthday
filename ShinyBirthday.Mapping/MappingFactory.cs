using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg.MappingSchema;
using System.Reflection;
using ConfOrm;
using ConfOrm.NH;
using ConfOrm.Shop.CoolNaming;
using ConfOrm.Patterns;
using ShinyBirthday.Entity;

namespace EntityMapping
{
    public class MappingFactory
    {
        public HbmMapping CreateMapping()
        {
            var orm = new ObjectRelationalMapper();
            //主键生成策略(自增)
            orm.Patterns.PoidStrategies.Add(new NativePoidPattern());
            //实体表们
            var entities = new[]{
                typeof(ShinyInformation),
                typeof(Messages)
            };
            orm.TablePerClass(entities);
            var mappr = new Mapper(orm, new CoolPatternsAppliersHolder(orm));
            return mappr.CompileMappingFor(Assembly.Load(typeof(ShinyInformation).Namespace).GetTypes());
        }
    }
}
