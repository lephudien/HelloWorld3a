using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;


namespace MSBuild.IndSoft.NuGet.Tasks
{
  public class IndSoftReplaceNugetReferences : Task
  {
    private ITaskItem[] referencesIn;
    private ITaskItem[] referencesOut;
    string oneReferenceOut;

    [Required]
    public string FolderNugetCacheNeedReplaceMask { get; set; }

    [Required]
    public string FolderLibraryDev { get; set; }

    public ITaskItem[] ReferencesIn
    {
      get
      {
        return referencesIn;
      }

      set
      {
        referencesIn = value;
      }
    }

    // The Output attribute indicates to MSBuild that the value of this property can be gathered after the
    // task has returned from Execute(), if the project has an <Output> tag under this task's element for
    // this property.
    [Output]
    // A project may need the subset of the inputs that were actually created, so make that available here.
    public string OneReferenceOut
    {
      get
      {
        return oneReferenceOut;
      }
    }

    // The Output attribute indicates to MSBuild that the value of this property can be gathered after the
    // task has returned from Execute(), if the project has an <Output> tag under this task's element for
    // this property.
    [Output]
    // A project may need the subset of the inputs that were actually created, so make that available here.
    public ITaskItem[] ReferencesOut
    {
      get
      {
        return referencesOut;
      }
    }

    public override bool Execute()
    {
      oneReferenceOut = "";
      ArrayList itemsOut = new ArrayList();
      foreach (ITaskItem reference in referencesIn)
      {
        string ReferencePathOrg = reference.ItemSpec;
        Log.LogMessage("ReferencePathOrg: '{0}'", ReferencePathOrg);
        Log.LogMessage("FolderNeedReplaceMask: '{0}'", FolderNugetCacheNeedReplaceMask);
        Log.LogMessage("FolderLibraryDev: '{0}'", FolderLibraryDev);

        // Nazacatku nastavime ReferencePathNew = ReferencePathOrg at deje co se deje
        string ReferencePathNew = ReferencePathOrg;
        string path = ReferencePathOrg;
        string sDir = Path.GetDirectoryName(ReferencePathOrg);
        if (sDir.Contains(FolderNugetCacheNeedReplaceMask))
        {
          ReferencePathNew = Path.Combine(FolderLibraryDev, Path.GetFileName(ReferencePathOrg));
          Log.LogMessage("Output ReferencePathNew: '{0}'", ReferencePathOrg);
          oneReferenceOut += ";" + ReferencePathNew;
          if (!File.Exists(ReferencePathNew))
            Log.LogError("Output ReferencePathNew in path '{0}' does not exist!", ReferencePathNew);

          // Add to the list of created directories
          reference.ItemSpec = ReferencePathNew;
          itemsOut.Add(reference);
        }
      }

      // Vyhodime prvni znak ';'      
      if (oneReferenceOut.Length > 0)
        oneReferenceOut = oneReferenceOut.Substring(1);
      Log.LogMessage("Final oneReferenceOut: '{0}'", oneReferenceOut);

      // Populate the "DirectoriesCreated" output items.
      referencesOut = (ITaskItem[])itemsOut.ToArray(typeof(ITaskItem));

      return true;
    }
  }

   /*
       * Class: MakeDir
       *
       * An MSBuild task that creates one or more directories.
       *
      */
  public class IndSoftMakeDir : Task
  {
    // The Required attribute indicates the following to MSBuild:
    //	     - if the parameter is a scalar type, and it is not supplied, fail the build immediately
    //	     - if the parameter is an array type, and it is not supplied, pass in an empty array
    // In this case the parameter is an array type, so if a project fails to pass in a value for the
    // Directories parameter, the task will get invoked, but this implementation will do nothing,
    // because the array will be empty.
    [Required]
    // Directories to create.
    public ITaskItem[] Directories
    {
      get
      {
        return directories;
      }

      set
      {
        directories = value;
      }
    }

    // The Output attribute indicates to MSBuild that the value of this property can be gathered after the
    // task has returned from Execute(), if the project has an <Output> tag under this task's element for
    // this property.
    [Output]
    // A project may need the subset of the inputs that were actually created, so make that available here.
    public ITaskItem[] DirectoriesCreated
    {
      get
      {
        return directoriesCreated;
      }
    }

    private ITaskItem[] directories;
    private ITaskItem[] directoriesCreated;

    /// <summary>
    /// Execute is part of the Microsoft.Build.Framework.ITask interface.
    /// When it's called, any input parameters have already been set on the task's properties.
    /// It returns true or false to indicate success or failure.
    /// </summary>
    public override bool Execute()
    {
      ArrayList items = new ArrayList();
      foreach (ITaskItem directory in Directories)
      {
        // ItemSpec holds the filename or path of an Item
        if (directory.ItemSpec.Length > 0)
        {
          try
          {
            // Only log a message if we actually need to create the folder
            if (!Directory.Exists(directory.ItemSpec))
            {
              Log.LogMessage(MessageImportance.Normal, "Creating directory " + directory.ItemSpec);
              Directory.CreateDirectory(directory.ItemSpec);
            }

            // Add to the list of created directories
            items.Add(directory);
          }
          // If a directory fails to get created, log an error, but proceed with the remaining
          // directories.
          catch (Exception ex)
          {
            if (ex is IOException
                || ex is UnauthorizedAccessException
                || ex is PathTooLongException
                || ex is DirectoryNotFoundException
                || ex is SecurityException)
            {
              Log.LogError("Error trying to create directory " + directory.ItemSpec + ". " + ex.Message);
            }
            else
            {
              throw;
            }
          }
        }
      }

      // Populate the "DirectoriesCreated" output items.
      directoriesCreated = (ITaskItem[])items.ToArray(typeof(ITaskItem));

      // Log.HasLoggedErrors is true if the task logged any errors -- even if they were logged
      // from a task's constructor or property setter. As long as this task is written to always log an error
      // when it fails, we can reliably return HasLoggedErrors.
      return !Log.HasLoggedErrors;
    }
  }

  /*
  public class IndSoftReplaceReference : Task
  {
    [Required]
    public string ReferencePathOrg { get; set; }

    [Required]
    public string LibraryDevFolder { get; set; }

    [Output]
    public string ReferencePathNew { get; set; }

    public override bool Execute()
    {
      Log.LogMessage("ReferencePathOrg: '{0}'", ReferencePathOrg);
      Log.LogMessage("LibraryDevFolder: '{0}'", LibraryDevFolder);

      // Nazacatku nastavime ReferencePathNew = ReferencePathOrg at deje co se deje
      ReferencePathNew = ReferencePathOrg;
      string path = ReferencePathOrg;
      string sDir = Path.GetDirectoryName(ReferencePathOrg);
      if ((sDir != LibraryDevFolder) && (Path.GetExtension(ReferencePathOrg).ToLower() == ".dll"))
        ReferencePathNew = Path.Combine(LibraryDevFolder, Path.GetFileName(ReferencePathOrg));
      Log.LogMessage("Output ReferencePathNew: '{0}'", ReferencePathOrg);
      if (File.Exists(ReferencePathNew))
      {
        return true;
      }
      else
      {
        Log.LogError("Output ReferencePathNew in path '{0}' does not exist!", ReferencePathNew);
        return false;
      }
    }
  }
  */
}