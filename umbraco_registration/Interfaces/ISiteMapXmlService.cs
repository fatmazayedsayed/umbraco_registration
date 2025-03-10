namespace umbraco_registration.Interfaces
{
    public interface ISiteMapXmlService
    {
        string GenerateXml(Guid startNode, bool includeSelf = true);
    }
}