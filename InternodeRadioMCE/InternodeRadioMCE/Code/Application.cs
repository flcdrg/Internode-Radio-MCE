using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Microsoft.MediaCenter;
using Microsoft.MediaCenter.Hosting;
using Microsoft.MediaCenter.UI;

namespace InternodeRadioMCE
{
    public class Application : ModelItem
    {
        private AddInHost host;
        private HistoryOrientedPageSession session;

        public Application()
            : this(null, null)
        {
        }

        public Application(HistoryOrientedPageSession session, AddInHost host)
        {
            this.session = session;
            this.host = host;
        }

        public MediaCenterEnvironment MediaCenterEnvironment
        {
            get
            {
                if (host == null) return null;
                return host.MediaCenterEnvironment;
            }
        }

        public MediaExperience MediaExperience
        {
            get { return MediaCenterEnvironment.MediaExperience; }
        }

        public void GoToMenu()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties["Application"] = this;

            if (session != null)
            {
                session.GoToPage("resx://InternodeRadioMCE/InternodeRadioMCE.Resources/Menu", properties);
            }
            else
            {
                Debug.WriteLine("GoToMenu");
            }
        }

        public void DialogTest(string strClickedText, string link)
        {
            int timeout = 5;
            bool modal = true;
            string caption = Resources.DialogCaption;

            if (session != null)
            {
                if (MediaCenterEnvironment.PlayMedia(MediaType.Audio, link, true))
                    MediaExperience.GoToFullScreen(); 

                
            }
            else
            {
                Debug.WriteLine("DialogTest");
            }
        }

        
        public Radio[] MyData
        {
            get
            {
                XDocument feedXML = XDocument.Load("http://feeds.internode.on.net/radio.rss");

                var feeds = from feed in feedXML.Descendants("item")
                            select new Radio()
                            {
                                Title = feed.Element("title").Value,
                                Link = feed.Element("link").Value,
                                Description = feed.Element("description").Value
                            };

                var list = new Choice();
                var array = feeds.OrderBy(x => x.Title).ToArray();
                list.Options = array;

                //return list;
                return array; // new string[4] { "Alpha", "Bravo", "Charlie", "Delta" };
            }
        }
    }

    public class Radio
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
    }
}