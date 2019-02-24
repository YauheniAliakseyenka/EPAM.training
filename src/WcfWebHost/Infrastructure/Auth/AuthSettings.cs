using System;
using System.Configuration;
using System.Linq;

namespace WcfWebHost.Infrastructure.Auth
{
    internal class AuthSettings : ConfigurationSection
    {
        private static AuthSettings settings = ConfigurationManager.GetSection("authentication") as AuthSettings;
        public static AuthSettings Settings
        {
            private set { settings = ConfigurationManager.GetSection("authentication") as AuthSettings; }
            get
            {
                return settings;
            }
        }

        [ConfigurationProperty("clients")]
        public ClientCollection Clients
        {
            get { return (ClientCollection)this["clients"]; }
        }
    }

    [ConfigurationCollection(typeof(ClientElement))]
    internal class ClientCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "client";

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ClientElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClientElement)element).Name;
        }

        public new ClientElement this[string username]
        {
            get
            {
                return this.OfType<ClientElement>().FirstOrDefault(item => item.Username == username);
            }
        }

        protected override string ElementName => PropertyName;

        protected override bool IsElementName(string elementName) =>
            elementName.Equals(PropertyName, StringComparison.OrdinalIgnoreCase);

        public ClientElement this[int idx]
        {
            get { return (ClientElement)BaseGet(idx); }
        }
    }

    internal class ClientElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public string Username
        {
            get { return (string)base["username"]; }
            set { base["username"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }       

        [ConfigurationProperty("roles")]
        public RoleCollection Roles
        {
            get { return (RoleCollection)this["roles"]; }
        }
    }

    [ConfigurationCollection(typeof(RoleElement))]
    internal class RoleCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "role";

        protected override ConfigurationElement CreateNewElement()
        {
            return new RoleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RoleElement)element).Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }

        protected override string ElementName => PropertyName;
    }

    internal class RoleElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }
    }
}