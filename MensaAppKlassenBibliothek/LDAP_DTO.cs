using System.DirectoryServices.Protocols;

namespace MensaAppKlassenBibliothek
{
    public class LDAP_DTO
    {
        public string UserType { get; set; }
        public SearchResultEntryCollection UserDetails { get; set; }
    }
}
