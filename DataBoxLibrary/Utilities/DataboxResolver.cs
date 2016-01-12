using DataBoxLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataBoxLibrary.Utilities
{
    /// <summary>
    /// Custom <see cref="DataContractResolver"/>.
    /// </summary>
    public class DataboxResolver: DataContractResolver
    {
        private const string uri = "http://steven.law";

        /// <summary>
        /// Override this method to map the specified xsi:type name and namespace to a data contract type during deserialization.
        /// </summary>
        /// <param name="typeName">The xsi:type name to map.</param>
        /// <param name="typeNamespace">The xsi:type namespace to map.</param>
        /// <param name="declaredType">The type declared in the data contract.</param>
        /// <param name="knownTypeResolver">The known type resolver.</param>
        /// <returns>
        /// The type the xsi:type name and namespace is mapped to.
        /// </returns>
        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            if (typeName == "Tag" && typeNamespace == uri)
                return typeof(Tag);
            else if (typeName == "Entry" && typeNamespace == uri)
                return typeof(Entry);
            else if (typeName == "LinkEntry" && typeNamespace == uri)
                return typeof(LinkEntry);
            else if (typeName == "LinkItem" && typeNamespace == uri)
                return typeof(LinkItem);
            else return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
        }

        /// <summary>
        /// Override this method to map a data contract type to an xsi:type name and namespace during serialization.
        /// </summary>
        /// <param name="type">The type to map.</param>
        /// <param name="declaredType">The type declared in the data contract.</param>
        /// <param name="knownTypeResolver">The known type resolver.</param>
        /// <param name="typeName">The xsi:type name.</param>
        /// <param name="typeNamespace">The xsi:type namespace.</param>
        /// <returns>
        /// true if mapping succeeded; otherwise, false.
        /// </returns>
        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out System.Xml.XmlDictionaryString typeName, out System.Xml.XmlDictionaryString typeNamespace)
        {
            if (type == typeof(Tag))
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add("Tag");
                typeNamespace = dictionary.Add(uri);
                return true;
            }
                else if (type == typeof(Entry))
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add("Entry");
                typeNamespace = dictionary.Add(uri);
                return true;
            }
            else if (type == typeof(LinkEntry))
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add("LinkEntry");
                typeNamespace = dictionary.Add(uri);
                return true;
            }
            else if (type == typeof(LinkItem))
            {
                XmlDictionary dictionary = new XmlDictionary();
                typeName = dictionary.Add("LinkItem");
                typeNamespace = dictionary.Add(uri);
                return true;
            }
            else
                return knownTypeResolver.TryResolveType(type, declaredType, knownTypeResolver, out typeName, out typeNamespace);
        }
    }
}
