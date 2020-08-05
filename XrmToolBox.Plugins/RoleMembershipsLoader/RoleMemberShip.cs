using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;

namespace RoleMembershipsLoader.Models
{
    public class SpreadSheetRangeDefinition
    {
        public string WorksheetName { get; set; }
        public Tuple<int, int, int> UserName { get; set; }
        public Tuple<int, int, int> Manager { get; set; }
        public Tuple<int, int, int> Position { get; set; }
        public Tuple<int, int, int> Site { get; set; }
        public Tuple<int, int, int> Territory { get; set; }
        public Tuple<int, int, int> LegalEntity { get; set; }
        public Tuple<int, int, int> BusinessUnit { get; set; }
        public Tuple<int, int, int> SecurityRoles { get; set; }
        public Tuple<int, int, int> FieldSecurityProfile { get; set; }
        public bool AllRows { get; set; }
        public Tuple<int, int, int> RowsToLoad { get; set; }
    }
    public class ImportContent
    {
        public ImportContent()
        {
            this.RoleMemberships = new List<RoleMembership>();
            this.ProfileMemberships = new List<ProfileMembership>();
        }
        public List<RoleMembership> RoleMemberships { get; set; }
        public List<ProfileMembership> ProfileMemberships { get; set; }
        public List<User> Users { get; set; }
    }

    public class RoleMembership
    {
        public User User { get; set; }
        public Role Role { get; set; }
        public bool Assign { get; set; }
    }

    public class EntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class BusinessUnit : EntityBase
    {
    }

    public class Role : EntityBase
    {
        public BusinessUnit BusinessUnit { get; set; }
    }

    public class User : EntityBase
    {
        public User()
        {
            IsActive = true;
        }
        public User Manager { get; set; }
        public bool IsManagerIncluded { get; set; }
        public Position Position { get; set; }
        public bool IsPositionIncluded { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public Site Site { get; set; }
        public bool IsSiteIncluded { get; set; }
        public Territory Territory { get; set; }
        public bool IsTerritoryIncluded { get; set; }
        public LegalEntity LegalEntity { get; set; }
        public bool IsLegalEntityIncluded { get; set; }
        public bool IsActive { get; set; }
    }

    public class Territory : EntityBase
    {
    }
    public class Position : EntityBase
    {
    }

    public class LegalEntity : EntityBase
    {
    }

    public class Site : EntityBase
    {
    }

    public class FieldSecurityProfile : EntityBase
    {
    }

    public class ProfileMembership
    {
        public User User { get; set; }
        public FieldSecurityProfile FieldSecurityProfile { get; set; }
        public bool Assign { get; set; }
    }

    public class NotificationMessage: EventArgs
    {
        public NotificationMessage(string message)
        {
            this.Message = message;
        }
        public NotificationMessage(string message, int percentage) : this(message)
        {
            this.Percentage = percentage;
        }

        public string Message { get; set; }
        public int Percentage { get; set; }
    }
}
