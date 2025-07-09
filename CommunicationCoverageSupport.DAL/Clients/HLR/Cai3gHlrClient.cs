using CommunicationCoverageSupport.Models.DTOs;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace CommunicationCoverageSupport.DAL.Clients.HLR
{
    public class Cai3gHlrClient: ICai3gHlrClient
    {

        private readonly HttpClient _httpClient;
        private readonly string _login;
        private readonly string _password;

        private string _sessionId;
        private DateTime _sessionStartTime;

        private readonly TimeSpan _sessionLifetime = TimeSpan.FromMinutes(9); // expire slightly before 10min

        public Cai3gHlrClient(IHttpClientFactory httpClientFactory, IOptions<HlrSettings> hlrSettings)
        {
            _httpClient = httpClientFactory.CreateClient("HlrClient");
            _login = hlrSettings.Value.Username;
            _password = hlrSettings.Value.Password;
        }
        private bool IsSessionExpired()
        {
            return string.IsNullOrEmpty(_sessionId) || (DateTime.UtcNow - _sessionStartTime) > _sessionLifetime;
        }

        private async Task<string> GetSessionIdAsync()
        {
            if (IsSessionExpired())
            {
                _sessionId = await SendLoginRequestAsync();
                _sessionStartTime = DateTime.UtcNow;
            }

            return _sessionId;
        }
        public async Task<HlrStatusDto> GetAsync(string imsi)
        {
            string sessionId = await GetSessionIdAsync();

            string xmlTemplate = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:cai3=""http://schemas.ericsson.com/cai3g1.2/"" xmlns:gsm=""http://schemas.ericsson.com/ema/UserProvisioning/GsmHlr/"">
   <soapenv:Header>
      <cai3:SequenceId>?</cai3:SequenceId>
      <cai3:TransactionId>?</cai3:TransactionId>
      <cai3:SessionId>{0}</cai3:SessionId>
   </soapenv:Header>
   <soapenv:Body>
      <cai3:Get>
         <cai3:MOType>Subscription@http://schemas.ericsson.com/ema/UserProvisioning/GsmHlr/</cai3:MOType>
         <cai3:MOId>
            <gsm:imsi>{1}</gsm:imsi>
         </cai3:MOId>
      </cai3:Get>
   </soapenv:Body>
</soapenv:Envelope>";

            string requestBody = string.Format(xmlTemplate, sessionId, imsi);

            var request = new HttpRequestMessage(HttpMethod.Post, "")
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/xml")
            };

            var response = await _httpClient.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();
            XDocument doc = XDocument.Parse(responseContent);

            XNamespace gsmNs = "http://schemas.ericsson.com/ema/UserProvisioning/GsmHlr/";

            var msisdnState = doc.Descendants(gsmNs + "msisdnstate").FirstOrDefault()?.Value;

            HlrStatusDto hlrStatus = new HlrStatusDto
            {
                Imsi = imsi,
                MsisdnState = msisdnState
            };

            return hlrStatus;
        }


        public async Task LoginAsync()
        {
            _sessionId = await SendLoginRequestAsync();
            _sessionStartTime = DateTime.UtcNow;
        }
        private async Task<string> SendLoginRequestAsync()
        {
            // Hardcoded XML template with placeholders
            string xmlTemplate = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:cai3=""http://schemas.ericsson.com/cai3g1.2/"">
    <soapenv:Header/>
    <soapenv:Body>
        <cai3:Login>
            <cai3:userId>{0}</cai3:userId>
            <cai3:pwd>{1}</cai3:pwd>
        </cai3:Login>
    </soapenv:Body>
</soapenv:Envelope>";

            // Format the XML with the username and password
            string requestBody = string.Format(xmlTemplate, _login, _password);

            var request = new HttpRequestMessage(HttpMethod.Post, "") // empty because BaseAddress is already set
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "text/xml")
            };

            try
            {
                var response = await _httpClient.SendAsync(request);

                var context = await response.Content.ReadAsStringAsync();

                var document = XDocument.Parse(context);
                XNamespace ns = "http://schemas.ericsson.com/cai3g1.2/";
                return document.Descendants(ns + "sessionId").FirstOrDefault()?.Value;
            }
            catch (HttpRequestException ex)
            {
                // You can log ex.Message here if you want
                return $"HTTP Request failed: {ex.Message}";
            }
        }



    }
}
