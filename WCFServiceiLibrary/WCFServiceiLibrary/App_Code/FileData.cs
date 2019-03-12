using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for FileData
/// </summary>
public class FileData
{
    public string FileName { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? CreateTime { get; set; }
    public string Path { get; set; }
    public FileData()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}