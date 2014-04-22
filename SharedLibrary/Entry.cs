using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Posti.Domain
{
    public sealed class Entry
    {
        private String description;

	    public String Description
	    {
		    get { return description; }
		    set { description = value; }
	    }

        private KeyValuePair<String, String> registration;
        public KeyValuePair<String, String> Registration
        {
            get { return registration; }
            set { registration = value; }
        }

        private KeyValuePair<String, String> location;
        public KeyValuePair<String, String> Location
        {
            get { return location; }
            set { location = value; }
        }

        public sealed override string ToString()
        {
            return description + "\n" + registration.Key + " " + registration.Value +
                "\n" + location.Key + " " + location.Value;
        }

        public sealed override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }
            Entry that = obj as Entry;

            return this.description == that.description && this.registration.Key.Equals(that.registration.Key) &&
                this.registration.Value.Equals(that.registration.Value) && this.location.Key.Equals(that.location.Key) &&
                this.location.Value.Equals(that.location.Value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
