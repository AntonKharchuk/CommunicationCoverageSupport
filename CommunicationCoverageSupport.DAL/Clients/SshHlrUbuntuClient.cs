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
    public class SshHlrUbuntuClient : ISshHlrClient
    {

        private readonly SshSettings _settings;


        public SshHlrUbuntuClient(IOptions<SshSettings> options)
        {
            _settings = options.Value;
        }
        private async Task<string> ExecuteCommandAsync(string command)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using var client = new SshClient(_settings.Host, _settings.Port, _settings.Username, _settings.Password);
                    client.Connect();

                    if (!client.IsConnected)
                    {
                        return "Unable to connect to HLR.";
                    }

                    var result = client.RunCommand(command).Result;
                    client.Disconnect();

                    return result;
                }
                catch (Exception ex)
                {
                    return $"SSH command failed - {ex.Message}";
                }
            });
        }

        public async Task<string> AddSimCardAsync(string imsi)
        {
            string command = $"/home/ubuntu/simcards/add_imsi.sh {imsi}";
            return await ExecuteCommandAsync(command);
        }

        public async Task<string> RemoveSimCardAsync(string imsi)
        {
            string command = $"/home/ubuntu/simcards/remove_imsi.sh {imsi}";
            return await ExecuteCommandAsync(command);
        }
    }
}
