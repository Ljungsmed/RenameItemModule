namespace Sitecore.Modules.FriendlyItemNames.Components
{

    // TODO: \App_Config\include\FINEditorWarning.config created automatically when creating FINEditorWarning class.

    using Sitecore.Pipelines.GetContentEditorWarnings;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Globalization;

    public class FINEditorWarning
    {
        public void Process([NotNull] GetContentEditorWarningsArgs args)
        {
            Database masterDB = Database.GetDatabase("master");
            Item renameSettings = masterDB.GetItem("{A4A86376-4175-4DD2-B3E1-4EA50D25B0A0}");

            if (!renameSettings["disable"].Equals("1"))
            {
                if (renameSettings["show warning in editor"].Equals("1"))
                {
                    if (!args.Item["__display name"].Equals(""))
                    {
                        var warning = args.Add();
                        string warningText = Translate.Text("FINWarningText").Replace("#name#", args.Item.Name);
                        warning.Title = Translate.Text("FINWarningTitle");
                        warning.Text = warningText;
                    }
                }
            }
        }
    }
}