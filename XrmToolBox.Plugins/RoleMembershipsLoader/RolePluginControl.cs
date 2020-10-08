using ExcelDataReader;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using RoleMembershipsLoader.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace RoleMembershipsLoader
{
    /// <summary>
    /// Plugin Control
    /// </summary>
    public partial class RolePluginControl : PluginControlBase
    {
        private Settings mySettings;
        private DataSet _fileContent;

        public RolePluginControl()
        {
            InitializeComponent();

            _fileContent = new DataSet();
        }

        #region RolePluginControl

        private void RolePluginControl_Load(object sender, EventArgs e)
        {
            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();
            }
            else
            {
                if (mySettings.SpreadSheetRangeDefinition != null)
                {
                    // Loads existing definition
                    this.txtBu.Text = mySettings.SpreadSheetRangeDefinition.BusinessUnit;
                    this.txtManager.Text = mySettings.SpreadSheetRangeDefinition.Manager;
                    this.txtOccupancy.Text = mySettings.SpreadSheetRangeDefinition.Manager;
                    this.txtProfile.Text = mySettings.SpreadSheetRangeDefinition.FieldSecurityProfile;
                    this.txtRowsToImport.Text = mySettings.SpreadSheetRangeDefinition.RowsToLoad;
                    this.txtSecurityRole.Text = mySettings.SpreadSheetRangeDefinition.SecurityRoles;
                    this.txtSite.Text = mySettings.SpreadSheetRangeDefinition.Site;
                    this.txtTerritory.Text = mySettings.SpreadSheetRangeDefinition.Territory;
                    this.txtUserName.Text = mySettings.SpreadSheetRangeDefinition.UserName;
                }
            }
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RolePluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBu.Text))
            {
                MessageBox.Show("Business Unit cell address is required", "Required definition", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(ImportData);
        }

        /// <summary>
        /// Browses for file to load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRolesBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Select file to load security roles and profiles assignments";
                dlg.Filter = "Excel |*.xlsx";
                if (dlg.ShowDialog() == DialogResult.OK && dlg.CheckFileExists)
                {
                    this.txtFileName.Text = dlg.FileName;

                    using (var stream = File.Open(this.txtFileName.Text, FileMode.Open, FileAccess.Read))
                    {
                        // Auto-detect format, supports:
                        //  - Binary Excel files (2.0-2003 format; *.xls)
                        //  - OpenXml Excel files (2007 format; *.xlsx)
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var conf = new ExcelDataSetConfiguration
                            {
                                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                                {
                                    UseHeaderRow = true
                                }
                            };

                            // 2. Use the AsDataSet extension method
                            var result = reader.AsDataSet();

                            this.lstWorksheetName.Items.Clear();
                            this.lstWorksheetName.Items.AddRange(result.Tables.OfType<DataTable>().Select(x => x.TableName).ToArray());

                            _fileContent = result;
                        }
                    }
                }
            }

            EnableLoadButton();
        }

        /// <summary>
        /// Enable/disable load button
        /// </summary>
        private void EnableLoadButton()
        {
            this.btnLoad.Enabled = this.lstWorksheetName.Items.Count > 0 && this.lstWorksheetName.SelectedItem != null;
        }

        /// <summary>
        /// Import all rows or select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAllRows_CheckedChanged(object sender, EventArgs e)
        {
            this.txtRowsToImport.Visible = !chkAllRows.Checked;
            if (!this.txtRowsToImport.Visible)
            {
                var userName = Helper.FindCell(this.txtUserName.Text);

                this.chkAllRows.Text = $"Rows range to load: {userName.Item2}";
                this.txtRowsToImport.Text = $"{userName.Item1 + 1}";
            }
            else
            {
                this.chkAllRows.Text = "Import all rows";
            }
        }

        private void lstWorksheetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableLoadButton();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            ValidateNoCellsRange((TextBox)sender);
        }

        private void txtManager_TextChanged(object sender, EventArgs e)
        {
            ValidateNoCellsRange((TextBox)sender);
        }

        private void txtOccupancy_TextChanged(object sender, EventArgs e)
        {
            ValidateNoCellsRange((TextBox)sender);
        }

        private void txtSite_TextChanged(object sender, EventArgs e)
        {
            ValidateNoCellsRange((TextBox)sender);
        }

        private void txtTerritory_TextChanged(object sender, EventArgs e)
        {
            ValidateNoCellsRange((TextBox)sender);
        }

        private void txtBu_TextChanged(object sender, EventArgs e)
        {
            ValidateNoCellsRange((TextBox)sender);
        }

        /// <summary>
        /// Cells range is mandatory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSecurityRole_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            var field = Helper.FindCell(text);
            if (field.Item1 <= 0 || field.Item2 <= 0)
            {
                MessageBox.Show($"Invalid cell range: {text}", "Invalid range", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Cells range is mandatory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtProfile_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            var field = Helper.FindCell(text);
            if (field.Item1 <= 0 || field.Item2 <= 0)
            {
                MessageBox.Show($"Invalid cell range: {text}", "Invalid range", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            using (About frm = new About())
            {
                frm.ShowDialog();
            }
        }

        #endregion RolePluginControl

        #region Private

        /// <summary>
        /// Performs import
        /// </summary>
        private void ImportData()
        {
            btnLoad.Enabled = false;
            MainLoader manager = new MainLoader(Service, _fileContent);
            manager.OnWarning += Manager_OnWarning;

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Parsing worksheet {this.lstWorksheetName.SelectedItem}...",
                Work = (mainWorker, mainArgs) =>
                {
                    SpreadSheetRangeDefinition definition = new SpreadSheetRangeDefinition
                    {
                        WorksheetName = this.lstWorksheetName.SelectedItem.ToString(),
                        UserName = this.txtUserName.Text,
                        Manager = this.txtManager.Text,
                        Position = this.txtOccupancy.Text,
                        Site = this.txtSite.Text,
                        Territory = this.txtTerritory.Text,
                        BusinessUnit = this.txtBu.Text,
                        SecurityRoles = this.txtSecurityRole.Text,
                        FieldSecurityProfile = this.txtProfile.Text,
                        AllRows = this.chkAllRows.Checked,
                        RowsToLoad = this.txtRowsToImport.Text
                    };

                    mySettings.SpreadSheetRangeDefinition = definition;

                    mainArgs.Result = manager.ParseFile(definition);
                },
                PostWorkCallBack = (mainArgs) =>
                {
                    if (mainArgs.Error != null)
                    {
                        MessageBox.Show(mainArgs.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var result = mainArgs.Result as ImportContent;
                    if (result != null)
                    {
                        WorkAsync(new WorkAsyncInfo
                        {
                            ProgressChanged = e =>
                            {
                                SetWorkingMessage(e.UserState.ToString());
                            },
                            Message = "Fetching system users, security roles and field security profiles from your Organization...",
                            Work = (worker, args) =>
                            {                                
                                args.Result = manager.FetchData(result);
                                var content = args.Result as ImportContent;
                                if (content != null)
                                {
                                    int count = content.Users.Count();
                                    worker.ReportProgress(-1, $"{count} Users to process.");
                                }                                
                            },
                            PostWorkCallBack = (args) =>
                            {
                                if (args.Error != null)
                                {
                                    MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                var res = args.Result as ImportContent;
                                if (res != null)
                                {
                                    WorkAsync(new WorkAsyncInfo
                                    {
                                        ProgressChanged = e =>
                                        {
                                            SetWorkingMessage(e.UserState.ToString());
                                        },
                                        Message = "Please wait... Processing users.",
                                        Work = (worker, nargs) =>
                                        {
                                            var counter = 1;
                                            foreach (var user in res.Users)
                                            {
                                                worker.ReportProgress(counter, $"Updating user { user.Name }.");
                                                try
                                                {
                                                    manager.UpdateUser(user);
                                                }
                                                catch (Exception e)
                                                {
                                                    LogError(e.Message);
                                                    //ShowErrorNotification(e.Message, null);
                                                }
                                                finally
                                                {
                                                    worker.ReportProgress(counter, $"User { user.Name } has been updated.");
                                                }
                                            }

                                            foreach (var membership in res.RoleMemberships)
                                            {
                                                worker.ReportProgress(counter, $"Processing user { membership.User.Name } and role { membership.Role.Name } memberships.");
                                                try
                                                {
                                                    manager.EnsureSystemUserRoleMembership(membership);
                                                }
                                                catch (Exception e)
                                                {
                                                    LogError(e.Message);
                                                    //ShowErrorNotification(e.Message, null);
                                                }
                                                finally
                                                {
                                                    worker.ReportProgress(counter, $"Processed user { membership.User.Name } and role { membership.Role.Name } memberships completed.");
                                                }
                                            }
                                            foreach (var profile in res.ProfileMemberships)
                                            {
                                                worker.ReportProgress(counter, $"Processing user { profile.User.Name } and security profile { profile.FieldSecurityProfile.Name } memberships.");
                                                try
                                                {
                                                    manager.EnsureSystemUserProfileMembership(profile);
                                                }
                                                catch (Exception e)
                                                {
                                                    LogError(e.Message);
                                                    //ShowErrorNotification(e.Message, null);
                                                }
                                                finally
                                                {
                                                    worker.ReportProgress(counter, $"Processed user { profile.User.Name } and security profile { profile.FieldSecurityProfile.Name } memberships completed.");
                                                }
                                            }
                                        },
                                        PostWorkCallBack = (nargs) =>
                                        {
                                            if (args.Error != null)
                                            {
                                                MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            this.btnLoad.Enabled = true;
                                            this.lstWorksheetName.SelectedIndex = -1;
                                            
                                            if (MessageBox.Show("Import System Users along with their Security roles and Field Security Profiles memberships has been completed. Do you want to review a log file?", "Import", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                            {
                                                OpenLogFile();
                                            }
                                        }
                                    });
                                }
                            }
                        });
                    }
                }
            });
        }

        /// <summary>
        /// Event handler for warnings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Manager_OnWarning(object sender, EventArgs e)
        {
            NotificationMessage msg = e as NotificationMessage;
            if (msg == null) return;

            LogWarning(msg.Message);
        }

        /// <summary>
        /// Cells range is not support for such as definition
        /// </summary>
        /// <param name="textBox"></param>
        private void ValidateNoCellsRange(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text)) return;
            if (textBox.Text.Contains(":"))
            {
                textBox.Text = "";
                MessageBox.Show("Character ':' is not valid for such as field. Don't use range but single column only.", "Invalid character", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Private

        #region PluginControlBase

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }

        #endregion PluginControlBase                
    }
}