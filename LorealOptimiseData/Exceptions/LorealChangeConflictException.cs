using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace LorealOptimiseData.Exceptions
{
    public class LorealChangeConflictException : ChangeConflictException
    {
        public bool IsConflictOnField
        {
            get; set;
        }

        public LorealChangeConflictException(string message, Exception innerException, bool isConflictOnField) : base(message, innerException)
        {
            IsConflictOnField = isConflictOnField;
        }
    }
}
