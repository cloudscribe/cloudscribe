using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Messaging.Email
{

    public class ConfigEmailSenderResolver : IEmailSenderResolver
    {
        public ConfigEmailSenderResolver(
            IEnumerable<IEmailSender> allSenders
            )
        {
            _allSenders = allSenders;
        }

        private IEnumerable<IEmailSender> _allSenders;

        /// <summary>
        /// in this implementation the lookupkey is used to lookup a sender by name
        /// if found it will return that
        /// else it will try to return the first configured sender it finds
        /// else the first sender in the list configured or not
        /// else null
        /// </summary>
        /// <param name="lookupKey"></param>
        /// <returns></returns>
        public async Task<IEmailSender> GetEmailSender(string lookupKey = null)
        {
            
            if(!string.IsNullOrEmpty(lookupKey))
            {
                foreach(var sender in _allSenders)
                {
                    if(sender.Name == lookupKey)
                    {
                        return sender;
                    }
                }
            }

            // return first configured one
            foreach (var sender in _allSenders)
            {
                var configured = await sender.IsConfigured();
                if(configured) { return sender; }
            }

            // last ditch return the first one in the list configured or not
            foreach (var sender in _allSenders)
            {
                return sender;
            }


            return null;
            
        }
    }
}
