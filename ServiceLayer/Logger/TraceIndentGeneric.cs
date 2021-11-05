﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Logger
{
    public  class TraceIndentGeneric<T> : TraceIdentBaseDto
    {
        public TraceIndentGeneric(string traceIdentifier, T result)
            : base(traceIdentifier)
        {
            Result = result;
        }

        public T Result { get; private set; }
    }

}
