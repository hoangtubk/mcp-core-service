using System.Xml.Serialization;

namespace mcp_core_service.Models.Response
{
    [XmlRoot("rss")]
    public class XSKTResponse
    {
        [XmlAttribute("version")]
        public string? Version { get; set; }

        [XmlElement("channel")]
        public Channel? Channel { get; set; }
    }

    public class Channel
    {
        [XmlElement("title")]
        public string? Title { get; set; }

        [XmlElement("description")]
        public string? Description { get; set; }

        [XmlElement("link")]
        public string? Link { get; set; }

        [XmlElement("copyright")]
        public string? Copyright { get; set; }

        [XmlElement("generator")]
        public string? Generator { get; set; }

        [XmlElement("pubDate")]
        public string? PubDate { get; set; }

        [XmlElement("lastBuildDate")]
        public string? LastBuildDate { get; set; }

        [XmlElement("item")]
        public List<Item>? Items { get; set; }
    }

    public class Item
    {
        [XmlElement("title")]
        public string? Title { get; set; }

        [XmlElement("description")]
        public string? Description { get; set; }

        [XmlElement("link")]
        public string? Link { get; set; }

        [XmlElement("pubDate")]
        public string? PubDate { get; set; }
    }
}
