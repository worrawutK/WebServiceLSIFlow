using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StartLotEventArgs
/// </summary>
public class StartLotEventArgs
{
    public StartLotEventArgs()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public string LotNo { get; set; }
    public string MachineNo { get; set; }
    public string OpNo { get; set; }
    public string Recipe { get; set; }
    public string FunctionName { get; set; }
    public string LoadCarrierNo { get; set; }
    public string TransfersCarrierNo { get; set; }
    public int IsOnline { get; set; }
    //string lotNo, string mcNo, string opNo, string recipe,string functionName,
    //    RunMode runMode,string mcNoApcs = "",
    //    int locationNum = 1,int actPassQty = -1,
    //    string loadCarrierNo = "",string transferCarrierNo = "",
    //    int isOnline = 1
}