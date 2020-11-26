using iLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for PermissionParamitersEventArgs
/// </summary>
[DataContract()]
public class PermissionParamitersEventArgs
{
    public PermissionParamitersEventArgs()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    private UserInfo c_UserInformation;
    [DataMember()]
    public UserInfo UserInformation
    {
        get { return c_UserInformation; }
        set { c_UserInformation = value; }
    }
    //private string c_UserCode;
    //[DataMember()]
    //public string UserCode
    //{
    //    get { return c_UserCode; }
    //    set { c_UserCode = value; }
    //}
    private string c_ApplicationName;
    [DataMember()]
    public string ApplicationName
    {
        get { return c_ApplicationName; }
        set { c_ApplicationName = value; }
    }
    private string c_FunctionName;
    [DataMember()]
    public string FunctionName
    {
        get { return c_FunctionName; }
        set { c_FunctionName = value; }
    }

    //private string c_MahineNo;
    //[DataMember()]
    //public string MachineNo
    //{
    //    get { return c_MahineNo; }
    //    set { c_MahineNo = value; }
    //}
    private  DateTime?   c_DateTimeLog;
    [DataMember()]
    public DateTime? DateTimeLog
    {
        get { return c_DateTimeLog; }
        set { c_DateTimeLog = value; }
    }


}