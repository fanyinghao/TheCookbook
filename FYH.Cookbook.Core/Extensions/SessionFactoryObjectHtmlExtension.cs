using System.IO;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using Spring.Data.NHibernate;

namespace FYH.Cookbook.Core.Extensions
{
    /// <summary>
    /// Config NHibernate.Mapping.Attributes
    /// </summary>
    public class SessionFactoryObjectHtmlExtension : LocalSessionFactoryObject
    {
        public SessionFactoryObjectHtmlExtension() { }

        public string[] MappingAssemblyNames { get; set; }

        protected override void PostProcessConfiguration(Configuration config)
        {
            using (var stream = new MemoryStream())
            {
                HbmSerializer.Default.Validate = true;
                foreach (var name in MappingAssemblyNames)
                {
                    var asm = Assembly.Load(name);
                    HbmSerializer.Default.Serialize(stream, asm);
                    stream.Position = 0;
                    config.AddInputStream(stream);
                }
            }
        }
    }
}
