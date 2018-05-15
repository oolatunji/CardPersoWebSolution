using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardPerso.Library.ModelLayer.Utility
{
    public class ActiveDirectoryHelper : ConfigurationSection
    {
        ConfigurationProperty _activeDirectoryElement;

        public ActiveDirectoryHelper()
        {
            _activeDirectoryElement = new ConfigurationProperty("activeDirectory", typeof(ActiveDirectoryElement), null);

            this.Properties.Add(_activeDirectoryElement);
        }

        public ActiveDirectoryElement ActiveDirectory
        {
            get
            {
                return this[_activeDirectoryElement] as ActiveDirectoryElement;
            }
        }
    }

    public class ActiveDirectoryElement : ConfigurationElement
    {
        ConfigurationProperty _usesActiveDirectory;
        ConfigurationProperty _adServer;
        ConfigurationProperty _adContainer;
        ConfigurationProperty _adUsername;
        ConfigurationProperty _adPassword;
        ConfigurationProperty _adServer2;
        ConfigurationProperty _adContainer2;
        ConfigurationProperty _adUsername2;
        ConfigurationProperty _adPassword2;

        public ActiveDirectoryElement()
        {
            _usesActiveDirectory = new ConfigurationProperty("usesActiveDirectory", typeof(string), "");
            _adServer = new ConfigurationProperty("adServer", typeof(string), "");
            _adContainer = new ConfigurationProperty("adContainer", typeof(string), "");
            _adUsername = new ConfigurationProperty("adUsername", typeof(string), "");
            _adPassword = new ConfigurationProperty("adPassword", typeof(string), "");
            _adServer2 = new ConfigurationProperty("adServer2", typeof(string), "");
            _adContainer2 = new ConfigurationProperty("adContainer2", typeof(string), "");
            _adUsername2 = new ConfigurationProperty("adUsername2", typeof(string), "");
            _adPassword2 = new ConfigurationProperty("adPassword2", typeof(string), "");

            this.Properties.Add(_usesActiveDirectory);
            this.Properties.Add(_adServer);
            this.Properties.Add(_adContainer);
            this.Properties.Add(_adUsername);
            this.Properties.Add(_adPassword);
            this.Properties.Add(_adServer2);
            this.Properties.Add(_adContainer2);
            this.Properties.Add(_adUsername2);
            this.Properties.Add(_adPassword2);
        }

        public string UsesActiveDirectory
        {
            get
            {
                return (String)this[_usesActiveDirectory];
            }
            set
            {
                this[_usesActiveDirectory] = value;
            }
        }

        public string ADServer
        {
            get
            {
                return (String)this[_adServer];
            }
            set
            {
                this[_adServer] = value;
            }
        }

        public string ADContainer
        {
            get
            {
                return (String)this[_adContainer];
            }
            set
            {
                this[_adContainer] = value;
            }
        }

        public string ADUsername
        {
            get
            {
                return (String)this[_adUsername];
            }
            set
            {
                this[_adUsername] = value;
            }
        }

        public string ADPassword
        {
            get
            {
                return (String)this[_adPassword];
            }
            set
            {
                this[_adPassword] = value;
            }
        }

        public string ADServer2
        {
            get
            {
                return (String)this[_adServer2];
            }
            set
            {
                this[_adServer2] = value;
            }
        }

        public string ADContainer2
        {
            get
            {
                return (String)this[_adContainer2];
            }
            set
            {
                this[_adContainer2] = value;
            }
        }

        public string ADUsername2
        {
            get
            {
                return (String)this[_adUsername2];
            }
            set
            {
                this[_adUsername2] = value;
            }
        }

        public string ADPassword2
        {
            get
            {
                return (String)this[_adPassword2];
            }
            set
            {
                this[_adPassword2] = value;
            }
        }
    }
}
