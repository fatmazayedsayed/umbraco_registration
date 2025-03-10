using System.ServiceModel.Syndication;

namespace umbraco_registration.Interfaces
{
    public interface ISyndicationXmlService
    {
        Atom10FeedFormatter GenerateAtomXml(string title, string description);
        Rss20FeedFormatter GenerateRssXml(string title, string description);
    }
}