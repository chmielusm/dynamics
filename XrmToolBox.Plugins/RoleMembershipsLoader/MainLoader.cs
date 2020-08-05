using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RoleMembershipsLoader.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoleMembershipsLoader
{
    public class MainLoader
    {
        private readonly IOrganizationService _organizationService;
        private readonly DataSet _fileContent;

        public event EventHandler OnWarning;
        public event EventHandler OnProgress;

        public MainLoader(IOrganizationService organizationService, DataSet fileContent)
        {
            _organizationService = organizationService;
            _fileContent = fileContent;
        }

        public ImportContent ParseFile(SpreadSheetRangeDefinition definition)
        {
            ImportContent content = new ImportContent()
            {
                Users = new List<User>()
            };

            List<Role> roleList = new List<Role>();
            List<FieldSecurityProfile> profiles = new List<FieldSecurityProfile>();

            var worksheet = _fileContent.Tables[definition.WorksheetName];     

            // read roles
            for (int i = definition.SecurityRoles.Item2; i <= definition.SecurityRoles.Item3; i++)
            {
                roleList.Add(new Role() { Name = worksheet.Rows[definition.SecurityRoles.Item1][i].ToString() });
            }

            // read security profiles
            for (int i = definition.FieldSecurityProfile.Item2; i <= definition.FieldSecurityProfile.Item3; i++)
            {
                profiles.Add(new FieldSecurityProfile() { Name = worksheet.Rows[definition.FieldSecurityProfile.Item1][i].ToString() });
            }

            //parse user rows
            var startRow = definition.UserName.Item1 + 1;
            var endRow = worksheet.Rows.Count;

            if (!definition.AllRows)
            {
                startRow = definition.RowsToLoad.Item1;
                endRow = definition.RowsToLoad.Item2;
                if (endRow == 0) endRow = startRow + 1;
            }

            if (startRow > worksheet.Rows.Count)
            {
                throw new InvalidOperationException($"Invalid row #{startRow+1} for processing. Possible row# {worksheet.Rows.Count}");
            }

            for (int i = startRow; i < endRow; i++)
            {                
                User user = new User();

                for (int colCounter = definition.UserName.Item2; colCounter < worksheet.Columns.Count; colCounter++)
                {
                    // user name
                    if (colCounter == definition.UserName.Item2)
                    {
                        user.Name = worksheet.Rows[i][colCounter].ToString();

                        LogProgress($"Parsing user {user.Name}", i);
                    }
                    // manager
                    if (colCounter == definition.Manager.Item2)
                    {
                        user.Manager = new User
                        {
                            Name = worksheet.Rows[i][colCounter].ToString()
                        };
                        user.IsManagerIncluded = true;
                    }
                    // position
                    if (colCounter == definition.Position.Item2)
                    {
                        user.Position = new Position
                        {
                            Name = worksheet.Rows[i][colCounter].ToString()
                        };
                        user.IsPositionIncluded = true;
                    }
                    // site
                    if (colCounter == definition.Site.Item2)
                    {
                        user.Site = new Site
                        {
                            Name = worksheet.Rows[i][colCounter].ToString()
                        };
                        user.IsSiteIncluded = true;
                    }
                    // territory
                    if (colCounter == definition.Territory.Item2)
                    {
                        user.Territory = new Territory
                        {
                            Name = worksheet.Rows[i][colCounter].ToString()
                        };
                        user.IsTerritoryIncluded = true;
                    }
                    // le
                    if (colCounter == definition.LegalEntity.Item2)
                    {
                        user.LegalEntity = new LegalEntity
                        {
                            Name = worksheet.Rows[i][colCounter].ToString()
                        };
                        user.IsLegalEntityIncluded = true;
                    }
                    // bu
                    if (colCounter == definition.BusinessUnit.Item2)
                    {
                        user.BusinessUnit = new BusinessUnit
                        {
                            Name = worksheet.Rows[i][colCounter].ToString()
                        };
                    }
                    // security roles
                    if (colCounter >= definition.SecurityRoles.Item2 && colCounter <= definition.SecurityRoles.Item3)
                    {
                        content.RoleMemberships.Add(new RoleMembership()
                        {
                            User = user,
                            Role = GetRole(roleList[colCounter - definition.SecurityRoles.Item2], user.BusinessUnit),
                            Assign = Helper.ParseBoolean(worksheet.Rows[i][colCounter].ToString())
                        });
                    }
                    // field security profiles
                    if (colCounter >= definition.FieldSecurityProfile.Item2 && colCounter <= definition.FieldSecurityProfile.Item3)
                    {
                        content.ProfileMemberships.Add(new ProfileMembership()
                        {
                            User = user,
                            FieldSecurityProfile = profiles[colCounter - definition.FieldSecurityProfile.Item2],
                            Assign = Helper.ParseBoolean(worksheet.Rows[i][colCounter].ToString())
                        });
                    }
                }

                content.Users.Add(user);
            }

            return content;
        }

        public ImportContent BuildImportContent(ImportContent content)
        {
            if (content.Users.Count == 0) return null;

            LogProgress("Fetching data", 1);

            var managersFromCrm = FetchSystemUsers(content.Users.Where(x => x.Manager != null).Select(x => x.Manager).Distinct());
            var positionsFromCrm = FetchPositions(content.Users.Where(x => x.Position != null).Select(x => x.Position).Distinct());
            var sitesFromCrm = FetchSites(content.Users.Where(x => x.Site != null).Select(x => x.Site).Distinct());
            var territoriesFromCrm = FetchTerritories(content.Users.Where(x => x.Territory != null).Select(x => x.Territory).Distinct());
            var legalEntitiesFromCrm = FetchLegalEntities(content.Users.Where(x => x.LegalEntity != null).Select(x => x.LegalEntity).Distinct());
            var buFromCrm = FetchBusinessUnits(content.Users.Where(x => x.BusinessUnit != null).Select(x => x.BusinessUnit).Distinct());
            var usersFromCrm = FetchSystemUsers(content.RoleMemberships.Select(x => x.User).Distinct());
            var rolesFromCrm = FetchRoles(content.RoleMemberships.Select(x => x.Role).Distinct());
            var profilesFromCrm = FetchFieldSecurityPofiles(content.ProfileMemberships.Select(x => x.FieldSecurityProfile).Distinct());
            var roleMemberships = new List<RoleMembership>();
            var profileMemberships = new List<ProfileMembership>();

            LogProgress("Fetching data", 80);

            foreach (var user in content.Users)
            {
                var tempUser = usersFromCrm.FirstOrDefault(x => x.Name == user.Name);
                if (tempUser == null)
                {
                    LogWarning("User {0} is not found in Organization", user.Name);
                    continue;
                }
                else
                {
                    user.Id = tempUser.Id;
                    if (user.Manager != null)
                    {
                        var temp = managersFromCrm.FirstOrDefault(x => x.Name == user.Manager.Name);
                        if (temp == null)
                        {
                            user.IsManagerIncluded = false;
                            LogWarning("Manager {0} is not found in Organization", user.Manager.Name);
                        }
                        else
                        {
                            user.Manager = temp;
                        }
                    }
                    if (user.Position != null)
                    {
                        var temp = positionsFromCrm.FirstOrDefault(x => x.Name == user.Position.Name);
                        if (temp == null)
                        {
                            user.IsPositionIncluded = false;
                            LogWarning("Position {0} is not found in Organization", user.Position.Name);
                        }
                        else
                        {
                            user.Position = temp;
                        }
                    }
                    if (user.Site != null)
                    {
                        var temp = sitesFromCrm.FirstOrDefault(x => x.Name == user.Site.Name);
                        if (temp == null)
                        {
                            user.IsSiteIncluded = false;
                            LogWarning("Site {0} is not found in Organization", user.Position.Name);
                        }
                        else
                        {
                            user.Site = temp;
                        }
                    }
                    if (user.Territory != null)
                    {
                        var temp = territoriesFromCrm.FirstOrDefault(x => x.Name == user.Territory.Name);
                        if (temp == null)
                        {
                            user.IsTerritoryIncluded = false;
                            LogWarning("Territory {0} is not found in Organization", user.Territory.Name);
                        }
                        else
                        {
                            user.Territory = temp;
                        }
                    }
                    if (user.LegalEntity != null)
                    {
                        var temp = legalEntitiesFromCrm.FirstOrDefault(x => x.Name == user.LegalEntity.Name);
                        if (temp == null)
                        {
                            user.IsLegalEntityIncluded = false;
                            LogWarning("Legal Entity {0} is not found in Organization", user.LegalEntity.Name);
                        }
                        else
                        {
                            user.LegalEntity = temp;
                        }
                    }
                    if (user.BusinessUnit != null)
                    {
                        var temp = buFromCrm.FirstOrDefault(x => x.Name == user.BusinessUnit.Name);
                        if (temp == null)
                        {
                            user.BusinessUnit = null;
                            LogWarning("Business Unit {0} is not found in Organization", user.BusinessUnit.Name);
                        }
                        else
                        {
                            user.BusinessUnit = temp;
                        }
                    }
                }
            }

            LogProgress("Preparing memberships", 80);
            foreach (var membership in content.RoleMemberships)
            {
                var role = membership.Role;
                var user = membership.User;
                membership.Role = rolesFromCrm.FirstOrDefault(x => x.Name.ToLower() == role.Name.ToLower() && x.BusinessUnit.Name.ToLower() == role.BusinessUnit.Name.ToLower());
                membership.User = usersFromCrm.FirstOrDefault(x => x.Name.ToLower() == user.Name.ToLower());
                if (membership.Role == null)
                {
                    LogWarning("Role {0} is not found in Organization", role.Name);
                    continue;
                }
                if (membership.User == null)
                {
                    LogWarning("User {0} is not found in Organization", user.Name);
                    continue;
                }
                roleMemberships.Add(membership);
            }

            LogProgress("Preparing memberships", 90);
            foreach (var membership in content.ProfileMemberships)
            {
                var profile = membership.FieldSecurityProfile;
                var user = membership.User;
                membership.FieldSecurityProfile = profilesFromCrm.FirstOrDefault(x => x.Name.ToLower() == profile.Name.ToLower());
                membership.User = usersFromCrm.FirstOrDefault(x => x.Name.ToLower() == user.Name.ToLower());
                if (membership.FieldSecurityProfile == null)
                {
                    LogWarning("Field Security Profile {0} is not found in Organization", profile.Name);
                    continue;
                }
                if (membership.User == null)
                {
                    LogWarning("User {0} is not found in Organization", user.Name);
                    continue;
                }
                profileMemberships.Add(membership);
            }

            LogProgress("Preparing memberships", 100);

            return new ImportContent
            {
                Users = content.Users,
                ProfileMemberships = profileMemberships,
                RoleMemberships = roleMemberships
            };
        }

        private void LogWarning(string message, params object[] args)
        {
            if (OnWarning != null)
            {
                OnWarning.Invoke(this, new NotificationMessage(string.Format(message, args)));
            }
        }

        private void LogProgress(string message, int percent)
        {
            if (OnProgress != null)
            {
                OnProgress.Invoke(this, new NotificationMessage(message, percent));
            }
        }

        public void UpdateUser(User user)
        {
            Entity userToUpdate = new Entity("systemuser", user.Id);
            if (user.BusinessUnit != null)
            {
                if (user.BusinessUnit.Id == Guid.Empty)
                {
                    LogWarning($"Business Unit {user.BusinessUnit} cannot be empty Guid");
                } 
                else 
                    userToUpdate["businessunitid"] = new EntityReference("businessunit", user.BusinessUnit.Id);
            }
            if (user.IsManagerIncluded)
            {
                userToUpdate["parentsystemuserid"] = null;
                if (user.Manager != null)
                {
                    if (user.Manager.Id == Guid.Empty)
                    {
                        LogWarning($"Manager {user.Manager} cannot be empty Guid");
                    }
                    else
                        userToUpdate["parentsystemuserid"] = new EntityReference("systemuser", user.Manager.Id);
                }
            }
            if (user.IsPositionIncluded)
            {
                userToUpdate["positionid"] = null;
                if (user.Position != null)
                {
                    if (user.Position.Id == Guid.Empty)
                    {
                        LogWarning($"Position {user.Position} cannot be empty Guid");
                    }
                    else
                        userToUpdate["positionid"] = new EntityReference("position", user.Position.Id);
                }
            }
            if (user.IsTerritoryIncluded)
            {
                userToUpdate["territoryid"] = null;
                if (user.Territory != null)
                {
                    if (user.Territory.Id == Guid.Empty)
                    {
                        LogWarning($"Territory {user.Territory} cannot be empty Guid");
                    }
                    else
                        userToUpdate["territoryid"] = new EntityReference("territory", user.Territory.Id);
                }
            }
            if (user.IsSiteIncluded)
            {
                userToUpdate["siteid"] = null;
                if (user.Site != null)
                {
                    if (user.Site.Id == Guid.Empty)
                    {
                        LogWarning($"Site {user.Site} cannot be empty Guid");
                    }
                    else
                        userToUpdate["siteid"] = new EntityReference("site", user.Site.Id);
                }
            }
            if (user.IsLegalEntityIncluded)
            {
                userToUpdate["rs_legalentityid"] = null;
                if (user.LegalEntity != null)
                {
                    if (user.LegalEntity.Id == Guid.Empty)
                    {
                        LogWarning($"Legal Entity {user.LegalEntity} cannot be empty Guid");
                    }
                    else
                        userToUpdate["rs_legalentityid"] = new EntityReference("rs_legal_entity", user.LegalEntity.Id);
                }
            }

            _organizationService.Update(userToUpdate);
        }

        public void EnsureSystemUserRoleMembership(RoleMembership memberShip)
        {
            string query = $@"<fetch>
                                  <entity name='systemuserroles' >
                                    <attribute name='systemuserroleid' />
                                    <filter>
                                      <condition attribute='roleid' operator='eq' value='{ memberShip.Role.Id }' />
                                      <condition attribute='systemuserid' operator='eq' value='{ memberShip.User.Id }' />
                                    </filter>
                                  </entity>
                                </fetch>";

            Guid membershipFromCrm = Helper.FetchRecord(_organizationService, query, "systemuserroleid");
            if (memberShip.Assign)
            {
                if (membershipFromCrm == Guid.Empty)
                {
                    _organizationService.Associate("systemuser",
                                    memberShip.User.Id,
                                    new Relationship("systemuserroles_association"),
                                    new EntityReferenceCollection() { new EntityReference("role", memberShip.Role.Id) });
                }
            }
            else
            {
                if (membershipFromCrm != Guid.Empty)
                {
                    _organizationService.Disassociate("systemuser",
                                    memberShip.User.Id,
                                    new Relationship("systemuserroles_association"),
                                    new EntityReferenceCollection() { new EntityReference("role", memberShip.Role.Id) });
                }
            }
        }

        public void EnsureSystemUserProfileMembership(ProfileMembership memberShip)
        {
            string query = $@"<fetch>
                                  <entity name='systemuserprofiles' >
                                    <attribute name='systemuserprofileid' />
                                    <filter>
                                      <condition attribute='fieldsecurityprofileid' operator='eq' value='{ memberShip.FieldSecurityProfile.Id }' />
                                      <condition attribute='systemuserid' operator='eq' value='{ memberShip.User.Id }' />
                                    </filter>
                                  </entity>
                                </fetch>";

            Guid membershipFromCrm = Helper.FetchRecord(_organizationService, query, "systemuserprofileid");
            if (memberShip.Assign)
            {
                if (membershipFromCrm == Guid.Empty)
                {
                    _organizationService.Associate("systemuser",
                                    memberShip.User.Id,
                                    new Relationship("systemuserprofiles_association"),
                                    new EntityReferenceCollection() { new EntityReference("fieldsecurityprofile", memberShip.FieldSecurityProfile.Id) });
                }
            }
            else
            {
                if (membershipFromCrm != Guid.Empty)
                {
                    _organizationService.Disassociate("systemuser",
                                    memberShip.User.Id,
                                    new Relationship("systemuserprofiles_association"),
                                    new EntityReferenceCollection() { new EntityReference("fieldsecurityprofile", memberShip.FieldSecurityProfile.Id) });
                }
            }
        }

        public static Tuple<int, int, int> FindCell(string cellStr)
        {
            if (string.IsNullOrWhiteSpace(cellStr)) return new Tuple<int, int, int>(0, 0, 0);

            var parts = cellStr.Split(':');
            int counter = 0;
            int retValCol = 0;
            int retValRow = 0;
            int retValColEnd = 0;

            while (counter < parts.Length)
            {
                var match = Regex.Match(parts[counter], @"(?<col>[A-Z]+)(?<row>\d+)");
                var colStr = match.Groups["col"].ToString();
                var col = Convert.ToInt32(colStr.Select((t, i) => (colStr[i] - 64) * Math.Pow(26, colStr.Length - i - 1)).Sum());
                var row = int.Parse(match.Groups["row"].ToString());

                if (counter == 0)
                {
                    retValCol = col;
                    retValRow = row;
                }
                else
                {
                    retValColEnd = col;
                }

                counter++;
            }

            return new Tuple<int, int, int>(retValRow - 1, retValCol - 1, retValColEnd - 1);
        }

        private List<User> FetchSystemUsers(IEnumerable<User> userList)
        {
            List<User> retVal = new List<User>();
            string query = $@"<fetch>
                                  <entity name='systemuser' >
                                    <attribute name='systemuserid' />
                                    <attribute name='domainname' />
                                    <filter>
                                      <condition attribute='domainname' operator='in' >
                                        { string.Join(" ", userList.Select(x => $"<VALUE>{ x.Name }</VALUE>")) }
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = _organizationService.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return retVal;
            foreach (var record in result.Entities)
            {
                retVal.Add(new User()
                {
                    Id = record.GetAttributeValue<Guid>("systemuserid"),
                    Name = record.GetAttributeValue<string>("domainname")
                });
            }
            return retVal;
        }

        private List<Position> FetchPositions(IEnumerable<Position> positions)
        {
            List<Position> retVal = new List<Position>();
            string query = $@"<fetch>
                                  <entity name='position' >
                                    <attribute name='positionid' />
                                    <attribute name='name' />
                                    <filter>
                                      <condition attribute='name' operator='in' >
                                        { string.Join(" ", positions.Select(x => $"<VALUE>{ x.Name }</VALUE>")) }
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = _organizationService.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return retVal;
            foreach (var record in result.Entities)
            {
                retVal.Add(new Position()
                {
                    Id = record.GetAttributeValue<Guid>("positionid"),
                    Name = record.GetAttributeValue<string>("name")
                });
            }
            return retVal;
        }

        private List<Site> FetchSites(IEnumerable<Site> sites)
        {
            List<Site> retVal = new List<Site>();
            string query = $@"<fetch>
                                  <entity name='site' >
                                    <attribute name='siteid' />
                                    <attribute name='name' />
                                    <filter>
                                      <condition attribute='name' operator='in' >
                                        { string.Join(" ", sites.Select(x => $"<VALUE>{ x.Name }</VALUE>")) }
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = _organizationService.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return retVal;
            foreach (var record in result.Entities)
            {
                retVal.Add(new Site()
                {
                    Id = record.GetAttributeValue<Guid>("siteid"),
                    Name = record.GetAttributeValue<string>("name")
                });
            }
            return retVal;
        }

        private List<Territory> FetchTerritories(IEnumerable<Territory> territories)
        {
            List<Territory> retVal = new List<Territory>();
            string query = $@"<fetch>
                                  <entity name='territory' >
                                    <attribute name='territoryid' />
                                    <attribute name='name' />
                                    <filter>
                                      <condition attribute='name' operator='in' >
                                        { string.Join(" ", territories.Select(x => $"<VALUE>{ x.Name }</VALUE>")) }
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = _organizationService.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return retVal;
            foreach (var record in result.Entities)
            {
                retVal.Add(new Territory()
                {
                    Id = record.GetAttributeValue<Guid>("territoryid"),
                    Name = record.GetAttributeValue<string>("name")
                });
            }
            return retVal;
        }

        private List<LegalEntity> FetchLegalEntities(IEnumerable<LegalEntity> legalEntities)
        {
            List<LegalEntity> retVal = new List<LegalEntity>();
            string query = $@"<fetch>
                                  <entity name='rs_legal_entity' >
                                    <attribute name='rs_legal_entityid' />
                                    <attribute name='rs_name' />
                                    <filter>
                                      <condition attribute='rs_name' operator='in' >
                                        { string.Join(" ", legalEntities.Select(x => $"<VALUE>{ x.Name }</VALUE>")) }
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = _organizationService.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return retVal;
            foreach (var record in result.Entities)
            {
                retVal.Add(new LegalEntity()
                {
                    Id = record.GetAttributeValue<Guid>("rs_legal_entityid"),
                    Name = record.GetAttributeValue<string>("rs_name")
                });
            }
            return retVal;
        }

        private List<BusinessUnit> FetchBusinessUnits(IEnumerable<BusinessUnit> entries)
        {
            List<BusinessUnit> retVal = new List<BusinessUnit>();
            string query = $@"<fetch>
                                  <entity name='businessunit' >
                                    <attribute name='businessunitid' />
                                    <attribute name='name' />
                                    <filter>
                                      <condition attribute='name' operator='in' >
                                        { string.Join(" ", entries.Select(x => $"<VALUE>{ x.Name }</VALUE>")) }
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = _organizationService.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return retVal;
            foreach (var record in result.Entities)
            {
                retVal.Add(new BusinessUnit()
                {
                    Id = record.GetAttributeValue<Guid>("businessunitid"),
                    Name = record.GetAttributeValue<string>("name")
                });
            }
            return retVal;
        }

        private List<Role> FetchRoles(IEnumerable<Role> roleList)
        {
            List<Role> retVal = new List<Role>();
            IEnumerable<string> buList = roleList.Select(x => x.BusinessUnit.Name).Distinct();
            string query = $@"<fetch>
                              <entity name='role' >
                                <attribute name='roleid' />
                                <attribute name='name' />
                                <filter>
                                  <condition attribute='name' operator='in' >
                                    { string.Join(" ", roleList.Select(x => $"<VALUE>{ x.Name }</VALUE>")) }
                                  </condition>
                                </filter>
                                <link-entity name='businessunit' from='businessunitid' to='businessunitid' link-type='inner' alias='bu' >
                                  <attribute name='businessunitid' />
                                  <attribute name='name' />
                                  <filter>
                                    <condition attribute='name' operator='in' >
                                      { string.Join(" ", buList.Select(x => $"<VALUE>{ x }</VALUE>")) }
                                    </condition>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = _organizationService.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return retVal;
            foreach (var record in result.Entities)
            {
                retVal.Add(new Role()
                {
                    Id = record.GetAttributeValue<Guid>("roleid"),
                    Name = record.GetAttributeValue<string>("name"),
                    BusinessUnit = new BusinessUnit()
                    {
                        Id = new Guid(record.GetAttributeValue<AliasedValue>("bu.businessunitid").Value.ToString()),
                        Name = record.GetAttributeValue<AliasedValue>("bu.name").Value.ToString()
                    }
                });
            }
            return retVal;
        }

        private List<FieldSecurityProfile> FetchFieldSecurityPofiles(IEnumerable<FieldSecurityProfile> profiles)
        {
            List<FieldSecurityProfile> retVal = new List<FieldSecurityProfile>();
            string query = $@"<fetch>
                              <entity name='fieldsecurityprofile' >
                                <attribute name='fieldsecurityprofileid' />
                                <attribute name='name' />
                                <filter>
                                  <condition attribute='name' operator='in' >
                                    { string.Join(" ", profiles.Select(x => $"<VALUE>{ x.Name }</VALUE>")) }
                                  </condition>
                                </filter>
                              </entity>
                            </fetch>";
            var result = _organizationService.RetrieveMultiple(new FetchExpression(query));
            if (result == null || result.Entities.Count == 0) return retVal;
            foreach (var record in result.Entities)
            {
                retVal.Add(new FieldSecurityProfile()
                {
                    Id = record.GetAttributeValue<Guid>("fieldsecurityprofileid"),
                    Name = record.GetAttributeValue<string>("name")
                });
            }
            return retVal;
        }

        private Role GetRole(Role role, BusinessUnit bu)
        {
            return new Role() { Name = role.Name, BusinessUnit = bu };
        }
    }
}
