using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace RoleMembershipsLoader
{
    public static class Helper
    {
        public static bool ParseBoolean(string word)
        {
            bool state = false;
            if (bool.TryParse(word, out state))
            {
                return state;
            }
            else
            {
                if (word.ToLower() == "no") return false;
                if (word.ToLower() == "yes") return true;
            }
            return state;
        }

        public static Guid FetchRecord(IOrganizationService service, string query, string filedName)
        {
            var result = service.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return Guid.Empty;
            foreach (var record in result.Entities)
            {
                return record.GetAttributeValue<Guid>(filedName);
            }
            return Guid.Empty;
        }
    }
}
