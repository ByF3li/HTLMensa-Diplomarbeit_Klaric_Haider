using MensaWebsite.Models.DB;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.DirectoryServices.Protocols;

namespace MensaWebsite.Controllers.DB
{
    [Route("api/[controller]")]
    [ApiController]
    public class LdapAPIController : ControllerBase
    {
        private MenuContext _context = new MenuContext();

        public LdapAPIController(MenuContext context)
        {
            this._context = context;
        }

        [HttpGet("getLDAP")]
        public async Task<IActionResult> AsyncGetLDAP(string username, string password)
        {
            string ldapServer = "10.10.80.42";
            string ldap_password = "2RFCdoJEQc!2vuWD=s#E";
            string baseDnLehrer = "OU=701417_Lehrer,OU=701417,OU=CAMPUS,DC=SYNCHTLINN,DC=local";
            string baseDnSchüler = "OU=701417_Schueler,OU=701417,OU=CAMPUS,DC=SYNCHTLINN,DC=local";
            //string bindUserDN = "CN=MensaLDAP,OU=ADMIN,DC=SYNCHTLINN,DC=local";

            var connection = await AsyncConnectToLDAP(ldapServer, ldap_password);
            if (connection != null)
            {
                Console.WriteLine("Connection Success!");

                bool validate = await AsyncValidateUser(connection, username, password);
                if (validate)
                {
                    SearchResultEntryCollection isTeacher = SearchUser(connection, baseDnLehrer, username);
                    SearchResultEntryCollection isStudent = SearchUser(connection, baseDnSchüler, username);

                    if (isTeacher != null)
                    {
                        return new JsonResult(isTeacher);
                    }
                    else if (isStudent != null)
                    {
                        return new JsonResult(isStudent);
                    }
                    else
                    {
                        Console.WriteLine("Houston we have a problem");
                    }
                }
                else
                {
                    Console.WriteLine("User doesn't exist");
                }

            }
            else
            {
                Console.WriteLine("Connection Failed!");
            }
            return new JsonResult(true);
        }

        private async Task<LdapConnection> AsyncConnectToLDAP(string ldapServer, string ldap_password)
        {
            LdapConnection ldapConnection = null;

            try
            {
                // Set up credentials and LDAP directory identifier
                var credentials = new NetworkCredential("MensaLDAP", ldap_password, "SYNCHTLINN");
                var serverId = new LdapDirectoryIdentifier(ldapServer, 389, false, false);
                ldapConnection = new LdapConnection(serverId, credentials);
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.Bind(credentials);
                return ldapConnection;

            }
            catch (LdapException e)
            {
                Console.WriteLine(e);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            return ldapConnection;
        }

        private async Task<bool> AsyncValidateUser(LdapConnection ldapConnection, string username, string password)
        {
            var credentials = new NetworkCredential(username, password, "SYNCHTLINN");
            var serverId = new LdapDirectoryIdentifier("10.10.80.42", 389, false, false);

            var conn = new LdapConnection(serverId, credentials);
            conn.SessionOptions.ProtocolVersion = 3;
            conn.AuthType = AuthType.Basic;
            try
            {
                conn.Bind(credentials);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            conn.Dispose();
        }

        private SearchResultEntryCollection SearchUser(LdapConnection ldapConnection, string baseDn, string username)
        {
            // Create an LDAP search request
            var searchRequest = new SearchRequest(baseDn, $"(&(objectClass=person)(sAMAccountName={username}))", SearchScope.Subtree, null);

            // Perform the search and get the result
            try
            {
                var searchResponse = (SearchResponse)ldapConnection.SendRequest(searchRequest);
                // Process the search result
                foreach (SearchResultEntry entry in searchResponse.Entries)
                {
                    Console.WriteLine($"Firstname: {entry.Attributes["givenName"][0]}");
                    Console.WriteLine($"Lastname: {entry.Attributes["sn"][0]}");
                }
                if (searchResponse.Entries.Count == 1)
                {
                    return searchResponse.Entries;
                }
                else
                {
                    return null; 
                }
            }
            catch (LdapException e)
            {
                Console.WriteLine(e);
                return null; 
            }
        }
    }
}
