using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace SharedLibrary.Posti.Dao
{
    
    class TrackPageParser
    {
        private readonly String html;

        public TrackPageParser(String html)
        {
            this.html = html;
        }
        public Domain.TrackedItem Parse()
        {
            var item = new Domain.TrackedItem();
            var descriptions = Parse("<div class=\"shipment-event-table-header\" colspan=\"2\">(.*)</div>");
            var labels = Parse("<span class=\"shipment-event-table-label\">(.*)</span>");
            var data = Parse("<span class=\"shipment-event-table-data\">(.*)</span>");

            for (int i = 0; i < descriptions.Count; ++i)
            {
                Domain.Entry e = new Domain.Entry();
                e.Description = descriptions[i];
                e.Registration = new KeyValuePair<String, String>(labels[2 * i], data[2 * i]);
                e.Location = new KeyValuePair<String, String>(labels[2 * i + 1], data[2 * i + 1]);
                item.AddEntry(e);

            }
            return item;
        }

        private List<String> Parse(string regex)
        {
            var r = new Regex(regex);
            var matches = r.Matches(html);

            List<String> values = new List<String>();
            foreach (Match match in matches)
            {
                String str = match.Groups[1].ToString();

                if (str == String.Empty)
                {
                    continue;
                }

                values.Add(match.Groups[1].ToString());
            }
            return values;
        }


    }
}
