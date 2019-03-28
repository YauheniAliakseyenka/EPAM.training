using System.Configuration;
using System.Linq;

namespace TicketManagementWPF.Infrastructure
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

        [ConfigurationProperty("credentials")]
        public CredentialsCollection Credentials
		{
            get { return (CredentialsCollection)this["credentials"]; }
        }
    }

    [ConfigurationCollection(typeof(CredentialsElement))]
    internal class CredentialsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CredentialsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CredentialsElement)element).Name;
        }

        public new CredentialsElement this[string name]
        {
            get
            {
                return this.OfType<CredentialsElement>().FirstOrDefault(item => item.Name == name);
            }
        }
    }

    internal class CredentialsElement : ConfigurationElement
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
    }
}