using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BT.ILSQueue.Business.Constants
{
    public enum CartStatus
    {
        Open = 1,
        Ordered = 2,
        Downloaded = 3,
        Submitted = 5,
        Archived = 4,
        Deleted = -1,
        Quote_Submitted = 6,
        Quoted = 7,
        Quote_Transmitted = 8,
        Ordered_Quote = 9,
        Processing = 10,
        VIP_Submitted = 11,
        VIP_Ordered = 12
    }

    public enum ILSState
    {
        ILSNew = 1,
        ILSValidationInProcess = 2,
        ILSValidationCompleted = 3,
        ILSOrderValidationInProcess = 4,
        ILSOrderValidationCompleted = 5,
        ILSOrderInProcess = 6,
        ILSOrderCompleted = 7,
        ILSNone = 0
    }

    public enum ILSSystemStatus
    {
        ReadyForPickUp = 1,
        PickedUp = 2,
        InProgress = 3,
        Completed = 4
    }

    public enum UserAlertTemplateID
    {
        CopyCart = 22,
        MergeCart = 23,
        MoveCart = 24,
        ApplyGridTemplate = 25,
        ILSOrderError = 44,
        ILSValidationError = 45,
        ILSOrderSucess = 46,
        ILSValidationSucess = 47,
        ILSOrderValidationError = 48,
        ILSOrderValidationSuccess = 49
    }
    public enum PolarisJobStatus
    {
        Enqueued= 1,
        Processing = 2,
        Succeeded = 3,
        Failed = 4,
        Scheduled = 5

    }

    public enum AccountType
    {
        Book = 0,
        Entertainment = 1,
        BTDML = 4,
        EBRRY = 5,
        NETLB = 6,
        GALEE = 7,
        OE = -1,
        eBook = 2, // will be removed
        BookEntertainment = 10,
        VIP = 8,
        OneBox = 11
    }

    public enum ESPStateType
    {
        None = 0,
        Requested = 1,
        InProcess = 2,
        Submitted = 3,
        Failed = 4,
        Successful = 5,
        Wizard = 6
    }

}
