using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.Email
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
        public virtual async Task<IEmailSender> GetEmailSender(string lookupKey = null)
        {
            //if lookupkey matches provider name return that one
            if(!string.IsNullOrEmpty(lookupKey))
            {
                foreach(var sender in _allSenders)
                {
                    if(sender.Name == lookupKey)
                    {
                        var configured = await sender.IsConfigured(lookupKey);
                        return sender;
                    }
                }
            }

            // return first configured one
            foreach (var sender in _allSenders)
            {
                var configured = await sender.IsConfigured(lookupKey);
                if(configured) { return sender; }
            }

            // last ditch return the first one in the list configured or not
            foreach (var sender in _allSenders)
            {
                //var configured = await sender.IsConfigured(lookupKey);
                return sender;
            }


            return null;
            
        }
    }
}
