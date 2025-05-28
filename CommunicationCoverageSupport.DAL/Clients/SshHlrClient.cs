using CommunicationCoverageSupport.Models.Entities;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.DAL.Clients
{
    public class SshHlrClient: ISshHlrClient
    {

        private readonly SshSettings _settings;


        public SshHlrClient(IOptions<SshSettings> options)
        {
            _settings = options.Value;
        }



        public async Task<string> ExecuteCommandAsync(string command)
        {
            return await Task.Run(() =>
            {
                using var client = new SshClient(_settings.Host, _settings.Port, _settings.Username, _settings.Password);
                client.Connect();

                var result = client.RunCommand(command).Result;
                client.Disconnect();

                return result;
            });
        }


    }
}
