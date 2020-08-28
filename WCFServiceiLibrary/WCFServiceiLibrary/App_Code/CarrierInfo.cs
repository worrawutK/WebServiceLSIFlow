using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for CarrierInfo
/// </summary>
[DataContract()]
public class CarrierInfo
{
    public CarrierInfo()
    {
        //
        // TODO: Add constructor logic here
        //
        c_LoadCarrierNo = "";
        c_TransferCarrierNo = "";
        c_RegisterCarrierNo = "";
        c_UnloadCarrierNo = "";
    }
    [DataMember()]
    public bool IsPass { get; set; }
    [DataMember()]
    public string Reason { get; set; }
    
    private string c_LoadCarrierNo;
    [DataMember()]
    public string LoadCarrierNo
    {
        get { return c_LoadCarrierNo; }
        set { c_LoadCarrierNo = value; }
    }

    [DataMember()]
    public CarrierStatus LoadCarrier { get; set; }
    
    private string c_TransferCarrierNo;
    [DataMember()]
    public string TransferCarrierNo
    {
        get { return c_TransferCarrierNo; }
        set { c_TransferCarrierNo = value; }
    }

    [DataMember()]
    public CarrierStatus TransferCarrier { get; set; }
    
    private string c_UnloadCarrierNo;
    [DataMember()]
    public string UnloadCarrierNo
    {
        get { return c_UnloadCarrierNo; }
        set { c_UnloadCarrierNo = value; }
    }

    [DataMember()]
    public CarrierStatus UnloadCarrier { get; set; }
    
    private string c_RegisterCarrierNo;
    [DataMember()]
    public string RegisterCarrierNo
    {
        get { return c_RegisterCarrierNo; }
        set { c_RegisterCarrierNo = value; }
    }
    private string c_CurrentCarrierNo;
    [DataMember()]
    public string CurrentCarrierNo
    {
        get { return c_CurrentCarrierNo; }
        set { c_CurrentCarrierNo = value; }
    }
    private string c_NextCarrierNo;
    [DataMember()]
    public string NextCarrierNo
    {
        get { return c_NextCarrierNo; }
        set { c_NextCarrierNo = value; }
    }
    [DataMember()]
    public CarrierStatus RegisterCarrier { get; set; }
    [DataMember()]
    public CarrierStatus EnabledControlCarrier { get; set; }
    [DataMember()]
    public CarrierStatus InControlCarrier { get; set; }

    [DataContract()]
    public enum CarrierStatus
    {
        [EnumMember()]
        No_Use=0,
        [EnumMember()]
        Use=1,
        [EnumMember()]
        Use_OnLotEnd = 2
    }
}