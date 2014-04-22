using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Posti.Domain
{
    public sealed class TrackedItem
    {
        private String trackingCode;
        public System.String TrackingCode
        {
            get { return trackingCode; }
            set { trackingCode = value; }
        }
        private List<Entry> entries = new List<Entry>();

        public IList<Entry> Entries
        {
            get { return entries; }
            set { entries = value as List<Entry>; }
        } 


        public void AddEntry(Domain.Entry e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("Entry to item must not be null");
            }

            entries.Add(e);
        }

        public sealed override String ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(trackingCode);
            foreach (var entry in entries)
            {
                builder.AppendLine(entry.ToString() + "\n");                
            }
            builder.AppendLine("\n");
            return builder.ToString();
        }

        public sealed override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }
            TrackedItem that = obj as TrackedItem;

            if (this.trackingCode != that.trackingCode || this.entries.Count != that.entries.Count)
            {
                return false;
            }

            for (int i = 0; i < this.entries.Count; ++i)
            {
                if (!this.entries[i].Equals(that.entries[i]))
                {
                    return false;
                }
            }


            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
