using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Data;
using Sitecore.Collections;
using Sitecore.Diagnostics;

namespace Sitecore.Modules.FriendlyItemNames.Components
{
    public class RenameSaveAction
    {
        private bool displayNameChanged = false;
        public void Rename(object sender, EventArgs args)
        {
            Database masterDB = Database.GetDatabase("master");
            Item renameSettings = masterDB.GetItem("{A4A86376-4175-4DD2-B3E1-4EA50D25B0A0}");
            string enableLogging = renameSettings["enable logging"];
            if (!renameSettings["disable"].Equals("1"))
            {
                Item savedItm = Event.ExtractParameter(args, 0) as Item;
                if (savedItm.Paths.IsContentItem)
                {
                    bool logging;
                    
                    if (enableLogging.Equals("1"))
                        logging = true;
                    else
                        logging = false;

                    if (logging)
                        Log.Info("Rename Save Action: Trying to rename " + savedItm.Name, this);
                    Item renameFolder = masterDB.GetItem("{C98952EE-F64C-4E8D-8B52-7AC7229F00D0}");
                    if (logging && renameFolder != null)
                        Log.Info("Rename Save Action: Rename folder exists", this);
                    string includedTemplates = renameSettings["included templates"];
                    if (includedTemplates.Equals("") || includedTemplates.Contains(savedItm.TemplateID.ToString()))
                    {

                        ChildList childList = renameFolder.GetChildren();
                        foreach (Item child in childList)
                        {
                            if (savedItm.Name.Contains(child["change from"]))
                            {
                                if (renameSettings["use display name"].Equals("1") && !displayNameChanged)
                                {
                                    using (new Sitecore.SecurityModel.SecurityDisabler())
                                    {
                                        displayNameChanged = true;
                                        savedItm.Editing.BeginEdit();
                                        savedItm["__display name"] = savedItm.Name;
                                        savedItm.Editing.EndEdit();

                                    }
                                }
                                using (new Sitecore.SecurityModel.SecurityDisabler())
                                {
                                    savedItm.Editing.BeginEdit();
                                    savedItm.Name = savedItm.Name.Replace(child["change from"], child["change to"]);
                                    savedItm.Editing.EndEdit();
                                }
                            }
                        }
                    }
                }
            }
            displayNameChanged = false;
        }        
    }
}